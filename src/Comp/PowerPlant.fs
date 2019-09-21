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
    }
    static member Zero
        with get() =
            let zero =
                {
                    Guid = Guid.NewGuid()
                    Name = ""
                    Manufacturer = "Aurora Industries"

                    Count = 0<comp>
                    Size = 1.0<hs/comp>
                    PowerBoost = Technology.powerBoost.[0]
                    Technology = Technology.powerPlant.[0]
                }
            { zero with
                Name = zero.GeneratedName
            }

    //#region Calculated Values
    member private this._BuildCost =
        lazy (
            let cost = this.Power * 3.0</power>
            { BuildCost.Zero with
                BuildPoints = cost
                Boronide = cost
            }
        )
    member private this._Crew =
        lazy (
            match this.Size with
            | sz when sz < 0.75<hs/comp> -> 1.0<people/comp>
            | sz when sz < 1.0<hs/comp> -> 2.0<people/comp>
            | _ -> this.Size * 2.0<people/hs>
            |> float2int
        )
    member private this._Power =
        lazy (
            (1.0 + this.PowerBoost.PowerBoost)
            * this.Technology.PowerOutput
            * this.Size
        )
    member private this._MaintenanceClass =
        lazy (
            match this.Count > 0<comp> with
            | true -> Military
            | false -> Commercial
        )
    member private this._GeneratedName =
        lazy (
            String.Format("{0} Technology PB-{1}", this.Technology.Name, (1.0 + this.PowerBoost.PowerBoost))
        )
    //#endregion

    //#region Accessors
    member this.BuildCost with get() = this._BuildCost.Value
    member this.Crew with get() = this._Crew.Value
    member this.Power with get() = this._Power.Value
    member this.MaintenanceClass with get() = this._MaintenanceClass.Value
    member this.GeneratedName with get() = this._GeneratedName.Value
    //#endregion

