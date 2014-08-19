namespace LINQPadUtils
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Web;

    using LINQPad;

    public static class ValueInspector
    {
        static readonly Lazy<Type> RawHtmlType = new Lazy<Type>(() => Assembly.GetAssembly(typeof(ExecutionEngine)).GetType("LINQPad.ObjectGraph.RawHtml"));

        static Func<object, PropertyInfo[]> objectHeadingsFunc = GetHeadingsForType();

        /// <summary>
        /// Gets the display value by performing a ToString() operation.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>A string representation of the object.</returns>
        public static string GetDisplayValue(object obj)
        {
            return GetDisplayValue(obj, obj.GetType());
        }

        /// <summary>
        /// Gets the display value by performing a ToString() operation.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="objType">The object type.</param>
        /// <returns>A string representation of the object.</returns>
        public static string GetDisplayValue(object obj, Type objType)
        {
            string displayValue;
            if (RawHtmlType.Value == objType)
            {
                var fi = RawHtmlType.Value.GetField("Html");

                displayValue = fi.GetValue(obj).ToString();
            }
            else
            {
                displayValue = obj.ToString();
            }

            return HttpUtility.HtmlEncode(displayValue);
        }

        public static bool IsPrimitiveObject(object obj)
        {
            var objType = obj.GetType();

            return IsPrimitiveType(objType);
        }

        static bool IsPrimitiveType(Type objType)
        {
            return
                objType == typeof(string) || objType.IsPrimitive || objType == typeof(DateTime);
        }

        public static bool IsPrimitiveEnumerable(object obj)
        {
            var objType = obj.GetType();

            if (obj is IEnumerable)
            {
                if (objType.IsArray)
                {
                    var elementType = objType.GetElementType();
                    return IsPrimitiveType(elementType);
                }

                if (objType.IsGenericType)
                {
                    var kvpType = objType.GetInterfaces().FirstOrDefault(i => i.Name.Contains("IEnumerable`"));

                    if (kvpType != null)
                    {
                        var elementType = kvpType.GetGenericArguments()[0]; // IEnumerable only has one generic type parameter.
                        return IsPrimitiveType(elementType);
                    }
                }
            }

            return false;
        }

        public static bool IsObjectBasedEnumerable(object obj)
        {
            var objType = obj.GetType();

            if (obj is Hashtable)
            {
                return true;
            }

            if (obj is IEnumerable)
            {
                if (objType.IsArray)
                {
                    var elementType = objType.GetElementType();
                    return elementType.BaseType == null;
                }

                if (objType.IsGenericType)
                {
                    var kvpType = objType.GetInterfaces().FirstOrDefault(i => i.Name.Contains("IEnumerable`"));

                    if (kvpType != null)
                    {
                        var elementType = kvpType.GetGenericArguments()[0]; // IEnumerable only has one generic type parameter.
                        return elementType.BaseType == null;
                    }
                }
            }

            return false;
        }

        public static bool IsStaticTypeEnumerable(object obj)
        {
            Type elementType;
            return IsStaticTypeEnumerable(obj, out elementType);
        }

        public static bool IsStaticTypeEnumerable(object obj, out Type elementType)
        {
            elementType = null;

            var objType = obj.GetType();

            if (obj is IEnumerable)
            {
                if (objType.IsArray)
                {
                    elementType = objType.GetElementType();

                    return elementType.BaseType != null;
                }

                if (objType.IsGenericType)
                {
                    var kvpType = objType.GetInterfaces().FirstOrDefault(i => i.Name.Contains("IEnumerable`"));

                    if (kvpType != null)
                    {
                        elementType = kvpType.GetGenericArguments()[0]; // IEnumerable only has one generic type parameter.
                        return elementType.BaseType != null;
                    }
                }
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