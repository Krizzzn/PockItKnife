using PockItKnife;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using FluentAssertions;
using System.Linq;

namespace PockItKnifeTest
{
    
    
    /// <summary>
    ///This is a test class for CommandlineArgumentsTest and is intended
    ///to contain all CommandlineArgumentsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CommandlineArgumentsTest
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
        public void ParseCommandLineArguments__does_not_fail_on_null_or_empty()
        {
            //ARRANGE
            string[] cmdArgs1 = null;
            var cmdArgs2 = new string[] {};
            
            //ACT
            var ca1 = CommandlineArguments.ParseCommandLineArguments(cmdArgs1);
            var ca2 = CommandlineArguments.ParseCommandLineArguments(cmdArgs2);

            //ASSERT
            ca1.Should().NotBeNull();
            ca2.Should().NotBeNull();
            ca1.Should().BeOfType<CommandlineArguments>();
            ca2.Should().BeOfType<CommandlineArguments>();
        }

        [TestMethod]
        public void Count__gets_count_of_arguments()
        {
            //ARRANGE
            var cmdArgs1 = new[] { "--aaa", "-be", "--cc:4" };
            var cmdArgs2 = new string[] { };
            string[] cmdArgs3 = null;


            //ACT
            var ca1 = CommandlineArguments.ParseCommandLineArguments(cmdArgs1);
            var ca2 = CommandlineArguments.ParseCommandLineArguments(cmdArgs2);
            var ca3 = CommandlineArguments.ParseCommandLineArguments(cmdArgs3);

            //ASSERT
            ca1.Count.Should().Be(3);
            ca2.Count.Should().Be(0);
            ca3.Count.Should().Be(0);
        }

        [TestMethod]
        public void Indexer__returns_null_for_not_existant()
        {
            //ARRANGE
            var cmdArgs = new string[] { };

            //ACT
            var ca = CommandlineArguments.ParseCommandLineArguments(cmdArgs);
            
            //ASSERT
            ca["notthere"].Should().BeNull();
        }

        [TestMethod]
        public void Indexer__returns_boolean_string_values_for_flags()
        {
            //ARRANGE
            var cmdArgs = new string[] { "-flag" };

            //ACT
            var ca = CommandlineArguments.ParseCommandLineArguments(cmdArgs);

            //ASSERT
            ca["flag"].Should().Be("true");
            Convert.ToBoolean(ca["flag"]).Should().BeTrue();
        }

        [TestMethod]
        public void Indexer__finds_results_for_different_param_styles()
        {
            //ARRANGE
            var cmdArgs = new string[] { "-flag1", "--flag2", "/flag3" };

            //ACT
            var ca = CommandlineArguments.ParseCommandLineArguments(cmdArgs);

            //ASSERT
            ca["flag1"].Should().Be("true");
            ca["flag2"].Should().Be("true");
            ca["flag3"].Should().Be("true");
        }

        [TestMethod]
        public void Indexer__findes_value_for_arguments_colon()
        {
            //ARRANGE
            var cmdArgs = new string[] { "-flag1:", "mebevalue", "-flag2:", "mebevalue", "-flag3:", "me be value"};

            //ACT
            var ca = CommandlineArguments.ParseCommandLineArguments(cmdArgs);
            //ASSERT

            ca["flag1"].Should().Be("mebevalue", "flag1");
            ca["flag2"].Should().Be("mebevalue", "flag2");
            ca["flag3"].Should().Be("me be value", "flag3");
        }

        [TestMethod]
        public void Indexer__finds_path()
        {
            //ARRANGE
            var cmdArgs = new string[] { @"-flag1:", @"C:\bla\bla\bla.txt" };

            //ACT
            var ca = CommandlineArguments.ParseCommandLineArguments(cmdArgs);
            //ASSERT

            ca["flag1"].Should().Be(@"C:\bla\bla\bla.txt");
        }

        [TestMethod]
        public void Indexer__finds_flags_without_any_delimiters()
        {
            //ARRANGE
            var cmdArgs = new string[] { @"flag1","bla", "flag2", "blubb", "true" };

            //ACT
            var ca = CommandlineArguments.ParseCommandLineArguments(cmdArgs);
            //ASSERT

            ca["flag1"].Should().Be(@"bla");
            ca["flag2"].Should().Be(@"blubb");
            ca["true"].Should().Be(@"true");
        }

        [TestMethod]
        public void Indexer__finds_with_equal()
        {
            //ARRANGE
            var cmdArgs = new string[] { @"flag1=","bla", "flag2=", " ", " ", "blubb" };

            //ACT
            var ca = CommandlineArguments.ParseCommandLineArguments(cmdArgs);
            //ASSERT

            ca["flag1"].Should().Be(@"bla");
            ca["flag2"].Should().Be(@"blubb");
        }


        [TestMethod]
        public void Indexer__should_be_case_insensitive()
        {
            //ARRANGE
            var cmdArgs = new string[] { @"flag1=", "bla" };

            //ACT
            var ca = CommandlineArguments.ParseCommandLineArguments(cmdArgs);
            //ASSERT

            ca["flAg1"].Should().Be(@"bla");
            ca["fLag1"].Should().Be(@"bla");
            ca["FLAG1"].Should().Be(@"bla");
        }

        [TestMethod]
        public void Indexer__test_ldap_path()
        {
            //ARRANGE
            var cmdArgs = new string[] { @"-flag1=", "ldap://blabla" };

            //ACT
            var ca = CommandlineArguments.ParseCommandLineArguments(cmdArgs);
            //ASSERT

            ca["flAg1"].Should().Be(@"ldap://blabla");
        }


        [TestMethod]
        public void All__gets_list_of_all_keys()
        {
            //ARRANGE
            var cmdArgs = new string[] { @"flag1=","bla", "flag2=","blubb", "/flag4", "/flag3", "/flag5" };

            //ACT
            var ca = CommandlineArguments.ParseCommandLineArguments(cmdArgs);
            var list = ca.All.ToList();

            //ASSERT
            list.Should().HaveCount(5);
            list[4].Key.Should().Be("flag5");
            list[1].Key.Should().Be("flag2");
            list[1].Value.Should().Be("blubb");
        }

        [TestMethod]
        public void All__does_not_fail_on_null()
        {
            //ARRANGE
            var cmdArgs = new string[] {};

            //ACT
            var ca = CommandlineArguments.ParseCommandLineArguments(cmdArgs);
            var list = ca.All.ToList();

            //ASSERT
            list.Should().HaveCount(0);
        }

        [TestMethod]
        [ExpectedException (typeof(ArgumentNullException))]
        public void AutomagicInit__throws_if_input_is_null()
        {
            //ARRANGE
            var cmdArgs = new string[] { @"parm1=", "bla", "param2=", "blubb", "param3=", "blibb" };
            
            //ACT
            var args = CommandlineArguments.ParseCommandLineArguments(cmdArgs);
            object x = null;
            args.AutomagicInit(x);

            //ASSERT
            true.Should().BeTrue();
        }

        [TestMethod]
        public void AutomagicInit__complete_automagic()
        {
            //ARRANGE
            var cmdArgs = new string[] { @"param1=", "bla", "param2=", "blubb", "param3=", "blibb" };

            //ACT
            var args = CommandlineArguments.ParseCommandLineArguments(cmdArgs);
            TestClass1 tc = new TestClass1();
            args.AutomagicInit(tc);

            //ASSERT
            tc.param1.Should().Be("bla");
            tc.param2.Should().Be("blubb");
            tc.param3.Should().Be("blibb");
        }

        [TestMethod]
        public void AutomagicInit__complete_automagic_can_handle_param_not_found()
        {
            //ARRANGE
            var cmdArgs = new string[] { @"param1=", "bla", "param2=", "blubb" };

            //ACT
            var args = CommandlineArguments.ParseCommandLineArguments(cmdArgs);
            TestClass1 tc = new TestClass1();
            args.AutomagicInit(tc);

            //ASSERT
            tc.param1.Should().Be("bla");
            tc.param2.Should().Be("blubb");
            tc.param3.Should().BeNull();
        }

        [TestMethod]
        public void AutomagicInit__can_handle_boolean()
        {
            //ARRANGE
            var cmdArgs = new string[] { @"param4" };

            //ACT
            var args = CommandlineArguments.ParseCommandLineArguments(cmdArgs);
            TestClass1 tc = new TestClass1();
            args.AutomagicInit(tc);

            //ASSERT
            tc.param4.Should().BeTrue();
        }

        [TestMethod]
        public void AutomagicInit__can_handle_numeric_values()
        {
            //ARRANGE
            var cmdArgs = new string[] { @"param5=", "1", "param6=", "1.1", "param7=", "1.2" };

            //ACT
            var args = CommandlineArguments.ParseCommandLineArguments(cmdArgs);
            TestClass1 tc = new TestClass1();
            args.AutomagicInit(tc);

            //ASSERT
            float f = 1.1F;
            double d = 1.2;

            tc.param5.Should().Equals(1);
            tc.param6.Should().BeGreaterOrEqualTo(f);
            tc.param7.Should().BeGreaterOrEqualTo(d);
        }

        [TestMethod]
        public void AutomagicInit__Loads_param_using_attribute()
        {
            //ARRANGE
            var cmdArgs = new string[] { @"foooo=", "yeah" };

            //ACT
            var args = CommandlineArguments.ParseCommandLineArguments(cmdArgs);
            TestClass1 tc = new TestClass1();
            args.AutomagicInit(tc);

            //ASSERT
            tc.param8.Should().Be("yeah");
        }

        [TestMethod]
        public void AutomagicInit__Loads_param_using_attribute_type_safe()
        {
            //ARRANGE
            var cmdArgs = new string[] { @"-boooo" };

            //ACT
            var args = CommandlineArguments.ParseCommandLineArguments(cmdArgs);
            TestClass1 tc = new TestClass1();
            args.AutomagicInit(tc);

            //ASSERT
            tc.param9.Should().BeTrue();
        }

        [TestMethod]
        public void PrintHelpfile__gets_default_helpfile_from_assemble_and_prints_it()
        {
            //ARRANGE
            var cmdArgs = new string[] { };
            string assert = "";

            //ACT
            var args = CommandlineArguments.ParseCommandLineArguments(cmdArgs);
            args.PrintHelpfile( (s) => assert = s);

            //ASSERT
            assert.Should().Contain("Help!!");
        }

        [TestMethod]
        public void PrintHelpfile__gets_specified_helpfile_from_assembly_and_prints_it()
        {
            //ARRANGE
            var cmdArgs = new string[] { };
            string assert = "";

            //ACT
            var args = CommandlineArguments.ParseCommandLineArguments(cmdArgs);
            args.PrintHelpfile("HELP.txt", (s) => assert = s);

            //ASSERT
            assert.Should().Contain("Help!!");
        }

        [TestMethod]
        public void PrintHelpfile__gets_specified_caseinsensitive_helpfile_from_assembly_and_prints_it()
        {
            //ARRANGE
            var cmdArgs = new string[] { };
            string assert = "";

            //ACT
            var args = CommandlineArguments.ParseCommandLineArguments(cmdArgs);
            args.PrintHelpfile("HelP.txt", (s) => assert = s);

            //ASSERT
            assert.Should().Contain("Help!!");
        }

        [TestMethod]
        [ExpectedException (typeof(System.IO.FileNotFoundException))]
        public void PrintHelpfile__throws_file_not_found_exception()
        {
            //ARRANGE
            var cmdArgs = new string[] { };
            string assert = "";

            //ACT
            var args = CommandlineArguments.ParseCommandLineArguments(cmdArgs);
            args.PrintHelpfile("He11lP.txt", (s) => assert = s);

            //ASSERT
            true.Should().BeTrue();
        }

        [TestMethod]
        public void PrintHelpfile__default_does_not_fail()
        {
            //ARRANGE
            var cmdArgs = new string[] { };

            //ACT
            var args = CommandlineArguments.ParseCommandLineArguments(cmdArgs);
            args.PrintHelpfile();

            //ASSERT
            true.Should().BeTrue();
        }

        [TestMethod]
        [ExpectedException(typeof(System.IO.FileNotFoundException))]
        public void PrintHelpfile__default_does_fail()
        {
            //ARRANGE
            var cmdArgs = new string[] { };

            //ACT
            var args = CommandlineArguments.ParseCommandLineArguments(cmdArgs);
            args.PrintHelpfile("YELP");

            //ASSERT
            true.Should().BeTrue();
        }

        [TestMethod]
        public void PrintHelpfile__default_does_not_fail_2()
        {
            //ARRANGE
            var cmdArgs = new string[] { };

            //ACT
            var args = CommandlineArguments.ParseCommandLineArguments(cmdArgs);
            args.PrintHelpfile("HELP");

            //ASSERT
            true.Should().BeTrue();
        }

        [TestMethod]
        public void PrintHelpfileIfRequested__prints_to_delegate_if_help_flag_is_set()
        {
            //ARRANGE
            var cmdArgs = new string[] { "/help" };
            string assert = null;

            //ACT
            var args = CommandlineArguments.ParseCommandLineArguments(cmdArgs);
            var result = args.PrintHelpfileIfRequested("HELP", (s) => assert = s );

            //ASSERT
            result.Should().BeTrue();
            assert.Should().Contain("Help!!");
        }

        [TestMethod]
        public void PrintHelpfileIfRequested__does_not_print_to_delegate_if_help_flag_is_not_set()
        {
            //ARRANGE
            var cmdArgs = new string[] { "/hulp" };
            string assert = null;

            //ACT
            var args = CommandlineArguments.ParseCommandLineArguments(cmdArgs);
            var result = args.PrintHelpfileIfRequested("HELP", (s) => assert = s);

            //ASSERT
            result.Should().BeFalse();
            assert.Should().BeNull();
        }

        [TestMethod]
        public void PrintHelpfileIfRequested__reacts_on_different_types_of_help_flags()
        {
            //ARRANGE
            var cmdArgs = new string[] { "/help", "-help", "--help", "-?", "--?", "/?", "help", "?" };
            int assert = 0;

            Action<string> act = (s) => {
                var args = new[] { s };
                var cmdArg = CommandlineArguments.ParseCommandLineArguments(cmdArgs);
                if (cmdArg.PrintHelpfileIfRequested((su) => assert++))
                    assert++;
            };

            //ACT
            cmdArgs.ForEach(act);

            //ASSERT
            assert.Should().Be(8 * 2);
        }

        [TestMethod]
        public void Contains__false_if_argument_not_given()
        {
            //ARRANGE
            var cmdArgs = new string[] { "/hulp" };

            //ACT
            var obj = cmdArgs.ParseCommandlineArguments();
            bool result = obj.Contains("help");

            //ASSERT
            result.Should().BeFalse();
        }

        [TestMethod]
        public void Contains__true_if_argument_not_given()
        {
            //ARRANGE
            var cmdArgs = new string[] { "/help" };

            //ACT
            var obj = cmdArgs.ParseCommandlineArguments();
            bool result = obj.Contains("help");

            //ASSERT
            result.Should().BeTrue();
        }

        public class TestClass1 {
            public string param1 { get; set; }
            public string param2 { get; set; }
            public string param3 { get; set; }

            public bool param4 { get; set; }

            public int param5 { get; set; }
            public float param6 { get; set; }
            public double param7 { get; set; }

            [CommandlineArguments.AutomagicLoad ("foooo")]
            public string param8 { get; set; }

            [CommandlineArguments.AutomagicLoad("boooo")]
            public bool param9 { get; set; }
        }
    }
}
