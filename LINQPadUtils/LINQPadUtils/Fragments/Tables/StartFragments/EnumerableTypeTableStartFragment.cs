namespace LINQPadUtils.Fragments.Tables.StartFragments
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Text;

    using LINQPadUtils.MetadataProviders;

    internal class EnumerableTypeTableStartFragment : FragmentBase
    {
        public EnumerableTypeTableStartFragment(TypeMetadataProviderBase metadata)
            : base(metadata)
        {
        }

        public override string Render()
        {
            if (base.Metadata.IsEnumerable)
            {
                var tableHead = new StringBuilder(LinqPadUtilResources.EnumerableResultTableHead);

                int count = 0;

                if (base.Metadata.SourceObject is ICollection)
                {
                    count = ((ICollection)this.Metadata.SourceObject).Count;
                }
                else
                {
                    while (((IEnumerable)base.Metadata.SourceObject).GetEnumerator().MoveNext())
                    {
                        count++;
                    }
                }

                tableHead.Replace("{colspan}", this.Metadata.Properties.Length.ToString(CultureInfo.InvariantCulture))
                    .Replace("{typename}", base.GetTypeFriendlyDisplayText(base.Metadata.SourceObjectType))
                    .Replace("{itemcount}", count.ToString(CultureInfo.InvariantCulture));

                return tableHead.ToString();
            }

            throw new ApplicationException("This fragment can only render IEnumerable.");
        }
    }
}