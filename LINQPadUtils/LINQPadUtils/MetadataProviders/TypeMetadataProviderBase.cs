namespace LINQPadUtils.MetadataProviders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public abstract class TypeMetadataProviderBase
    {
        protected TypeMetadataProviderBase(object obj)
        {
            this.SourceObject = obj;
            this.SourceObjectType = obj.GetType();
        }

        public static TypeMetadataProviderBase GetMetadataProvider(object obj)
        {
            if (ValueInspector.IsPrimitiveObject(obj))
            {
                return new PrimitiveTypeMetadataProvider(obj);
            }
            
            if (obj is System.Collections.IEnumerable)
            {
                if (ValueInspector.IsPrimitiveEnumerable(obj))
                {
                    return new EnumerablePrimitiveTypeMetadataProvider(obj);
                }

                if (ValueInspector.IsObjectBasedEnumerable(obj))
                {
                    return new EnumerableObjectTypeMetadataProvider(obj);
                }

                if (ValueInspector.IsStaticTypeEnumerable(obj))
                {
                    return new EnumerableComplexObjectTypeMetadataProvider(obj);
                }

                throw new ApplicationException("Could not determine enumerable type.");
            }

            return new ComplexTypeMetadataProvider(obj);
        }
        
        public Object SourceObject { get; private set; }

        public bool IsPrimitiveElement
        {
            get
            {
                if (IsEnumerable && ValueInspector.IsPrimitiveEnumerable(SourceObject))
                {
                    return true;
                }

                return ValueInspector.IsPrimitiveObject(this.SourceObject);
            }
        }

        public bool IsEnumerable
        {
            get
            {
                return SourceObject is System.Collections.IEnumerable;
            }
        }

        public abstract bool IsEnumerableObject { get; }

        public abstract  bool IsEnumerableStaticType { get; }

        public bool IsAnonymousType
        {
            get
            {
                return SourceObjectType.Name.Contains("AnonymousType")
                       && SourceObjectType.CustomAttributes.Any(
                           attr => attr.AttributeType == typeof(CompilerGeneratedAttribute));
            }
        }

        public Type SourceObjectType { get; private set; }

        public abstract PropertyInfo[] Properties { get; protected set; }
    }
}