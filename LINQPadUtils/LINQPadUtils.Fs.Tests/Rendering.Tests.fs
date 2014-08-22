namespace ``LINQPad Utils Tests``

open System
open FsUnit.Xunit
open Xunit
open LINQPadUtils
open LINQPadUtils.Markup
open LINQPadUtils.MetadataProviders
open LINQPadUtils.Fragments
open System.Reflection
open ``Metadata Tests``

type ``When rendering a value to the screen``() = 
    
    [<Fact>]
    member public this.``the Fragment should join all string fragments``() = 
        let document = new StringJoiner("foo", "bar")
        document.AppendFunc(fun () -> "baz")
        let expectedOutput = "foobazbar"
        let actualOutput = document.ToString()
        expectedOutput |> should equal actualOutput
    
    [<Fact>]
    member public this.``and it's a simple primitive type, render the tag``() = 
        let tag = HtmlTag.WrapValue("xx", "yy")
        let newTag = tag.ToString()
        let expectedTag = "<xx>yy</xx>"
        newTag |> should equal expectedTag

    [<Fact>]
    member public this.``and the value is a null, return an empty string``() = 
        let value = ValueDisplay.GetDisplayValue(null)
        value |> should equal String.Empty

type ``When rendering a table``() = 
    
    [<Fact>]
    member public this.``and metadata contains no properties, do not render headings``() = 
        let metadata = new PrimitiveTypeMetadataProvider("foo", typedefof<string>)
        let tableFragment = new EnumerableTypeTableHeadingFragment(metadata)
        let result = tableFragment.Render(0)
        result |> should equal String.Empty
    
    [<Fact>]
    member public this.``and metadata contains properties, render a heading``() = 
        let testType = 
            { Foo = "foo"
              Bar = 0 }
        
        let metadata = new ComplexTypeMetadataProvider(testType)
        let tableFragment = new EnumerableTypeTableHeadingFragment(metadata)
        let result = tableFragment.Render(0)
        let expectedResult = "<tr><th title='System.String'>Test</th></tr>"
        result |> should equal expectedResult
    
    [<Fact>]
    member public this.``and the type is a simple type, return the correct property headings``() = 
        let record = 
            { Foo = "123"
              Bar = 123 }
        
        let sequenceMetadata = TypeMetadataProviderBase.GetMetadataProvider(record)
        let typeProperties = sequenceMetadata.Properties
        
        let comparison = 
            Seq.compareWith (fun (x : MemberInfo) (y : MemberInfo) -> 
                if x.Name = y.Name then 0
                else 1) typeProperties
        
        let expectedProperties = record.GetType().GetMembers()
        expectedProperties
        |> comparison
        |> should equal 0
