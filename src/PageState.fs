module PageState

open Elmish.Navigation
open Elmish.UrlParser
open Browser
open App.Model

let pageParser: Parser<Page->Page,Page> =
    oneOf [
        map Ships (s "ships")
        map Tech (s "tech")
    ]

let urlUpdate (result : Page option) model =
    match result with
    | None ->
        console.error("Error parsing url")
        model, Navigation.modifyUrl (toHash model.CurrentPage)
    | Some page ->
        { model with CurrentPage = page }, []
