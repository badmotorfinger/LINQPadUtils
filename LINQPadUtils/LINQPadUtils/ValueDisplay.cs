namespace LINQPadUtils
{
    using System.Collections;

    public static class ValueDisplay
    {
        public static string GetDisplayValue(object obj)
        {
            return obj.ToString();
        }

        public static bool IsPlainVanilaDisplay(object obj)
        {
            var objType = obj.GetType();

            return objType == typeof(string) || objType.IsPrimitive;
        }

        public static bool IsPrimitiveEnumerable(object obj)
        {
            var objType = obj.GetType();

            if (obj is IEnumerable)
            {
                if (objType.IsArray)
                {
                    var elementType = objType.GetElementType();
                    return IsPlainVanilaDisplay(elementType);
                }
            }
        }
    }
}