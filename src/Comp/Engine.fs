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
        ThermalEfficiencyTech: ThermalEfficiencyTech
        Size: int<hs/comp>
        Count: int<comp>

        // calculated values
        EnginePower: float<ep/comp>
        ThermalOutput: float<therm/comp>
        FuelConsumption: float<kl/hr/comp>
        Crew: int<people/comp>
        MaintenanceClass: MaintenanceClass
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
                ThermalEfficiencyTech = Technology.thermalEfficiency.[0]
                Size = 1<hs/comp>
                Count = 1<comp>

                EnginePower = 0.0<ep/comp>
                ThermalOutput = 0.0<therm/comp>
                FuelConsumption = 0.0<kl/hr/comp>
                Crew = 0<people/comp>
                MaintenanceClass = Commercial
                BuildCost = BuildCost.Zero
            }.calculate
    member this.calculate =
        let crew =
            flooruom (float this.Size * this.PowerModTech.PowerMod)
            * 1<people/comp>
        let enginePower =
            int2float this.Size
            * this.EngineTech.PowerPerHs
            * this.PowerModTech.PowerMod
        let consumption =
            enginePower
            * this.EfficiencyTech.Efficiency
            * Math.Pow(this.PowerModTech.PowerMod, 2.5)
            * (1.0 - ((int2float this.Size) / 100.0<hs/comp>))
        let price =
            enginePower
            * (this.PowerModTech.PowerMod / 2.0)
            * this.ThermalEfficiencyTech.CostMultiplier
            * 1.0</ep>
        let therms =
            enginePower
            * this.ThermalEfficiencyTech.ThermalEfficiency
        let maint =
            match (this.Size < 25<hs/comp> || this.PowerModTech.PowerMod > 0.5) && this.Count > 0<comp> with
            | true -> Military
            | false -> Commercial;
        let cl =
            match maint with
            | Military -> ""
            | Commercial -> "Commercial "
        { this with
            Name = sprintf "%.0fEP %s%s Engine" enginePower cl this.EngineTech.Name
            FuelConsumption = consumption
            EnginePower = enginePower
            ThermalOutput = therms
            Crew = crew
            MaintenanceClass = maint
            BuildCost =
                { BuildCost.Zero with
                    BuildPoints = price
                    Gallicite = price
                }
        }
