namespace LINQPadUtils.Fragments
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Text;

    using LINQPadUtils.Markup;
    using LINQPadUtils.MetadataProviders;

    class EnumerableObjectTableRowsFragment : FragmentBase
    {
        public EnumerableObjectTableRowsFragment(TypeMetadataProviderBase metadata)
            : base(metadata) { }

        public override string Render()
        {
            var rowDataSb = new StringBuilder();

            if (this.Metadata.IsEnumerable)
            {
                // check the collection to see if it's all strongly typed, or all primitives so the columns can be built.
                foreach (var item in (IEnumerable)this.Metadata.SourceObject)
                {
                    rowDataSb.Append("<tr>"); //TODO: I don't think we need all this code if it's just going to render primitive types.

                    var itemMetadata = TypeMetadataProviderBase.GetMetadataProvider(item);

                    //if (itemMetadata.Properties.Any())
                    //{
                    //    foreach (var propertyInfo in itemMetadata.Properties)
                    //    {
                    //        var column = String.Format(
                    //            "<th title='{0}'>{1}</th>",
                    //            propertyInfo.PropertyType,
                    //            propertyInfo.Name);

                    //        rowDataSb.Append(column);
                    //    }
                    //}

                    string renderedValue;
                    if (itemMetadata.IsPrimitiveElement)
                    {
                        renderedValue = ValueInspector.GetDisplayValue(item);
                    }
                    else
                    {
                        var renderer = FragmentBase.GetFragment(itemMetadata);

                        renderedValue = renderer.Render();
                    }

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