namespace LINQPadUtils.Fragments
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Text;

    using LINQPadUtils.Markup;
    using LINQPadUtils.MetadataProviders;

    class PrimitiveEnumerableTableRowsFragment : FragmentBase
    {
        public PrimitiveEnumerableTableRowsFragment(TypeMetadataProviderBase metadata)
            : base(metadata) { }

        public override string Render()
        {
            var rowDataSb = new StringBuilder();

            if (base.Metadata.IsEnumerable && base.Metadata.IsPrimitiveElement)
            {
                // check the collection to see if it's all strongly typed, or all primitives so the columns can be built.
                foreach (var item in (IEnumerable)this.Metadata.SourceObject)
                {
                    rowDataSb.Append("<tr>"); //TODO: I don't think we need all this code if it's just going to render primitive types.
                        
                    var renderedValue = ValueInspector.GetDisplayValue(item);

                    var newTag = HtmlTag.WrapValue("td", renderedValue);

                    rowDataSb.Append(newTag);

                    rowDataSb.Append("</tr>");
                }
            }
            else
            {
                throw new InvalidOperationException("Cannot render rows for a type which does not implement IEnumerable<T>.");
            }
            
            return rowDataSb.ToString();
        }
    }
}