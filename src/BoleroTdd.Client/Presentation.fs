module BoleroTddTemplate.Client.Presentation

open Bolero
open Bolero.Html
open Bolero.Remoting
open Elmish
open Markdig
open Markdig.SyntaxHighlighting
open UseCases.Presentation

type Service =
    {
        getPresentation: int -> Async<string>
    }

    interface IRemoteService with
        member this.BasePath = "/presentation"

let adaptEffect service = function
    | None -> Cmd.none
    | MessageEffect m -> Cmd.ofMsg m
    | RetrievePresentation (pageNumber, msg) -> Cmd.OfAsync.perform service.getPresentation pageNumber msg  

type ViewTemplate = Template<"wwwroot/presentation.html">

let view model dispatch =
    let pipeline = MarkdownPipelineBuilder().UseAdvancedExtensions().UseSyntaxHighlighting().Build()
    
    ViewTemplate()
        .Markdown(cond model.presentation <| function
            | Option.None ->
                empty
            | Some markdown ->
                RawHtml <| Markdown.ToHtml(markdown, pipeline))
        .PrevPage(fun _ -> dispatch PrevPage)
        .NextPage(fun _ -> dispatch NextPage)
        .Elt()