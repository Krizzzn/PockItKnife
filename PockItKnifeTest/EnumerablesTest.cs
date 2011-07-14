using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using FluentAssertions;

namespace PockItKnifeTest
{
    
    
    /// <summary>
    ///This is a test class for PockItKnifeTest and is intended
    ///to contain all PockItKnifeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class EnumerablesTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        [TestMethod]
        public void ForEach_HandleNull()
        {
            //ARRANGE
            IEnumerable<bool> b = null;

            //ACT
            b.ForEach((en) => { });

            //ASSERT
            Assert.IsTrue(true);
        }

        [TestMethod]
        [ExpectedException (typeof(ArgumentNullException))]
        public void ForEach_ThrowsIfActionIsNull()
        {
            //ARRANGE
            var arr = new[] { "a", "b" };

            //ACT
            arr.ForEach(null);
            
            //ASSERT
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void ForEach_PerformsActionOnEveryElemet()
        {
            //ARRANGE
            var arr = new[] { "a", "b" };
            var assert = 0;

            //ACT
            arr.ForEach((a) => { assert += (int)a.ToCharArray()[0]; });
            
            //ASSERT
            Assert.AreEqual(97 + 98, assert);
        }

        [TestMethod]
        [ExpectedException (typeof(ArgumentNullException))]
        public void ForEach_ThrowIfPredicateEmpty()
        {
            //ARRANGE
            var arr = new[] { "a" };

            //ACT
            arr.ForEach(null, (s) => { });

            //ASSERT
            Assert.IsTrue(true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ForEach_ThrowIfActionIsNullAndPredicateIsNot()
        {
            //ARRANGE
            var arr = new[] { "a" };

            //ACT
            arr.ForEach((s) => true, null);

            //ASSERT
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void ForEach_PerfomActionOnlyOnPredicateMatches()
        {
            //ARRANGE
            var assert = 0;
            var arr = new[] { 1, 2, 3, 4, 5, 6};

            //ACT
            arr.ForEach((i) => i >= 4, (i) => assert += i );

            //ASSERT
            Assert.AreEqual(15, assert);
        }

        [TestMethod]
        public void ForEach_DoesNotInterfereWithDefaultForEachOnList()
        {
            //ARRANGE
            var assert = 0;
            var l = new List<int>();
            l.Add(3);
            l.Add(3);
            l.Add(3);

            //ACT
            l.ForEach((i) => assert += i);

            //ASSERT
            Assert.AreEqual(9, assert);
        }

        [TestMethod]
        public void Humanize__returns_humanizer()
        {
            //ARRANGE
            string[] hum = new[] { "a", "b" }; 

            //ACT
            var result = hum.Humanize().ConcatWithAnd();

            //ASSERT
            result.Should().Match("a and b");
        }

        [TestMethod]
        public void Humanize__handle_null_or_empty()
        {
            //ARRANGE
            string[] hum1 = null;
            string[] hum2 = new string[]{};

            //ACT
            var result1 = hum1.Humanize().ConcatWithAnd("nope");
            var result2 = hum2.Humanize().ConcatWithAnd("nope");

            //ASSERT
            result1.Should().Match("nope");
            result2.Should().Match("nope");
        }
    }
}
