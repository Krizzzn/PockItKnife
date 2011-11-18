using PockItKnife.Humanizers;

using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace PockItKnifeTest
{
    
    
    /// <summary>
    ///This is a test class for HumanizeEnumerablesTest and is intended
    ///to contain all HumanizeEnumerablesTest Unit Tests
    ///</summary>
    [TestFixture()]
    public class HumanizeEnumerablesTest
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


        [Test]
        public void ctor__sets_subject()
        {
            //ARRANGE
            var enumr = new[] { "a", "ab" };

            //ACT
            var x = new HumanizeEnumerables_Accessor<string>(enumr);

            //ASSERT
            x.Subject.Should().BeEquivalentTo(enumr);
        }

        [Test]
        public void ctor__does_not_fail_on_null()
        {
            //ARRANGE
            IEnumerable<bool> enumr = null;

            //ACT
            var x = new HumanizeEnumerables_Accessor<bool>(enumr);

            //ASSERT
            x.Subject.Should().BeNull();
        }

        [Test]
        public void ConcatWithAnd__returns_concatenated_string()
        {
            //ARRANGE
            var enumr = new[] { "a", "ab", "aa" };

            //ACT
            var hmnze = new HumanizeEnumerables_Accessor<string>(enumr);
            var result = hmnze.ConcatWithAnd();

            //ASSERT
            result.Should().MatchEquivalentOf("a, ab and aa");
        }

        [Test]
        public void ConcatWithAnd__returns_coalesce_string()
        {
            //ARRANGE
            string[] enumr = null;

            //ACT
            var hmnze = new HumanizeEnumerables_Accessor<string>(enumr);
            var result = hmnze.ConcatWithAnd("nope");

            //ASSERT
            result.Should().MatchEquivalentOf("nope");
        }

        [Test]
        public void ConcatWithOr__returns_concatenated_string()
        {
            //ARRANGE
            var enumr = new[] { "a", "ab", "aa" };

            //ACT
            var hmnze = new HumanizeEnumerables_Accessor<string>(enumr);
            var result = hmnze.ConcatWithOr();

            //ASSERT
            result.Should().MatchEquivalentOf("a, ab or aa");
        }

        [Test]
        public void ConcatWithOr__returns_coalesce_string()
        {
            //ARRANGE
            string[] enumr = null;

            //ACT
            var hmnze = new HumanizeEnumerables_Accessor<string>(enumr);
            var result = hmnze.ConcatWithOr("nope");

            //ASSERT
            result.Should().MatchEquivalentOf("nope");
        }

        [Test]
        public void ConcatWith__does_not_fail_on_subject_null_or_empty()
        {
            //ARRANGE
            string[] enumr1 = null;
            string[] enumr2 = new string[]{};

            //ACT
            var hmnze1 = new HumanizeEnumerables_Accessor<string>(enumr1);
            var result1 = hmnze1.ConcatWith(" "," ");

            var hmnze2 = new HumanizeEnumerables_Accessor<string>(enumr1);
                var result2 = hmnze2.ConcatWith(" ", " ");

            //ASSERT
            result1.Should().NotBeNull();
            result2.Should().NotBeNull();
        }

        [Test]
        public void ConcatWith__uses_coalesce_to_replace_empty_strings()
        {
            //ARRANGE
            string[] enumr1 = null;
            string[] enumr2 = new string[]{};

            //ACT
            var hmnze1 = new HumanizeEnumerables_Accessor<string>(enumr1);
            var result1 = hmnze1.ConcatWith(" "," ", "n/a");

            var hmnze2 = new HumanizeEnumerables_Accessor<string>(enumr2);
                var result2 = hmnze2.ConcatWith(" ", " ", "nö");

            //ASSERT
            result1.Should().Match("n/a");
            result2.Should().Match("nö");
        }

        [Test]
        public void ConcatWith__filters_null_or_empty_values_and_trims_values()
        {
            //ARRANGE
            string[] enumr2 = new string[] { "            a", null, "           ", "       d               " };

            //ACT
            var hmnze2 = new HumanizeEnumerables_Accessor<string>(enumr2);
            var result2 = hmnze2.ConcatWith(" ", " <-> ", "nö");

            //ASSERT
            result2.Should().Match("a <-> d");
        }

        [Test]
        public void ConcatWith__works_on_different_scenarios()
        {
            //ARRANGE
            var enumr4 = new[] { "a", "ab", "aa", "cd" };
            var enumr3 = new[] { "a", "ab", "aa" };
            var enumr2 = new[] { "a", "ab" };
            var enumr1 = new[] { "a"};

            //ACT
            var hmnze1 = new HumanizeEnumerables_Accessor<string>(enumr1);
            var result1 = hmnze1.ConcatWith("/", "-->");

            var hmnze2 = new HumanizeEnumerables_Accessor<string>(enumr2);
            var result2 = hmnze2.ConcatWith("/", "-->");

            var hmnze3 = new HumanizeEnumerables_Accessor<string>(enumr3);
            var result3 = hmnze3.ConcatWith("/", "-->");

            var hmnze4 = new HumanizeEnumerables_Accessor<string>(enumr4);
            var result4 = hmnze4.ConcatWith("/", "-->");

            //ASSERT
            result1.Should().Match("a");
            result2.Should().Match("a-->ab");
            result3.Should().Match("a/ab-->aa");
            result4.Should().Match("a/ab/aa-->cd");
        }

        [Test]
        public void ConcatWith__handle_different_types()
        {
            //ARRANGE
            var enumr1 = new[] { 1, 2, 3};
            var enumr2 = new[] { true, false, true };
            var enumr3 = new[] { new object(), new object() };

            //ACT
            var hmnze1 = new HumanizeEnumerables_Accessor<int>(enumr1);
            var result1 = hmnze1.ConcatWith("/", "-->");

            var hmnze2 = new HumanizeEnumerables_Accessor<bool>(enumr2);
            var result2 = hmnze2.ConcatWith("/", "-->");

            var hmnze3 = new HumanizeEnumerables_Accessor<object>(enumr3);
            var result3 = hmnze3.ConcatWith("/", "-->");

            //ASSERT
            result1.Should().Match("1/2-->3");
            result2.Should().Match("True/False-->True");
            result3.Should().Match("System.Object-->System.Object");
        }
    }
}
