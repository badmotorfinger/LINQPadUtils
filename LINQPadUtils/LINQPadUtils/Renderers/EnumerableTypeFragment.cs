namespace LINQPadUtils.Fragments
{
    using LINQPadUtils.MetadataProviders;

    class EnumerableTypeFragment : FragmentBase
    {
        public EnumerableTypeFragment(TypeMetadataProviderBase metadata)
            : base(metadata) { }

        public override string Render()
        {
            var tableBuilder = new TableBuilder(base.Metadata);

            FragmentBase rowRenderer = base.Metadata.IsEnumerableObject
                ? new EnumerableObjectTableRowsFragment(base.Metadata)
                : base.Metadata.IsEnumerableStaticType
                    ? new EnumerableComplexObjectTableRowsFragment(base.Metadata) as FragmentBase
                    : new PrimitiveEnumerableTableRowsFragment(base.Metadata);

            tableBuilder.AddFragment(new FragmentBase[]
            {
                new EnumerableTypeTableStartFragment(base.Metadata), 
                new EnumerableTypeTableHeadingFragment(base.Metadata),
                rowRenderer,
                new TableEndFragment(base.Metadata), 
            });

            return tableBuilder.ToString();
        }
    }
}