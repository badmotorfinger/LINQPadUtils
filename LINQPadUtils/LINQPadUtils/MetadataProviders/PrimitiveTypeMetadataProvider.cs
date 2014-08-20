namespace LINQPadUtils.MetadataProviders
{
    using System;
    using System.Reflection;

    class PrimitiveTypeMetadataProvider : TypeMetadataProviderBase
    {
        public PrimitiveTypeMetadataProvider(object obj, Type objType)
            : base(obj, objType)
        {
            this.Properties = new PropertyInfo[0];
        }

        public override bool IsEnumerableObject
        {
            get
            {
                return  false;
            }
        }

        public override bool IsEnumerableStaticType
        {
            get
            {
                return false;
            }
        }

        public override sealed PropertyInfo[] Properties { get; protected set; }
    }
}