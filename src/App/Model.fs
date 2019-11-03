module App.Model

open Global

open Comp.Ship
open Comp.ShipComponent

type Page =
    | Ships
    | Tech

let toHash page =
    match page with
    | Ships -> "#ships"
    | Tech -> "#tech"

type Model =
    {
        CurrentPage: Page
        CurrentShip: Ship option
        AllShips: Map<GameObjectId, Ship>
        AllComponents: Map<GameObjectId, ShipComponent>
        CurrentTechnology: GameObjectId list
        AllTechnologies: Technology.AllTechnologies
        Presets: Model.GameInfo.Preset list
        FullyInitialized: bool
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
        }
