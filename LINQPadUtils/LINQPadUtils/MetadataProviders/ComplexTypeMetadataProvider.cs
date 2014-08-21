namespace LINQPadUtils.MetadataProviders
{
    using System.Reflection;

    class ComplexTypeMetadataProvider : TypeMetadataProviderBase
    {
        public ComplexTypeMetadataProvider(object obj)
            : base(obj)
        {
            this.Properties = base.SourceObjectType.GetProperties();
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