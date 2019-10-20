module App.Model

open System

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
        AllShips: Map<Guid, Ship>
        AllComponents: Map<Guid, ShipComponent>
        CurrentTechnology: Technology.TechBase list
    }
    static member empty =
        {
            CurrentPage = Ships
            CurrentShip = None
            AllShips = Map.empty
            AllComponents = Map.empty
            CurrentTechnology = List.empty
        }
