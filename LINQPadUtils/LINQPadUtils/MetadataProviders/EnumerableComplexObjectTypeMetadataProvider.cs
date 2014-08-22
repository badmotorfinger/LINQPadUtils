namespace LINQPadUtils.MetadataProviders
{
    using System;
    using System.Reflection;

    class EnumerableComplexObjectTypeMetadataProvider : TypeMetadataProviderBase
    {
        public EnumerableComplexObjectTypeMetadataProvider(object obj, Type elementType)
            : base(obj)
        {
            if (elementType.BaseType != null)
            {
                this.Properties = elementType.GetProperties();
            }
            else
            {
                this.Properties = new PropertyInfo[0];
            }
        }

        public override bool IsEnumerableOfKnownType
        {
            get
            {
                return true;
            }
        }

        public override sealed MemberInfo[] Properties { get; protected set; }
    }
}