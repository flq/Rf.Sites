using System;
using HtmlTags;
using NUnit.Framework;
using Rf.Sites.Frame;
using FluentAssertions;

namespace Rf.Sites.Test
{
    [TestFixture]
    public class DynamicJsonTests
    {
        [Test]
        public void DateTimeIsRereadable()
        {
            var expected = new DateTime(2011,10,10, 0,0,0, DateTimeKind.Utc);
            var json = JsonUtil.ToJson(new { Date = expected });
            Console.WriteLine(json);

            var obj = DynamicJson.Parse(json);
            var dt = (DateTime)obj.Date;
            dt.Should().Be(expected);
        }
    }
}