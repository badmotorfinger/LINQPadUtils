namespace LINQPad
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Web;
    using System.Web.Script.Serialization;

    public static class LinqPadExtensions
    {
        const string IndentString = "  ";

        public static object DumpReflect<T>(this T obj)
        {
            return Reflect(obj).Dump();
        }

        public static object DumpReflect<T>(this T obj, bool toDataGrid)
        {
            return Reflect(obj).Dump(toDataGrid);
        }

        public static object DumpReflect<T>(this T obj, int depth)
        {
            return Reflect(obj, depth).Dump(depth + 2);
        }

        public static object DumpReflect<T>(this T obj, string description)
        {
            return Reflect(obj).Dump(description);
        }

        public static object DumpReflect<T>(this T obj, string description, bool toDataGrid)
        {
            return Reflect(obj).Dump();
        }

        public static object DumpReflect<T>(this T obj, string description, int depth)
        {
            return Reflect(obj, depth).Dump(description, depth + 2);
        }

        public static object Reflect<T>(this T obj, int depth = 0)
        {
            return ReflectOnType(obj, depth, 0);
        }

        static object ReflectOnType(object obj, int depth, int currentDepth)
        {
            PropertyInfo[] properties = obj.GetType().GetProperties((BindingFlags)~0);

            return
                 new[]
                {
                    new
                    {
                        Accessibility = (object)String.Empty,
                        Name = String.Empty,
                        Value = Util.RawHtml(@"<span style='color: Green;font-weight:Bold;'>" + HttpUtility.HtmlEncode(obj) + @"</span>")
                    }
                }
                .Union(
                    from p in properties
                    let isIndexed = p.GetIndexParameters().Length > 0
                    let getSupported = p.GetMethod != null
                    where !isIndexed && getSupported
                    select new
                    {
                        Accessibility = getSupported ? GetAccessibility(p.GetMethod) : (object)"",
                        p.Name,
                        Value =
                        getSupported
                            ? InvokeMethod(() => p.GetValue(obj, null), depth, currentDepth)
                            : "<i>Get accessor not implemented.</i>"
                    }
                )
                .Union(
                    from m in obj.GetType().GetMethods((BindingFlags)~0)
                    let isCallableMethod = m.GetParameters().Length == 0
                    let getterName = m.Name.StartsWith("get_")
                        ? m.Name.Substring(4)
                        : String.Empty
                    let setterName = m.Name.StartsWith("set_")
                        ? m.Name.Substring(4)
                        : String.Empty

                    where isCallableMethod && !properties.Any(p => p.Name == getterName || p.Name == setterName)

                    let result = InvokeMethod(() => m.Invoke(obj, null), depth, currentDepth)

                    select new
                    {
                        Accessibility = GetAccessibility(m),
                        Name = m.Name + "()",
                        Value = result
                    }
                )
                .OrderBy(a => a.Name)
                .Skip(0);
        }

        public static string DumpJson<T>(this T obj)
        {
            return DumpJson(obj, null, false);
        }

        public static string DumpJson<T>(this T obj, string description)
        {
            return DumpJson(obj, description, false);
        }

        public static string DumpJson<T>(this T obj, string description, bool toDataGrid)
        {
            var json =
                new JavaScriptSerializer().Serialize(obj);

            return FormatJson(json).Dump(description, toDataGrid);
        }

        static string FormatJson(string json)
        {
            int indentation = 0;
            int quoteCount = 0;

            var result =
                from ch in json
                let quotes = (ch == '"' || ch == '\'') ? quoteCount++ : quoteCount
                let lineBreak = ch == ',' && quotes % 2 == 0 ? ch + Environment.NewLine + String.Concat(Enumerable.Repeat(IndentString, indentation)) : null
                let openChar = ch == '{' || ch == '[' ? ch + Environment.NewLine + String.Concat(Enumerable.Repeat(IndentString, ++indentation)) : ch.ToString()
                let closeChar = ch == '}' || ch == ']' ? Environment.NewLine + String.Concat(Enumerable.Repeat(IndentString, --indentation)) + ch : ch.ToString()

                // When quotes are closed and there's a space, ignore it unless we are between quotes.
                where (quotes % 2 == 0 && !Char.IsSeparator(ch)) || quotes % 2 == 1

                select lineBreak ??
                    (openChar != null && openChar.Length > 1 ? openChar : closeChar);

            return String.Concat(result);
        }

        static object InvokeMethod(Func<object> f, int depth, int currentDepth)
        {
            try
            {
                var result = f();

                if (result == null)
                {
                    return null;
                }

                if (result.GetType().IsPrimitive || currentDepth >= depth)
                {
                    return result.GetType().IsArray
                        ? result
                        : result.ToString();
                }

                return ReflectOnType(result, depth, ++currentDepth);
            }
            catch (TargetInvocationException tex)
            {
                return tex.InnerException ?? tex;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        static object GetAccessibility(MethodBase method)
        {
            var accessibility =
                method.IsPublic
                    ? "public"
                    : method.IsPrivate
                        ? "private"
                        : method.IsVirtual
                            ? "virtual"
                                : method.IsAbstract
                                    ? "abstract"
                                    : method.IsConstructor
                                        ? "ctor"
                                        : method.IsAssembly
                                            ? "internal"
                                            : method.IsFamily
                                                ? "protected"
                                                : method.IsFamilyOrAssembly
                                                    ? "protected internal"
                                                    : "--";
            accessibility += method.IsStatic
                ? " static"
                : "";

            return Util.RawHtml(@"<span style='color: Blue'>" + accessibility + @"</span>");
        }
    }
}