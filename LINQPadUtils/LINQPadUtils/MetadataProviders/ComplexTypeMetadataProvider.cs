namespace LINQPadUtils.MetadataProviders
{
    using System.Linq;
    using System.Reflection;
    using System.Windows.Markup;

    class ComplexTypeMetadataProvider : TypeMetadataProviderBase
    {
        public ComplexTypeMetadataProvider(object obj)
            : base(obj)
        {
            MemberInfo[] props = base.SourceObjectType.GetProperties();
            MemberInfo[] fields = base.SourceObjectType.GetFields();

            this.Properties = fields.Concat(props).ToArray();
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