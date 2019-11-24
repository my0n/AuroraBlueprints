module App.State

open Elmish
open Global

open App.Model
open App.Msg
open Comp.Ship
open Model.Measures
open Model.Names

open Saving.LocalStorage

let getInitInfo () =
    promise {
        let! techs = File.CsvReader.allTechnologies
        let! gameInfo = GameInfo.gameInfo
        
        return
            (
                techs,
                gameInfo
            )
    }

let saveCurrentTechnologies currentTechnology =
    currentTechnology
    |> Saving.Technology.serialize
    ||||> save

let saveComponent (comp: Comp.ShipComponent.ShipComponent) =
    match comp.BuiltIn with
    | false ->
        comp
        |> Saving.Components.serialize
        ||||> save
    | true -> ()

let removeComponent comp =
    comp
    |> Saving.Components.serialize
    ||||> delete

let saveShip (ship: Comp.Ship.Ship) =
    ship
    |> Saving.Ships.serialize
    ||||> save

let removeShip (ship: Comp.Ship.Ship) =
    ship
    |> Saving.Ships.serialize
    ||||> delete

let init result =
    let (model, cmd) = PageState.urlUpdate result Model.empty
    model, Cmd.batch [
        cmd
        Cmd.OfPromise.perform
            getInitInfo ()
            InitializeGame
    ]

let update msg model =
    match msg with
    | Noop ->
        model, Cmd.none
    
    // UI
    | SetSectionExpanded (section, expanded) ->
        match expanded with
        | true ->
            { model with
                CollapsedSections = List.except [section] model.CollapsedSections
            }
        | false ->
            { model with
                CollapsedSections = model.CollapsedSections @ [section]
            }
        , Cmd.none

    // Initialization
    | InitializeGame (techs, gameInfo) ->
        let currentTechnology =
            load "ct" Saving.Technology.deserialize
            |> Seq.tryHead
            |> Option.defaultValue NotFound
            |> function
                | Success ct -> ct
                | NotFound -> []
                | Failure _ -> []

        let applyPreset =
            match currentTechnology with
            | [] -> Cmd.ofMsg <| ApplyPreset gameInfo.DefaultPreset
            | _ -> Cmd.none

        let customComponents =
            load "comp" (Saving.Components.deserialize techs)
            |> Seq.map (function
                | Success comp -> Some comp
                | NotFound -> None
                | Failure _ -> None
            )
            |> Seq.choose id
            |> Seq.map (fun comp -> comp.Id, comp)
            |> Map.ofSeq

        let customShips =
            load "ship" (Saving.Ships.deserialize (Map.values customComponents) techs)
            |> Seq.map (function
                | Success ship -> Some ship
                | NotFound -> None
                | Failure _ -> None
            )
            |> Seq.choose id
            |> Seq.map (fun ship -> ship.Id, ship)
            |> Map.ofSeq

        { model with
            AllTechnologies = techs
            CurrentTechnology = currentTechnology
            AllComponents =
                Map.empty
                %+ Comp.ShipComponent.Bridge         ({ Comp.Bridge.Bridge.Zero with BuiltIn = true })
                %+ Comp.ShipComponent.CargoHold      ({ Comp.CargoHold.CargoHold.Zero with BuiltIn = true })
                %+ Comp.ShipComponent.Engine         ({ Comp.Engine.engine techs with Name = "Engine"; BuiltIn = true })
                %+ Comp.ShipComponent.FuelStorage    ({ Comp.FuelStorage.FuelStorage.Zero with BuiltIn = true })
                %+ Comp.ShipComponent.Magazine       ({ Comp.Magazine.magazine techs with Name = "Magazine"; BuiltIn = true })
                %+ Comp.ShipComponent.PowerPlant     ({ Comp.PowerPlant.powerPlant techs with Name = "Power Plant"; BuiltIn = true })
                %+ Comp.ShipComponent.Sensors        ({ Comp.Sensors.Sensors.Zero with BuiltIn = true })
                %+ Comp.ShipComponent.TroopTransport ({ Comp.TroopTransport.TroopTransport.Zero with BuiltIn = true })
                %@ customComponents
            AllShips = customShips
            Presets = gameInfo.Presets
            FullyInitialized = true
        }, Cmd.batch [
            applyPreset
        ]
    | ApplyPreset preset ->
        let preset' =
            model.Presets
            |> List.tryFind (fun p -> p.Name = preset)
        match preset' with
        | None ->
            model, Cmd.none
        | Some preset' ->
            { model with
                CurrentTechnology = preset'.Technologies
            }, Cmd.none

    // Ships
    | NewShip ->
        let ship = Comp.Ship.ship model.AllTechnologies
        saveShip ship
        { model with
            AllShips = model.AllShips %+ ship
            CurrentShip = Some ship
        },
        Cmd.none
    | DuplicateShip ship ->
        let ship' =
            { ship with
                Id = GameObjectId.generate ()
                Name = nameOfCopy ship.Name
            }
        saveShip ship'
        { model with
            AllShips = model.AllShips %+ ship'
            CurrentShip = Some ship'
        },
        Cmd.none
    | RemoveShip ship ->
        removeShip ship

        let orNoneIf pred inp =
            inp |> Option.bind (fun a -> match pred a with true -> None | false -> inp)

        { model with
            AllShips = model.AllShips %- ship
            CurrentShip = model.CurrentShip |> orNoneIf (fun s -> s.Id = ship.Id)
        },
        Cmd.none
    | ReplaceShip ship ->
        saveShip ship

        let orOtherIf pred other inp =
            inp |> Option.bind (fun a -> match pred a with true -> Some other | false -> inp)

        { model with
            AllShips = model.AllShips %+ ship
            CurrentShip = model.CurrentShip |> orOtherIf (fun s -> s.Id = ship.Id) ship
        },
        Cmd.none
    | ShipUpdateName (ship, newName) ->
        model,
        ReplaceShip { ship with Name = newName }
        |> Cmd.ofMsg
    | ShipUpdateClass (ship, newName) ->
        model,
        ReplaceShip { ship with ShipClass = newName }
        |> Cmd.ofMsg
    | SelectShip ship ->
        { model with
            CurrentShip =
                model.AllShips
                |> Map.tryFind ship.Id
        },
        Cmd.none
    // Components
    | AddComponentToShip (ship, comp) ->
        model,
        match comp.BuiltIn with
        | false ->
            ship.Components
            |> Map.tryFind comp.Id
            |> function
            | None ->
                ReplaceShip
                    { ship with
                        Components =
                            ship.Components.Add(
                                comp.Id,
                                (1<comp>, comp)
                            )
                    }
                |> Cmd.ofMsg
            | Some (count, _) ->
                match comp.Composite with
                | true ->
                    Cmd.none
                | false ->
                    SetComponentCount (ship, comp, count + 1<comp>)
                    |> Cmd.ofMsg
        | true ->
            [
                UpdateComponent comp
                ReplaceShip
                    { ship with
                        Components =
                            ship.Components.Add(
                                comp.Id,
                                (1<comp>, comp)
                            )
                    }
            ]
            |> List.map Cmd.ofMsg
            |> Cmd.batch
    | CopyComponentToShip (ship, comp) ->
        let comp' = comp.duplicate
        model,
        [
            UpdateComponent comp'
            AddComponentToShip (ship, comp')
        ]
        |> List.map Cmd.ofMsg
        |> Cmd.batch
    | UpdateComponentInShip (ship, comp) ->
        model,
        ship.Components
        |> Map.tryFind comp.Id
        |> function
        | None ->
            Cmd.none
        | Some (count, _) ->
            [
                ReplaceShip
                    { ship with
                        Components =
                            ship.Components.Add(
                                comp.Id,
                                (count, comp)
                            )
                    }
            ]
            |> List.map Cmd.ofMsg
            |> Cmd.batch
    | RemoveComponentFromShip (ship, comp) ->
        let replaceShip =
            ReplaceShip
                { ship with
                    Components =
                        ship.Components.Remove comp.Id
                }
            |> Cmd.ofMsg
        let cleanup =
            match (comp.Locked, comp.Composite) with
            | false, true -> Cmd.ofMsg <| RemoveComponent comp
            | _ -> Cmd.none

        model, Cmd.batch [
            replaceShip
            cleanup
        ]
    | SetComponentCount (ship, comp, count) ->
        let count' =
            match comp.Composite with
            | true -> 1<comp>
            | false -> count

        model,
        ReplaceShip
            { ship with
                Components =
                    ship.Components.Add(
                        comp.Id,
                        (count', comp)
                    )
            }
        |> Cmd.ofMsg
    | UpdateComponent comp ->
        saveComponent comp

        { model with
            AllComponents = model.AllComponents.Add(comp.Id, comp)
        },
        model.AllShips
        |> Map.values
        |> Seq.filter (fun ship -> ship.Components.ContainsKey comp.Id)
        |> Seq.map (fun ship ->
            UpdateComponentInShip (ship, comp)
            |> Cmd.ofMsg
        )
        |> Cmd.batch
    | RemoveComponent comp ->
        match Model.canDeleteComponent model comp with
        | true ->
            removeComponent comp
            
            { model with
                AllComponents = model.AllComponents.Remove comp.Id
            },
            Cmd.none
        | false ->
            model,
            Cmd.none
    | LockComponent comp ->
        model,
        UpdateComponent <| comp.WithLocked true
        |> Cmd.ofMsg
    | UnlockComponent comp ->
        model,
        UpdateComponent <| comp.WithLocked false
        |> Cmd.ofMsg

    // Technologies
    | AddTechnology tech ->
        let model =
            { model with
                CurrentTechnology =
                    match List.contains tech model.CurrentTechnology with
                    | false -> model.CurrentTechnology @ [tech]
                    | true ->  model.CurrentTechnology
            }

        saveCurrentTechnologies model.CurrentTechnology
        model, Cmd.none
    | RemoveTechnology tech ->
        let removed =
            model.CurrentTechnology
            |> List.except ((model.AllTechnologies.GetAllChildren model.CurrentTechnology tech) @ [tech])

        let model =
            { model with
                CurrentTechnology = removed
            }
        
        saveCurrentTechnologies model.CurrentTechnology
        model, Cmd.none
