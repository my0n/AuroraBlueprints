module Comp.TroopTransport

open Global
open Model.BuildCost
open Model.MaintenanceClass
open Model.Measures
open Model.Technology

type TroopTransport =
    {
        Id: GameObjectId
        Locked: bool
        BuiltIn: bool
        
        TroopTransports: Map<TroopTransportTech, int<comp>>
    }
    static member Zero
        with get() =
            {
                Id = GameObjectId.generate()
                Locked = false
                BuiltIn = false

                TroopTransports = Map.empty
            }
    //#region Calculated Values
    member private this._Size =
        lazy (
            (
                this.TroopTransports
                |> Seq.sumBy (fun kvp -> kvp.Key.HsPerComp * int2float kvp.Value)
                |> hs2ton
                |> float2int
            )
            * 1</comp>
        )
    member private this._Crew =
        lazy (
            (
                this.TroopTransports
                |> Seq.sumBy (fun kvp -> kvp.Key.CrewPerComp * kvp.Value)
            )
            * 1</comp>
        )
    member private this._CryoDropCapability =
        lazy (
            (
                this.TroopTransports
                |> Seq.sumBy (fun kvp -> kvp.Key.CryoDropCapacity * kvp.Value)
            )
            * 1</comp>
        )
    member private this._CombatDropCapability =
        lazy (
            (
                this.TroopTransports
                |> Seq.sumBy (fun kvp -> kvp.Key.CombatDropCapacity * kvp.Value)
            )
            * 1</comp>
        )
    member private this._TroopTransportCapability =
        lazy (
            (
                this.TroopTransports
                |> Seq.sumBy (fun kvp -> kvp.Key.TroopTransportCapacity * kvp.Value)
            )
            * 1</comp>
        )
    member private this._MaintenanceClass =
        lazy (
            match this.TroopTransports
                  |> Seq.exists (fun kvp -> kvp.Key.IsMilitary && kvp.Value > 0<comp>) with
            | true -> Military
            | false -> Commercial
        )
    member private this._BuildCost =
        lazy (
            this.TroopTransports
            |> Seq.sumBy (fun kvp ->
                { BuildCost.Zero with
                    BuildPoints = (kvp.Key.DuraniumCost + kvp.Key.NeutroniumCost + kvp.Key.MercassiumCost) * float kvp.Value
                    Duranium    = (kvp.Key.DuraniumCost) * float kvp.Value
                    Neutronium  = (kvp.Key.NeutroniumCost) * float kvp.Value
                    Mercassium  = (kvp.Key.MercassiumCost) * float kvp.Value
                }
            )
        )
    //#endregion

    //#region Accessors
    member this.BuildCost with get() = this._BuildCost.Value
    member this.Crew with get() = this._Crew.Value
    member this.CombatDropCapability with get() = this._CombatDropCapability.Value
    member this.CryoDropCapability with get() = this._CryoDropCapability.Value
    member this.MaintenanceClass with get() = this._MaintenanceClass.Value
    member this.Size with get() = this._Size.Value
    member this.TroopTransportCapability with get() = this._TroopTransportCapability.Value
    //#endregion
