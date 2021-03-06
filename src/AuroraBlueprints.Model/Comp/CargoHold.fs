module Comp.CargoHold

open Global
open Model.BuildCost
open Model.Measures
open Model.Technology

type CargoHold =
    {
        Id: GameObjectId
        Locked: bool
        BuiltIn: bool

        CargoHolds: Map<CargoHoldTech, int<comp>>
        CargoHandlingSystems: Map<CargoHandlingTech, int<comp>>
    }
    static member Zero
        with get() =
            {
                Id = GameObjectId.generate()
                Locked = false
                BuiltIn = false

                CargoHolds = Map.empty
                CargoHandlingSystems = Map.empty
            }

    //#region Calculated Values
    member private this._Size =
        lazy (
            (
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
            * 1</comp>
        )
    member private this._BuildCost =
        lazy (
            (
                this.CargoHolds
                |> Seq.sumBy (fun kvp ->
                    { BuildCost.Zero with
                        BuildPoints = kvp.Key.DuraniumCost * float kvp.Value
                        Duranium = kvp.Key.DuraniumCost * float kvp.Value
                    }
                )
            )
            +
            (
                this.CargoHandlingSystems
                |> Seq.sumBy (fun kvp ->
                    { BuildCost.Zero with
                        BuildPoints = (kvp.Key.DuraniumCost + kvp.Key.MercassiumCost) * float kvp.Value
                        Duranium = kvp.Key.DuraniumCost * float kvp.Value
                        Mercassium = kvp.Key.MercassiumCost * float kvp.Value
                    }
                )
            )
        )
    member private this._CargoCapacity =
        lazy (
            (
                this.CargoHolds
                |> Seq.sumBy (fun kvp -> kvp.Key.CargoCapacity * kvp.Value)
            )
            * 1</comp>
        )
    member private this._Crew =
        lazy (
            (
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
            * 1</comp>
        )
    member private this._TractorStrength =
        lazy (
            (
                this.CargoHandlingSystems
                |> Seq.sumBy (fun kvp -> kvp.Key.TractorStrength * kvp.Value)
            )
            * 1</comp>
        )
    //#endregion

    //#region Accessors
    member this.BuildCost with get() = this._BuildCost.Value
    member this.CargoCapacity with get() = this._CargoCapacity.Value
    member this.Crew with get() = this._Crew.Value
    member this.Size with get() = this._Size.Value
    member this.TractorStrength with get() = this._TractorStrength.Value
    //#endregion
