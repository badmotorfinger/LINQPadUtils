namespace ``LINQPad Utils Tests``

open System
open FsUnit.Xunit
open Xunit
open LINQPadUtils
open LINQPadUtils.Markup

module RenderingTests =
    
    type ``When rendering a value to the screen``()=
    
        [<Fact>]
        member public this.``the renderer should gjoin all string fragments``()=
           
            let document = new LinqPadHtmlDocument("foo", "bar")

            document.AddRenderer (fun() -> "baz")

            let expectedOutput = "foobarbaz"

            let actualOutput = document.ToString()

            expectedOutput |> should equal actualOutput



