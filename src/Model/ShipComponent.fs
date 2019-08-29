module Model.ShipComponent

open System

open Model.BuildCost
open Model.Measures
open Model.Technology

type MaintenanceClass =
    | Commercial
    | Military

type Bridge =
    {
        Guid: Guid
        ShipGuid: Guid

        Count: int<comp>
    }
    static member empty =
        {
            Guid = Guid.NewGuid()
            ShipGuid = Guid.Empty
            Count = 1<comp>
        }
    member this.Size = 1<hs/comp>
    member this.Crew = 5<people/comp>
    member this.BuildCost =
        { BuildCost.Zero with
            BuildPoints = 10.0</comp>
            Duranium = 5.0</comp>
            Corbomite = 5.0</comp>
        }

type Sensors =
    {
        Guid: Guid
        ShipGuid: Guid
        
        StandardGeo: int<comp>
        ImprovedGeo: int<comp>
        AdvancedGeo: int<comp>
        PhasedGeo: int<comp>
        
        StandardGrav: int<comp>
        ImprovedGrav: int<comp>
        AdvancedGrav: int<comp>
        PhasedGrav: int<comp>

        // calculated values
        Size: int<hs>
        Crew: int<people>
        BuildCost: TotalBuildCost
        GeoSensorRating: int
        GravSensorRating: int
        MaintenenceClass: MaintenanceClass
    }
    static member empty =
        {
            Guid = Guid.NewGuid()
            ShipGuid = Guid.Empty
            StandardGeo = 0<comp>
            ImprovedGeo = 0<comp>
            AdvancedGeo = 0<comp>
            PhasedGeo = 0<comp>
            StandardGrav = 0<comp>
            ImprovedGrav = 0<comp>
            AdvancedGrav = 0<comp>
            PhasedGrav = 0<comp>

            Size = 0<hs>
            Crew = 0<people>
            BuildCost = TotalBuildCost.Zero
            GeoSensorRating = 0
            GravSensorRating = 0
            MaintenenceClass = Commercial
        }
    member this.calculate =
        let total = this.StandardGeo + this.StandardGrav
                  + this.ImprovedGeo + this.ImprovedGrav
                  + this.AdvancedGeo + this.AdvancedGrav
                  + this.PhasedGeo + this.PhasedGrav
        let cost = ((this.StandardGeo + this.StandardGrav) * 100
                  + (this.ImprovedGeo + this.ImprovedGrav) * 150
                  + (this.AdvancedGeo + this.AdvancedGrav) * 200
                  + (this.PhasedGeo + this.PhasedGrav) * 300
                   ) * 1</comp>
        let maint = match [ this.StandardGrav; this.ImprovedGrav; this.AdvancedGrav; this.PhasedGrav ]
                          |> List.exists (fun a -> a > 0<comp>) with
                    | true -> Military
                    | false -> Commercial
        { this with
            Size = total * 5<hs/comp>
            Crew = total * 10<people/comp>
            GeoSensorRating = (this.StandardGeo * 1
                             + this.ImprovedGeo * 2
                             + this.AdvancedGeo * 3
                             + this.PhasedGeo * 5
                              ) * 1</comp>
            GravSensorRating = (this.StandardGrav * 1
                              + this.ImprovedGrav * 2
                              + this.AdvancedGrav * 3
                              + this.PhasedGrav * 5
                               ) * 1</comp>
            BuildCost =
                { TotalBuildCost.Zero with
                    BuildPoints = float cost
                    Uridium = float cost
                }
            MaintenenceClass = maint
        }

type FuelStorage =
    {
        Guid: Guid
        ShipGuid: Guid

        Tiny: int<comp>
        Small: int<comp>
        Standard: int<comp>
        Large: int<comp>
        VeryLarge: int<comp>
        UltraLarge: int<comp>
        
        // calculated values
        TotalSize: float<hs>
        FuelCapacity: float<kl>
        BuildCost: TotalBuildCost
    }
    static member empty =
        {
            Guid = Guid.NewGuid()
            ShipGuid = Guid.Empty

            Tiny = 0<comp>
            Small = 0<comp>
            Standard = 0<comp>
            Large = 0<comp>
            VeryLarge = 0<comp>
            UltraLarge = 0<comp>

            TotalSize = 0.0<hs>
            FuelCapacity = 0.0<kl>
            BuildCost = TotalBuildCost.Zero
        }
    member this.calculate =
        // cost does not scale proportionately to size and capacity
        let cost = (int2float this.Tiny * 1.0
                  + int2float this.Small * 1.5
                  + int2float this.Standard * 5.0
                  + int2float this.Large * 15.0
                  + int2float this.VeryLarge * 35.0
                  + int2float this.UltraLarge * 100.0) * 1.0</comp>
        { this with
            TotalSize = (int2float this.Tiny * 0.1
                       + int2float this.Small * 0.2
                       + int2float this.Standard * 1.0
                       + int2float this.Large * 5.0
                       + int2float this.VeryLarge * 20.0
                       + int2float this.UltraLarge * 100.0) * 1.0<hs/comp>
            FuelCapacity = (int2float this.Tiny * 1.0
                          + int2float this.Small * 2.0
                          + int2float this.Standard * 10.0
                          + int2float this.Large * 50.0
                          + int2float this.VeryLarge * 200.0
                          + int2float this.UltraLarge * 1000.0) * 5.0<kl/comp>
            BuildCost =
                { TotalBuildCost.Zero with
                    BuildPoints = cost * 2.0
                    Duranium = cost
                    Boronide = cost
                }
        }

type Engine =
    {
        Guid: Guid
        ShipGuid: Guid
        
        Name: string
        Manufacturer: string

        EngineTech: EngineTech
        PowerModTech: PowerModTech
        EfficiencyTech: EngineEfficiencyTech
        Size: int<hs/comp>
        Count: int<comp>

        // calculated values
        EnginePower: float<ep/comp>
        FuelConsumption: float<kl/hr/comp>
        Crew: int<people/comp>
        MaintenenceClass: MaintenanceClass
        BuildCost: BuildCost
    }
    static member empty =
        {
            Guid = Guid.NewGuid()
            ShipGuid = Guid.Empty

            Name = ""
            Manufacturer = "Aurora Industries"

            EngineTech = Technology.engine.[0]
            PowerModTech = Technology.allPowerMods.[0]
            EfficiencyTech = Technology.engineEfficiency.[0]
            Size = 1<hs/comp>
            Count = 1<comp>

            EnginePower = 0.0<ep/comp>
            FuelConsumption = 0.0<kl/hr/comp>
            Crew = 0<people/comp>
            MaintenenceClass = Commercial
            BuildCost = BuildCost.Zero
        }.calculate
    member this.calculate =
        let crew = (float this.Size * this.PowerModTech.PowerMod |> Math.Floor |> int) * 1<people/comp>
        let enginePower = int2float this.Size * this.EngineTech.PowerPerHs * this.PowerModTech.PowerMod
        let consumption = enginePower
                        * this.EfficiencyTech.Efficiency
                        * Math.Pow(this.PowerModTech.PowerMod, 2.5)
                        * (1.0 - ((int2float this.Size) / 100.0<hs/comp>))
        let maint = match this.Size < 25<hs/comp> || this.PowerModTech.PowerMod > 0.5 with
                    | true -> Military
                    | false -> Commercial;
        let cl = match maint with Military -> "" | Commercial -> "Commercial "
        let price = 1.0</ep> * enginePower * (this.PowerModTech.PowerMod / 2.0)
        { this with
            Name = sprintf "%.0fEP %s%s Engine" enginePower cl this.EngineTech.Name
            FuelConsumption = consumption
            EnginePower = enginePower
            Crew = crew
            MaintenenceClass = maint
            BuildCost =
                { BuildCost.Zero with
                    BuildPoints = price
                    Gallicite = price
                    Duranium = price
                }
        }

type ShipComponent =
    | Engine of Engine
    | FuelStorage of FuelStorage
    | Bridge of Bridge
    | Sensors of Sensors
    member this.Guid
        with get() =
            match this with
            | Engine c -> c.Guid
            | FuelStorage c -> c.Guid
            | Bridge c -> c.Guid
            | Sensors c -> c.Guid
    member this.ShipGuid
        with get() =
            match this with
            | Engine c -> c.ShipGuid
            | FuelStorage c -> c.ShipGuid
            | Bridge c -> c.ShipGuid
            | Sensors c -> c.ShipGuid
    member this.Name
        with get() =
            match this with
            | Engine c -> c.Name
            | FuelStorage c -> "Fuel Storage"
            | Bridge c -> "Bridge"
            | Sensors c -> "Sensors"
    member this.duplicate (shipGuid: Guid) =
        match this with
        | Engine c -> Engine { c with Guid = Guid.NewGuid(); ShipGuid = shipGuid }
        | FuelStorage c -> FuelStorage { c with Guid = Guid.NewGuid(); ShipGuid = shipGuid }
        | Bridge c -> Bridge { c with Guid = Guid.NewGuid(); ShipGuid = shipGuid }
        | Sensors c -> Sensors { c with Guid = Guid.NewGuid(); ShipGuid = shipGuid }
    member this.calculate =
        match this with
        | Engine c -> Engine c.calculate
        | FuelStorage c -> FuelStorage c.calculate
        | Bridge c -> Bridge c
        | Sensors c -> Sensors c.calculate
