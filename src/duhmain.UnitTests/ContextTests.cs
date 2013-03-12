using NUnit.Framework;
using System;

namespace duhmain.UnitTests.Core
{
    [TestFixture]
    public class ExecutionContextTests
    {
        [Test]
        [ExpectedException(exceptionType: typeof(NullReferenceException))]
        public void Throws_If_No_ServiceLocator_Passed()
        {
            var subject = new Context(null);
        }


    }
}
