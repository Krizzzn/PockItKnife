using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
namespace PockItKnifeTest
{
    
    
    /// <summary>
    ///This is a test class for PockItKnifeTest and is intended
    ///to contain all PockItKnifeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class StringTest
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
        public void Inject_CanHandleNull()
        {
            //ARRANGE
            string a = null;

            //ACT
            var result = a.Inject(true, 6, 8);

            //ASSERT
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Inject_CanHandleNoArguments()
        {
            //ARRANGE
            string a = "bla bla";

            //ACT
            var result = a.Inject();

            //ASSERT
            Assert.AreEqual("bla bla", result);                        
        }

        [TestMethod]
        public void Inject_AppliesFormatString()
        {
            //ARRANGE
            string a = "bla {0} {2}{1}";

            //ACT
            var result = a.Inject("bla", "!", "blubb");

            //ASSERT
            Assert.AreEqual("bla bla blubb!", result);                                    
        }

        [TestMethod]
        public void Crypt_CanHandleNullValue()
        {
            //ARRANGE
            string handle = null;

            //ACT
            var result = handle.Crypt();

            //ASSERT
            Assert.IsNotNull(result);

        }

        [TestMethod]
        public void Crypt_CanHandleEmptyValue()
        {
            //ARRANGE
            string handle = "";

            //ACT
            var result = handle.Crypt();

            //ASSERT
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void IsNullOrEmpty__behaves_like_original()
        {
            //ARRANGE

            //ACT
            Assert.AreEqual(string.IsNullOrEmpty(""), "".IsNullOrEmpty());
            Assert.AreEqual(string.IsNullOrEmpty(default(string)), (default(string)).IsNullOrEmpty());
            Assert.AreEqual(string.IsNullOrEmpty("b"), "b".IsNullOrEmpty());
            Assert.AreEqual(string.IsNullOrEmpty("basdasdasdasd a"), "basdasdasdasd a".IsNullOrEmpty());

            //ASSERT
            
        }

        [TestMethod]
        public void ParseCommandLineArguments_routes_commandline_arguments()
        {
            //ARRANGE
            var cmd = new[] { "a", "b", "c" };
            
            //ACT
            var result = cmd.ParseCommandlineArguments();

            //ASSERT
            result["c"].Should().Be("true");
            result["a"].Should().Be("b");
        }

        [TestMethod]
        public void Limit__wont_fail_on_null_or_empty()
        {
            //ARRANGE
            string target = null;

            //ACT
            var result1 = target.Limit(12, "");
            target = "";
            var result2 = target.Limit(12, "");

            //ASSERT
            result1.Should().BeNull();
            result2.Should().BeBlank();
        }

        [TestMethod]
        public void Limit__wont_fail_on_wrong_parameters()
        {
            //ARRANGE
            string test = "abc";

            //ACT
            var result1 = test.Limit(-1, "");
            var result2 = test.Limit(0, "...");
            var result3 = test.Limit(1, null);

            //ASSERT
            result1.Should().BeEquivalentTo("");
            result2.Should().BeEquivalentTo("");
            result3.Should().BeEquivalentTo("a");
        }

        [TestMethod]
        public void Limit__cuts_text()
        {
            //ARRANGE
            string test = "abcdefgehijk";
            
            //ACT
            var result1 = test.Limit(5, "...");
            var result2 = test.Limit(5, ".");

            //ASSERT
            result1.Should().BeEquivalentTo("ab...");
            result2.Should().BeEquivalentTo("abcd.");
        }

        [TestMethod]
        public void Limit__does_not_cut_text()
        {
            //ARRANGE
            string test = "abcdef";

            //ACT
            var result1 = test.Limit(6, "...");
            var result2 = test.Limit(2000, "...");

            //ASSERT
            result1.Should().BeEquivalentTo(test);
            result2.Should().BeEquivalentTo(test);
        }

        [TestMethod]
        public void Limit__trims_whitespaces()
        {
            //ARRANGE
            string subject = "foo   O_o";

            //ACT
            var result = subject.Limit(7);

            //ASSERT
            result.Should().BeEquivalentTo("foo...");
        }

        [TestMethod]
        public void Limit__use_overload()
        {
            //ARRANGE
            var test = "123456789";

            //ACT

            //ASSERT
            test.Limit(9).Should().Be(test.Limit(9, "..."));
            test.Limit(3).Should().Be(test.Limit(3, "..."));
        }
    }
}
