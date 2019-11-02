module Comp.FuelStorage

open Global
open Model.BuildCost
open Model.Measures

type FuelStorage =
    {
        Id: GameObjectId

        Tiny: int<comp>
        Small: int<comp>
        Standard: int<comp>
        Large: int<comp>
        VeryLarge: int<comp>
        UltraLarge: int<comp>
    }
    static member Zero
        with get() =
            {
                Id = GameObjectId.generate()

                Tiny = 0<comp>
                Small = 0<comp>
                Standard = 0<comp>
                Large = 0<comp>
                VeryLarge = 0<comp>
                UltraLarge = 0<comp>
            }

    //#region Calculated Values
    member private this._TotalSize =
        lazy (
            (
                this.Tiny * 5
                + this.Small * 10
                + this.Standard * 50
                + this.Large * 250
                + this.VeryLarge * 1000
                + this.UltraLarge * 5000
            ) * 1<ton/comp>
        )
    member private this._FuelCapacity =
        lazy (
            (
                int2float this.Tiny * 1.0
                + int2float this.Small * 2.0
                + int2float this.Standard * 10.0
                + int2float this.Large * 50.0
                + int2float this.VeryLarge * 200.0
                + int2float this.UltraLarge * 1000.0
            ) * 5.0<kl/comp>
        )
    member private this._BuildCost =
        lazy (
            let cost =
                (
                    int2float this.Tiny * 1.0
                    + int2float this.Small * 1.5
                    + int2float this.Standard * 5.0
                    + int2float this.Large * 15.0
                    + int2float this.VeryLarge * 35.0
                    + int2float this.UltraLarge * 100.0
                ) * 1.0</comp>
            { TotalBuildCost.Zero with
                BuildPoints = cost * 2.0
                Duranium = cost
                Boronide = cost
            }
        )
    //#endregion

    //#region Accessors
    member this.BuildCost with get() = this._BuildCost.Value
    member this.FuelCapacity with get() = this._FuelCapacity.Value
    member this.TotalSize with get() = this._TotalSize.Value
    //#endregion
