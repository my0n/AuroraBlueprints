module App.State

open Elmish
open Global
open Types
open System

let init result =
    let (model, cmd) =
        PageState.urlUpdate result
            {
                CurrentPage = Ships
                CurrentShip = None
                AllShips = Map.empty
                AllComponents = Map.empty
                                %+ FuelStorage FuelStorage.empty
            }

    model, Cmd.batch [ cmd ]
    
let orNoneIf pred inp =
    inp |> Option.bind (fun a -> match pred a with true -> None | false -> inp)

let orOtherIf pred other inp =
    inp |> Option.bind (fun a -> match pred a with true -> Some other | false -> inp)

let update msg model =
    match msg with
    | Noop ->
        model, Cmd.none
        
    // Ships
    | NewShip ->
        let ship = Types.Ship.empty
        { model with
            AllShips = model.AllShips %+ ship
            CurrentShip = Some ship
        }, Cmd.none
    | RemoveShip ship ->
        { model with
            AllShips = model.AllShips %- ship
            CurrentShip = model.CurrentShip |> orNoneIf (fun s -> s.Guid = ship.Guid)
        }, Cmd.none
    | ReplaceShip ship ->
        { model with
            AllShips = model.AllShips %+ ship
            CurrentShip = model.CurrentShip |> orOtherIf (fun s -> s.Guid = ship.Guid) ship
        }, Cmd.none
    | ShipUpdateName (ship, newName) ->
        model, Cmd.ofMsg (ReplaceShip { ship with Name = newName })
    | SelectShip ship ->
        { model with
            CurrentShip = Some ship
        }, Cmd.none

    // Components - Design
    | NewComponentDesign shipComponent ->
        { model with
            AllComponents = model.AllComponents %+ shipComponent
        }, Cmd.none
    | RemoveComponentDesign shipComponent ->
        { model with
            AllComponents = model.AllComponents %- shipComponent
        }, Cmd.none
    | ReplaceComponentDesign shipComponent ->
        { model with
            AllComponents = model.AllComponents %+ shipComponent
        }, Cmd.none

    // Components
    | SaveComponentToDesigns shipComponent ->
        model, Cmd.ofMsg (NewComponentDesign (shipComponent.duplicate Guid.Empty))
    | CopyComponentToShip (ship, shipComponent) ->
        model, Cmd.ofMsg (ReplaceShip { ship with Components = ship.Components %+ (shipComponent.duplicate ship.Guid)})
    | RemoveComponentFromShip shipComponent ->
        let ship = model.AllShips.TryFind shipComponent.ShipGuid
        match ship with
        | None -> model, Cmd.none
        | Some ship ->
            model, Cmd.ofMsg (ReplaceShip { ship with Components = ship.Components %- shipComponent })
    | ReplaceShipComponent shipComponent ->
        let ship = model.AllShips.TryFind shipComponent.ShipGuid
        match ship with
        | None -> model, Cmd.none
        | Some ship ->
            model, Cmd.ofMsg (ReplaceShip { ship with Components = ship.Components %+ shipComponent.calculate })
