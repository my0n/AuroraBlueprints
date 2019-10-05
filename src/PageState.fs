module PageState

open Elmish.Browser.Navigation
open Elmish.Browser.UrlParser
open Fable.Import.Browser
open Global
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
