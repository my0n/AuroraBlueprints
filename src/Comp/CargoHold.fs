module Comp.CargoHold

open Global
open Model.BuildCost
open Model.Measures

type CargoHold =
    {
        Id: GameObjectId

        CargoHolds: Map<Technology.CargoHoldTech, int<comp>>
        CargoHandlingSystems: Map<Technology.CargoHandlingTech, int<comp>>
    }
    static member Zero
        with get() =
            {
                Id = GameObjectId.generate()

                CargoHolds = Map.empty
                CargoHandlingSystems = Map.empty
            }

    //#region Calculated Values
    member private this._TotalSize =
        lazy (
            (
                this.CargoHolds
                |> Seq.sumBy (fun kvp -> kvp.Key.HsPerComp * kvp.Value)
            )
            +
            (
                this.CargoHandlingSystems
                |> Seq.sumBy (fun kvp -> kvp.Key.HsPerComp * kvp.Value)
            )
            |> hs2tonint
        )
    member private this._BuildCost =
        lazy (
            (
                this.CargoHolds
                |> Seq.sumBy (fun kvp ->
                    { TotalBuildCost.Zero with
                        BuildPoints = kvp.Key.DuraniumCost * int2float kvp.Value
                        Duranium = kvp.Key.DuraniumCost * int2float kvp.Value
                    }
                )
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
            this.CargoHolds
            |> Seq.sumBy (fun kvp -> kvp.Key.CargoCapacity * kvp.Value)
        )
    member private this._Crew =
        lazy (
            (
                this.CargoHolds
                |> Seq.sumBy (fun kvp -> kvp.Key.CrewPerComp * kvp.Value)
            )
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
