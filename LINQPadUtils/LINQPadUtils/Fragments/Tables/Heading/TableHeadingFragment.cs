namespace LINQPadUtils.Fragments
{
    using System;
    using System.Linq;
    using System.Text;

    using LINQPadUtils.MetadataProviders;

    class EnumerableTypeTableHeadingFragment : FragmentBase
    {
        public EnumerableTypeTableHeadingFragment(TypeMetadataProviderBase metadata)
            : base(metadata)
        {
        }

        public override string Render(int depthLimit, int currentDepth)
        {
            if (!base.Metadata.Properties.Any())
            {
                return "<tr><th>Sort</th></tr></thead>";
            }

            var stringBuilder = new StringBuilder();
            
            // Only display table headings if there are rows to display.
            if (base.Metadata.IsEnumerableOfKnownType && base.Metadata.Count > 0)
            {
                stringBuilder.Append("<tr>");

                foreach (var property in this.Metadata.Properties)
                {
                    var heading = String.Format("<th title='{0}'>{1}</th>", property.MemberType, property.Name);

                    stringBuilder.Append(heading);
                }

                stringBuilder.Append("</tr>"); 
            }

            stringBuilder.Append("</thead>");

            return stringBuilder.ToString();
        }
    }
}