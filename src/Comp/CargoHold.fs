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

        CargoHandlingSystem: int<comp>
        ImprovedCargoHandlingSystem: int<comp>
        AdvancedCargoHandlingSystem: int<comp>
        GravAssistedCargoHandlingSystem: int<comp>
    }
    static member Zero
        with get() =
            {
                Id = GameObjectId.generate()

                Tiny = 0<comp>
                Small = 0<comp>
                Standard = 0<comp>
                CargoHandlingSystem = 0<comp>
                ImprovedCargoHandlingSystem = 0<comp>
                AdvancedCargoHandlingSystem = 0<comp>
                GravAssistedCargoHandlingSystem = 0<comp>
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
                this.CargoHandlingSystem
                + this.ImprovedCargoHandlingSystem
                + this.AdvancedCargoHandlingSystem
                + this.GravAssistedCargoHandlingSystem
            ) * 100<ton/comp>
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
            { TotalBuildCost.Zero with
                BuildPoints = float this.CargoHandlingSystem * 10.0
                Duranium = float this.CargoHandlingSystem * 5.0
                Mercassium = float this.CargoHandlingSystem * 5.0
            }
            +
            { TotalBuildCost.Zero with
                BuildPoints = float this.ImprovedCargoHandlingSystem * 25.0
                Duranium = float this.ImprovedCargoHandlingSystem * 10.0
                Mercassium = float this.ImprovedCargoHandlingSystem * 15.0
            }
            +
            { TotalBuildCost.Zero with
                BuildPoints = float this.AdvancedCargoHandlingSystem * 50.0
                Duranium = float this.AdvancedCargoHandlingSystem * 20.0
                Mercassium = float this.AdvancedCargoHandlingSystem * 30.0
            }
            +
            { TotalBuildCost.Zero with
                BuildPoints = float this.GravAssistedCargoHandlingSystem * 100.0
                Duranium = float this.GravAssistedCargoHandlingSystem * 25.0
                Mercassium = float this.GravAssistedCargoHandlingSystem * 75.0
            }
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
            + this.CargoHandlingSystem * 10<people/comp>
            + this.ImprovedCargoHandlingSystem * 10<people/comp>
            + this.AdvancedCargoHandlingSystem * 10<people/comp>
            + this.GravAssistedCargoHandlingSystem * 10<people/comp>
        )
    member private this._TractorStrength =
        lazy (
            this.CargoHandlingSystem * 5<tractorStrength/comp>
            + this.ImprovedCargoHandlingSystem * 10<tractorStrength/comp>
            + this.AdvancedCargoHandlingSystem * 20<tractorStrength/comp>
            + this.GravAssistedCargoHandlingSystem * 40<tractorStrength/comp>
        )
    //#endregion

    //#region Accessors
    member this.BuildCost with get() = this._BuildCost.Value
    member this.CargoCapacity with get() = this._CargoCapacity.Value
    member this.Crew with get() = this._Crew.Value
    member this.TotalSize with get() = this._TotalSize.Value
    member this.TractorStrength with get() = this._TractorStrength.Value
    //#endregion
