module ShipComponent

open System

open Measures
open Technology
open BuildCost

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
        TotalSize: float<hs>
        FuelCapacity: float<kl>
        BuildCost: BuildCost
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

            TotalSize = 0.0<hs>
            FuelCapacity = 0.0<kl>
            BuildCost = BuildCost.empty
        }
    member this.calculate =
        { this with
            TotalSize = (float this.Tiny * 0.1
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

type MaintenenceClass =
    | Commercial
    | Military

type Engine =
    {
        Guid: Guid
        ShipGuid: Guid
        
        Name: string
        Manufacturer: string

        EngineTech: EngineTech
        PowerModTech: PowerModTech
        EfficiencyTech: EngineEfficiencyTech
        Size: int<hs>
        Count: int

        // calculated values
        TotalSize: float<hs>
        EnginePower: float<ep>
        TotalEnginePower: float<ep>
        FuelConsumption: float<kl/hr>
        Crew: int
        MaintenenceClass: MaintenenceClass
        BuildCost: BuildCost
    }
    static member empty =
        {
            Guid = Guid.NewGuid()
            ShipGuid = Guid.Empty

            Name = ""
            Manufacturer = "Aurora Industries"

            EngineTech = Technology.engine.[0]
            PowerModTech = Technology.highPowerMod.[0]
            EfficiencyTech = Technology.engineEfficiency.[0]
            Size = 1<hs>
            Count = 1

            TotalSize = 0.0<hs>
            EnginePower = 0.0<ep>
            TotalEnginePower = 0.0<ep>
            FuelConsumption = 0.0<kl/hr>
            Crew = 0
            MaintenenceClass = Commercial
            BuildCost = BuildCost.empty
        }.calculate
    member this.calculate =
        let size = float this.Count * int2float this.Size
        let crew = this.Count * (float this.Size * this.PowerModTech.PowerMod |> Math.Floor |> int)
        let enginePower = int2float this.Size * this.EngineTech.PowerPerHs * this.PowerModTech.PowerMod
        let totalEp = int2float this.Count * enginePower
        let consumption = enginePower
                        * this.EfficiencyTech.Efficiency
                        * Math.Pow(this.PowerModTech.PowerMod, 2.5)
                        * (1.0 - ((int2float this.Size) / 100.0<hs>))
        let maint = match this.Size < 25<hs> || this.PowerModTech.PowerMod > 0.5 with
                    | true -> Military
                    | false -> Commercial;
        let cl = match maint with Military -> "" | Commercial -> "Commercial "
        let price = 1.0</ep> * float this.Count * this.EnginePower * (this.PowerModTech.PowerMod / 2.0)
        { this with
            Name = sprintf "%.0fEP %s%s Engine" this.EnginePower cl this.EngineTech.Name

            TotalSize = size
            FuelConsumption = consumption
            EnginePower = enginePower
            TotalEnginePower = totalEp
            Crew = crew
            MaintenenceClass = maint
            BuildCost = {
                            BuildPoints = price
                            Gallicite = price
                        }
        }

type ShipComponent =
    | Engine of Engine
    | FuelStorage of FuelStorage
    member this.Guid
        with get() =
            match this with
            | Engine c -> c.Guid
            | FuelStorage c -> c.Guid
    member this.ShipGuid
        with get() =
            match this with
            | Engine c -> c.ShipGuid
            | FuelStorage c -> c.ShipGuid
    member this.Name
        with get() =
            match this with
            | Engine c -> c.Name
            | FuelStorage c -> "Fuel Storage"
    member this.duplicate (shipGuid: Guid) =
        match this with
        | Engine c -> Engine { c with Guid = Guid.NewGuid(); ShipGuid = shipGuid }
        | FuelStorage c -> FuelStorage { c with Guid = Guid.NewGuid(); ShipGuid = shipGuid }
    member this.calculate =
        match this with
        | Engine c -> Engine c.calculate
        | FuelStorage c -> FuelStorage c.calculate
    member this.WithCount count =
        match this with
        | Engine c -> Engine { c with Count = count }
        | FuelStorage c -> FuelStorage c
