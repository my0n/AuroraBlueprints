module App.State

open Elmish
open Global
open System

open App.Model
open App.Msg
open Comp.Ship

let init result =
    let (model, cmd) = PageState.urlUpdate result Model.empty
    model, Cmd.batch [
        cmd
        Cmd.ofPromise
            (fun _ -> Technology.allTechnologies) ()
            InitializeTechnologies
            (fun err -> 
                Fable.Import.Browser.console.log (err) |> ignore
                Noop
            )
    ]
    
let orNoneIf pred inp =
    inp |> Option.bind (fun a -> match pred a with true -> None | false -> inp)

let orOtherIf pred other inp =
    inp |> Option.bind (fun a -> match pred a with true -> Some other | false -> inp)

let update msg model =
    match msg with
    | Noop ->
        model, Cmd.none
    
    // Initialization
    | InitializeTechnologies techs ->
        { model with
            AllTechnologies = techs
            AllComponents =
                Map.empty
                %+ Comp.ShipComponent.Bridge         (Comp.Bridge.Bridge.Zero)
                %+ Comp.ShipComponent.CargoHold      (Comp.CargoHold.CargoHold.Zero)
                %+ Comp.ShipComponent.Engine         (Comp.Engine.engine techs)
                %+ Comp.ShipComponent.FuelStorage    (Comp.FuelStorage.FuelStorage.Zero)
                %+ Comp.ShipComponent.Magazine       (Comp.Magazine.magazine techs)
                %+ Comp.ShipComponent.PowerPlant     (Comp.PowerPlant.powerPlant techs)
                %+ Comp.ShipComponent.Sensors        (Comp.Sensors.Sensors.Zero)
                %+ Comp.ShipComponent.TroopTransport (Comp.TroopTransport.TroopTransport.Zero)
            FullyInitialized = true
        }, Cmd.none

    // Ships
    | NewShip ->
        let ship = Comp.Ship.ship model.AllTechnologies
        { model with
            AllShips = model.AllShips %+ ship
            CurrentShip = Some ship
        }, Cmd.none
    | RemoveShip ship ->
        { model with
            AllShips = model.AllShips %- ship
            CurrentShip = model.CurrentShip |> orNoneIf (fun s -> s.Id = ship.Id)
        }, Cmd.none
    | ReplaceShip ship ->
        { model with
            AllShips = model.AllShips %+ ship
            CurrentShip = model.CurrentShip |> orOtherIf (fun s -> s.Id = ship.Id) ship
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
        { model with
            CurrentTechnology =
                match List.contains tech model.CurrentTechnology with
                | false -> model.CurrentTechnology @ [tech]
                | true ->  model.CurrentTechnology
        }, Cmd.none
    | RemoveTechnology tech ->
        let removed =
            model.CurrentTechnology
            |> List.except ((model.AllTechnologies.GetAllChildren model.CurrentTechnology tech) @ [tech])
        { model with CurrentTechnology = removed }, Cmd.none
