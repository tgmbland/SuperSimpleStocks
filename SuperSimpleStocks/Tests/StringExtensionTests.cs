using SuperSimpleStocks.ExtensionMethods;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace SuperSimpleStocks.Tests
{
    [TestFixture]
    public class StringExtensionTests
    {
        [Test]
        [TestCase(null, "")]
        [TestCase("UPPER", "Upper")]
        [TestCase("lower", "Lower")]
        [TestCase("wOnKy", "Wonky")]
        public void EnsureToTitleCaseExtensionMethodBehavesAsExpected(string input, string expectedResult)
        {
            var result = input.ToTitleCase();
            Assert.AreEqual(expectedResult, result);
        }
    }
}
