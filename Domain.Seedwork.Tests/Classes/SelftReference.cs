namespace NLayerApp.Domain.Seedwork.Tests.Classes
{
    class SelfReference
        : ValueObject<SelfReference>
    {
        public SelfReference()
        {
        }
        public SelfReference(SelfReference value)
        {
            Value = value;
        }
        public SelfReference Value { get; set; }
    }
}
