namespace LINQPadUtils.Renderers.Tables.RowFragments
{
    using System;
    using System.Collections;
    using System.Text;

    using LINQPadUtils.Fragments;
    using LINQPadUtils.Markup;
    using LINQPadUtils.MetadataProviders;
    using LINQPadUtils.Renderers;

    class EnumerableObjectTableRowsFragment : FragmentBase
    {
        public EnumerableObjectTableRowsFragment(TypeMetadataProviderBase metadata)
            : base(metadata) { }

        public override string Render()
        {
            var rowDataSb = new StringBuilder();

            if (base.Metadata.IsEnumerable)
            {
                // check the collection to see if it's all strongly typed, or all primitives so the columns can be built.
                foreach (var item in (IEnumerable)this.Metadata.SourceObject)
                {
                    rowDataSb.Append("<tr>");

                    var renderedValue = ValueDisplay.DisplayValue(item);

                    var newTag = HtmlTag.WrapValue("td", renderedValue);

                    rowDataSb.Append(newTag);

                    rowDataSb.Append("</tr>");
                }
            }
            else
            {
                throw new InvalidOperationException("Cannot render rows for a type which does not implement IEnumerable.");
            }
            
            return rowDataSb.ToString();
        }
    }
}