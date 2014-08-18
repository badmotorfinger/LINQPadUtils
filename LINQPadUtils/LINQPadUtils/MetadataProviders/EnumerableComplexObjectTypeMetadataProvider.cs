namespace LINQPadUtils.MetadataProviders
{
    using System;
    using System.Reflection;

    class EnumerableComplexObjectTypeMetadataProvider : TypeMetadataProviderBase
    {
        public EnumerableComplexObjectTypeMetadataProvider(object obj)
            : base(obj)
        {
            Type elementType;

            this.Properties = ValueInspector.IsStaticTypeEnumerable(obj, out elementType) 
                ? elementType.GetProperties() 
                : new PropertyInfo[0];
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