namespace LINQPadUtils.Fragments
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    using LINQPadUtils.MetadataProviders;

    class ComplexTypeTableStartFragment : FragmentBase
    {
        public ComplexTypeTableStartFragment(TypeMetadataProviderBase metadata)
            : base(metadata) { }

        public override string Render()
        {
            if (!base.Metadata.IsEnumerable)
            {
                var tableHead = new StringBuilder(LinqPadUtilResources.ObjectResultTableHead);

                tableHead.Replace("{colspan}", this.Metadata.Properties.Length.ToString(CultureInfo.InvariantCulture));

                if (base.Metadata.IsAnonymousType)
                {
                    tableHead.Replace("{typename}", "&#248;");
                }
                else
                {
                    tableHead.Replace("{typename}", base.GetTypeFriendlyDisplayText(base.Metadata.SourceObjectType));
                }

                return tableHead.ToString();
            }

            throw new ApplicationException("This fragment can only render IEnumerable.");
        }
    }
}