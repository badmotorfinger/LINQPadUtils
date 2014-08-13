namespace LINQPad
{
    using System;
    using System.Collections.Generic;
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
            var webBrowser = new WebBrowser();

            var tableRenderer = new TableBuilder(obj); 

            webBrowser.NavigateToString(tableRenderer.ToString());

            PanelManager.DisplayWpfElement(webBrowser);

            return obj;
        }

        

        static string GetRawHtmlValue(object rawHtml)
        {
            var mi = Assembly.GetCallingAssembly().GetTypes().First(a => a.Name.Contains("Raw")).GetFields().First(a => a.Name.Contains("Html"));

            mi.GetValue(rawHtml).Dump();

            return "";
        }
    }
}