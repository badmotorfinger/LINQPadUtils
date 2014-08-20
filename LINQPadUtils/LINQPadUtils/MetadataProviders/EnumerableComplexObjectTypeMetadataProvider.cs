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