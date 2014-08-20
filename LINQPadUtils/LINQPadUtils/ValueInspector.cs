namespace LINQPadUtils
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using System.Windows.Media.Animation;

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
            if (obj == null)
            {
                return String.Empty;
            }

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
            if (obj == null)
            {
                return String.Empty;
            }

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

        public static bool IsPrimitiveObject(object obj, out Type elementType)
        {
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
                        elementType = kvpType.GetGenericArguments()[0]; // IEnumerable only has one generic type parameter.
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

        //public static bool IsStaticTypeEnumerable(object obj)
        //{
        //    Type elementType;
        //    return IsStaticTypeEnumerable(obj, out elementType);
        //}

        //public static bool IsStaticTypeEnumerable(object obj, out Type elementType)
        //{
        //    elementType = null;

        //    var objType = obj.GetType();

        //    if (obj is IEnumerable)
        //    {
        //        if (objType.IsArray)
        //        {
        //            elementType = objType.GetElementType();

        //            return elementType.BaseType != null;
        //        }

        //        if (objType.IsGenericType)
        //        {
        //            var kvpType = objType.GetInterfaces().FirstOrDefault(i => i.Name.Contains("IEnumerable`"));

        //            if (kvpType != null)
        //            {
        //                // IEnumerable only has one generic type parameter.
        //                elementType = kvpType.GetGenericArguments()[0]; 
        //                return elementType.BaseType != null;
        //            }
        //        }

        //        if (obj is Hashtable || obj is SortedList)
        //        {
        //            elementType = typeof(DictionaryEntry);
        //            return true;
        //        }
        //    }

        //    return false;
        //}

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
                    var genericTypeArgument = objType.GetInterfaces().FirstOrDefault(i => i.Name.Contains("IEnumerable`"));

                    Debug.Assert(genericTypeArgument != null, "A generic type argument for a generic IEnumerable was not found.");

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