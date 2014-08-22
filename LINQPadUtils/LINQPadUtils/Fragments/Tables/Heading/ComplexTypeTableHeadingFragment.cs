namespace LINQPadUtils.Fragments
{
    using System;

    using LINQPadUtils.MetadataProviders;

    class ComplexTypeTableHeadingFragment : FragmentBase
    {
        public ComplexTypeTableHeadingFragment(TypeMetadataProviderBase metadata)
            : base(metadata)
        {
        }

        public override string Render(int depthLimit, int currentDepth)
        {
            if (!base.Metadata.IsEnumerable)
            {
                return String.Format(
                    "<tr id='sum2'><td colspan='2' class='summary'>{0}</td></tr>",
                    this.Metadata.SourceObject);
            }

            throw new ApplicationException("Renderer does not support type " + base.Metadata.SourceObjectType.Name);
        }
    }
}