namespace LINQPadUtils.MetadataProviders
{
    using System.Reflection;

    class EnumerablePrimitiveTypeMetadataProvider : TypeMetadataProviderBase
    {
        public EnumerablePrimitiveTypeMetadataProvider(object obj)
            : base(obj)
        {
            this.Properties = new PropertyInfo[0];
        }

        public override bool IsEnumerableOfKnownType
        {
            get
            {
                return false;
            }
        }

        public override sealed PropertyInfo[] Properties { get; protected set; }
    }
}