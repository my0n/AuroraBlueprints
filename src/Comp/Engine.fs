module Comp.Engine

open System
open Model.BuildCost
open Model.MaintenanceClass
open Model.Measures
open Model.Technology

type Engine =
    {
        Guid: Guid

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
    static member Zero
        with get() =
            {
                Guid = Guid.NewGuid()

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
