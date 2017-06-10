namespace NLayerApp.Domain.Seedwork
{
    using System;
    using System.Reflection;
    /// <summary>
    /// Base class for entities
    /// </summary>
    public abstract partial class EntityInt : Entity, IEntity<int>
    {
        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        public int Id { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as EntityInt);
        }

        private static bool IsTransient(EntityInt obj)
        {
            return obj != null && Equals(obj.Id, default(int));
        }

        private Type GetUnproxiedType()
        {
            return GetType();
        }

        public virtual bool Equals(EntityInt other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (!IsTransient(this) &&
                !IsTransient(other) &&
                Equals(Id, other.Id))
            {
                var otherType = other.GetUnproxiedType();
                var thisType = GetUnproxiedType();
                return thisType.IsAssignableFrom(otherType) ||
                        otherType.IsAssignableFrom(thisType);
            }

            return false;
        }

        public override int GetHashCode()
        {
            if (Equals(Id, default(int)))
                return base.GetHashCode();
            return Id.GetHashCode();
        }

        public static bool operator ==(EntityInt x, EntityInt y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(EntityInt x, EntityInt y)
        {
            return !(x == y);
        }
    }
}
