module Comp.Engine

open System
open Global
open Model.BuildCost
open Model.MaintenanceClass
open Model.Measures
open Technology

type Engine =
    {
        Id: GameObjectId
        Locked: bool
        BuiltIn: bool

        Name: string
        Manufacturer: string

        EngineTech: EngineTech
        PowerModTech: float
        EfficiencyTech: EngineEfficiencyTech
        ThermalEfficiencyTech: EngineThermalTech
        Size: int<hs/comp>
    }

    //#region Calculated Values
    member private this._EnginePower =
        lazy (
            int2float this.Size
            * this.EngineTech.PowerPerHs
            * this.PowerModTech
        )
    member private this._ThermalOutput =
        lazy (
            this.EnginePower
            * this.ThermalEfficiencyTech.ThermalEfficiency
        )
    member private this._FuelConsumption =
        lazy (
            this.EnginePower
            * this.EfficiencyTech.Efficiency
            * Math.Pow(this.PowerModTech, 2.5)
            * (1.0 - ((int2float this.Size * 1.0<comp/hs>) / 100.0))
        )
    member private this._Crew =
        lazy (
            flooruom (float this.Size * this.PowerModTech)
            * 1<people/comp>
        )
    member private this._MaintenanceClass =
        lazy (
            match (this.Size < 25<hs/comp> || this.PowerModTech > 0.5) with
            | true -> Military
            | false -> Commercial
        )
    member private this._BuildCost =
        lazy (
            let cost =
                this.EnginePower
                * (this.PowerModTech / 2.0)
                * this.ThermalEfficiencyTech.CostMultiplier
                * 1.0</ep>
            { BuildCost.Zero with
                BuildPoints = cost
                Gallicite = cost
            }
        )
    member private this._GeneratedName =
        lazy (
            let cl =
                match this.MaintenanceClass with
                | Military -> ""
                | Commercial -> "Commercial "
            sprintf "%.0fEP %s%s Engine" this.EnginePower cl this.EngineTech.Name
        )
    //#endregion

    //#region Accessors
    member this.BuildCost with get() = this._BuildCost.Value
    member this.Crew with get() = this._Crew.Value
    member this.EnginePower with get(): float<ep/comp> = this._EnginePower.Value
    member this.FuelConsumption with get() = this._FuelConsumption.Value
    member this.GeneratedName with get() = this._GeneratedName.Value
    member this.MaintenanceClass with get() = this._MaintenanceClass.Value
    member this.ThermalOutput with get() = this._ThermalOutput.Value
    //#endregion

let engine (allTech: AllTechnologies) =
    let zero =
        {
            Id = GameObjectId.generate()
            Locked = false
            BuiltIn = false

            Name = ""
            Manufacturer = "Aurora Industries"

            EngineTech = allTech.DefaultEngine
            PowerModTech = allTech.DefaultPowerMod
            EfficiencyTech = allTech.DefaultEngineEfficiency
            ThermalEfficiencyTech = allTech.DefaultThermalEfficiency
            Size = 1<hs/comp>
        }
    { zero with
        Name = zero.GeneratedName
    }
