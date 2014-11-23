namespace LINQPadUtils
{
    using System.Reflection;

    public static class ReflectionExtensions
    {
        public static object GetValue(this MemberInfo member, object sourceObject)
        {
            if (member is FieldInfo)
                return ((FieldInfo)member).GetValue(sourceObject);

            try
            {
                return ((PropertyInfo)member).GetValue(sourceObject, null);
            }
            catch (System.Exception ex)
            {
                return ex; // Return the exception so it can be reflected against.
            }
        }
    }
}