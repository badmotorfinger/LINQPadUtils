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
            let displayValue = ValueDisplay.GetDisplayValue('c')
            displayValue |> should equal "c"
        
        [<Fact>]
        member public this.``and the primitive is a int, display the int``() = 
            let displayValue = ValueDisplay.GetDisplayValue(1)
            displayValue |> should equal "1"
        
        [<Fact>]
        member public this.``and the primitive is a DateTime, display the date time``() = 
            let displayValue = ValueDisplay.GetDisplayValue(DateTime.Parse("01/01/01"))
            displayValue |> should equal (DateTime.Parse("01/01/01").ToString())
    
    type ``When deciding whether to display a value``() = 
        
        [<Fact>]
        member public this.``it should display the value if its a primitive or string``() = 
            let canBeDisplayed = ValueDisplay.IsPlainVanilaDisplay(1)
            canBeDisplayed |> should equal true
        
        [<Fact>]
        member public this.``it should not display the value if its a primitive or string``() = 
            let canBeDisplayed = ValueDisplay.IsPlainVanilaDisplay([ 1 ])
            canBeDisplayed |> should equal false

//    type ``When displaying a primitive``() = 
//        
//        [<Fact>]
//        member public this.``it should be a HTML document with a single value``() = 
//            
//            let table = new TableBuilder("foo")
//
//            let tag = HtmlTag.WrapValue("span")
//
//            let expectedHtml = LinqPadUtilResources.DumpExtended.Replace("{content}", "foo")
//
//            let actualHtml = table.ToString()
//
//            actualHtml |> should equal expectedHtml

    type ``When examining a type``() =

        [<Fact>]
        member public this.``and the type is a primitive collection``()=
            let displayArray = ValueDisplay.IsPrimitiveEnumerable([|1;2;3|])
            displayArray |> should equal true