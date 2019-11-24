module App.State

open Elmish

open Global

open State.Model
open State.Builder
open State.Components
open State.Initialization
open State.Saving
open State.Ships
open State.Technology
open State.UI
open State.Msg
open Comp.Ship
open Model.Measures

open Saving.LocalStorage

let storage =
    {
        LoadComponents = fun allTechs ->
            load "comp" (Saving.Components.deserialize allTechs)
            |> Seq.map (function
                | Success comp -> Some comp
                | NotFound -> None
                | Failure _ -> None
            )
            |> Seq.choose id
            |> Seq.toList
        SaveComponent = fun comp ->
            comp
            |> Saving.Components.serialize
            ||||> save
        DeleteComponent = fun comp ->
            comp
            |> Saving.Components.serialize
            ||||> delete
        LoadShips = fun allTechs customComponents ->
            load "ship" (Saving.Ships.deserialize (Map.values customComponents) allTechs)
            |> Seq.map (function
                | Success ship -> Some ship
                | NotFound -> None
                | Failure _ -> None
            )
            |> Seq.choose id
            |> Seq.toList
        SaveShip = fun ship ->
            ship
            |> Saving.Ships.serialize
            ||||> save
        DeleteShip = fun ship ->
            ship
            |> Saving.Ships.serialize
            ||||> delete
        LoadCurrentTechnology = fun _ ->
            load "ct" Saving.Technology.deserialize
            |> Seq.tryHead
            |> Option.defaultValue NotFound
            |> function
                | Success ct -> ct
                | NotFound -> []
                | Failure _ -> []
        SaveCurrentTechnology = fun techs ->
            techs
            |> Saving.Technology.serialize
            ||||> save
    } : Storage

let getInitInfo () =
    promise {
        let! techs = File.CsvReader.allTechnologies
        let! gameInfo = GameInfo.gameInfo
        
        return
            (
                techs,
                gameInfo.Presets
            )
    }

let init result =
    let (model, cmd) = PageState.urlUpdate result Model.empty
    model, Cmd.batch [
        cmd
        Cmd.OfPromise.perform
            getInitInfo ()
            InitializeGame
    ]

let update msg model =
    let builderParams = (storage, model)

    match msg with
    | Noop ->
        builder builderParams {
            return Cmd.none
        }

    // Initialization
    | InitializeGame (techs, gameInfo) ->
        builder builderParams {
            do! Model.initializeModel techs gameInfo storage
            return Cmd.none
        }
    | ApplyPreset preset ->
        builder builderParams {
            let! _ = Model.setPreset preset
            return Cmd.none
        }

    // UI
    | SetSectionExpanded (section, expanded) ->
        builder builderParams {
            do! Model.setSectionExpanded section expanded
            return Cmd.none
        }
    | SelectShip ship ->
        builder builderParams {
            do! Model.setCurrentShip ship
            return Cmd.none
        }

    // Ships
    | NewShip ->
        builder builderParams {
            let! ship = Model.createNewShip
            do! Model.setCurrentShip ship
            return Cmd.none
        }
    | DuplicateShip ship ->
        builder builderParams {
            let! ship' = Model.duplicateShip ship
            do! Model.setCurrentShip ship'
            return Cmd.none
        }
    | RemoveShip ship ->
        builder builderParams {
            do! Model.removeShip ship
            return Cmd.none
        }
    | ReplaceShip ship ->
        builder builderParams {
            do! Model.replaceShip ship
            return Cmd.none
        }
    | ShipUpdateName (ship, newName) ->
        builder builderParams {
            let ship' = { ship with Name = newName }
            do! Model.replaceShip ship'
            return Cmd.none
        }
    | ShipUpdateClass (ship, newName) ->
        builder builderParams {
            let ship' = { ship with ShipClass = newName }
            do! Model.replaceShip ship'
            return Cmd.none
        }

    // Components
    | AddComponentToShip (ship, comp) ->
        builder builderParams {
            let! count = Model.getComponentCount comp ship
            let! _ = Model.setComponentCount (count + 1<comp>) comp ship
            return Cmd.none
        }
    | CopyComponentToShip (ship, comp) ->
        builder builderParams {
            let! comp' = Model.duplicateComponent comp
            let! _ = Model.setComponentCount 1<comp> comp' ship
            return Cmd.none
        }
    | RemoveComponentFromShip (ship, comp) ->
        builder builderParams {
            let! _ = Model.removeComponentFromShip comp ship
            return Cmd.none
        }
    | SetComponentCount (ship, comp, count) ->
        builder builderParams {
            let! _ = Model.setComponentCount count comp ship
            return Cmd.none
        }
    | UpdateComponent comp ->
        builder builderParams {
            let! _ = Model.updateComponent comp
            return Cmd.none
        }
    | RemoveComponent comp ->
        builder builderParams {
            let! _ = Model.removeComponent comp
            return Cmd.none
        }
    | LockComponent comp ->
        builder builderParams {
            let comp' = comp.WithLocked true
            let! _ = Model.updateComponent comp'
            return Cmd.none
        }
    | UnlockComponent comp ->
        builder builderParams {
            let comp' = comp.WithLocked false
            let! _ = Model.updateComponent comp'
            return Cmd.none
        }

    // Technologies
    | AddTechnology tech ->
        builder builderParams {
            let! _ = Model.addCurrentTechnology tech
            return Cmd.none
        }
    | RemoveTechnology tech ->
        builder builderParams {
            let! _ = Model.removeCurrentTechnology tech
            return Cmd.none
        }
