// ReSharper disable CheckNamespace

namespace LINQPad
// ReSharper restore CheckNamespace
{
    using System;
    using System.Windows.Controls;

    using LINQPadUtils;
    using LINQPadUtils.Fragments;
    using LINQPadUtils.Markup;
    using LINQPadUtils.MetadataProviders;

    // ReSharper disable UnusedMember.Global
    public static partial class LinqPadExtensions
    // ReSharper restore UnusedMember.Global
    {
        // ReSharper disable UnusedMember.Global
        public static object DumpBrowser<T>(this T obj)
        // ReSharper restore UnusedMember.Global
        {
            var metaData = TypeMetadataProviderBase.GetMetadataProvider(obj);

            var renderer = FragmentBase.GetFragment(metaData);

            string result = renderer.Render();

            //TODO: Work out how to get multiple result sets in to the same browser control.

            var document = new StringJoiner(
                LinqPadUtilResources.DumpExtendedHead,
                LinqPadUtilResources.DumpExtendedFoot);

            document.Append(result);

            document.Document.Replace("{script}", LinqPadUtilResources.jquery_1_11_1_min)
                .Replace("{tablesorter}", LinqPadUtilResources.jquery_tablesorter_min)
                .Replace("{firebug}", LinqPadUtilResources.firebug_lite_compressed);

            var webBrowser = new WebBrowser();

            webBrowser.NavigateToString(document.ToString());

            PanelManager.DisplayWpfElement(webBrowser);

            return obj;
        }
    }
}
//var browser = sender as WebBrowser;
//            dynamic doc = browser.Document;
//            dynamic dEl = doc.documentElement;
//dEl.innerHTML