namespace LINQPadUtils.Fragments
{
    using System.Text;
    using LINQPadUtils.Markup;
    using LINQPadUtils.MetadataProviders;

    class ComplexObjectTableRowsFragment : FragmentBase
    {
        public ComplexObjectTableRowsFragment(TypeMetadataProviderBase metadata)
            : base(metadata) { }

        public override string Render()
        {
            var rowDataSb = new StringBuilder();

            foreach (var propertyInfo in this.Metadata.Properties)
            {
                rowDataSb.Append("<tr>");

                rowDataSb.AppendFormat(
                    "<th class='member' title='{0}'>{1}</th>",
                    propertyInfo.PropertyType,
                    propertyInfo.Name);

                object value = propertyInfo.GetValue(this.Metadata.SourceObject);

                string displayValue = ValueInspector.GetDisplayValue(value);

                rowDataSb.Append(HtmlTag.WrapValue("td", displayValue));

                rowDataSb.Append("</tr>");
            }

            return rowDataSb.ToString();
        }
    }
}