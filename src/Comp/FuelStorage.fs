module Comp.FuelStorage

open System
open Model.BuildCost
open Model.Measures

type FuelStorage =
    {
        Guid: Guid

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
                Guid = Guid.NewGuid()

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
                int2float this.Tiny * 0.1
                + int2float this.Small * 0.2
                + int2float this.Standard * 1.0
                + int2float this.Large * 5.0
                + int2float this.VeryLarge * 20.0
                + int2float this.UltraLarge * 100.0
            ) * 1.0<hs/comp>
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
    member this.TotalSize with get() = this._TotalSize.Value
    member this.FuelCapacity with get() = this._FuelCapacity.Value
    member this.BuildCost with get() = this._BuildCost.Value
    //#endregion
