using System;
using System.Collections.Generic;
using System.Linq;
using Google.GData.Analytics;
using Google.GData.Client;

namespace Rf.Sites.Frame
{
    public class GoogleApiSettings
    {
        public string User { get; set; }
        public string Password { get; set; }

        //"https://www.google.com/analytics/feeds/data"
        public string AnalyticsUrl { get; set; }

        public int ResultCount { get; set; }
    }

    /// <summary>
    /// Interaction with Google Analytics to get the top ranked realfiction pages
    /// </summary>
    public class AnalyticsTopPagesProvider
    {
        private readonly GoogleApiSettings _settings;

        public AnalyticsTopPagesProvider(GoogleApiSettings settings)
        {
            _settings = settings;
        }

        public List<string> GetTopUrls()
        {
            try
            {
                var asv = new AnalyticsService("gaExportAPI_v2.0");
                asv.setUserCredentials(_settings.User, _settings.Password);

                var query = GetQuery();

                var feed = asv.Query(query);

                return TransformQuery(feed);
            }
            catch (GDataRequestException x)
            {
                return new List<string>();
            }
        }

        private DataQuery GetQuery()
        {
            return new DataQuery(_settings.AnalyticsUrl)
                       {
                           Ids = "ga:3274365",
                           Dimensions = "ga:hostName,ga:pagePath",
                           Metrics = "ga:visits",
                           Sort = "-ga:visits",
                           NumberToRetrieve = _settings.ResultCount,
                           GAStartDate = ThirtyDaysAgo,
                           GAEndDate = Today
                       };
        }

        private static List<string> TransformQuery(DataFeed feed)
        {
            var vals = (from entry in feed.Entries
                        let extensions = entry.ExtensionElements.Where(e => e is Dimension || e is Metric).Cast<dynamic>()
                        select extensions.ToDictionary(elem => ((string)elem.Name).ToLowerInvariant(), elem => (string)elem.Value)).ToList();
            return vals.Select(d => string.Concat(d["ga:hostname"], d["ga:pagepath"])).ToList();
        }

        private static string ThirtyDaysAgo
        {
            get { return DateTime.UtcNow.AddDays(-30).ToString("yyyy-MM-dd"); }
        }

        private static string Today
        {
            get { return DateTime.UtcNow.ToString("yyyy-MM-dd"); }
        }
    }
}