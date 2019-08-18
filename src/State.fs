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

let orOtherIf pred other inp =
    inp |> Option.bind (fun a -> match pred a with true -> Some other | false -> inp)

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
    | ReplaceShip (guid, newShip) ->
        { model with
            Ships = model.Ships %+ (guid, newShip)
            CurrentShip = model.CurrentShip |> orOtherIf (fun ship -> ship.Guid = guid) newShip
        }, Cmd.none
    | ShipUpdateName (ship, newName) ->
        model, Cmd.ofMsg (ReplaceShip (ship.Guid, { ship with Name = newName }))
    | SelectShip ship ->
        { model with
            CurrentShip = Some ship
        }, Cmd.none
