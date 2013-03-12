using NUnit.Framework;
using System;

namespace duhmain.UnitTests
{
    [TestFixture]
    public class VersionedEntityTests : EntityTests
    {
        [Test]
        public void Accepts_Null_Version()
        {
            var subject = GetVersionedSubject("testId", null);
            Assert.IsNull(subject.Version);
        }

        [Test]
        public void Accepts_Empty_Version()
        {
            var subject = GetVersionedSubject("testId", string.Empty);
            Assert.IsEmpty(subject.Version);
        }

        [Test]
        public void Defaults_to_Null_Version()
        {
            var subject = GetVersionedSubject("testId");
            Assert.IsNull(subject.Version);
        }

        [Test]
        public void Records_expected_version()
        {
            string expected = "this is an expected version" + DateTime.UtcNow.Ticks.ToString(); // Random to prevent fake test.
            var subject = GetVersionedSubject("testId", expected);
            Assert.AreEqual(expected, subject.Version);
        }


        #region helpers

        protected override IEntity GetSubject()
        {
            return GetVersionedSubject();
        }

        protected override IEntity GetSubject(string id)
        {
            return GetVersionedSubject(id);
        }

        protected virtual IVersionedEntity GetVersionedSubject()
        {
            return new TestVersionedEntity();
        }

        protected virtual IVersionedEntity GetVersionedSubject(string id)
        {
            return new TestVersionedEntity(id);
        }

        protected virtual IVersionedEntity GetVersionedSubject(string id, string version)
        {
            return new TestVersionedEntity(id, version);
        }



        private class TestVersionedEntity : VersionedEntity
        {
            public TestVersionedEntity()
            {

            }

            public TestVersionedEntity(string id, string version = null)
            : base(id,version)
            {

            }

        }

        #endregion

    }
}
