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
            var subject = GetSubject(null);

        }

        [Test]
        [ExpectedException(ExpectedException = typeof(ArgumentNullException))]
        public void Throws_when_empty_id_passed()
        {
            var subject = GetSubject(string.Empty);

        }

        [Test]
        public void Returns_Set_Id()
        {
            string expected = "This is an expected value" + DateTime.Now.Ticks.ToString(); // Add random value at the end so test can't be easily faked.
            var subject = GetSubject(expected);

            Assert.AreEqual(expected, subject.Id);
        }

        [Test]
        public void Defaults_To_Null_Id()
        {
            var subject = GetSubject();
            Assert.IsNull(subject.Id);
        }



        #region test helpers

        protected virtual IEntity GetSubject()
        {
            return new TestEntity();
        }

        protected virtual IEntity GetSubject(string id)
        {
            return new TestEntity(id);
        }

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
