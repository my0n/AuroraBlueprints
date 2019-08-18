module Types

open Global
open System

type FuelStorage =
    {
        Tiny: int
        Small: int
        Standard: int
        Large: int
        VeryLarge: int
        UltraLarge: int
    }
    static member empty =
        {
            Tiny = 0
            Small = 0
            Standard = 0
            Large = 0
            VeryLarge = 0
            UltraLarge = 0
        }

type ShipComponentSpec =
    | FuelStorage of FuelStorage

type ShipComponentDesign =
    {
        Guid: Guid
        Spec: ShipComponentSpec
    }

type Ship =
    {
        Guid: Guid
        Name: string
        Weight: double
        Components: ShipComponentSpec list
    }
    static member empty =
        {
            Guid = Guid.NewGuid()
            Name = "New ship"
            Weight = 0.0
            Components = []
        }

type Msg =
    | Noop

    // Ships
    | NewShip
    | RemoveShip of Ship
    | ReplaceShip of Ship
    | SelectShip of Ship
    | ShipUpdateName of Ship * string

    // Component Designs
    | NewComponentDesign of ShipComponentDesign
    | RemoveComponentDesign of ShipComponentDesign
    | ReplaceComponentDesign of ShipComponentDesign

    // Components
    | SaveComponentToDesigns of ShipComponentSpec
    | CopyComponentToShip of Ship * ShipComponentSpec
    | RemoveComponentFromShip of Ship * ShipComponentSpec

type Model =
    {
        CurrentPage: Page
        CurrentShip: Ship option
        AllShips: Map<Guid, Ship>
        AllComponents: Map<Guid, ShipComponentDesign>
    }
    static member empty =
        {
            CurrentPage = Ships
            CurrentShip = None
            AllShips = Map.empty
            AllComponents = Map.empty
        }
