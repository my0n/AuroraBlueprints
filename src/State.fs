module App.State

open Elmish
open Global
open Types

let init result =
    let (model, cmd) =
        PageState.urlUpdate result
            {
                CurrentPage = Ships
                CurrentShip = None
                Ships = Map.empty
            }

    model, Cmd.batch [ cmd ]

let orNoneIf pred inp =
    inp |> Option.bind (fun a -> match pred a with true -> None | false -> inp)

let update msg model =
    match msg with
    | NewShip ->
        let ship = Types.Ship.empty
        { model with
            Ships = model.Ships %+ (ship.Guid, ship)
            CurrentShip = Some ship
        }, Cmd.none
    | RemoveShip guid ->
        { model with
            Ships = model.Ships %- guid
            CurrentShip = model.CurrentShip |> orNoneIf (fun ship -> ship.Guid = guid)
        }, Cmd.none
    | ReplaceShip (guid, ship) ->
        { model with
            Ships = model.Ships %+ (guid, ship)
            CurrentShip = Some ship
        }, Cmd.none
    | SelectShip ship ->
        { model with
            CurrentShip = Some ship
        }, Cmd.none
