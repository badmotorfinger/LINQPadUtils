namespace LINQPadUtils.Renderers
{
    using LINQPadUtils.Fragments;
    using LINQPadUtils.MetadataProviders;

    public static class ValueDisplay
    {
        public static string DisplayValue(object obj)
        {
            var itemMetadata = TypeMetadataProviderBase.GetMetadataProvider(obj);

            if (itemMetadata.IsPrimitiveElement)
            {
                return ValueInspector.GetDisplayValue(obj);
            }

            var renderer = FragmentBase.GetFragment(itemMetadata);

            return renderer.Render();
        }
    }
}