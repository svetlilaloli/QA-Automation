using NUnit.Framework;

namespace Summator.Tests
{
    public class SumTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase(0, 100)]
        [TestCase(-13, 13)]
        [TestCase(-13, -13)]
        public void Test_Sum_TwoNumbers(int x, int y)
        {
            int expected = x + y;
            int[] arr = new int[] {x, y};
            long actual = Summator.Sum(arr);
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void Test_Sum_OneNumber()
        {
            long actual = Summator.Sum(new int[] { 10 });
            Assert.AreEqual(10, actual);
        }
        [Test]
        public void Test_Sum_EmptyArray()
        {
            long actual = Summator.Sum(new int[] { });
            Assert.AreEqual(0, actual);
        }
        [Test]
        public void Test_Sum_BigNumbers()
        {
            long actual = Summator.Sum(new int[] {987654321, 876543210, 765432109 });
            Assert.AreEqual(2629629640, actual);
        }
        [TestCase(0, 100)]
        [TestCase(13, -13)]
        [TestCase(-13, -13)]
        public void Test_Average_TwoNumbers(int x, int y)
        {
            int expected = (x + y) / 2;
            long actual = Summator.Average(new int[] { x, y });
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void Test_Average_OneNumber()
        {
            long actual = Summator.Average(new int[] { 10 });
            Assert.AreEqual(10, actual);
        }
        [Test]
        public void Test_Average_EmptyArray()
        {
            long actual = Summator.Average(new int[] { });
            Assert.AreEqual(0, actual);
        }
        [Test]
        public void Test_Average_BigNumbers()
        {
            long actual = Summator.Average(new int[] { 987654321, 876543210, 765432109 });
            Assert.AreEqual(2629629640 / 3, actual);
        }
    }
}