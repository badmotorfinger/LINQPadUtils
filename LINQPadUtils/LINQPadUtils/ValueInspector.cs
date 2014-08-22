namespace LINQPadUtils
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;

    public static class ValueInspector
    {
        static Func<object, PropertyInfo[]> objectHeadingsFunc = GetHeadingsForType();        

        public static bool IsPrimitiveObject(object obj, out Type elementType)
        {
            elementType = null;

            if (obj == null)
            {
                return true;
            }

            elementType = obj.GetType();

            return IsPrimitiveType(elementType);
        }

        static bool IsPrimitiveType(Type objType)
        {
            return
                objType == typeof(string) || objType.IsPrimitive || objType == typeof(DateTime);
        }

        public static bool IsPrimitiveEnumerable(object obj, out Type elementType)
        {
            elementType = null;

            var objType = obj.GetType();

            if (obj is IEnumerable)
            {
                if (objType.IsArray)
                {
                    elementType = objType.GetElementType();
                    return IsPrimitiveType(elementType);
                }

                if (objType.IsGenericType)
                {
                    var kvpType = objType.GetInterfaces().FirstOrDefault(i => i.Name.Contains("IEnumerable`"));

                    if (kvpType != null)
                    {
                        elementType = kvpType.GetGenericArguments()[0];
                            // IEnumerable only has one generic type parameter.
                        return IsPrimitiveType(elementType);
                    }
                }
            }

            return false;
        }

        public static bool IsObjectBasedEnumerable(object obj)
        {
            var objType = obj.GetType();

            if (obj is IEnumerable)
            {
                if (objType.IsArray)
                {
                    var elementType = objType.GetElementType();
                    return elementType.BaseType == null;
                }

                // It could be a generic collection of System.Objects.
                if (objType.IsGenericType)
                {
                    var kvpType = objType.GetInterfaces().FirstOrDefault(i => i.Name.Contains("IEnumerable`"));

                    if (kvpType != null)
                    {
                        // IEnumerable only has one generic type parameter.
                        var elementType = kvpType.GetGenericArguments()[0];
                        return elementType.BaseType == null;
                    }
                }
                return true;
            }

            return false;
        }

        public static bool IsEnumerableOfKnownType(object obj, out Type elementType)
        {
            elementType = null;

            var objType = obj.GetType();

            if (obj is IEnumerable)
            {
                if (objType.IsArray)
                {
                    var elementTypeTmp = objType.GetElementType();

                    if (elementTypeTmp.BaseType != null)
                    {
                        elementType = elementTypeTmp;
                    }
                }
                else if (objType.IsGenericType)
                {
                    var genericTypeArgument =
                        objType.GetInterfaces().FirstOrDefault(i => i.Name.Contains("IEnumerable`"));

                    Debug.Assert(
                        genericTypeArgument != null,
                        "A generic type argument for a generic IEnumerable was not found.");

                    // IEnumerable only has one generic type parameter.
                    var t = genericTypeArgument.GetGenericArguments()[0];

                    if (t.BaseType != null)
                    {
                        elementType = t;
                    }
                }
                else if (obj is IDictionary)
                {
                    elementType = typeof(DictionaryEntry);
                }

                return elementType != null;
            }

            return false;
        }

        static Func<object, PropertyInfo[]> GetHeadingsForType()
        {
            var typeInfo = new Dictionary<Type, PropertyInfo[]>();

            return source =>
            {
                var sourceType = source.GetType();

                PropertyInfo[] headings;

                if (typeInfo.TryGetValue(sourceType, out headings))
                {
                    return headings;
                }

                if (IsPrimitiveType(sourceType))
                {
                    return typeInfo[sourceType] = new PropertyInfo[0];
                }

                return typeInfo[sourceType] = sourceType.GetProperties().ToArray();
            };
        }
    }
}