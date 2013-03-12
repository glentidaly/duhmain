using NUnit.Framework;
using System;

namespace duhmain.UnitTests.Core
{
    [TestFixture]
    public class CommandTests
    {

        [Test]
        public void State_is_NotExecuted_before_executing()
        {
            var subject = GetCommand<object>();
            Assert.AreEqual(CommandState.NotExecuted, subject.State);
        }

        [Test]
        public void State_is_CompletedSucessfully_after_executing()
        {
            var subject = GetCommand<object>(resultToReturn: new object());
            var cxt = GetContext();
            subject.Execute(cxt);
            Assert.AreEqual(CommandState.CompletedSuccessfully, subject.State);
        }

        [Test]
        public void State_is_CompletedWithError_after_executing()
        {
            var subject = GetCommand<object>(exceptionToThrow:new Exception());
            try
            {
                var cxt = GetContext();
                subject.Execute(cxt);
            }
            catch { }
            Assert.AreEqual(CommandState.CompletedWithError, subject.State);
        }

        [Test]
        public void Return_correct_result()
        {
            const string expected = "This is a test";
            var subject = GetCommand<string>(resultToReturn: expected);
            var cxt = GetContext();
            string actual = subject.Execute(cxt) as string;

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expected, subject.Result.Result);
        }

        [Test]
        [ExpectedException(ExpectedException=typeof(Exception))]
        public void Throw_on_Execute_Fail()
        {
            var subject = GetCommand<object>(exceptionToThrow: new Exception());
            var cxt = GetContext();
            subject.Execute(cxt);
            
        }

       



        #region helpers

        private Context GetContext()
        {
            var repo = new ServiceRepository();
            var locator = repo.GetServiceLocator();
            var context = new Context(locator);
            return context;
        }

        protected class TestCommand<TResult> : Command<TResult>
        {
            public Exception ExceptionToThrow { get; set; }
            public TResult ResultToReturn { get; set; }
            public bool WasExecuted { get; private set; }

            protected override TResult OnExecute(IContext context)
            {
                if (null != ExceptionToThrow)
                {
                    throw ExceptionToThrow;
                }
                else
                {
                    
                    return ResultToReturn;
                }
            }
        }

        protected virtual ICommand GetCommand<TResult>(TResult resultToReturn = default(TResult), Exception exceptionToThrow = null )
        {
            var cmd = new TestCommand<TResult>();
            cmd.ResultToReturn = resultToReturn;
            cmd.ExceptionToThrow = exceptionToThrow;
            return cmd;
        }

        #endregion

    }
}
