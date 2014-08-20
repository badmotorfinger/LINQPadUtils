namespace ``Metadata Tests``

open Xunit
open LINQPadUtils
open LINQPadUtils.MetadataProviders
open FsUnit
open System
open System.Reflection

type Foo = 
    { Foo : string
      Bar : int }

type ``When getting metadata about a non-enumearble type``() = 
    
    [<Fact>]
    member public this.``and the type is a primitive, return a primitive metadata provider``() = 
        let sequenceMetadata = TypeMetadataProviderBase.GetMetadataProvider(1)
        let metadataType = sequenceMetadata.GetType()
        metadataType |> should equal typedefof<PrimitiveTypeMetadataProvider>
    
    [<Fact>]
    member public this.``and the type is a string, return a primitive metadata provider``() = 
        let sequenceMetadata = TypeMetadataProviderBase.GetMetadataProvider(String.Empty)
        let metadataType = sequenceMetadata.GetType()
        metadataType |> should equal typedefof<PrimitiveTypeMetadataProvider>
    
    [<Fact>]
    member public this.``and the type is a date/time, return a primitive metadata provider``() = 
        let sequenceMetadata = TypeMetadataProviderBase.GetMetadataProvider(DateTime.Now)
        let metadataType = sequenceMetadata.GetType()
        metadataType |> should equal typedefof<PrimitiveTypeMetadataProvider>
    
    [<Fact>]
    member public this.``and the type is a simple type, return a complex metadata provider``() = 
        let record = 
            { Foo = "123"
              Bar = 123 }
        
        let sequenceMetadata = TypeMetadataProviderBase.GetMetadataProvider(record)
        let metadataType = sequenceMetadata.GetType()
        metadataType |> should equal typedefof<ComplexTypeMetadataProvider>

type ``When getting metadata about a enumearble type``() = 
    
    [<Fact>]
    member public this.``and it's a primitive array, it should return the EnumerablePrimitiveTypeProvider``() = 
        let primitiveArray = [| 1; 2; 3 |]
        let metadata = TypeMetadataProviderBase.GetMetadataProvider(primitiveArray)
        let metadataType = metadata.GetType()
        let expectedMetadataType = typedefof<EnumerablePrimitiveTypeMetadataProvider>
        metadataType |> should equal expectedMetadataType
    
    [<Fact>]
    member public this.``and it's a generic primitive IEnumerable, it should return the EnumerablePrimitiveTypeProvider``() = 
        let primitiveEnumerable = new ResizeArray<string>()
        let metadata = TypeMetadataProviderBase.GetMetadataProvider(primitiveEnumerable)
        let metadataType = metadata.GetType()
        let expectedMetadataType = typedefof<EnumerablePrimitiveTypeMetadataProvider>
        metadataType |> should equal expectedMetadataType
    
    [<Fact>]
    member public this.``and it's an object based array, it should return the EnumerableObjectTypeProvider``() = 
        let objectArray = 
            [| new obj()
               new obj() |]
        
        let metadata = TypeMetadataProviderBase.GetMetadataProvider(objectArray)
        let metadataType = metadata.GetType()
        let expectedMetadataType = typedefof<EnumerableObjectTypeMetadataProvider>
        metadataType |> should equal expectedMetadataType
    
    [<Fact>]
    member public this.``and it's an object based generic IEnumerable, it should return the EnumerableObjectTypeProvider``() = 
        let objectList = new ResizeArray<obj>()
        let metadata = TypeMetadataProviderBase.GetMetadataProvider(objectList)
        let metadataType = metadata.GetType()
        let expectedMetadataType = typedefof<EnumerableObjectTypeMetadataProvider>
        metadataType |> should equal expectedMetadataType
    
    [<Fact>]
    member public this.``and it's a complex object generic IEnumerable, it should return the EnumerableComplexObjectTypeMetadataProvider``() = 
        let complexObjectList = new ResizeArray<Foo>()
        let metadata = TypeMetadataProviderBase.GetMetadataProvider(complexObjectList)
        let metadataType = metadata.GetType()
        let expectedMetadataType = typedefof<EnumerableComplexObjectTypeMetadataProvider>
        metadataType |> should equal expectedMetadataType
    
    [<Fact>]
    member public this.``and it's a complex object array, it should return the EnumerableComplexObjectTypeMetadataProvider``() = 
        let complexObjectArray = 
            [| { Foo = "foo"
                 Bar = 0 } |]
        
        let metadata = TypeMetadataProviderBase.GetMetadataProvider(complexObjectArray)
        let metadataType = metadata.GetType()
        let expectedMetadataType = typedefof<EnumerableComplexObjectTypeMetadataProvider>
        metadataType |> should equal expectedMetadataType
        metadata.Properties |> Seq.length |> should equal 2

    [<Fact>]
    member public this.``and it's a generic dictionary, it should return the EnumerableComplexObjectTypeMetadataProvider``() = 
        let dictionary = new System.Collections.Generic.Dictionary<int,string>()
        let metadata = TypeMetadataProviderBase.GetMetadataProvider(dictionary)
        let metadataType = metadata.GetType()
        let expectedMetadataType = typedefof<EnumerableComplexObjectTypeMetadataProvider>

        metadataType |> should equal expectedMetadataType
        metadata.Properties |> Seq.length |> should equal 2

    [<Fact>]
    member public this.``and it's a HashTable, it should return the EnumerableComplexObjectTypeMetadataProvider``() = 
        let hashtable = new System.Collections.Hashtable()
        let metadata = TypeMetadataProviderBase.GetMetadataProvider(hashtable)
        let metadataType = metadata.GetType()
        let expectedMetadataType = typedefof<EnumerableComplexObjectTypeMetadataProvider>
        
        metadataType |> should equal expectedMetadataType
        metadata.Properties |> Seq.length |> should equal 2

