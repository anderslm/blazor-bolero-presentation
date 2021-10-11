module BoleroTddTemplate.Client.Main

open Elmish
open Bolero
open Bolero.Remoting
open Bolero.Remoting.Client
open Bolero.Templating.Client
open UseCases
open UseCases.Main

let router = Router.infer SetPage (fun model -> model.page)

type MainTemplate = Template<"wwwroot/main.html">

let view model dispatch =
    MainTemplate()
        .Body(
            Presentation.view model.presentationModel (PresentationMsg >> dispatch)
        ).Elt()

type MyApp() =
    inherit ProgramComponent<Model, Message>()

    override this.Program =
        let presentationService = this.Remote<Presentation.Service>()
        
        let rec adaptEffect = function
            | PresentationEffect e ->
                Presentation.adaptEffect presentationService e |> Cmd.map PresentationMsg
            | BatchEffect es ->
                es
                |> List.map adaptEffect
                |> Cmd.batch
            | MessageEffect msg ->
                Cmd.ofMsg msg
            | None -> Cmd.none
        
        let update message model =
            let model, effect = update model message
            
            model, adaptEffect effect
        Program.mkProgram (fun _ -> init, Cmd.ofMsg (PresentationMsg Presentation.GetPresentation)) update view
        |> Program.withRouter router
