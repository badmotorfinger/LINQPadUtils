namespace ``LINQPad Utils Tests``

open System
open FsUnit.Xunit
open Xunit
open LINQPadUtils
open LINQPadUtils.Markup
open ``Metadata Tests``

module PrimitiveValues = 
    type ``When displaying the value of a primitive``() = 
        
        [<Fact>]
        member public this.``and the primitive is a char, display the char``() = 
            let displayValue = ValueInspector.GetDisplayValue('c')
            displayValue |> should equal "c"
        
        [<Fact>]
        member public this.``and the primitive is a int, display the int``() = 
            let displayValue = ValueInspector.GetDisplayValue(1)
            displayValue |> should equal "1"
        
        [<Fact>]
        member public this.``and the primitive is a DateTime, display the date time``() = 
            let displayValue = ValueInspector.GetDisplayValue(DateTime.Parse("01/01/01"))
            displayValue |> should equal (DateTime.Parse("01/01/01").ToString())
        
        [<Fact>]
        member public this.``and it's the LINQPad RawHtml object, render markup``() = 
            let markup = LINQPad.Util.RawHtml("<span>foo</span>")
            let renderedValue = ValueInspector.GetDisplayValue(markup)
            let expectedTag = "&lt;span&gt;foo&lt;/span&gt;"
            renderedValue |> should equal expectedTag
    
    type ``When deciding whether to display a value``() = 
        
        [<Fact>]
        member public this.``it should display the value if its a primitive or string``() = 
            let canBeDisplayed, _ = ValueInspector.IsPrimitiveObject(1)
            canBeDisplayed |> should equal true
        
        [<Fact>]
        member public this.``it should not display the value if its a primitive or string``() = 
            let canBeDisplayed, _ = ValueInspector.IsPrimitiveObject([ 1 ])
            canBeDisplayed |> should equal false
    
    type ``When examining a type to see if it's a primitive sequence``() = 
        
        [<Fact>]
        member public this.``and the type is a primitive array, it should return true``() = 
            let displayArray, _ = ValueInspector.IsPrimitiveEnumerable([| 1; 2; 3 |])
            displayArray |> should equal true
        
        [<Fact>]
        member public this.``and the type is a primitive IEnumerable<T>, it should return true``() = 
            let displayArray, _ = ValueInspector.IsPrimitiveEnumerable([ 1; 2; 3 ])
            displayArray |> should equal true
        
        [<Fact>]
        member public this.``and the type does not implement IEnumerable<T>, it should return false``() = 
            let displayArray, _ = 
                ValueInspector.IsPrimitiveEnumerable({ Foo = "12"
                                                       Bar = 12 })
            displayArray |> should equal false
        
        [<Fact>]
        member public this.``and the type is a generic IEnumerable<object> containing boxed primitives, it should return false``() = 
            let displayArray, _ = 
                ValueInspector.IsPrimitiveEnumerable([ box 1
                                                       box 2 ])
            displayArray |> should equal false
    
    type ``When examining a type to see if it's a object based sequence``() = 
        
        [<Fact>]
        member public this.``and the type is an array of objects, it should return true``() = 
            let displayArray = 
                ValueInspector.IsObjectBasedEnumerable([| new obj()
                                                          new obj() |])
            displayArray |> should equal true
        
        [<Fact>]
        member public this.``and the type is a list of objects, it should return true``() = 
            let displayArray = 
                ValueInspector.IsObjectBasedEnumerable([ new obj()
                                                         new obj() ])
            displayArray |> should equal true
    
    type ``When examining a type to see if it's sequence of known objects``() = 
        [<Fact>]
        member public this.``and the type is an array of Foo, it should return true``() = 
            let displayArray, _ = 
                ValueInspector.IsEnumerableOfKnownType([| { Foo = "a"
                                                            Bar = 1 } |])
            displayArray |> should equal true

        [<Fact>]
        member public this.``and the type is a list of Foo, it should return true``() = 
            let displayArray, _ = 
                ValueInspector.IsEnumerableOfKnownType([ { Foo = "a"; Bar = 1 } ])
            displayArray |> should equal true
