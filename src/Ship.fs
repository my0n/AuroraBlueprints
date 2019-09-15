module Ship

open System

open Global

open Model.BuildCost
open Model.MaintenanceClass
open Model.Measures
open Comp.ShipComponent
open Model.Technology

type Ship =
    {
        // all of the following fields that are marked as "included in x" are only meant for display purposes
        // they have already been rolled into the corresponding value and should not be counted a second time

        Guid: Guid
        Name: string
        ShipClass: string
        Components: Map<Guid, ShipComponent>
        
        Size: float<hs> // calculated
        BuildCost: TotalBuildCost // calculated
        MaintenanceClass: MaintenanceClass // calculated

        // power plants
        TotalPower: float<power>

        // engines and fuel
        FuelCapacity: float<kl> // calculated
        TotalEnginePower: float<ep> // calculated
        HasEngines: bool // calculated
        FuelRange: float<km> // calculated
        FullPowerTime: float<mo> // calculated
        Speed: float<km/s> // calculated

        // armor
        ArmorDepth: int
        ArmorTechnology: ArmorTech
        ArmorWidth: int // calculated
        ArmorBuildCost: TotalBuildCost // calculated (included in BuildCost)
        ArmorSize: float<hs> // calculated (included in Size)
        ArmorStrength: float // calculated

        // crew
        Crew: int<people>
        SpareBerths: int<people>
        CryogenicBerths: int<people>
        DeployTime: float<mo>
        CrewQuartersSize: float<hs> // calculated (included in Size)
        CrewQuartersBuildCost: TotalBuildCost // calculated (included in BuildCost)
    }
    static member Zero
        with get() =
            {
                Guid = Guid.NewGuid()
                Name = "Tribal"
                ShipClass = "Cruiser"
                Size = 0.0<hs>
                BuildCost = TotalBuildCost.Zero
                Components = Map.empty
                MaintenanceClass = Commercial
                FuelCapacity = 0.0<kl>
                HasEngines = false
                FuelRange = 0.0<km>
                FullPowerTime = 0.0<mo>
                TotalEnginePower = 0.0<ep>
                Speed = 0.0<km/s>
                TotalPower = 0.0<power>
                
                ArmorDepth = 1
                ArmorTechnology = Technology.armor.[0]
                ArmorWidth = 0
                ArmorBuildCost = TotalBuildCost.Zero
                ArmorSize = 0.0<hs>
                ArmorStrength = 0.0
            
                Crew = 0<people>
                SpareBerths = 0<people>
                DeployTime = 3.0<mo>
                CrewQuartersSize = 0.0<hs>
                CrewQuartersBuildCost = TotalBuildCost.Zero
                CryogenicBerths = 0<people>
            }.calculate
    member this.calculate =
        let maint =
            this.Components
            |> Map.values
            |> List.tryFindMap (fun c -> match c.MaintenanceClass with Military -> Some Military | Commercial -> None)
            |> Option.defaultValue Commercial

        let crew =
            match this.Components
                  |> Map.values
                  |> List.map (fun c -> c.Crew)
                  |> List.sum
                  |> int2float with
            | crew when this.DeployTime < 0.1<mo> -> crew / 6.0
            | crew when this.DeployTime < 0.5<mo> -> crew / 2.0
            | crew -> crew
            |> ceiluom
            |> max 1<people>

        // crew quarters modules are priced, weighted, etc. as multiples of the smallest one
        let crewQuartersSize =
            let berths = this.SpareBerths + crew
            let tonsPerPerson = this.DeployTime * 1.0<ton/people/mo>
                                |> powuom (1.0/3.0)
                                |> rounduom 10.0<ton/people>
            rounduom 2.0<ton> (int2float berths * tonsPerPerson)
            |> ton2hs
        let crewQuartersCost =
            { TotalBuildCost.Zero with
                BuildPoints = crewQuartersSize * 10.0</hs>
                Duranium = crewQuartersSize * 2.5</hs>
                Mercassium = crewQuartersSize * 7.5</hs>
            }

        // aggregate
        let totalPower =
            this.Components
            |> Map.values
            |> List.map (fun c ->
                match c with
                | PowerPlant c -> c.Power * int2float c.Count
                | _ -> 0.0<power>
            )
            |> List.sum

        let fuelCapacity =
            this.Components
            |> Map.values
            |> List.map (fun c ->
                match c with
                | FuelStorage c -> c.FuelCapacity
                | _ -> 0.0<kl>
            )
            |> List.sum
            
        let buildCostBeforeArmor =
            this.Components
            |> Map.values
            |> List.map (fun c -> c.Cost)
            |> List.append [ crewQuartersCost ]
            |> List.sum

        let sizeBeforeArmor =
            this.Components
            |> Map.values
            |> List.map (fun c -> c.Size)
            |> List.append [ crewQuartersSize ]
            |> List.sum

        // calculate armor
        let armorCalc =
            Model.ArmorCalc.shipArmor sizeBeforeArmor this.ArmorDepth this.ArmorTechnology

        // final aggregate
        let sz = sizeBeforeArmor + armorCalc.Size
        let bc = buildCostBeforeArmor + armorCalc.Cost

        // engine stuff
        let hasEngines =
            this.Components
            |> Map.values
            |> List.exists (fun c ->
                match c with
                | Engine c -> c.Count > 0<comp>
                | _ -> false
            )

        let totalEp =
            this.Components
            |> Map.values
            |> List.map (fun c ->
                match c with
                | Engine c -> c.EnginePower * int2float c.Count
                | _ -> 0.0<ep>
            )
            |> List.sum

        let fuelConsumption =
            this.Components
            |> Map.values
            |> List.tryFindMap (fun c ->
                match c with
                | Engine c -> Some (c.FuelConsumption * int2float c.Count)
                | _ -> None
            )
            |> Option.defaultValue 0.0<kl/hr>

        let (speed, fuelTime, fuelRange) =
            match hasEngines && fuelCapacity > 0.0<kl> with
            | true ->
                let speed = totalEp * 1000.0<(km/s)/(ep/hs)> / sz
                let fuelTime = fuelCapacity / fuelConsumption
                let fuelRange = (min2s <| hr2min fuelTime) * speed
                speed, fuelTime, fuelRange
            | false -> 1.0<km/s>, 0.0<hr>, 0.0<km>

        { this with
            Size = sz
            Crew = crew
            BuildCost = bc
            MaintenanceClass = maint
            FuelCapacity = fuelCapacity
            HasEngines = hasEngines
            FuelRange = fuelRange
            FullPowerTime = day2mo <| hr2day fuelTime
            TotalEnginePower = totalEp
            Speed = speed
            TotalPower = totalPower

            ArmorBuildCost = armorCalc.Cost
            ArmorSize = armorCalc.Size
            ArmorStrength = armorCalc.Strength
            ArmorWidth = armorCalc.Width

            CrewQuartersSize = crewQuartersSize
            CrewQuartersBuildCost = crewQuartersCost
        }
