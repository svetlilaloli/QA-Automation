using NUnit.Framework;
using System;
using System.Linq;

namespace Collections.Tests
{
    public class CollectionsTests
    {
        private Collection<int> numbers;
        private const int numbersCount = 1000000;
        [SetUp]
        public void Init()
        {
            numbers = new Collection<int>();
        }
        [Test]
        public void Test_EmptyConstructor()
        {
            // arrange
            // act
            // assert
            Assert.AreEqual("[]", numbers.ToString());
        }
        [Test]
        public void Test_ConstructorSingleItem()
        {
            // arrange
            // act
            numbers = new Collection<int>(1);
            // assert
            Assert.AreEqual("[1]", numbers.ToString());
        }
        [Test]
        public void Test_ConstructorMultipleItems()
        {
            numbers = new Collection<int>(5, 10, 15);
            Assert.AreEqual("[5, 10, 15]", numbers.ToString());
        }

        [Test]
        [Timeout(1000)]
        public void Test_1MillionItems()
        {
            numbers.AddRange(Enumerable.Range(1, numbersCount).ToArray());
            Assert.AreEqual(numbersCount, numbers.Count);
            Assert.That(numbers.Capacity >= numbers.Count);
            for (int i = numbersCount - 1; i >= 0; i--)
                numbers.RemoveAt(i);
            Assert.AreEqual("[]", numbers.ToString());
            Assert.That(numbers.Capacity >= numbers.Count);
        }

        [TestCase(new int[0], 16)]
        [TestCase(new int[] { 1 }, 16)]
        [TestCase(new int[] { 1, 2, 3, 4, 5, 6, 7, 8}, 16)]
        [TestCase(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 32)]
        [TestCase(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 }, 34)]
        public void Test_CountAndCapacity(int[] arr, int capacity)
        {
            // arrange
            numbers = new Collection<int>(arr);
            // act
            // assert
            Assert.AreEqual(arr.Length, numbers.Count);
            Assert.AreEqual(capacity, numbers.Capacity);
        }
        [Test]
        public void Test_Add()
        {
            // arrange
            // act
            numbers.Add(1);
            numbers.Add(2);
            // assert
            Assert.AreEqual(2, numbers.Count);
            Assert.AreEqual("[1, 2]", numbers.ToString());
        }
        [Test]
        public void Test_AddWithGrow()
        {
            // arrange
            // act
            for (int i = 0; i < 17; i++)
            {
                numbers.Add(i);
            }
            // assert
            Assert.AreEqual(32, numbers.Capacity); // the capacity should double its initial size
        }
        [Test]
        public void Test_AddRange()
        {
            // arrange
            // act
            numbers.AddRange(1, 2, 3);
            // assert
            Assert.AreEqual(3, numbers.Count);
            Assert.AreEqual("[1, 2, 3]", numbers.ToString());
        }
        [Test]
        public void Test_AddRangeWithGrow()
        {
            // arrange
            int[] arr = new int[17];
            // act
            numbers.AddRange(arr);
            // assert
            Assert.AreEqual(32, numbers.Capacity);
        }
        [TestCase("Peter", 0, "Peter")]
        [TestCase("Peter, Maria, Steve", 0, "Peter")]
        [TestCase("Peter, Maria, Steve", 1, "Maria")]
        [TestCase("Peter, Maria, Steve", 2, "Steve")]
        public void Test_GetByValidIndex(string data, int index, string expectedResult)
        {
            // Arrange
            var names = new Collection<string>(data.Split(", "));
            // Act
            string name = names[index];
            // Assert
            Assert.AreEqual(expectedResult, name);
        }
        [TestCase("", 0)]
        [TestCase("Peter", -1)]
        [TestCase("Peter, Maria, Steve", -1)]
        [TestCase("Peter, Maria, Steve", 3)]
        public void Test_GetByInvalidIndex(string data, int index)
        {
            // arrange
            var names = new Collection<string>(data.Split(", ", StringSplitOptions.RemoveEmptyEntries));
            // act
            // assert
            Assert.Throws<ArgumentOutOfRangeException>(() => { string name = names[index]; });
        }
        [TestCase(new[] { 1, 2, 3 }, 0)]
        [TestCase(new[] { 1, 2, 3 }, 1)]
        [TestCase(new[] { 1, 2, 3 }, 2)]
        public void Test_SetByValidIndex(int[] arr, int index)
        {
            // arrange
            numbers = new Collection<int>(arr);
            // act
            numbers[index] = 33;
            // assert
            Assert.AreEqual(33, numbers[index]);
        }
        [TestCase(new int[0], -1)]
        [TestCase(new int[0], 0)]
        [TestCase(new[] {1, 2, 3}, -1)]
        [TestCase(new[] {1, 2, 3}, 3)]
        public void Test_SetByInvalidIndex(int[] arr, int index)
        {
            // arrange
            numbers = new Collection<int>(arr);
            // act
            // assert
            Assert.Throws<ArgumentOutOfRangeException>(() => numbers[index] = 33);
        }
        [Test]
        public void Test_InsertAtStart()
        {
            // arrange
            numbers = new Collection<int>(1, 2, 3);
            // act
            numbers.InsertAt(0, 0);
            // assert
            Assert.AreEqual("[0, 1, 2, 3]", numbers.ToString());
        }
        [Test]
        public void Test_InsertAtEnd()
        {
            // arrange
            numbers = new Collection<int>(1, 2, 3);
            // act
            numbers.InsertAt(3, 0);
            // assert
            Assert.AreEqual("[1, 2, 3, 0]", numbers.ToString());
        }
        [Test]
        public void Test_InsertAtMiddle()
        {
            // arrange
            numbers = new Collection<int>(1, 2, 3);
            // act
            numbers.InsertAt(1, 0);
            // assert
            Assert.AreEqual("[1, 0, 2, 3]", numbers.ToString());
        }
        [Test]
        public void Test_InsertAtWithGrow()
        {
            // arrange
            numbers = new Collection<int>(1, 2, 3, 4, 5, 6, 7, 8);
            // act
            for (int i = 0; i < 9; i++)
            {
                numbers.InsertAt(5, i);
            }
            // assert
            Assert.AreEqual(32, numbers.Capacity); // the capacity should double its initial size
        }
        [TestCase(new int[0], -1)]
        [TestCase(new int[0], 1)]
        [TestCase(new[] { 1, 2}, -1)]
        [TestCase(new[] { 1, 2 }, 3)]
        public void Test_InsertAtInvalidIndex(int[] arr, int index) 
        {
            // arrange
            numbers = new Collection<int>(arr);
            // act
            // assert
            Assert.Throws<ArgumentOutOfRangeException>(() => numbers.InsertAt(index, 33));
        }
        [Test]
        public void Test_ExchangeMiddle()
        {
            // arrange
            numbers = new Collection<int>(1, 2, 3, 4);
            // act
            numbers.Exchange(1, 2);
            // assert
            Assert.AreEqual("[1, 3, 2, 4]", numbers.ToString());
        }
        [Test]
        public void Test_ExchangeFirstLast()
        {
            // arrange
            numbers = new Collection<int>(1, 2, 3, 4);
            // act
            numbers.Exchange(0, 3);
            // assert
            Assert.AreEqual("[4, 2, 3, 1]", numbers.ToString());
        }
        [TestCase(-1, 2)]
        [TestCase(3, 4)]
        public void Test_ExchangeInvalidIndexes(int index1, int index2)
        {
            // arrange
            numbers = new Collection<int>(1, 2, 3, 4);
            // act
            // assert
            Assert.Throws<ArgumentOutOfRangeException>(() => numbers.Exchange(index1, index2));
        }
        [Test]
        public void Test_RemoveAtStart()
        {
            // arrange
            numbers = new Collection<int>(1, 2, 3);
            // act
            numbers.RemoveAt(0);
            // assert
            Assert.AreEqual("[2, 3]", numbers.ToString());
        }
        [Test]
        public void Test_RemoveAtEnd()
        {
            // arrange
            numbers = new Collection<int>(1, 2, 3);
            // act
            numbers.RemoveAt(2);
            // assert
            Assert.AreEqual("[1, 2]", numbers.ToString());
        }
        [Test]
        public void Test_RemoveAtMiddle()
        {
            // arrange
            numbers = new Collection<int>(1, 2, 3);
            // act
            numbers.RemoveAt(1);
            // assert
            Assert.AreEqual("[1, 3]", numbers.ToString());
        }
        [TestCase(new int[0], 0)]
        [TestCase(new int[] { 1, 2 }, -1)]
        [TestCase(new int[] { 1, 2 }, 2)]   
        public void Test_RemoveAtInvalidIndex(int[] arr, int index)
        {
            // arrange
            numbers = new Collection<int>(arr);
            // act
            // assert
            Assert.Throws<ArgumentOutOfRangeException>(() => numbers.RemoveAt(index));
        }
        [Test]
        public void Test_RemoveAll()
        {
            // arrange
            var numbers = new Collection<int>(1, 2, 3);
            // act
            for (int i = numbers.Count - 1; i >= 0; i--)
            {
                numbers.RemoveAt(i);
            }
            // assert
            Assert.AreEqual("[]", numbers.ToString());
        }
        [Test]
        public void Test_Clear()
        {
            // arrange
            numbers = new Collection<int>(1, 2, 3);
            // act
            numbers.Clear();
            // assert
            Assert.AreEqual("[]", numbers.ToString());
            Assert.Throws<ArgumentOutOfRangeException>(() => { int num = numbers[0]; });
        }
        [Test]
        public void Test_ToStringEmpty()
        {
            // arrange
            // act
            // assert
            Assert.AreEqual("[]", numbers.ToString());
        }
        [Test]
        public void Test_ToStringSingle()
        {
            // arrange
            numbers = new Collection<int>(1);
            // act
            // assert
            Assert.AreEqual("[1]", numbers.ToString());
        }
        [Test]
        public void Test_ToStringMultiple()
        {
            // arrange
            numbers = new Collection<int>(1, 2, 3);
            // act
            // assert
            Assert.AreEqual("[1, 2, 3]", numbers.ToString());
        }
        [Test]
        public void Test_ToStringNestedCollections()
        {
            // arrange
            numbers = new Collection<int>(10, 20);
            var names = new Collection<string>("Teddy", "Gerry");
            var dates = new Collection<DateTime>();
            var nested = new Collection<object>(names, numbers, dates);
            // act
            string nestedToString = nested.ToString();
            // assert
            Assert.AreEqual("[[Teddy, Gerry], [10, 20], []]", nestedToString);
        }

    }
}