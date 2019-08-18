module App.State

open Elmish
open Elmish.Browser.Navigation
open Elmish.Browser.UrlParser
open Fable.Import.Browser
open Global
open Types
open System

let pageParser: Parser<Page->Page,Page> =
    oneOf [
        map Ships (s "ships")
    ]

let urlUpdate (result : Page option) model =
    match result with
    | None ->
        console.error("Error parsing url")
        model, Navigation.modifyUrl (toHash model.CurrentPage)
    | Some page ->
        { model with CurrentPage = page }, []

let init result =
    let (model, cmd) =
        urlUpdate result
            {
                CurrentPage = Ships
                CurrentShip = None
                Ships = Map.empty
            }

    model, Cmd.batch [ cmd ]

let update msg model =
    match msg with
    | NewShip ->
        let ship = Types.Ship.empty
        { model with
            Ships = model.Ships %+ (ship.Guid, ship)
        }, Cmd.none
    | RemoveShip guid ->
        { model with
            Ships = model.Ships %- guid
        }, Cmd.none
    | ReplaceShip (guid, ship) ->
        { model with
            Ships = model.Ships %+ (guid, ship)
        }, Cmd.none
