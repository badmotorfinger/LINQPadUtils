namespace LINQPadUtils.MetadataProviders
{
    using System;
    using System.Reflection;

    class EnumerableComplexObjectTypeMetadataProvider : TypeMetadataProviderBase
    {
        public EnumerableComplexObjectTypeMetadataProvider(object obj, Type elementType)
            : base(obj)
        {
            this.Properties = elementType.GetProperties();
        }

        public override bool IsEnumerableObject
        {
            get
            {
                return false;
            }
        }

        public override bool IsEnumerableStaticType
        {
            get
            {
                return true;
            }
        }

        public override sealed PropertyInfo[] Properties { get; protected set; }
    }
}