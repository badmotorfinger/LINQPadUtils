namespace LINQPadUtils.Fragments
{
    using System.Text;

    using LINQPadUtils.Markup;
    using LINQPadUtils.MetadataProviders;
    using LINQPadUtils.Renderers;

    class ComplexObjectTableRowsFragment : FragmentBase
    {
        public ComplexObjectTableRowsFragment(TypeMetadataProviderBase metadata)
            : base(metadata)
        {
        }

        public override string Render(int depthLimit, int currentDepth)
        {
            var rowDataSb = new StringBuilder();

            foreach (var propertyInfo in this.Metadata.Properties)
            {
                rowDataSb.Append("<tr>");

                rowDataSb.AppendFormat(
                    "<th class='member' title='{0}'>{1}</th>",
                    propertyInfo.MemberType,
                    propertyInfo.Name);

                object value = propertyInfo.GetValue(this.Metadata.SourceObject);

                var displayValue = ValueDisplay.DisplayValue(value, depthLimit, currentDepth);

                rowDataSb.Append(HtmlTag.WrapValue("td", displayValue));

                rowDataSb.Append("</tr>");
            }

            return rowDataSb.ToString();
        }
    }
}