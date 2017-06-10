namespace NLayerApp.Domain.Seedwork
{
    using System;
    public abstract class AuditableEntity : Entity, IAuditableEntity
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedAt { get; set; }
    }
}
