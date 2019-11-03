module Comp.CargoHold

open Global
open Model.BuildCost
open Model.Measures

type CargoHold =
    {
        Id: GameObjectId

        Tiny: int<comp>
        Small: int<comp>
        Standard: int<comp>

        CargoHandlingSystems: Map<Technology.CargoHandlingTech, int<comp>>
    }
    static member Zero
        with get() =
            {
                Id = GameObjectId.generate()

                Tiny = 0<comp>
                Small = 0<comp>
                Standard = 0<comp>
                CargoHandlingSystems = Map.empty
            }

    //#region Calculated Values
    member private this._TotalSize =
        lazy (
            (
                this.Tiny * 50
                + this.Small * 100
                + this.Standard * 500
            ) * 50<ton/comp>
            +
            (
                this.CargoHandlingSystems
                |> Seq.sumBy (fun kvp -> kvp.Key.HsPerComp * kvp.Value)
                |> hs2tonint
            )
        )
    member private this._BuildCost =
        lazy (
            { TotalBuildCost.Zero with
                BuildPoints = 6.0
                Duranium = 6.0
            }
            *
            int2float (
                this.Tiny * 6</comp>
                + this.Small * 12</comp>
                + this.Standard * 50</comp>
            )
            +
            (
                this.CargoHandlingSystems
                |> Seq.sumBy (fun kvp ->
                    { TotalBuildCost.Zero with
                        BuildPoints = (kvp.Key.DuraniumCost + kvp.Key.MercassiumCost) * int2float kvp.Value
                        Duranium = kvp.Key.DuraniumCost * int2float kvp.Value
                        Mercassium = kvp.Key.MercassiumCost * int2float kvp.Value
                    }
                )
            )
        )
    member private this._CargoCapacity =
        lazy (
            this.Tiny * 2500<cargoCapacity/comp>
            + this.Small * 5000<cargoCapacity/comp>
            + this.Standard * 25000<cargoCapacity/comp>
        )
    member private this._Crew =
        lazy (
            this.Tiny * 1<people/comp>
            + this.Small * 2<people/comp>
            + this.Standard * 5<people/comp>
            +
            (
                this.CargoHandlingSystems
                |> Seq.sumBy (fun kvp -> kvp.Key.CrewPerComp * kvp.Value)
            )
        )
    member private this._TractorStrength =
        lazy (
            this.CargoHandlingSystems
            |> Seq.sumBy (fun kvp -> kvp.Key.TractorStrength * kvp.Value)
        )
    //#endregion

    //#region Accessors
    member this.BuildCost with get() = this._BuildCost.Value
    member this.CargoCapacity with get() = this._CargoCapacity.Value
    member this.Crew with get() = this._Crew.Value
    member this.TotalSize with get() = this._TotalSize.Value
    member this.TractorStrength with get() = this._TractorStrength.Value
    //#endregion
