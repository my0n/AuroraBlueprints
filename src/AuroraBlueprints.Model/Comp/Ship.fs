module Comp.Ship

open Global

open Model.BuildCost
open Model.MaintenanceClass
open Model.Measures
open Comp.ShipComponent

open Model.Technology

type Ship =
    {
        Id: GameObjectId
        Name: string
        ShipClass: string
        Components: Map<GameObjectId, int<comp> * ShipComponent>

        // armor
        ArmorDepth: int
        ArmorTechnology: ArmorTech

        // crew
        SpareBerths: int<people>
        CryogenicBerths: int<people>
        DeployTime: float<mo>
    }
    //#region Cost
    member private this._Cost =
        lazy (
            this.CrewQuartersBuildCost
            + this.ArmorBuildCost
        )
    member private this._CrewQuartersBuildCost =
        lazy (
            { TotalBuildCost.Zero with
                BuildPoints = int2float this.CrewQuartersSize * 0.2</ton>
                Duranium = int2float this.CrewQuartersSize * 0.05</ton>
                Mercassium = int2float this.CrewQuartersSize * 0.15</ton>
            }
        )
    member private this._ArmorBuildCost =
        lazy (
            let costFactor = this.ArmorStrength * 1.0</armorStrength>
            { TotalBuildCost.Zero with
                BuildPoints = costFactor
                Duranium = this.ArmorTechnology.DuraniumRatio * costFactor
                Neutronium = this.ArmorTechnology.NeutroniumRatio * costFactor
            }
        )
    //#endregion

    //#region Size
    member private this._TotalBerths =
        lazy (
            this.SpareBerths
            + this.Crew
        )
    member private this._TonsPerPerson =
        lazy (
            this.DeployTime
            * 1.0<ton/people/mo>
            |> powuom (1.0/3.0)
            |> rounduom 10.0<ton/people>
        )
    member private this._CrewQuartersSize =
        lazy (
            int2float this.TotalBerths
            * this.TonsPerPerson
            |> rounduom 2.0<ton>
            |> float2int
        )
    member private this._ComponentSize =
        lazy (
            this.Components
            |> Map.values
            |> List.sumBy (fun (count, comp) -> count * comp.Size)
        )
    member private this._SizeBeforeArmor =
        lazy (
            this.ComponentSize
            + this.CrewQuartersSize
        )
    member private this._Size =
        lazy (
            this.SizeBeforeArmor
            + this.ArmorSize
        )
    //#endregion

    //#region Armor
    member private this._ArmorCalculation =
        lazy (
            Model.ArmorCalc.shipArmor this.SizeBeforeArmor this.ArmorDepth this.ArmorTechnology
        )
    //#endregion

    //#region Crew Quarters
    member private this._Crew =
        lazy (
            let baseCrew =
                this.Components
                |> Map.values
                |> List.sumBy (fun (count, comp) -> count * comp.Crew)
                |> int2float
            match baseCrew with
            | crew when this.DeployTime < 0.1<mo> -> crew / 6.0
            | crew when this.DeployTime < 0.5<mo> -> crew / 2.0
            | crew -> crew
            |> ceiluom
            |> max 1<people>
        )
    //#endregion

    //#region Cargo
    member private this._CargoCapacity =
        lazy (
            this.Components
            |> Map.values
            |> List.sumBy (function
                | count, CargoHold c -> count * c.CargoCapacity
                | _ -> 0<cargoCapacity>
            )
        )
    member private this._CargoHandlingMultiplier =
        lazy (
            this.Components
            |> Map.values
            |> List.sumBy (function
                | count, CargoHold c -> count * c.TractorStrength
                | _ -> 0<tractorStrength>
            )
            |> max 1<tractorStrength>
        )
    member private this._LoadTime =
        lazy (
            int2float this.CargoCapacity
            / 100.0<cargoCapacity/hr>
            / int2float this.CargoHandlingMultiplier
            * 1.0<tractorStrength>
        )
    //#endregion

    //#region Magazines
    member private this._TotalAmmoCapacity =
        lazy (
            this.Components
            |> Map.values
            |> List.sumBy (function
                | count, Magazine c -> count * c.Capacity
                | _ -> 0<ammo>
            )
        )
    //#endregion

    //#region Troop Transport
    member private this._CryoDropCapability =
        lazy (
            this.Components
            |> Map.values
            |> List.sumBy (function
                | count, TroopTransport c -> count * c.CryoDropCapability
                | _ -> 0<company>
            )
        )
    member private this._CombatDropCapability =
        lazy (
            this.Components
            |> Map.values
            |> List.sumBy (function
                | count, TroopTransport c -> count * c.CombatDropCapability
                | _ -> 0<company>
            )
        )
    member private this._TroopTransportCapability =
        lazy (
            this.Components
            |> Map.values
            |> List.sumBy (function
                | count, TroopTransport c -> count * c.TroopTransportCapability
                | _ -> 0<company>
            )
        )
    //#endregion

    //#region Engines
    member private this._EngineCount =
        lazy (
            this.Components
            |> Map.values
            |> List.sumBy (function
                | count, Engine _ -> count
                | _ -> 0<comp>
            )
        )
    member private this._HasEngines =
        lazy (
            this.EngineCount > 0<comp>
        )
    member private this._TotalEnginePower =
        lazy (
            this.Components
            |> Map.values
            |> List.sumBy (function
                | count, Engine c -> c.EnginePower * int2float count
                | _ -> 0.0<ep>
            )
        )
    member private this._Speed =
        lazy (
            match this.HasEngines && this.FuelCapacity > 0.0<l> with
            | true ->
                this.TotalEnginePower
                * 1000.0<(km/s)/(ep/hs)>
                / ton2hs (int2float this.Size)
            | false ->
                1.0<km/s>
        )
    //#endregion

    //#region Fuel
    member private this._FuelCapacity =
        lazy (
            this.Components
            |> Map.values
            |> List.sumBy (function
                | count, FuelStorage c -> c.FuelCapacity * int2float count
                | _ -> 0.0<l>
            )
        )
    member private this._FuelConsumption =
        lazy (
            this.Components
            |> Map.values
            |> List.tryFindMap (function
                | count, Engine c -> Some (c.FuelConsumption * int2float count)
                | _ -> None
            )
            |> Option.defaultValue 0.0<l/hr>
        )
    member private this._FullPowerTime =
        lazy (
            match this.HasEngines && this.FuelCapacity > 0.0<l> with
            | true ->
                this.FuelCapacity / this.FuelConsumption
            | false ->
                0.0<hr>
            |> hr2day
            |> day2mo
        )
    member private this._FuelRange =
        lazy (
            match this.HasEngines && this.FuelCapacity > 0.0<l> with
            | true ->
                kps2kphr this.Speed
                * this.FuelCapacity
                / this.FuelConsumption
            | false ->
                0.0<km>
        )
    //#endregion

    //#region Signatures
    member private this._EngineThermalSignatureContribution =
        lazy (
            this.Components
            |> Map.values
            |> List.tryFindMap (function
                | _, Engine c -> Some c.ThermalOutput
                | _ -> None
            )
            |> Option.defaultValue 0.0<therm/comp>
        )
    member private this._ThermalSignature =
        lazy (
            this.Components
            |> Map.values
            |> List.sumBy (function
                | count, Engine c -> c.ThermalOutput * int2float count
                | _ -> 0.0<therm>
            )
        )
    //#endregion

    //#region Power Plants
    member private this._TotalPower =
        lazy (
            this.Components
            |> Map.values
            |> List.sumBy (function
                | count, PowerPlant c -> c.Power * int2float count
                | _ -> 0.0<power>
            )
        )
    //#endregion

    //#region Miscellaneous
    member private this._MaintenanceClass =
        lazy (
            this.Components
            |> Map.values
            |> List.tryFindMap (fun (count, c) -> match c.MaintenanceClass with Military -> Some Military | Commercial -> None)
            |> Option.defaultValue Commercial
        )
    //#endregion

    //#region Accessors
    member this.ArmorBuildCost with get(): TotalBuildCost = this._ArmorBuildCost.Value
    member private this.ArmorCalculation with get(): Model.ArmorCalc.ArmorCalculation = this._ArmorCalculation.Value
    member this.ArmorSize with get() = float2int <| hs2ton this.ArmorCalculation.Size
    member this.ArmorStrength with get() = this.ArmorCalculation.Strength
    member this.ArmorWidth with get() = this.ArmorCalculation.Width
    member this.BuildCost with get(): TotalBuildCost = this._Cost.Value
    member this.CargoCapacity with get() = this._CargoCapacity.Value
    member this.CargoHandlingMultiplier with get() = this._CargoHandlingMultiplier.Value
    member this.CombatDropCapability with get() = this._CombatDropCapability.Value
    member private this.ComponentSize with get(): int<ton> = this._ComponentSize.Value
    member this.Crew with get(): int<people> = this._Crew.Value
    member this.CrewQuartersBuildCost with get(): TotalBuildCost = this._CrewQuartersBuildCost.Value
    member this.CrewQuartersSize with get(): int<ton> = this._CrewQuartersSize.Value
    member this.CryoDropCapability with get() = this._CryoDropCapability.Value
    member this.EngineCount with get() = this._EngineCount.Value
    member this.EngineThermalSignatureContribution with get() = this._EngineThermalSignatureContribution.Value
    member this.HasEngines with get() = this._HasEngines.Value
    member this.FuelCapacity with get() = this._FuelCapacity.Value
    member this.FuelConsumption with get(): float<l/hr> = this._FuelConsumption.Value
    member this.FuelRange with get() = this._FuelRange.Value
    member this.FullPowerTime with get() = this._FullPowerTime.Value
    member this.LoadTime with get() = this._LoadTime.Value
    member this.MaintenanceClass with get() = this._MaintenanceClass.Value
    member this.Size with get(): int<ton> = this._Size.Value
    member private this.SizeBeforeArmor with get(): int<ton> = this._SizeBeforeArmor.Value
    member this.Speed with get() = this._Speed.Value
    member this.ThermalSignature with get() = this._ThermalSignature.Value
    member this.TonsPerPerson with get() = this._TonsPerPerson.Value
    member this.TotalAmmoCapacity with get() = this._TotalAmmoCapacity.Value
    member this.TotalBerths with get() = this._TotalBerths.Value
    member this.TotalEnginePower with get(): float<ep> = this._TotalEnginePower.Value
    member this.TotalPower with get() = this._TotalPower.Value
    member this.TroopTransportCapability with get() = this._TroopTransportCapability.Value
    //#endregion

let ship (allTechs: AllTechnologies) =
    {
        Id = GameObjectId.generate()
        Name = "Tribal"
        ShipClass = "Cruiser"
        Components = Map.empty

        ArmorDepth = 1
        ArmorTechnology = allTechs.DefaultArmor
    
        SpareBerths = 0<people>
        DeployTime = 3.0<mo>
        CryogenicBerths = 0<people>
    }
