namespace LINQPad
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Web.Script.Serialization;

    public static class LinqPadExtensions
    {
        const string IndentString = "  ";

        #region Public Methods and Operators

        public static IEnumerable<object> DumpReflect<T>(this T obj)
        {
            return DumpReflect(obj, null, false);
        }

        public static IEnumerable<object> DumpReflect<T>(this T obj, string description)
        {
            return DumpReflect(obj, description, false);
        }

        public static IEnumerable<object> DumpReflect<T>(this T obj, string description, bool toDataGrid)
        {
            var result = ReflectOnType(obj);

            return result.Dump(description, toDataGrid);
        }

        static IEnumerable<object> ReflectOnType(object obj)
        {
            PropertyInfo[] properties = obj.GetType().GetProperties();

            return
                 new[]
                {
                    new
                    {
                        Name = obj.GetType().ToString(),
                        Value = (object)obj.ToString()
                    }
                }
                     .Union(
                         from p in properties
                         let isIndexed = p.GetIndexParameters().Length > 0
                         where !isIndexed
                         select new
                         {
                             p.Name,
                             Value = p.GetValue(obj, null)
                         }
                     )
                     .Union(
                         from m in obj.GetType().GetMethods(~BindingFlags.NonPublic)
                         let isCallableMethod = m.GetParameters().Length == 0
                         let getterName = m.Name.StartsWith("get_")
                             ? m.Name.Substring(4)
                             : String.Empty
                         let setterName = m.Name.StartsWith("set_")
                             ? m.Name.Substring(4)
                             : String.Empty
                         where isCallableMethod && !properties.Any(p => p.Name == getterName || p.Name == setterName)
                         select new
                         {
                             Name = m.Name + "()",
                             Value = m.Invoke(obj, null)
                         }
                     ); 
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
            var result = ReflectOnType(obj);

            var json =
                new JavaScriptSerializer().Serialize(result);

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

        #endregion
    }
}