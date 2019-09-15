module App.Model

open System

open Ship
open Comp.ShipComponent

type Page =
    | Ships

let toHash page =
    match page with
    | Ships -> "#ships"

type Model =
    {
        CurrentPage: Page
        CurrentShip: Ship option
        AllShips: Map<Guid, Ship>
        AllComponents: Map<Guid, ShipComponent>
    }
    static member empty =
        {
            CurrentPage = Ships
            CurrentShip = None
            AllShips = Map.empty
            AllComponents = Map.empty
        }
