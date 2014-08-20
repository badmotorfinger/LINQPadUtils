namespace LINQPadUtils
{
    using System.Collections.Generic;
    using System.Windows.Documents;

    using LINQPadUtils.Markup;
    using LINQPadUtils.MetadataProviders;
    using LINQPadUtils.Fragments;

    class TableBuilder
    {
        readonly TypeMetadataProviderBase metadata;

        readonly StringJoiner stringJoiner;

        readonly List<FragmentBase> fragments = new List<FragmentBase>();

        public TableBuilder(TypeMetadataProviderBase metadata)
            : this(metadata, new StringJoiner()) { }

        public TableBuilder(TypeMetadataProviderBase metadata, StringJoiner stringJoiner)
        {
            this.metadata = metadata;
            this.stringJoiner = stringJoiner;
        }

        public void AddFragment(params FragmentBase[] renderers)
        {
            this.fragments.AddRange(renderers);
        }

        public override string ToString()
        {
            foreach (var renderer in fragments)
            {
                stringJoiner.AppendFunc(renderer.Render);
            }

            return stringJoiner.ToString();
        }
    }
}