module Comp.PowerPlant

open System
open Model.BuildCost
open Model.Measures
open Model.Technology
open Model.MaintenanceClass

type PowerPlant =
    {
        Guid: Guid
        Name: string
        Manufacturer: string

        Count: int<comp>
        Size: float<hs/comp>
        PowerBoost: PowerBoostTech
        Technology: PowerPlantTech

        // calculated values
        BuildCost: BuildCost
        Crew: int<people/comp>
        Power: float<power/comp>
        MaintenanceClass: MaintenanceClass
    }
    static member Zero
        with get() =
            {
                Guid = Guid.NewGuid()
                Name = ""
                Manufacturer = "Aurora Industries"

                Count = 0<comp>
                Size = 1.0<hs/comp>
                PowerBoost = Technology.powerBoost.[0]
                Technology = Technology.powerPlant.[0]

                BuildCost = BuildCost.Zero
                Crew = 0<people/comp>
                Power = 0.0<power/comp>
                MaintenanceClass = Commercial
            }.calculate
    member this.calculate =
        let name = String.Format("{0} Technology PB-{1}", this.Technology.Name, (1.0 + this.PowerBoost.PowerBoost))
        let power = (1.0 + this.PowerBoost.PowerBoost) * this.Technology.PowerOutput * this.Size
        let crew =
            match this.Size with
            | sz when sz < 0.75<hs/comp> -> 1.0<people/comp>
            | sz when sz < 1.0<hs/comp> -> 2.0<people/comp>
            | _ -> this.Size * 2.0<people/hs>
        let costFactor = power * 3.0</power>
        let cost =
            { BuildCost.Zero with
                BuildPoints = costFactor
                Boronide = costFactor
            }

        { this with
            Name = name
            Power = power
            Crew = float2int crew
            BuildCost = cost
            MaintenanceClass =
                match this.Count > 0<comp> with
                | true -> Military
                | false -> Commercial
        }
