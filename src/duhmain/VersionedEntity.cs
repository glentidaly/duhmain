
namespace duhmain
{
    /// <summary>
    /// Represents an entity that contains an opaque version identifier (like an etag)
    /// </summary>
    public interface IVersionedEntity : IEntity
    {
        /// <summary>
        /// An opaque identifier for the version of this entity that may be 
        /// used for optimistic concurrenty purposes.
        /// </summary>
        string Version { get; }

    }

    /// <summary>
    /// Base implementation of an entity that retains an opaque version identifier
    /// that may be used for optimistic concurrency purposes.
    /// </summary>
    public abstract class VersionedEntity : Entity, IVersionedEntity
    {
        #region constructors

        public VersionedEntity()
        {
        }

        public VersionedEntity(string id, string version = null)
            : base(id)
        {
            this.Version = version;
        }

        #endregion

        /// <summary>
        /// An opaque identifier for the version of this entity that may be 
        /// used for optimistic concurrenty purposes.
        /// </summary>
        public string Version { get; private set; }

    }

}
