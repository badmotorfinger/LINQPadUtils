namespace LINQPadUtils.MetadataProviders
{
    using System.Reflection;

    class EnumerableObjectTypeMetadataProvider : TypeMetadataProviderBase
    {
        public EnumerableObjectTypeMetadataProvider(object obj)
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

        public override sealed MemberInfo[] Properties { get; protected set; }
    }
}