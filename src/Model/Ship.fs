module Model.Ship

open System

open Global

open Model.BuildCost
open Model.Measures
open Model.ShipComponent

type Ship =
    {
        Guid: Guid
        Name: string
        ShipClass: string
        Size: float<hs>
        BuildCost: TotalBuildCost
        Components: Map<Guid, ShipComponent>
        MaintenenceClass: MaintenanceClass

        // crew
        Crew: int<people>
        SpareBerths: int<people>
        DeployTime: float<mo>
        // this is included in Size, but shows the portion that is dedicated to crew quarters
        CrewQuartersSize: float<hs>
        CrewQuartersBuildCost: TotalBuildCost
        CryogenicBerths: int<people> // calculated
    }
    static member empty =
        {
            Guid = Guid.NewGuid()
            Name = "Tribal"
            ShipClass = "Cruiser"
            Size = 0.0<hs>
            BuildCost = TotalBuildCost.Zero
            Components = Map.empty
            MaintenenceClass = Commercial
            
            Crew = 0<people>
            SpareBerths = 0<people>
            DeployTime = 3.0<mo>
            CrewQuartersSize = 0.0<hs>
            CrewQuartersBuildCost = TotalBuildCost.Zero
            CryogenicBerths = 0<people>
        }
    member this.calculate =
        let maint =
            match this.Components
                  |> Map.values
                  |> List.exists (fun c ->
                    match c with
                    | Engine c      -> c.MaintenenceClass = Military
                    | Sensors c     -> c.MaintenenceClass = Military
                    | _             -> false
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

        let bc =
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

        let size =
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
            
        { this with
            Size = size
            Crew = crew
            BuildCost = bc
            MaintenenceClass = maint
            CrewQuartersSize = crewQuartersSize
            CrewQuartersBuildCost = crewQuartersCost
        }
