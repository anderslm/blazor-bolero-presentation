module UseCases.Presentation

type Message =
    | GetPresentation
    | GotPresentation of string
    | PrevPage
    | NextPage
    
type Effect =
    | RetrievePresentation of (int * (string -> Message))
    | MessageEffect of Message
    | None
    
type Model =
    {
        presentation : string option
        pageNumber : int
    }

let init =
    {
        presentation = Option.None
        pageNumber = 1
    }

let update model = function
    | GetPresentation ->
        model, RetrievePresentation (model.pageNumber, GotPresentation)
    | GotPresentation markdown ->
        { model with presentation = Some markdown }, None
    | PrevPage ->
        { model with pageNumber = model.pageNumber - 1 }, MessageEffect GetPresentation
    | NextPage ->
        { model with pageNumber = model.pageNumber + 1 }, MessageEffect GetPresentation