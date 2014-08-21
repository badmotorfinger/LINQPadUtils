namespace LINQPadUtils.Fragments
{
    using LINQPadUtils.MetadataProviders;

    class ComplexTypeFragment : FragmentBase
    {
        public ComplexTypeFragment(TypeMetadataProviderBase metadata)
            : base(metadata)
        {
        }

        public override string Render()
        {
            var tableBuilder = new TableBuilder(base.Metadata);

            tableBuilder.AddFragment(
                new FragmentBase[]
                {
                    new ComplexTypeTableStartFragment(base.Metadata),
                    new ComplexTypeTableHeadingFragment(base.Metadata),
                    new ComplexObjectTableRowsFragment(base.Metadata),
                    new TableEndFragment(base.Metadata),
                });

            return tableBuilder.ToString();
        }
    }
}