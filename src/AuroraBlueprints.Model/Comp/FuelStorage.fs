module Comp.FuelStorage

open Global
open Model.BuildCost
open Model.Measures
open Model.Technology

type FuelStorage =
    {
        Id: GameObjectId
        Locked: bool
        BuiltIn: bool

        FuelStorages: Map<FuelStorageTech, int<comp>>
    }
    static member Zero
        with get() =
            {
                Id = GameObjectId.generate()
                Locked = false
                BuiltIn = false

                FuelStorages = Map.empty
            }

    //#region Calculated Values
    member private this._Size =
        lazy (
            (
                this.FuelStorages
                |> Seq.sumBy (fun kvp -> kvp.Key.HsPerComp * int2float kvp.Value)
                |> hs2ton
                |> float2int
            )
            * 1</comp>
        )
    member private this._FuelCapacity =
        lazy (
            (
                this.FuelStorages
                |> Seq.sumBy (fun kvp -> kvp.Key.FuelCapacity * kvp.Value)
                |> int2float
            )
            * 1.0</comp>
        )
    member private this._BuildCost =
        lazy (
            this.FuelStorages
            |> Seq.sumBy (fun kvp ->
                { BuildCost.Zero with
                    BuildPoints = (kvp.Key.DuraniumCost + kvp.Key.BoronideCost) * float kvp.Value
                    Duranium = kvp.Key.DuraniumCost * float kvp.Value
                    Boronide = kvp.Key.BoronideCost * float kvp.Value
                }
            )
        )
    //#endregion

    //#region Accessors
    member this.BuildCost with get() = this._BuildCost.Value
    member this.FuelCapacity with get() = this._FuelCapacity.Value
    member this.Size with get() = this._Size.Value
    //#endregion
