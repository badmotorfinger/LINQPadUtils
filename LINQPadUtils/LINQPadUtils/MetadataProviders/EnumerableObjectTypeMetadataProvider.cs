namespace LINQPadUtils.MetadataProviders
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    class EnumerableObjectTypeMetadataProvider : TypeMetadataProviderBase
    {
        public EnumerableObjectTypeMetadataProvider(object obj)
            : base(obj)
        {
            if (obj is Hashtable)
            {
                this.Properties = new[] { base.SourceObjectType.GetProperty("Keys"), base.SourceObjectType.GetProperty("Values") };
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
                return true;
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