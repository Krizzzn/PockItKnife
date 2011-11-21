using System;

using System.Reflection;
using FluentAssertions;
using NUnit.Framework;
using System.Data;
using System.Web.Script.Serialization;


namespace PockItKnifeTest
{


    /// <summary>
    ///This is a test class for OtherExtensionsTest and is intended
    ///to contain all OtherExtensionsTest Unit Tests
    ///</summary>
    [TestFixture()]
    public class OtherExtensionsTest
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
        [ExpectedException(typeof(System.IO.FileNotFoundException))]
        public void LoadEmbeddedFile__throws_file_not_found_exception()
        {
            //ARRANGE

            //ACT
            var s = Assembly.GetExecutingAssembly().LoadEmbeddedFile("thisfiledoesnotexist");

            //ASSERT
        }

        [Test]
        public void LoadEmbeddedFile__reads_file_contents()
        {
            //ARRANGE

            //ACT
            var s = Assembly.GetExecutingAssembly().LoadEmbeddedFile("HELP");

            //ASSERT
            s.Should().Contain("Help!!");
        }

        [Test]
        public void LoadEmbeddedFile__reads_file_contents_with_extension()
        {
            //ARRANGE

            //ACT
            var s = Assembly.GetExecutingAssembly().LoadEmbeddedFile("HELP.txt");

            //ASSERT
            s.Should().Contain("Help!!");
        }

        [Test]
        public void LoadEmbeddedFile__reads_file_contents_with_differnt_encodings()
        {
            //ARRANGE

            //ACT
            var s1 = Assembly.GetExecutingAssembly().LoadEmbeddedFile("HELP.txt", System.Text.UTF8Encoding.ASCII);
            var s2 = Assembly.GetExecutingAssembly().LoadEmbeddedFile("HELP.txt", System.Text.UTF8Encoding.UTF32);

            //ASSERT
            s1.Should().NotMatch(s2);
        }

        [Test]
        public void Jsonize__jsonizes_datatable()
        {
            // ARRANGE
            var dt = new DataTable();
            var serializer = new JavaScriptSerializer();

            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("thefoo", typeof(string));

            for (int i = 0; i < 5; i++) {
                var row = dt.NewRow();
                row[0] = i;
                row[1] = "bort {0}".Inject(i);
                dt.Rows.Add(row);
            }

            //ACT
            var jsonString = dt.Jsonize();
            var result = serializer.DeserializeObject(jsonString) as object[];

            //ASSERT
            result.Should().NotBeNull();
            result.Should().HaveCount(5);
        }

        [Test]
        public void Jsonize__jsonizes_datacolumn()
        {
            // ARRANGE
            var dt = new DataTable();
            var serializer = new JavaScriptSerializer();

            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("thefoo", typeof(string));

            for (int i = 0; i < 5; i++) {
                var row = dt.NewRow();
                row[0] = i;
                row[1] = "bort {0}".Inject(i);
                dt.Rows.Add(row);
            }

            //ACT
            var jsonString = dt.Rows[3].Jsonize();
            var result = serializer.DeserializeObject(jsonString) as object;

            //ASSERT
            result.Should().NotBeNull();
        }
    }
}
