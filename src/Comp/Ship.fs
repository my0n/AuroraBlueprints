module Comp.Ship

open System

open Global

open Model.BuildCost
open Model.MaintenanceClass
open Model.Measures
open Comp.ShipComponent
open Model.Technology

type Ship =
    {
        Guid: Guid
        Name: string
        ShipClass: string
        Components: Map<Guid, ShipComponent>

        // armor
        ArmorDepth: int
        ArmorTechnology: ArmorTech

        // crew
        SpareBerths: int<people>
        CryogenicBerths: int<people>
        DeployTime: float<mo>
    }
    static member Zero
        with get() =
            {
                Guid = Guid.NewGuid()
                Name = "Tribal"
                ShipClass = "Cruiser"
                Components = Map.empty

                ArmorDepth = 1
                ArmorTechnology = Technology.armor.[0]
            
                SpareBerths = 0<people>
                DeployTime = 3.0<mo>
                CryogenicBerths = 0<people>
            }
    //#region Cost
    member private this._Cost =
        lazy (
            this.CrewQuartersBuildCost + this.ArmorCalculation.Cost
        )
    member private this._CrewQuartersBuildCost =
        lazy (
            { TotalBuildCost.Zero with
                BuildPoints = this.CrewQuartersSize * 10.0</hs>
                Duranium = this.CrewQuartersSize * 2.5</hs>
                Mercassium = this.CrewQuartersSize * 7.5</hs>
            }
        )
    //#endregion

    //#region Size
    member private this._CrewQuartersSize =
        lazy (
            let berths = this.SpareBerths + this.Crew
            let tonsPerPerson = this.DeployTime * 1.0<ton/people/mo>
                                |> powuom (1.0/3.0)
                                |> rounduom 10.0<ton/people>
            rounduom 2.0<ton> (int2float berths * tonsPerPerson)
            |> ton2hs
        )
    member private this._SizeBeforeArmor =
        lazy (
            this.Components
            |> Map.values
            |> List.map (fun c -> c.Size)
            |> List.append [ this.CrewQuartersSize ]
            |> List.sum
        )
    member private this._Size =
        lazy (
            this.CrewQuartersSize + this.SizeBeforeArmor + this.ArmorCalculation.Size
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
                |> List.sumBy (fun c -> c.Crew)
                |> int2float
            match baseCrew with
            | crew when this.DeployTime < 0.1<mo> -> crew / 6.0
            | crew when this.DeployTime < 0.5<mo> -> crew / 2.0
            | crew -> crew
            |> ceiluom
            |> max 1<people>
        )
    //#endregion

    //#region Engines
    member private this._EngineCount =
        lazy (
            this.Components
            |> Map.values
            |> List.sumBy (fun c ->
                match c with
                | Engine c -> c.Count
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
            |> List.sumBy (fun c ->
                match c with
                | Engine c -> c.EnginePower * int2float c.Count
                | _ -> 0.0<ep>
            )
        )
    member private this._Speed =
        lazy (
            match this.HasEngines && this.FuelCapacity > 0.0<kl> with
            | true ->
                this.TotalEnginePower * 1000.0<(km/s)/(ep/hs)> / 1.0<hs> // TODO
            | false ->
                1.0<km/s>
        )
    //#endregion

    //#region Fuel
    member private this._FuelCapacity =
        lazy (
            this.Components
            |> Map.values
            |> List.sumBy (fun c ->
                match c with
                | FuelStorage c -> c.FuelCapacity
                | _ -> 0.0<kl>
            )
        )
    member private this._FuelConsumption =
        lazy (
            this.Components
            |> Map.values
            |> List.tryFindMap (fun c ->
                match c with
                | Engine c -> Some (c.FuelConsumption * int2float c.Count)
                | _ -> None
            )
            |> Option.defaultValue 0.0<kl/hr>
        )
    member private this._FullPowerTime =
        lazy (
            match this.HasEngines && this.FuelCapacity > 0.0<kl> with
            | true ->
                this.FuelCapacity / this.FuelConsumption
            | false ->
                0.0<hr>
            |> hr2day
            |> day2mo
        )
    member private this._FuelRange =
        lazy (
            match this.HasEngines && this.FuelCapacity > 0.0<kl> with
            | true ->
                kps2kphr this.Speed * this.FuelCapacity / this.FuelConsumption
            | false ->
                0.0<km>
        )
    //#endregion

    //#region Signatures
    member private this._EngineThermalSignatureContribution =
        lazy (
            this.Components
            |> Map.values
            |> List.tryFindMap (fun c ->
                match c with
                | Engine c -> Some c.ThermalOutput
                | _ -> None
            )
            |> Option.defaultValue 0.0<therm/comp>
        )
    member private this._ThermalSignature =
        lazy (
            this.Components
            |> Map.values
            |> List.sumBy (fun c ->
                match c with
                | Engine c -> c.ThermalOutput * int2float c.Count
                | _ -> 0.0<therm>
            )
        )
    //#endregion

    //#region Power Plants
    member private this._TotalPower =
        lazy (
            this.Components
            |> Map.values
            |> List.sumBy (fun c ->
                match c with
                | PowerPlant c -> c.Power * int2float c.Count
                | _ -> 0.0<power>
            )
        )
    //#endregion

    //#region Miscellaneous
    member private this._MaintenanceClass =
        lazy (
            this.Components
            |> Map.values
            |> List.tryFindMap (fun c -> match c.MaintenanceClass with Military -> Some Military | Commercial -> None)
            |> Option.defaultValue Commercial
        )
    //#endregion

    //#region Accessors
    member private this.ArmorCalculation with get(): Model.ArmorCalc.ArmorCalculation = this._ArmorCalculation.Value
    member this.ArmorBuildCost with get() = this.ArmorCalculation.Cost
    member this.ArmorSize with get() = this.ArmorCalculation.Size
    member this.ArmorStrength with get() = this.ArmorCalculation.Strength
    member this.ArmorWidth with get() = this.ArmorCalculation.Width
    member this.BuildCost with get() = this._Cost.Value
    member this.Crew with get() = this._Crew.Value
    member this.CrewQuartersBuildCost with get() = this._CrewQuartersBuildCost.Value
    member this.CrewQuartersSize with get() = this._CrewQuartersSize.Value
    member this.EngineCount with get() = this._EngineCount.Value
    member this.EngineThermalSignatureContribution with get() = this._EngineThermalSignatureContribution.Value
    member this.HasEngines with get() = this._HasEngines.Value
    member this.FuelCapacity with get() = this._FuelCapacity.Value
    member this.FuelConsumption with get() = this._FuelConsumption.Value
    member this.FuelRange with get() = this._FuelRange.Value
    member this.FullPowerTime with get() = this._FullPowerTime.Value
    member this.MaintenanceClass with get() = this._MaintenanceClass.Value
    member this.Size with get() = this._Size.Value
    member private this.SizeBeforeArmor with get() = this._SizeBeforeArmor.Value
    member this.Speed with get() = this._Speed.Value
    member this.ThermalSignature with get() = this._ThermalSignature.Value
    member this.TotalEnginePower with get() = this._TotalEnginePower.Value
    member this.TotalPower with get() = this._TotalPower.Value
    //#endregion
