module App.Model

open Global

open Comp.Ship
open Comp.ShipComponent
open Model.Technology

type Page =
    | Ships
    | Tech

let toHash page =
    match page with
    | Ships -> "#ships"
    | Tech -> "#tech"

type Model =
    {
        // stuff that gets saved
        AllShips: Map<GameObjectId, Ship>
        AllComponents: Map<GameObjectId, ShipComponent>
        CurrentTechnology: GameObjectId list

        // stuff that does not
        CurrentPage: Page
        CurrentShip: Ship option
        AllTechnologies: AllTechnologies
        Presets: GameInfo.Preset list
        FullyInitialized: bool
        CollapsedSections: string list
    }
    static member empty =
        {
            CurrentPage = Ships
            CurrentShip = None
            AllShips = Map.empty
            AllComponents = Map.empty
            CurrentTechnology = List.empty
            AllTechnologies = { Technologies = Map.empty }
            Presets = List.empty
            FullyInitialized = false
            CollapsedSections = List.empty
        }
        
module Model =
    let canDeleteComponent (model) (comp: ShipComponent) =
        not comp.BuiltIn
        &&
        not (
            model.AllShips
            |> Map.values
            |> Seq.exists (fun ship ->
                ship.Components.ContainsKey comp.Id
            )
        )
    let isExpanded key model =
        not <| List.contains key model.CollapsedSections