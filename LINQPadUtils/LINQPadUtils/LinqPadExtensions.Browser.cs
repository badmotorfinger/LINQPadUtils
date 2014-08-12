namespace LINQPad
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Windows.Controls;
    using LINQPadUtils;

    public static partial class LinqPadExtensions
    {
        public static object DumpBrowser<T>(this T obj)
        {
            var objGraph = (System.Collections.IList)ReflectOnType(obj, 0, 0);

            var cols = objGraph.GetType().GetProperties().Length;

            var webBrowser = new WebBrowser();

            var sb = new StringBuilder(

                String.Format("<tr>{0}{1}{2}</tr>",
                GetHeading("System.String", "Accessibility"),
                GetHeading("System.String", "Name"),
                GetHeading("System.Object", "Value")));

            foreach (dynamic item in objGraph)
            {
                sb.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>", 
                    item.Accessibility,
                    item.Name,
                    item.Value);
            }

            var table = LinqPadUtils.ResultTable
                .Replace("{colspan}", cols.ToString(CultureInfo.InvariantCulture))
                .Replace("{typename}", "List<>")
                .Replace("{itemcount}", objGraph.Count.ToString(CultureInfo.InvariantCulture))
                .Replace("{content}", sb.ToString());

            var document = LinqPadUtils.DumpExtended.Replace("{content}", table);

            webBrowser.NavigateToString(document);

            PanelManager.DisplayWpfElement(webBrowser);

            return "";
        }

        static string GetHeading(string title, string text)
        {
            return String.Format("<th title='{0}'>{1}</th>", title, text);
        }

        static string GetRawHtmlValue(object rawHtml)
        {
            var mi = Assembly.GetCallingAssembly().GetTypes().First(a => a.Name.Contains("Raw")).GetFields().First(a => a.Name.Contains("Html"));

            mi.GetValue(rawHtml).Dump();
        }
    }
}