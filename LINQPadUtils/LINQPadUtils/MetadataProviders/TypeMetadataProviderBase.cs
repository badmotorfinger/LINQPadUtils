namespace LINQPadUtils.MetadataProviders
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public abstract class TypeMetadataProviderBase
    {
        protected TypeMetadataProviderBase(object obj)
            : this(obj, obj.GetType()) { }

        protected TypeMetadataProviderBase(object obj, Type objType)
        {
            this.SourceObject = obj;
            this.SourceObjectType = objType;
        }

        public static TypeMetadataProviderBase GetMetadataProvider(object obj)
        {
            Type elementType;

            if (ValueInspector.IsPrimitiveObject(obj, out elementType))
            {
                return new PrimitiveTypeMetadataProvider(obj, elementType)
                {
                    IsPrimitiveElement = true
                };
            }

            if (ValueInspector.IsPrimitiveEnumerable(obj, out elementType))
            {
                return new EnumerablePrimitiveTypeMetadataProvider(obj)
                       {
                           IsPrimitiveElement = true,
                           IsEnumerable = true
                       };
            }

            if (ValueInspector.IsEnumerableOfKnownType(obj, out elementType))
            {
                return new EnumerableComplexObjectTypeMetadataProvider(obj, elementType)
                {
                    IsEnumerable = true
                };
            }

            if (ValueInspector.IsObjectBasedEnumerable(obj))
            {
                return new EnumerableObjectTypeMetadataProvider(obj)
                {
                    IsEnumerable = true
                };
            }

            return new ComplexTypeMetadataProvider(obj);
        }

        public Object SourceObject { get; private set; }

        public bool IsPrimitiveElement { get; protected set; }

        public bool IsEnumerable { get; protected set; }

        public abstract bool IsEnumerableOfKnownType { get; }

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