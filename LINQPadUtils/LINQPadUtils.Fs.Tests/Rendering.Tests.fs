namespace ``LINQPad Utils Tests``

open System
open FsUnit.Xunit
open Xunit
open LINQPadUtils
open LINQPadUtils.Markup

module RenderingTests =

    type ``When rendering a value to the screen``()=

        [<Fact>]
        member public this.``the renderer should join all string fragments``()=

            let document = new LinqPadHtmlDocument("foo", "bar")

            document.AddRenderer (fun() -> "baz")

            let expectedOutput = "foobazbar"

            let actualOutput = document.ToString()

            expectedOutput |> should equal actualOutput

        [<Fact>]
        member public this.``and it's a simple primitive type, render the tag``()=

            let tag = HtmlTag.WrapValue("xx", "yy")

            let newTag = tag.ToString()

            let expectedTag = "<xx>yy</xx>"

            newTag |> should equal expectedTag
