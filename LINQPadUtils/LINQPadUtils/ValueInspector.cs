namespace LINQPadUtils
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public static class ValueInspector
    {
        static Func<object, TableHeading[]> objectHeadingsFunc = GetHeadingsForType();

        /// <summary>
        /// Gets the display value by performing a ToString() operation.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>A string representation of the object.</returns>
        public static string GetDisplayValue(object obj)
        {
            return obj.ToString();
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
                    var elementType = objType.GetGenericArguments()[0];
                    return IsPrimitiveType(elementType);
                }

                throw new Exception("Could not determine IEnumerable element type.");
            }

            return false;
        }

        static Func<object, TableHeading[]> GetHeadingsForType()
        {
            var typeInfo = new Dictionary<Type, TableHeading[]>();

            return source =>
            {
                var sourceType = source.GetType();

                TableHeading[] headings;

                if (typeInfo.TryGetValue(sourceType, out headings))
                {
                    return headings;
                }

                if (ValueInspector.IsPrimitiveType(sourceType))
                {
                    return typeInfo[sourceType] = new TableHeading[0];
                }

                return typeInfo[sourceType] = sourceType.GetProperties()
                    .Select(
                        p => new TableHeading
                             {
                                 Title = p.Name,
                                 Type = p.PropertyType,
                                 PropertyInfo = p
                             })
                    .ToArray();
            };
        }
    }
}