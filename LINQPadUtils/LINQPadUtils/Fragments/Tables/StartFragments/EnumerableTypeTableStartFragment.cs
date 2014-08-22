namespace LINQPadUtils.Fragments.Tables.StartFragments
{
    using System;
    using System.Globalization;
    using System.Text;

    using LINQPadUtils.MetadataProviders;

    internal class EnumerableTypeTableStartFragment : FragmentBase
    {
        public EnumerableTypeTableStartFragment(TypeMetadataProviderBase metadata)
            : base(metadata)
        {
        }

        public override string Render(int depthLimit, int currentDepth)
        {
            if (base.Metadata.IsEnumerable)
            {
                var tableHead = new StringBuilder(LinqPadUtilResources.EnumerableResultTableHead);

                tableHead.Replace("{tablecount}", base.GetCurrentFragmentCount());

                // Only use table sorter if there are rows to display.
                tableHead.Replace("{tablesorter_class}", base.Metadata.Count > 0 ? "tablesorter" : String.Empty);

                tableHead.Replace("{colspan}", this.Metadata.Properties.Length.ToString(CultureInfo.InvariantCulture))
                    .Replace("{typename}", base.GetTypeFriendlyDisplayText(base.Metadata.SourceObjectType))
                    .Replace("{itemcount}", base.Metadata.Count.ToString(CultureInfo.InvariantCulture));

                return tableHead.ToString();
            }

            throw new ApplicationException("This fragment can only render IEnumerable.");
        }
    }
}