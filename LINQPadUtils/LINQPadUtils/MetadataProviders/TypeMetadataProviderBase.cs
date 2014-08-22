namespace LINQPadUtils.MetadataProviders
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public abstract class TypeMetadataProviderBase
    {
        readonly Lazy<int> enumerableCount;

        protected TypeMetadataProviderBase(object obj)
            : this(obj, obj.GetType())
        {
        }

        protected TypeMetadataProviderBase(object obj, Type objType)
        {
            this.SourceObject = obj;
            this.SourceObjectType = objType;

            enumerableCount = new Lazy<int>(
                () =>
                {
                    int count = 0;

                    if (this.SourceObject is ICollection)
                    {
                        count = ((ICollection)this.SourceObject).Count;
                    }
                    else
                    {
                        var enumerator = ((IEnumerable)this.SourceObject).GetEnumerator();

                        while (enumerator.MoveNext())
                        {
                            count++;
                        }
                    }

                    return count;
                });
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

        public bool IsPrimitiveElement { get; private set; }

        public bool IsEnumerable { get; private set; }

        public abstract bool IsEnumerableOfKnownType { get; }

        public int Count {
            get
            {
                return enumerableCount.Value;
            }
        }

        public bool IsAnonymousType
        {
            get
            {
                return this.SourceObjectType.Name.Contains("AnonymousType")
                       && this.SourceObjectType.CustomAttributes.Any(
                           attr => attr.AttributeType == typeof(CompilerGeneratedAttribute));
            }
        }

        public Type SourceObjectType { get; private set; }

        public abstract MemberInfo[] Properties { get; protected set; }
    }
}