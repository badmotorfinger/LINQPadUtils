namespace LINQPadUtils
{
    using System;
    using System.Collections.Generic;

    using LINQPadUtils.Fragments;
    using LINQPadUtils.Markup;
    using LINQPadUtils.MetadataProviders;

    class TableBuilder
    {
        readonly StringJoiner stringJoiner;

        readonly List<FragmentBase> fragments = new List<FragmentBase>();

        public TableBuilder(TypeMetadataProviderBase metadata)
            : this(metadata, new StringJoiner())
        {
        }

        public TableBuilder(TypeMetadataProviderBase metadata, StringJoiner stringJoiner)
        {
            this.stringJoiner = stringJoiner;
        }

        public void AddFragment(params FragmentBase[] renderers)
        {
            this.fragments.AddRange(renderers);
        }

        public override string ToString()
        {
            throw new NotSupportedException("Use the ToString() overload with the correct recursion depth.");
        }

        public string ToString(int depth, int currentDepth)
        {
            foreach (var renderer in this.fragments)
            {
                this.stringJoiner.AppendFunc(() => renderer.Render(depth, currentDepth));
            }

            return this.stringJoiner.ToString();
        }
    }
}