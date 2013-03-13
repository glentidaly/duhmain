using NUnit.Framework;
using System;

namespace duhmain.UnitTests.Core
{
    [TestFixture]
    public class QueryTests : ExecutableTests
    {

        #region helpers

        protected class TestQuery<TResult> : Query<TResult>
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

        protected override IExecutable<TResult> GetExecutable<TResult>(TResult resultToReturn = default(TResult), Exception exceptionToThrow = null )
        {
            var cmd = new TestQuery<TResult>();
            cmd.ResultToReturn = resultToReturn;
            cmd.ExceptionToThrow = exceptionToThrow;
            return cmd;
        }

        #endregion

    }
}
