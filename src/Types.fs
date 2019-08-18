module Types

open Global
open System

type ShipComponent =
    | FuelStorage

type Ship =
    {
        Guid: Guid
        Name: string
        Weight: double
        Components: ShipComponent list
    }
    static member empty =
        {
            Guid = Guid.NewGuid()
            Name = "New ship"
            Weight = 0.0
            Components = []
        }

type Msg =
    | NewShip
    | RemoveShip of Guid
    | ReplaceShip of Guid * Ship
    | SelectShip of Ship
    | ShipUpdateName of Ship * string

type Model =
    {
        CurrentPage: Page
        CurrentShip: Ship option
        Ships: Map<Guid, Ship>
    }
    static member empty =
        {
            CurrentPage = Ships
            CurrentShip = None
            Ships = Map.empty
        }
