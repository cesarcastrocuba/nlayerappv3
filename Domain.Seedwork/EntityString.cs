namespace NLayerApp.Domain.Seedwork
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Base class for entities
    /// </summary>
    public abstract class EntityString : Entity, IEntity<string>
    {
        private int? _requestedHashCode;

        public string Id { get; set; }

        public bool IsTransient()
        {
            return Id == null;
        }

        #region Overrides Methods
      
        public override bool Equals(object obj)
        {
            var entity = obj as EntityString;
            if (entity == null)
                return false;

            if (Object.ReferenceEquals(this, obj))
                return true;

            if (GetRealObjectType(this) != GetRealObjectType(obj))
                return false;

            if (IsTransient())
                return false;

            return entity.Id == Id;

        }
                
        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!_requestedHashCode.HasValue)
                    _requestedHashCode = Id.GetHashCode() ^ 31; // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)

                return _requestedHashCode.Value;
            }
            else
                return base.GetHashCode();
        }

        public static bool operator ==(EntityString left, EntityString right)
        {
            if (Object.Equals(left, null))
                return (Object.Equals(right, null)) ? true : false;
            else
                return left.Equals(right);
        }

        public static bool operator !=(EntityString left, EntityString right)
        {
            return !(left == right);
        }

        #endregion

        private Type GetRealObjectType(object obj)
        {
            var retVal = obj.GetType();
            //because can be compared two object with same id and 'types' but one of it is EF dynamic proxy type)
            if (retVal.GetTypeInfo().BaseType != null && retVal.Namespace == "System.Data.Entity.DynamicProxies")
            {
                retVal = retVal.GetTypeInfo().BaseType;
            }
            return retVal;
        }
    }
}
