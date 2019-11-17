module Comp.PowerPlant

open Global
open System
open Model.BuildCost
open Model.Measures
open Model.MaintenanceClass
open Technology

type PowerPlant =
    {
        Id: GameObjectId
        Locked: bool
        BuiltIn: bool

        Name: string
        Manufacturer: string

        Size: float<hs/comp>
        PowerBoost: ReactorBoostTech
        Technology: ReactorTech
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
    member private this._GeneratedName =
        lazy (
            String.Format("{0} Technology PB-{1}", this.Technology.Name, (1.0 + this.PowerBoost.PowerBoost))
        )
    //#endregion

    //#region Accessors
    member this.BuildCost with get() = this._BuildCost.Value
    member this.Crew with get() = this._Crew.Value
    member this.GeneratedName with get() = this._GeneratedName.Value
    member this.MaintenanceClass with get() = Military
    member this.Power with get() = this._Power.Value
    //#endregion

let powerPlant (allTechs: AllTechnologies) =
    let zero =
        {
            Id = GameObjectId.generate()
            Locked = false
            BuiltIn = false

            Name = ""
            Manufacturer = "Aurora Industries"

            Size = 1.0<hs/comp>
            PowerBoost = allTechs.DefaultPowerBoost
            Technology = allTechs.DefaultReactor
        }
    { zero with
        Name = zero.GeneratedName
    }
