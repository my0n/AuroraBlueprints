module App.State

open Elmish
open Global
open System

open App.Model
open App.Msg
open Comp.Ship

open Model.Technology

let init result =
    let (model, cmd) =
        PageState.urlUpdate result
            {
                CurrentPage = Ships
                CurrentShip = None
                AllShips = Map.empty
                AllComponents = Map.empty
                                %+ Comp.ShipComponent.Bridge         Comp.Bridge.Bridge.Zero
                                %+ Comp.ShipComponent.CargoHold      Comp.CargoHold.CargoHold.Zero
                                %+ Comp.ShipComponent.Engine         Comp.Engine.Engine.Zero
                                %+ Comp.ShipComponent.FuelStorage    Comp.FuelStorage.FuelStorage.Zero
                                %+ Comp.ShipComponent.PowerPlant     Comp.PowerPlant.PowerPlant.Zero
                                %+ Comp.ShipComponent.Sensors        Comp.Sensors.Sensors.Zero
                                %+ Comp.ShipComponent.TroopTransport Comp.TroopTransport.TroopTransport.Zero
                CurrentTechnology = Set.empty
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
        let ship = Ship.Zero
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
    | ShipUpdateClass (ship, newName) ->
        model, Cmd.ofMsg (ReplaceShip { ship with ShipClass = newName })
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
        model, Cmd.ofMsg (NewComponentDesign shipComponent.duplicate)
    | CopyComponentToShip (ship, shipComponent) ->
        model, Cmd.ofMsg (ReplaceShip { ship with Components = ship.Components %+ shipComponent.duplicate})
    | RemoveComponentFromShip (ship, shipComponent) ->
        model, Cmd.ofMsg (ReplaceShip { ship with Components = ship.Components %- shipComponent })
    | ReplaceShipComponent (ship, shipComponent) ->
        model, Cmd.ofMsg (ReplaceShip { ship with Components = ship.Components %+ shipComponent })

    // Technologies
    | AddTechnology tech ->
        { model with CurrentTechnology = model.CurrentTechnology |> Set.add tech }, Cmd.none
    | RemoveTechnology tech ->
        let rec parents unchk chk =
            match unchk with
            | [] -> chk
            | x::xs ->
                let p =
                    allTechnologies
                    |> List.filter (fun t -> t.Parents |> List.contains x)
                    |> List.map (fun t -> t.Tech)
                    |> List.filter model.CurrentTechnology.Contains
                parents (xs @ p) (chk @ [x])
        { model with CurrentTechnology = Set.difference model.CurrentTechnology (Set.ofList <| parents [tech] []) }, Cmd.none
