namespace LINQPad
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Controls;

    using LINQPadUtils;
    using LINQPadUtils.Fragments;
    using LINQPadUtils.Markup;
    using LINQPadUtils.MetadataProviders;

    public static partial class LinqPadExtensions
    {
        const string PanelName = "Sortable Results";

        static readonly List<Func<string>> Renderers = new List<Func<string>>();

        static WebBrowser webBrowser;

        public static object DumpSort<T>(this T obj)
        {
            return DumpSort<T>(obj, 1);
        }

        public static object DumpSort<T>(this T obj, int depth)
        {
            var metaData = TypeMetadataProviderBase.GetMetadataProvider(obj);

            var renderer = FragmentBase.GetFragment(metaData);

            Renderers.Add(() => renderer.Render(depth));


            var outputPanel = PanelManager.GetOutputPanel(PanelName);

            if (webBrowser == null || outputPanel == null)
            {
                webBrowser = new WebBrowser();

                PanelManager.DisplayWpfElement(webBrowser, PanelName);

                PanelManager.GetOutputPanel(PanelName).QueryEnded += OnQueryEnded;
            }

            return obj;
        }

        static void OnQueryEnded(object sender, EventArgs eventArgs)
        {
            PanelManager.GetOutputPanel(PanelName).QueryEnded -= LinqPadExtensions.OnQueryEnded;

            var document = new StringJoiner(
                    LinqPadUtilResources.DumpExtendedHead,
                    LinqPadUtilResources.DumpExtendedFoot);


            foreach (var renderer in Renderers)
            {
                document.AppendFunc(renderer);
            }

            document.Document.Replace("{script}", LinqPadUtilResources.jquery_1_11_1_min)
                .Replace("{tablesorter}", LinqPadUtilResources.jquery_tablesorter_min)
                .Replace("{firebug}", LinqPadUtilResources.firebug_lite_compressed);

            webBrowser.NavigateToString(document.ToString());
            
            Renderers.Clear();
            webBrowser = null; 
        }
    }
}