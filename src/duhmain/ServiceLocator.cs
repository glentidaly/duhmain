using System;
using System.Collections.Generic;

namespace duhmain
{
    /// <summary>
    /// Represents the ability to register and deregister services and get a locator for accessing the services
    /// </summary>
    public interface IServiceRepository : IDisposable
    {
        /// <summary>
        /// Registers a service that can be lazily created
        /// </summary>
        /// <typeparam name="TService">The type of service to register</typeparam>
        /// <param name="lazyService">The lazy init reference to a service</param>
        /// <param name="name">An optional name for the service. If null, it will represent the default instance.</param>
        /// <remarks>The service will be disposed (if it implements <see cref="System.IDisposible"/>) as it was lazily created.</remarks>
        ///  <exception cref="System.InvalidOperationException">Thrown if the service was already registered with that name.</exception>
        void RegisterService<TService>(Lazy<TService> lazyService, string name = null);

        /// <summary>
        /// Registers an already existing instance of a service
        /// </summary>
        /// <typeparam name="TService">The type of service to register</typeparam>
        /// <param name="instance">The instance to register</param>
        /// <param name="name">An optional name for the service. If null, it will represent the default instance.</param>
        /// <param name="dispose">True if the service should be disposed if it implements <see cref="System.IDisposible"/>. 
        /// Defaults to false as it is expected the creator will dispose of the service.</param>
        /// <exception cref="System.InvalidOperationException">Thrown if the service was already registered with that name.</exception>
        void RegisterService<TService>(TService instance, string name = null, bool dispose = false);


        /// <summary>
        /// Gets the locator that locates the services in this repository.
        /// </summary>
        IServiceLocator GetServiceLocator();

    }

    /// <summary>
    /// Represents a class that's responsible for holding onto services that will be used during 
    /// and execution context
    /// </summary>
    public interface IServiceLocator
    {
        
        /// <summary>
        /// Gets a service instance.
        /// </summary>
        /// <typeparam name="TService">The type of service to get.</typeparam>
        /// <param name="name">The optional name of the service. If null, a default instance will be returned if one exists.</param>
        /// <returns>The instance of the service</returns>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException>">Thrown if the service cannot be located.</exception>
        TService GetService<TService>(string name = null);

    }

    /// <summary>
    /// Implements the service locator pattern for registering and getting services.
    /// </summary>
    public class ServiceRepository : IServiceRepository
    {
        #region private classes

        /// <summary>
        /// Container for storing references to services
        /// </summary>
        private struct ServiceContainer
        {
            public ServiceContainer(object service, bool shouldDispose)
            {
                this.Service = service;
                this.ShouldDispose = shouldDispose;
            }

            public readonly object Service;
            public readonly bool ShouldDispose;
        }

        /// <summary>
        /// Contains the object that acts as a service locator.
        /// </summary>
        private class Locator : IServiceLocator
        {
            private ServiceRepository _repository;

            internal Locator(ServiceRepository repository)
            {
                _repository = repository;
            }


            /// <summary>
            /// Gets a service instance.
            /// </summary>
            /// <typeparam name="TService">The type of service to get.</typeparam>
            /// <param name="name">The optional name of the service. If null, a default instance will be returned if one exists.</param>
            /// <returns>The instance of the service</returns>
            /// <exception cref="System.Collections.Generic.KeyNotFoundException>">Thrown if the service cannot be located.</exception>
            public TService GetService<TService>(string name = null)
            {
                string key = _repository.GetServiceKey<TService>(name);

                lock (_repository._lockObj)
                {
                    ServiceContainer container;
                    bool instanceExists = _repository._services.TryGetValue(key, out container);
                    if (instanceExists)
                    {
                        TService service = (TService)container.Service;
                        return service;
                    }
                    else
                    {
                        // Instance doesn't exist, so try to get it from the lazy collection and add it to the instance collection.
                        Lazy<TService> lazy = (Lazy<TService>)_repository._lazyServices[key];
                        TService instance = lazy.Value;

                        ServiceContainer newContainer = new ServiceContainer(instance, shouldDispose: true); // We lazily created, so we should dispose
                        _repository._services.Add(key, newContainer);
                        _repository._lazyServices.Remove(key);

                        return instance;

                    }

                }
            }

        }

        #endregion

        #region fields

        private readonly object _lockObj = new object(); // Thread safety.

        private readonly Dictionary<string, ServiceContainer> _services =
            new Dictionary<string, ServiceContainer>();

        private readonly Dictionary<string, object> _lazyServices =
            new Dictionary<string, object>();

        #endregion

        /// <summary>
        /// Registers a service that can be lazily created
        /// </summary>
        /// <typeparam name="TService">The type of service to register</typeparam>
        /// <param name="lazyService">The lazy init reference to a service</param>
        /// <param name="name">An optional name for the service. If null, it will represent the default instance.</param>
        /// <remarks>The service will be disposed (if it implements <see cref="System.IDisposible"/>) as it was lazily created.</remarks>
        ///  <exception cref="System.InvalidOperationException">Thrown if the service was already registered with that name.</exception>
        public void RegisterService<TService>(Lazy<TService> lazyService, string name = null)
        {
            string key = GetServiceKey<TService>(name);

            lock (_lockObj)
            {
                // Validate no instance service has been added
                if (_services.ContainsKey(key))
                {
                    string message = string.Format("A service with key '{0}' has already been registered as an instance. Cannot register a lazy instance too.", key);
                    throw new InvalidOperationException(message);
                }

                try
                {
                    _lazyServices.Add(key, lazyService);
                }
                catch (System.ArgumentException ex)
                {
                    string message = string.Format("A service with key '{0}' has already been registered.", key);
                    throw new InvalidOperationException(message, ex);
                }
            }
        }

        /// <summary>
        /// Registers an already existing instance of a service
        /// </summary>
        /// <typeparam name="TService">The type of service to register</typeparam>
        /// <param name="instance">The instance to register</param>
        /// <param name="name">An optional name for the service. If null, it will represent the default instance.</param>
        /// <param name="dispose">True if the service should be disposed if it implements <see cref="System.IDisposible"/>. 
        /// Defaults to false as it is expected the creator will dispose of the service.</param>
        /// <exception cref="System.InvalidOperationException">Thrown if the service was already registered with that name.</exception>
        public void RegisterService<TService>(TService instance, string name = null, bool dispose = false)
        {
            string key = GetServiceKey<TService>(name);
            ServiceContainer container = new ServiceContainer(instance, dispose);

            lock (_lockObj)
            {
                // Validate no instance service has been added
                if (_services.ContainsKey(key))
                {
                    string message = string.Format("A service with key '{0}' has already been registered as a lazy created service. Cannot register an instance too.", key);
                    throw new InvalidOperationException(message);
                }

                try
                {
                    _services.Add(key, container);
                }
                catch (System.ArgumentException ex)
                {
                    string message = string.Format("A service with key '{0}' has already been registered.", key);
                    throw new InvalidOperationException(message, ex);
                }
            }
        }


        /// <summary>
        /// Gets the locator that locates the services in this repository.
        /// </summary>
        public IServiceLocator GetServiceLocator()
        {
            var locator = new Locator(this);
            return locator;
        }


        /// <summary>
        /// Called to dispose of the class
        /// </summary>
        public void Dispose()
        {
            foreach (var serviceEntry in _services.Values)
            {
                if (serviceEntry.ShouldDispose)
                {
                    var disposable = serviceEntry.Service as IDisposable;
                    if (null != disposable)
                    {
                        disposable.Dispose();
                    }
                }
            }
        }

        #region helper methods

        /// <summary>
        /// Gets a key for the service type and optional name
        /// </summary>
        private string GetServiceKey<TService>(string name = null)
        {
            const string format = "{0}|{1}";
            string key = string.Format(format, typeof(TService).AssemblyQualifiedName, name);
            return key;
        }

        #endregion





    }
}
