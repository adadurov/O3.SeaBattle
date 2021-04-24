using FluentAssertions;
using NUnit.Framework;
using O3.SeaBattle.Service.Parsers;
using System;

namespace O3.SeaBattle.Tests.Parsers
{
    [TestFixture]
    public class LocationFactoryTest
    {
        private static LocationParser LocationParserInstance => new LocationParser(1, 'A', short.MaxValue);

        [Test]
        public void Create_WithNullSpec_Should_Throw()
        {
            Action c = () => LocationParserInstance.Parse(null);
            c.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Create_WithEmptySpec_Should_Throw()
        {
            Action c = () => LocationParserInstance.Parse("");
            c.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Create_WithNegativeRow_Should_Throw()
        {
            Action c = () => LocationParserInstance.Parse("-1A");
            c.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Create_WithZeroRow_Should_Throw()
        {
            Action c = () => LocationParserInstance.Parse("0A");
            c.Should().Throw<ArgumentException>();
        }

    }
}
