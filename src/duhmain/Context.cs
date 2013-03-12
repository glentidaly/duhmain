using System;

namespace duhmain
{
    /// <summary>
    /// Represents the details of currently executing information and provides access to a service locator.
    /// </summary>
    public interface IContext
    {

        /// <summary>
        /// Enables callers to get references to service instances
        /// </summary>
        IServiceLocator ServiceLocator { get; }

    }

    /// <summary>
    /// Represents the details of currently executing information and provides access to a service locator.
    /// </summary>
    public class Context : IContext 
    {

        public Context(IServiceLocator serviceLocator)
        {
            if (null == serviceLocator)
            {
                throw new NullReferenceException("serviceLocator");
            }
            _serviceLocator = serviceLocator;
        }

        private readonly IServiceLocator _serviceLocator;

        /// <summary>
        /// A helper to locate external dependencies.
        /// </summary>
        public IServiceLocator ServiceLocator
        {
            get
            {
                return _serviceLocator;
            }
        }

       
    }
}
