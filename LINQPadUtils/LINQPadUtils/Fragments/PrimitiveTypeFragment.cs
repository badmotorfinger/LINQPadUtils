namespace LINQPadUtils.Fragments
{
    using LINQPadUtils.Markup;
    using LINQPadUtils.MetadataProviders;

    class PrimitiveTypeFragment : FragmentBase
    {
        public PrimitiveTypeFragment(TypeMetadataProviderBase metadata)
            : base(metadata)
        {
        }

        public override string Render(int depthLimit, int currentDepth)
        {
            var source = base.Metadata.SourceObject;

            var sourceObjectType = base.Metadata.SourceObjectType;

            var value = ValueDisplay.GetDisplayValue(source, sourceObjectType);

            return HtmlTag.WrapValue("span", value).ToString();
        }
    }
}