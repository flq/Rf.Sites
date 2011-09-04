using System;
using System.IO;
using System.Xml;
using FluentAssertions;
using NUnit.Framework;
using Rf.Sites.Entities;
using Rf.Sites.Features.Models;
using Rf.Sites.Frame.SiteInfrastructure;
using Rf.Sites.Test.DataScenarios;
using System.Linq;

namespace Rf.Sites.Test
{
    [TestFixture]
    internal class Using_sitemap_handler : In_memory_repository_context<Content>
    {
        private SiteMapModel _sitemap;
        private XmlDocument output;
        private XmlNamespaceManager nsMgr;

        [TestFixtureSetUp]
        public void Given()
        {
            ApplyData<ContentForSitemapScenario>();
            _sitemap = new SiteMapModel("http://localhost/{Id}", Repository.Select(s => s));
            var sw = new StringWriter();
            ((IStreamOutput)_sitemap).Accept(sw);
            output = new XmlDocument();
            output.LoadXml(sw.GetStringBuilder().ToString());
            nsMgr = new XmlNamespaceManager(output.NameTable);
            nsMgr.AddNamespace("ns0", "http://www.sitemaps.org/schemas/sitemap/0.9");
        }

        [Test]
        public void TheOutputIsAValidDocument()
        {
            output.Should().NotBeNull();
        }

        [Test]
        public void ChangeFrequencyOfRecentContentIsCorrect()
        {
            var node = getTheNodeOf(c => c.Title == "A");
            node.SelectSingleNode("ns0:changefreq", nsMgr).InnerText.Should().Be("weekly");
        }

        [Test]
        public void ChangeFrequencyOfOldContentIsCorrect()
        {
            var node = getTheNodeOf(c => c.Title == "C");
            node.SelectSingleNode("ns0:changefreq", nsMgr).InnerText.Should().Be("never");
        }

        [Test]
        public void UrlIsSetupCorrectly()
        {
            const string correctUrl = "http://localhost/0";
            var node = getTheNodeOf(c => c.Title == "A");
            node.SelectSingleNode("ns0:loc", nsMgr).InnerText.Should().Be(correctUrl);
        }

        private XmlNode getTheNodeOf(Func<Content, bool> func)
        {
            var cnt = Repository.Where(func).Single();
            var xpath = "//ns0:url[contains(./ns0:loc,'" + cnt.Id + "')]";
            var node = output.SelectSingleNode(xpath, nsMgr);
            return node;
        }
    }

   
}