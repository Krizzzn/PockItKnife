﻿using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PockItKnife;

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
            string nullString = null;
            var c = new Crypt_Accessor(nullString);
            var d = new Crypt_Accessor("");

            //ASSERT
            Assert.IsTrue(true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void En_ThrowsErrorCryptoSeedNull()
        {
            //ARRANGE

            //ACT
            var c = new Crypt_Accessor("blabla");
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
            var c = new Crypt_Accessor("blabla");

            try {
                c.En("1234567");
            }
            catch (ArgumentException) {
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
            string nullString = null;
            var n = new Crypt_Accessor(nullString).En("passwort1");
            var e = new Crypt_Accessor("").En("passwort1");

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
            var c = new Crypt_Accessor("blabla");
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
            var c = new Crypt_Accessor("blabla");

            try {
                c.De("1234567");
            }
            catch (ArgumentException) {
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
            string nullString = null;

            var n = new Crypt_Accessor(nullString).De("passwort1");
            var e = new Crypt_Accessor("").De("passwort1");

            //ASSERT
            Assert.IsNull(n);
            Assert.AreEqual("", e);
        }

        [TestMethod]
        public void En_StrangifiesInput()
        {
            //ARRANGE

            //ACT
            var c = new Crypt_Accessor("blabla");
            var result = c.En("thepassword");

            //ASSERT
            Assert.AreNotEqual("blabla", result);
        }

        [TestMethod]
        public void De_StrangifiesInput()
        {
            //ARRANGE

            //ACT
            var c = new Crypt_Accessor("3sKchzyYTVDb0KeroTKuVQ==");
            var result = c.De("thepassword");

            //ASSERT
            Assert.AreEqual("blabla", result);
        }

        [TestMethod]
        public void DeEn_AreInvertible()
        {
            //ARRANGE

            //ACT            
            var enc = new Crypt_Accessor("mebefooo");
            var encrypted = enc.En("thepassword");

            var dec = new Crypt_Accessor(encrypted);
            var decrypted = dec.De("thepassword");

            //ASSERT
            Assert.AreEqual("mebefooo", decrypted);
        }

        [TestMethod]
        public void De__wont_fail_if_source_is_not_encrypted_using_try_catch_for_invalid_length_exception()
        {
            //ARRANGE
            string unencrypted1 = "this is fooooooo";

            //ACT
            var dec = new Crypt_Accessor(unencrypted1);
            var result1 = dec.De("somepassword");

            //ASSERT
            result1.Should().Be(unencrypted1);
        }

        [TestMethod]
        public void De__wont_fail_if_source_is_not_encrypted_using_try_catch_for_invalid_character_exception()
        {
            //ARRANGE
            string unencrypted1 = "blabla!";

            //ACT
            var dec = new Crypt_Accessor(unencrypted1);
            var result1 = dec.De("somepassword");

            //ASSERT
            result1.Should().Be(unencrypted1);
        }

        [TestMethod]
        public void de__wont_fail_if_source_is_not_encrypted_using_try_catch_cryptographicexception()
        {
            //ARRANGE
            string unencrypted1 = "buhtial5";

            //ACT
            var dec = new Crypt_Accessor(unencrypted1);
            var result1 = dec.De("somepassword");

            //ASSERT
            result1.Should().Be(unencrypted1);
        }
    }
}
