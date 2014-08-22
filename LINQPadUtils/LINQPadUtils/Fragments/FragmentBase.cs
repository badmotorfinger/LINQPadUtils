namespace LINQPadUtils.Fragments
{
    using System;
    using System.Linq;

    using LINQPadUtils.MetadataProviders;

    abstract class FragmentBase
    {
        protected TypeMetadataProviderBase Metadata { get; private set; }

        FragmentBase()
        {
            tableCount++;
        }

        protected FragmentBase(TypeMetadataProviderBase metadata)
            : this()
        {
            this.Metadata = metadata;
        }

        public static FragmentBase GetFragment(TypeMetadataProviderBase metadata)
        {
            if (metadata.IsEnumerable)
            {
                return new EnumerableTypeFragment(metadata);
            }

            if (metadata.IsPrimitiveElement)
            {
                return new PrimitiveTypeFragment(metadata);
            }

            return new ComplexTypeFragment(metadata);
        }

        static int tableCount;

        protected string GetCurrentFragmentCount()
        {
            return tableCount.ToString();
        }

        public string Render(int depthLimit)
        {
            return Render(depthLimit, 0);
        }

        public abstract string Render(int depthLimit, int currentDepth);

        /// <summary>
        /// Generates a friendler string representation of a generic type.
        /// </summary>
        protected string GetTypeFriendlyDisplayText(Type type)
        {
            if (type.IsGenericType)
            {
                if (type.Name.Contains('`'))
                {
                    var cleanName = type.Name.Substring(0, type.Name.Length - 2);

                    var genericArgs = type.GetGenericArguments().Select(arg => arg.Name);

                    var genericArgsJoined = "&lt;" + String.Join(",", genericArgs) + "&gt;";

                    return String.Format("{0}{1}", cleanName, genericArgsJoined);
                }
            }
            return type.Name.Replace("UserQuery+", String.Empty);
        }
    }
}