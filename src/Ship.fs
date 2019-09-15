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
        Size: float<hs> // calculated
        BuildCost: TotalBuildCost // calculated
        Components: Map<Guid, ShipComponent>

        MaintenenceClass: MaintenanceClass // calculated

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
                MaintenenceClass = Commercial
                
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
            match this.Components
                  |> Map.values
                  |> List.exists (fun c ->
                    match c with
                    | Engine c  -> c.MaintenenceClass = Military
                    | Sensors c -> c.MaintenenceClass = Military
                    | _         -> false
                  ) with
            | true -> Military
            | false -> Commercial

        let crew =
            match this.Components
                  |> Map.values
                  |> List.map (fun c ->
                      match c with
                      | FuelStorage c -> 0<people>
                      | Engine c      -> c.Crew * c.Count
                      | Bridge c      -> c.Crew * c.Count
                      | Sensors c     -> c.Crew
                  )
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

        let buildCostBeforeArmor =
            this.Components
            |> Map.values
            |> List.map (fun c ->
                match c with
                | FuelStorage c -> c.BuildCost
                | Engine c      -> c.BuildCost * c.Count
                | Bridge c      -> c.BuildCost * c.Count
                | Sensors c     -> c.BuildCost
            )
            |> List.append [ crewQuartersCost ]
            |> List.sum

        let sizeBeforeArmor =
            this.Components
            |> Map.values
            |> List.map (fun c ->
                match c with
                | FuelStorage c -> c.TotalSize
                | Engine c      -> int2float c.Size * int2float c.Count
                | Bridge c      -> int2float c.Size * int2float c.Count
                | Sensors c     -> int2float c.Size
            )
            |> List.append [ crewQuartersSize ]
            |> List.sum

        // calculate armor
        let armorCalc =
            Model.ArmorCalc.shipArmor sizeBeforeArmor this.ArmorDepth this.ArmorTechnology

        { this with
            Size = sizeBeforeArmor + armorCalc.Size
            Crew = crew
            BuildCost = buildCostBeforeArmor + armorCalc.Cost
            MaintenenceClass = maint

            ArmorBuildCost = armorCalc.Cost
            ArmorSize = armorCalc.Size
            ArmorStrength = armorCalc.Strength
            ArmorWidth = armorCalc.Width

            CrewQuartersSize = crewQuartersSize
            CrewQuartersBuildCost = crewQuartersCost
        }
