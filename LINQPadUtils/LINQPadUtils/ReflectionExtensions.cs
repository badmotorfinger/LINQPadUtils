namespace LINQPadUtils
{
    using System.Reflection;

    public static class ReflectionExtensions
    {
        public static object GetValue(this MemberInfo member, object sourceObject)
        {
            if (member is FieldInfo)
                return ((FieldInfo)member).GetValue(sourceObject);
            
            return ((PropertyInfo)member).GetValue(sourceObject, null);
        }
    }
}