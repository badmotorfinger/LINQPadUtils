namespace ``LINQPad Utils Tests``

open System
open FsUnit.Xunit
open Xunit
open LINQPadUtils
open LINQPadUtils.Markup

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
        member public this.``and it's the LINQPad RawHtml object, render markup``()=
            
            let markup = LINQPad.Util.RawHtml("<span>foo</span>")

            let renderedValue = ValueInspector.GetDisplayValue(markup)

            let expectedTag = "<span>foo</span>"

            renderedValue |> should equal expectedTag
    
    type ``When deciding whether to display a value``() = 
        
        [<Fact>]
        member public this.``it should display the value if its a primitive or string``() = 
            let canBeDisplayed = ValueInspector.IsPrimitiveObject(1)
            canBeDisplayed |> should equal true
        
        [<Fact>]
        member public this.``it should not display the value if its a primitive or string``() = 
            let canBeDisplayed = ValueInspector.IsPrimitiveObject([ 1 ])
            canBeDisplayed |> should equal false
    
    type ``When examining a type``() = 
        
        [<Fact>]
        member public this.``and the type is a primitive array, it should return true``() = 
            let displayArray = ValueInspector.IsPrimitiveEnumerable([| 1; 2; 3 |])
            displayArray |> should equal true
        
        [<Fact>]
        member public this.``and the type is a primitive IEnumerable<T>, it should return true``() = 
            let displayArray = ValueInspector.IsPrimitiveEnumerable([ 1; 2; 3 ])
            displayArray |> should equal true
    
    