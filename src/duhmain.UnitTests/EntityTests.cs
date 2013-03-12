using NUnit.Framework;
using System;

namespace duhmain.UnitTests
{
    [TestFixture]
    public class EntityTests
    {
        [Test]
        [ExpectedException(ExpectedException=typeof(ArgumentNullException))]
        public void Throws_when_null_id_passed()
        {
            var subject = new TestEntity(null);

        }

        [Test]
        [ExpectedException(ExpectedException = typeof(ArgumentNullException))]
        public void Throws_when_empty_id_passed()
        {
            var subject = new TestEntity(String.Empty);

        }

        [Test]
        public void Returns_Set_Id()
        {
            string expected = "This is an expected value" + DateTime.Now.Ticks.ToString(); // Add random value at the end so test can't be easily faked.
            var subject = new TestEntity(expected);

            Assert.AreEqual(expected, subject.Id);
        }



        #region test helpers

        /// <summary>
        /// Mock entity
        /// </summary>
        private class TestEntity : Entity
        {
            public TestEntity()
                :base()
            {

            }

            public TestEntity(string id)
                :base(id)
            {

            }

        }

        #endregion

    }
}
