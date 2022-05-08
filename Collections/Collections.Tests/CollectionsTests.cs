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

        [TestCase(new int[0])]
        [TestCase(new int[] { 1 })]
        [TestCase(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 })]
        public void Test_CountAndCapacity(int[] arr)
        {
            // arrange
            numbers = new Collection<int>(arr);
            int capacity = 16;
            // act
            // assert
            Assert.AreEqual(arr.Length, numbers.Count);
            if (arr.Length > 16)
            {
                capacity = arr.Length * 2;
            }
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
            int oldCapacity = numbers.Capacity; // 16
            // act
            for (int i = 0; i < 17; i++)
            {
                numbers.Add(i);
            }
            // assert
            Assert.That(numbers.Capacity, Is.GreaterThan(oldCapacity)); // the capacity should double its size
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
            int oldCapacity = numbers.Capacity;
            // act
            numbers.AddRange(arr);
            // assert
            Assert.That(numbers.Capacity == oldCapacity * 2);
        }
        [Test]
        public void Test_GetByIndex()
        {
            // Arrange
            var names = new Collection<string>("Peter", "Maria");
            // Act
            string name0 = names[0];
            string name1 = names[1];
            // Assert
            Assert.That(name0, Is.EqualTo("Peter"));
            Assert.That(name1, Is.EqualTo("Maria"));
        }
        [TestCase(-1)]
        [TestCase(2)]
        public void Test_GetByInvalidIndex(int x)
        {
            // arrange
            var names = new Collection<string>("Bob", "Joe");
            // act
            // assert
            Assert.That(() => { string name = names[x]; }, Throws.InstanceOf<ArgumentOutOfRangeException>());
            Assert.That(names.ToString(), Is.EqualTo("[Bob, Joe]"));
        }
        [Test]
        public void Test_SetByIndex()
        {
            // arrange
            numbers = new Collection<int>(0, 0);
            // act
            numbers[0] = 1;
            numbers[1] = 2;
            // assert
            Assert.AreEqual(1, numbers[0]);
            Assert.AreEqual(2, numbers[1]);
        }
        [TestCase(-1)]
        [TestCase(0)]
        public void Test_SetByInvalidIndex(int x)
        {
            // arrange
            // act
            // assert
            Assert.That(() => numbers[x] = 100, Throws.InstanceOf<ArgumentOutOfRangeException>());
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
            int oldCapacity = numbers.Capacity; // 16
            // act
            for (int i = 0; i < 9; i++)
            {
                numbers.InsertAt(5, i);
            }
            // assert
            Assert.That(numbers.Capacity, Is.GreaterThan(oldCapacity)); // the capacity should double its size
        }
        [TestCase(-1)]
        [TestCase(2)]
        public void Test_InsertAtInvalidIndex(int x) 
        {
            // arrange
            numbers = new Collection<int>(1);
            // act
            // assert
            Assert.That(() => numbers.InsertAt(x, 333), Throws.InstanceOf<ArgumentOutOfRangeException>());
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
        public void Test_ExchangeInvalidIndexes(int x, int y)
        {
            // arrange
            numbers = new Collection<int>(1, 2, 3, 4);
            // act
            // assert
            Assert.That(() => numbers.Exchange(x, y), Throws.InstanceOf<ArgumentOutOfRangeException>());
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
        [TestCase(-1)]
        [TestCase(3)]
        public void Test_RemoveAtInvalidIndex(int x)
        {
            // arrange
            numbers = new Collection<int>(1, 2, 3);
            // act
            // assert
            Assert.That(() => numbers.RemoveAt(x), Throws.InstanceOf<ArgumentOutOfRangeException>());
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
            Assert.That(() => numbers[0], Throws.InstanceOf<ArgumentOutOfRangeException>());
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
            Assert.That(nestedToString, Is.EqualTo("[[Teddy, Gerry], [10, 20], []]"));
        }

    }
}