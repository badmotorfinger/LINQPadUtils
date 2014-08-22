namespace LINQPadUtils
{
    using System;
    using System.Reflection;
    using System.Web;

    using LINQPad;

    using LINQPadUtils.Fragments;
    using LINQPadUtils.MetadataProviders;

    public static class ValueDisplay
    {
        static readonly Lazy<Type> RawHtmlType =
           new Lazy<Type>(() => Assembly.GetAssembly(typeof(ExecutionEngine)).GetType("LINQPad.ObjectGraph.RawHtml"));

        public static string DisplayValue(object obj, int depthLimit, int currentDepth)
        {
            var itemMetadata = TypeMetadataProviderBase.GetMetadataProvider(obj);

            if (itemMetadata.IsPrimitiveElement || depthLimit == currentDepth)
            {
                return GetDisplayValue(obj);
            }

            currentDepth++;

            var renderer = FragmentBase.GetFragment(itemMetadata);

            return renderer.Render(depthLimit, currentDepth);
        }

        /// <summary>
        /// Gets the display value by performing a ToString() operation.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>A string representation of the object.</returns>
        internal static string GetDisplayValue(object obj)
        {
            if (obj == null)
            {
                return String.Empty;
            }

            return GetDisplayValue(obj, obj.GetType());
        }

        /// <summary>
        /// Gets the display value by performing a ToString() operation.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="objType">The object type.</param>
        /// <returns>A string representation of the object.</returns>
       internal static string GetDisplayValue(object obj, Type objType)
        {
            if (obj == null)
            {
                return String.Empty;
            }

            string displayValue;
            if (RawHtmlType.Value == objType)
            {
                var fi = RawHtmlType.Value.GetField("Html");

                displayValue = fi.GetValue(obj).ToString();
            }
            else
            {
                displayValue = obj.ToString();
            }

            return HttpUtility.HtmlEncode(displayValue);
        }
    }
}