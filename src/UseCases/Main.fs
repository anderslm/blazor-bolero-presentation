module UseCases.Main

type Page =
    | Home of int
    
type Message =
    | SetPage of Page
    | PresentationMsg of Presentation.Message
    
type Effect =
    | PresentationEffect of Presentation.Effect
    | BatchEffect of Effect list
    | MessageEffect of Message
    | None
    
type Model =
    {
        page: Page
        presentationModel: Presentation.Model
    }
    
let init =
    {
        page = Home 1
        presentationModel = Presentation.init
    }
    
let update model = function
    | SetPage page ->
        match page with
        | Home pageNumber ->
            { model with
                page = page
                presentationModel = { model.presentationModel with pageNumber = pageNumber }}, None
    | PresentationMsg msg ->
        let m, e = Presentation.update model.presentationModel msg
        
        { model with presentationModel = m }
        , BatchEffect [ PresentationEffect e ; MessageEffect <| SetPage (Home m.pageNumber) ]
