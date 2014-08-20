using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQPadUtils.Renderers
{
    using LINQPadUtils.MetadataProviders;

    public static class ValueDisplay
    {
        public static string DisplayValue(object obj)
        {
            var itemMetadata = TypeMetadataProviderBase.GetMetadataProvider(obj);


        }
    }
}
