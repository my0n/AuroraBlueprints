module Comp.FuelStorage

open Global
open Model.BuildCost
open Model.Measures

type FuelStorage =
    {
        Id: GameObjectId

        FuelStorages: Map<Technology.FuelStorageTech, int<comp>>
    }
    static member Zero
        with get() =
            {
                Id = GameObjectId.generate()

                FuelStorages = Map.empty
            }

    //#region Calculated Values
    member private this._TotalSize =
        lazy (
            this.FuelStorages
            |> Seq.sumBy (fun kvp -> kvp.Key.HsPerComp * int2float kvp.Value)
            |> hs2ton
            |> float2int
        )
    member private this._FuelCapacity =
        lazy (
            this.FuelStorages
            |> Seq.sumBy (fun kvp -> kvp.Key.FuelCapacity * kvp.Value)
            |> int2float
        )
    member private this._BuildCost =
        lazy (
            this.FuelStorages
            |> Seq.sumBy (fun kvp ->
                { TotalBuildCost.Zero with
                    BuildPoints = (kvp.Key.DuraniumCost + kvp.Key.BoronideCost) * int2float kvp.Value
                    Duranium = kvp.Key.DuraniumCost * int2float kvp.Value
                    Boronide = kvp.Key.BoronideCost * int2float kvp.Value
                }
            )
        )
    //#endregion

    //#region Accessors
    member this.BuildCost with get() = this._BuildCost.Value
    member this.FuelCapacity with get() = this._FuelCapacity.Value
    member this.TotalSize with get() = this._TotalSize.Value
    //#endregion
