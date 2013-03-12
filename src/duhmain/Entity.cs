using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace duhmain
{
    /// <summary>
    /// Represents an object that has an identity that is unique within a given context.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// The unique identifier for this entity.
        /// </summary>
        /// <remarks>If this is null or empty, it likely means that the entity state is new and hasn't been committed to its owning repository.</remarks>
        string Id { get; }
    }


    /// <summary>
    /// Base implementation of an entity.
    /// </summary>
    public abstract class Entity : IEntity
    {
        #region constructore

        public Entity()
        {
        }

        public Entity(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("id", "The entity id cannot be null or empty.");
            }
            this.Id = id;
        }

        #endregion


        /// <summary>
        /// The unique identifier for this entity.
        /// </summary>
        /// <remarks>If this is null or empty, it likely means that the entity state 
        /// is new and hasn't been committed to its owning repository.</remarks>
        public string Id { get; private set; }

    }
}
