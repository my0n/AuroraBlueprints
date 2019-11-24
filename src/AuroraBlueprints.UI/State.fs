module App.State

open Elmish

open App.Model
open App.Model.Builder
open App.Model.Components
open App.Model.Initialization
open App.Model.Ships
open App.Model.Technology
open App.Model.UI
open App.Msg
open Comp.Ship
open Model.Measures

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

    // Initialization
    | InitializeGame (techs, gameInfo) ->
        builder model {
            do! Model.initializeModel techs gameInfo
            return Cmd.none
        }
    | ApplyPreset preset ->
        builder model {
            let! _ = Model.setPreset preset
            return Cmd.none
        }

    // UI
    | SetSectionExpanded (section, expanded) ->
        builder model {
            do! Model.setSectionExpanded section expanded
            return Cmd.none
        }
    | SelectShip ship ->
        builder model {
            do! Model.setCurrentShip ship
            return Cmd.none
        }

    // Ships
    | NewShip ->
        builder model {
            let! ship = Model.createNewShip
            do! Model.setCurrentShip ship
            return Cmd.none
        }
    | DuplicateShip ship ->
        builder model {
            let! ship' = Model.duplicateShip ship
            do! Model.setCurrentShip ship'
            return Cmd.none
        }
    | RemoveShip ship ->
        builder model {
            do! Model.removeShip ship
            return Cmd.none
        }
    | ReplaceShip ship ->
        builder model {
            do! Model.replaceShip ship
            return Cmd.none
        }
    | ShipUpdateName (ship, newName) ->
        builder model {
            let ship' = { ship with Name = newName }
            do! Model.replaceShip ship'
            return Cmd.none
        }
    | ShipUpdateClass (ship, newName) ->
        builder model {
            let ship' = { ship with ShipClass = newName }
            do! Model.replaceShip ship'
            return Cmd.none
        }

    // Components
    | AddComponentToShip (ship, comp) ->
        builder model {
            let! count = Model.getComponentCount comp ship
            let! _ = Model.setComponentCount (count + 1<comp>) comp ship
            return Cmd.none
        }
    | CopyComponentToShip (ship, comp) ->
        builder model {
            let! comp' = Model.duplicateComponent comp
            let! _ = Model.setComponentCount 1<comp> comp' ship
            return Cmd.none
        }
    | RemoveComponentFromShip (ship, comp) ->
        builder model {
            let! _ = Model.removeComponentFromShip comp ship
            return Cmd.none
        }
    | SetComponentCount (ship, comp, count) ->
        builder model {
            let! _ = Model.setComponentCount count comp ship
            return Cmd.none
        }
    | UpdateComponent comp ->
        builder model {
            let! _ = Model.updateComponent comp
            return Cmd.none
        }
    | RemoveComponent comp ->
        builder model {
            let! _ = Model.removeComponent comp
            return Cmd.none
        }
    | LockComponent comp ->
        builder model {
            let comp' = comp.WithLocked true
            let! _ = Model.updateComponent comp'
            return Cmd.none
        }
    | UnlockComponent comp ->
        builder model {
            let comp' = comp.WithLocked false
            let! _ = Model.updateComponent comp'
            return Cmd.none
        }

    // Technologies
    | AddTechnology tech ->
        builder model {
            let! _ = Model.addCurrentTechnology tech
            return Cmd.none
        }
    | RemoveTechnology tech ->
        builder model {
            let! _ = Model.removeCurrentTechnology tech
            return Cmd.none
        }
