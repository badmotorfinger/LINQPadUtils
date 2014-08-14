namespace LINQPadUtils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class TypeMetadataProvider
    {
        protected TypeMetadataProvider(object obj)
        {
            SourceObject = obj;
        }

        public static TypeMetadataProvider GetMetadataProvider(object obj)
        {
            if (ValueInspector.IsPrimitiveObject(obj))
            {
                return new PrimitiveTypeMetadataProvider(obj);
            }
            
            if (ValueInspector.IsPrimitiveEnumerable(obj))
            {
                return new PrimitiveCollectionMetadataProvider(obj);
            }

            throw new Exception("Could not find a type provider for type " + obj.GetType());
        }

        public Object SourceObject { get; private set; }

        public abstract Type MainType { get; protected set; }

        public abstract IEnumerable<TableHeading> Headings { get; protected set; }
    }

    class PrimitiveTypeMetadataProvider : TypeMetadataProvider
    {
        public PrimitiveTypeMetadataProvider(object obj)
            : base(obj)
        {
            MainType = obj.GetType();

            Headings = Enumerable.Empty<TableHeading>();
        }

        public override sealed Type MainType { get; protected set; }

        public override sealed IEnumerable<TableHeading> Headings { get; protected set; }
    }

    class PrimitiveCollectionMetadataProvider : TypeMetadataProvider
    {
        public PrimitiveCollectionMetadataProvider(object obj)
            : base(obj)
        {
            MainType = obj.GetType();
        }

        public override sealed Type MainType { get; protected set; }

        public override IEnumerable<TableHeading> Headings { get; protected set; }
    }
}