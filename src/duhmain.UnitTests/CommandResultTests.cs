using NUnit.Framework;
using System;

namespace duhmain.UnitTests.Core
{
    [TestFixture]
    public class CommandResultTests
    {

        [TestCase]
        public void Records_Valid_Result()
        {
            const int expectedResult = 100;

            var subject = new CommandResult<int>(expectedResult);
            Assert.AreEqual(expectedResult, subject.Result);
        }

        [TestCase]
        public void Records_Exception()
        {
            Exception expectedException = new Exception("This is a test");
            var subject = new CommandResult<int>(expectedException);
            Assert.AreEqual(expectedException, subject.Exception);
            Assert.AreEqual(expectedException.Message, subject.Exception.Message);
        }

        [TestCase]
        public void Returns_not_CompletedSucessfully_on_Exception()
        {
            Exception expectedException = new Exception("This is a test");
            var subject = new CommandResult<int>(expectedException);
            Assert.IsFalse(subject.CompletedSucessfully);
        }

        [TestCase]
        public void Returns_CompletedSucessfully_on_valid_result()
        {
            const int expectedResult = 100;

            var subject = new CommandResult<int>(expectedResult);
            Assert.IsTrue(subject.CompletedSucessfully);
        }

        [TestCase]
        [ExpectedException(ExpectedException = typeof(InvalidOperationException))]
        public void Throws_if_access_result_on_Exception()
        {
            Exception expectedException = new Exception("This is a test");
            var subject = new CommandResult<int>(expectedException);

            // Attempt to access should throw exception
            var actual = subject.Result;

        }

    }
}
