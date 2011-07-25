using PockItKnife;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using FluentAssertions;

namespace PockItKnifeTest
{
    
    
    /// <summary>
    ///This is a test class for CryptTest and is intended
    ///to contain all CryptTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CryptTest
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
        public void ctor_CanHandleNullValues()
        {
            //ARRANGE

            //ACT
            var c = new Crypt(null);
            var d = new Crypt("");

            //ASSERT
            Assert.IsTrue(true);
        }

        [TestMethod]
        [ExpectedException (typeof(ArgumentNullException))]
        public void En_ThrowsErrorCryptoSeedNull()
        {
            //ARRANGE

            //ACT
            var c = new Crypt("blabla");
            c.En(null);
                
            //ASSERT
            Assert.IsTrue(true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void En_ThrowsErrorCryptoSeedLessThan8Characters()
        {
            //ARRANGE

            //ACT
            var c = new Crypt("blabla");

            try
            {
                c.En("1234567");
            }
            catch (ArgumentException)
            {
                c.En("12");
            }

            //ASSERT
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void En_CanHandleNullOrEmpty()
        {
            //ARRANGE

            //ACT
            var n = new Crypt(null).En("passwort1");
            var e = new Crypt("").En("passwort1");

            //ASSERT
            Assert.IsNull(n);
            Assert.AreEqual("", e);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void De_ThrowsErrorCryptoSeedNull()
        {
            //ARRANGE

            //ACT
            var c = new Crypt("blabla");
            c.De(null);

            //ASSERT
            Assert.IsTrue(true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void De_ThrowsErrorCryptoSeedLessThan8Characters()
        {
            //ARRANGE

            //ACT
            var c = new Crypt("blabla");

            try
            {
                c.De("1234567");
            }
            catch (ArgumentException)
            {
                c.De("12");
            }

            //ASSERT
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void De_CanHandleNullOrEmpty()
        {
            //ARRANGE

            //ACT
            var n = new Crypt(null).De("passwort1");
            var e = new Crypt("").De("passwort1");

            //ASSERT
            Assert.IsNull(n);
            Assert.AreEqual("", e);
        }

        [TestMethod]
        public void En_StrangifiesInput()
        {
            //ARRANGE

            //ACT
            var c = new Crypt("blabla");
            var result = c.En("thepassword");
            
            //ASSERT
            Assert.AreNotEqual("blabla", result);
        }

        [TestMethod]
        public void De_StrangifiesInput()
        {
            //ARRANGE

            //ACT
            var c = new Crypt("3sKchzyYTVDb0KeroTKuVQ==");
            var result = c.De("thepassword");

            //ASSERT
            Assert.AreEqual("blabla", result);
        }

        [TestMethod]
        public void DeEn_AreInvertible()
        {
            //ARRANGE

            //ACT            
            var enc = new Crypt("mebefooo");
            var encrypted = enc.En("thepassword");

            var dec = new Crypt(encrypted);
            var decrypted = dec.De("thepassword");

            //ASSERT
            Assert.AreEqual("mebefooo", decrypted);
        }

        [TestMethod]
        public void De__wont_fail_if_source_is_not_encrypted()
        {
            //ARRANGE
            string unencrypted = "this is fooooooo";

            //ACT
            var dec = new Crypt(unencrypted);
            var result = dec.De("somepassword");

            //ASSERT
            result.Should().Be(unencrypted);
        }
    }
}
