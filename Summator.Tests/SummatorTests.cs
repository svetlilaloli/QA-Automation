using NUnit.Framework;

namespace Summator.Tests
{
    public class SummatorTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase(0, 100)]
        [TestCase(-13, 13)]
        [TestCase(-13, -13)]
        [TestCase(0, 0), Category("Critical")]
        public void Test_Sum_TwoNumbers(int x, int y)
        {
            long expected = x + y;
            long actual = Summator.Sum(new int[] { x, y });
            Assert.AreEqual(expected, actual);
        }
        [TestCase(10)]
        [TestCase(-122)]
        [TestCase(0), Category("Critical")]
        public void Test_Sum_OneNumber(int x)
        {
            long expected = x;
            long actual = Summator.Sum(new int[] { x });
            Assert.AreEqual(expected, actual);
        }
        [Test, Category("Critical")]
        public void Test_Sum_EmptyArray()
        {
            long expected = 0;
            long actual = Summator.Sum(System.Array.Empty<int>());
            Assert.AreEqual(expected, actual);
        }
        [TestCase(987654321, 876543210, 765432109)]
        public void Test_Sum_BigNumbers(int x, int y, int z)
        {
            long expected = (long)x + y + z; //2629629640;
            long actual = Summator.Sum(new int[] { x, y, z });
            Assert.AreEqual(expected, actual);
        }
        [TestCase(0, 100)]
        [TestCase(13, -13)]
        [TestCase(-13, -13)]
        [TestCase(0, 0), Category("Critical")]
        public void Test_Average_TwoNumbers(int x, int y)
        {
            double expected = (x + y) / 2;
            double actual = Summator.Average(new int[] { x, y });
            Assert.AreEqual(expected, actual);
        }
        [TestCase(10)]
        [TestCase(-132)]
        [TestCase(0), Category("Critical")]
        public void Test_Average_OneNumber(int x)
        {
            double expected = x;
            double actual = Summator.Average(new int[] { x });
            Assert.AreEqual(expected, actual);
        }
        [Test, Category("Critical")]
        public void Test_Average_EmptyArray()
        {
            double actual = Summator.Average(System.Array.Empty<int>());
            Assert.AreEqual(0, actual);
        }
        [TestCase(987654321, 876543210, 765432109)]
        public void Test_Average_BigNumbers(int x, int y, int z)
        {
            double expected = ((long)x + y + z) / 3; 
            double actual = Summator.Average(new int[] { x, y, z });
            Assert.AreEqual(expected, actual);
        }
    }
}