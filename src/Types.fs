module Types

open Global
open System

type FuelStorage =
    {
        Guid: Guid
        ShipGuid: Guid

        Tiny: int
        Small: int
        Standard: int
        Large: int
        VeryLarge: int
        UltraLarge: int
        
        // calculated values
        Size: float<hs>
        FuelCapacity: float<kl>
    }
    static member empty =
        {
            Guid = Guid.NewGuid()
            ShipGuid = Guid.Empty

            Tiny = 0
            Small = 0
            Standard = 0
            Large = 0
            VeryLarge = 0
            UltraLarge = 0

            Size = 0.0<hs>
            FuelCapacity = 0.0<kl>
        }
    member this.calculate =
        { this with
            Size = (float this.Tiny * 0.1
                  + float this.Small * 0.2
                  + float this.Standard * 1.0
                  + float this.Large * 5.0
                  + float this.VeryLarge * 20.0
                  + float this.UltraLarge * 100.0) * 1.0<hs>
            FuelCapacity = (float this.Tiny * 1.0
                          + float this.Small * 2.0
                          + float this.Standard * 10.0
                          + float this.Large * 50.0
                          + float this.VeryLarge * 200.0
                          + float this.UltraLarge * 1000.0) * 5.0<kl>
        }

type ShipComponent =
    | FuelStorage of FuelStorage
    member this.Guid
        with get() =
            match this with
            | FuelStorage c -> c.Guid
    member this.ShipGuid
        with get() =
            match this with
            | FuelStorage c -> c.ShipGuid
    member this.duplicate (shipGuid: Guid) =
        match this with
        | FuelStorage c -> FuelStorage { c with Guid = Guid.NewGuid(); ShipGuid = shipGuid }
    member this.calculate =
        match this with
        | FuelStorage c -> FuelStorage c.calculate

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
                | FuelStorage c ->
                    c.Size
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
