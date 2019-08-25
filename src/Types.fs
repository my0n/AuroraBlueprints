module Types

open Global
open System
open Measures
open ShipComponent

type Ship =
    {
        Guid: Guid
        Name: string
        ShipClass: string
        Size: float<hs>
        Components: Map<Guid, ShipComponent>
    }
    static member empty =
        {
            Guid = Guid.NewGuid()
            Name = "Tribal"
            ShipClass = "Cruiser"
            Size = 0.0<hs>
            Components = Map.empty
        }
    member this.calculate =
        let size =
            this.Components
            |> Map.values
            |> List.map (fun c ->
                match c with
                | FuelStorage c -> c.TotalSize
                | Engine c      -> int2float c.Size * int2float c.Count
                | Bridge c      -> int2float c.Size * int2float c.Count
                )
            |> List.sum

        { this with
            Size = size
        }

type Msg =
    | Noop

    // Ships
    | NewShip
    | RemoveShip of Ship
    | ReplaceShip of Ship
    | SelectShip of Ship
    | ShipUpdateName of Ship * string
    | ShipUpdateClass of Ship * string

    // Component Designs
    | NewComponentDesign of ShipComponent
    | RemoveComponentDesign of ShipComponent
    | ReplaceComponentDesign of ShipComponent

    // Components
    | SaveComponentToDesigns of ShipComponent
    | CopyComponentToShip of Ship * ShipComponent
    | RemoveComponentFromShip of ShipComponent
    | ReplaceShipComponent of ShipComponent

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
