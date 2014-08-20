namespace LINQPadUtils.Fragments
{
    using System;
    using System.Linq;
    using System.Text;

    using LINQPadUtils.MetadataProviders;

    class EnumerableTypeTableHeadingFragment : FragmentBase
    {
        public EnumerableTypeTableHeadingFragment(TypeMetadataProviderBase metadata)
            : base(metadata) { }

        public override string Render()
        {
            if (!base.Metadata.Properties.Any())
            {
                return String.Empty;
            }

            if (base.Metadata.IsEnumerableOfKnownType)
            {
                var stringBuilder = new StringBuilder();

                stringBuilder.Append("<tr>");

                foreach (var property in this.Metadata.Properties)
                {
                    var heading = String.Format("<th title='{0}'>{1}</th>", property.PropertyType, property.Name);

                    stringBuilder.Append(heading);
                }

                return stringBuilder.ToString();
            }

            throw new ApplicationException("Renderer does not support type " + base.Metadata.SourceObjectType.Name);
        }
    }
}