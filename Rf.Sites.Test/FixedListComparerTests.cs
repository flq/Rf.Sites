using NUnit.Framework;
using Rf.Sites.Frame;
using System.Linq;
using FluentAssertions;

namespace Rf.Sites.Test
{
    [TestFixture]
    public class FixedListComparerTests
    {
        private FixedListComparer _comparer;
        private readonly string[] _template = new[] { "Z", "A", "M", "X" };

        [TestFixtureSetUp]
        public void Given()
        {
            _comparer = new FixedListComparer(_template);
        }

        [Test]
        public void Sorts_according_to_given_list()
        {
            var l = new[] { "A", "M", "X", "Z" };
            var l2 = l.OrderBy(s => s, _comparer);
            l2.Should().BeEquivalentTo(_template);
        }
    }
}