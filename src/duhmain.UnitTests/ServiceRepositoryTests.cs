using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;

namespace duhmain.UnitTests.Core
{
    [TestFixture]
    public class ServiceRepositoryTests
    {

        [Test]
        public void Store_and_Retrieve_Default_Service()
        {
            using (var subject = new ServiceRepository())
            {
                var expected = new object();

                subject.RegisterService(expected);

                var actual = subject.GetServiceLocator().GetService<object>();
                Assert.AreSame(expected, actual);
            }
        }

        [Test]
        public void Store_and_Retrieve_Named_Service()
        {
            const string name = "test service";
            using (var subject = new ServiceRepository())
            {
                var expected = new object();

                subject.RegisterService(expected, name);

                var actual = subject.GetServiceLocator().GetService<object>(name);
                Assert.AreSame(expected, actual);
            }
        }

        [Test]
        public void Store_both_named_and_default_instances()
        {
            const string name = "test service";
            using (var subject = new ServiceRepository())
            {
                var expectedNamed = new object();
                var expectedDefault = new object();

                subject.RegisterService(expectedDefault);
                subject.RegisterService(expectedNamed, name);

                var actualDefault = subject.GetServiceLocator().GetService<object>();
                Assert.AreSame(expectedDefault, actualDefault);

                var actualNamed = subject.GetServiceLocator().GetService<object>(name);
                Assert.AreSame(expectedNamed, actualNamed);
            }
        }

        [Test]
        public void Lazily_create_singleton_service()
        {
            int createCount = 0;
            object expected = new object();
            Lazy<object> lazy = new Lazy<object>(() =>
                {
                    Interlocked.Increment(ref createCount);
                    return expected;
                });

            Assert.AreEqual(0, createCount);

            using (var subject = new ServiceRepository())
            {
                subject.RegisterService<object>(lazy);

                Assert.AreEqual(0, createCount);

                var actual = subject.GetServiceLocator().GetService<object>();
                Assert.AreSame(expected, actual);
                Assert.AreEqual(1, createCount);
            }

        }



        [Test]
        [ExpectedException(ExpectedException = typeof(InvalidOperationException))]
        public void Throw_if_default_service_registered_twice()
        {
            using (var subject = new ServiceRepository())
            {
                var expected = new object();

                subject.RegisterService(expected);
                subject.RegisterService(expected);

            }
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(InvalidOperationException))]
        public void Throw_if_default_lazy_service_registered_twice()
        {
            using (var subject = new ServiceRepository())
            {
                var expected = new Lazy<object>();

                subject.RegisterService<object>(expected);
                subject.RegisterService<object>(expected);

            }
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(InvalidOperationException))]
        public void Throw_if_named_lazy_service_registered_twice()
        {
            const string name = "test";
            using (var subject = new ServiceRepository())
            {
                var expected = new Lazy<object>();

                subject.RegisterService<object>(expected, name);
                subject.RegisterService<object>(expected, name);

            }
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(InvalidOperationException))]
        public void Throw_if_lazy_and_instance_default_service_registerd_twice()
        {
            using (var subject = new ServiceRepository())
            {
                var expected = new object();
                var expectedLazy = new Lazy<object>();

                subject.RegisterService(expected);
                subject.RegisterService<Object>(expectedLazy);

            }
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(InvalidOperationException))]
        public void Throw_if_lazy_and_instance_named_service_registerd_twice()
        {
            const string name = "test";
            using (var subject = new ServiceRepository())
            {
                var expected = new object();
                var expectedLazy = new Lazy<object>();

                subject.RegisterService(expected, name);
                subject.RegisterService(expectedLazy, name);

            }
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(InvalidOperationException))]
        public void Throw_if_named_service_registered_twice()
        {
            const string name = "test";
            using (var subject = new ServiceRepository())
            {
                var expected = new object();

                subject.RegisterService(expected, name);
                subject.RegisterService(expected, name);

            };
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(KeyNotFoundException))]
        public void Throw_if_default_service_not_found()
        {
            using (var subject = new ServiceRepository())
            {
                var invalid = subject.GetServiceLocator().GetService<object>();
            };
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(KeyNotFoundException))]
        public void Throw_if_named_service_not_found()
        {
            const string name = "Test Service";
            using (var subject = new ServiceRepository())
            {
                var invalid = subject.GetServiceLocator().GetService<object>(name);
            };
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(KeyNotFoundException))]
        public void Throw_if_named_service_not_found_but_default_exists()
        {
            const string name = "Test Service";
            using (var subject = new ServiceRepository())
            {
                var expected = new object();
                subject.RegisterService(expected);
                var invalid = subject.GetServiceLocator().GetService<object>(name);
            };
        }

        [Test]
        public void Dispose_lazily_created_service()
        {
            TestService svc = new TestService();


            Lazy<object> lazy = new Lazy<object>(() =>
            {
                return svc;
            });



            using (var subject = new ServiceRepository())
            {
                subject.RegisterService<object>(lazy);

                Assert.IsFalse(svc.IsDisposed);

                var actual = subject.GetServiceLocator().GetService<object>();
                Assert.AreSame(svc, actual);
                Assert.IsFalse(svc.IsDisposed);
            }

            Assert.IsTrue(svc.IsDisposed);

        }

        [Test]
        public void Dont_dispose_instance_if_told_not_to()
        {
            TestService svc = new TestService();

            using (var subject = new ServiceRepository())
            {
                subject.RegisterService<object>(svc, dispose: false);

                Assert.IsFalse(svc.IsDisposed);

                var actual = subject.GetServiceLocator().GetService<object>();
                Assert.AreSame(svc, actual);
                Assert.IsFalse(svc.IsDisposed);
            }

            Assert.IsFalse(svc.IsDisposed);
        }

        [Test]
        public void Dispose_instance_if_told_to()
        {
            TestService svc = new TestService();

            using (var subject = new ServiceRepository())
            {
                subject.RegisterService<object>(svc, dispose: true);

                Assert.IsFalse(svc.IsDisposed);

                var actual = subject.GetServiceLocator().GetService<object>();
                Assert.AreSame(svc, actual);
                Assert.IsFalse(svc.IsDisposed);
            }

            Assert.IsTrue(svc.IsDisposed);
        }


        #region helpers

        private class TestService : IDisposable
        {



            public bool IsDisposed { get; set; }


            public void Dispose()
            {
                IsDisposed = true;
            }
        }


        #endregion
    }
}
