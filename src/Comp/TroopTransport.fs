module Comp.TroopTransport

open Global
open System
open Model.BuildCost
open Model.MaintenanceClass
open Model.Measures

type TroopTransport =
    {
        Id: GameObjectId
        
        TroopTransports: Map<Technology.TroopTransportTech, int<comp>>
    }
    static member Zero
        with get() =
            {
                Id = GameObjectId.generate()

                TroopTransports = Map.empty
            }
    //#region Calculated Values
    member private this._TotalSize =
        lazy (
            this.TroopTransports
            |> Seq.sumBy (fun kvp -> kvp.Key.HsPerComp * int2float kvp.Value)
            |> hs2ton
            |> float2int
        )
    member private this._Crew =
        lazy (
            this.TroopTransports
            |> Seq.sumBy (fun kvp -> kvp.Key.CrewPerComp * kvp.Value)
        )
    member private this._CryoDropCapability =
        lazy (
            this.TroopTransports
            |> Seq.sumBy (fun kvp -> kvp.Key.CryoDropCapacity * kvp.Value)
        )
    member private this._CombatDropCapability =
        lazy (
            this.TroopTransports
            |> Seq.sumBy (fun kvp -> kvp.Key.CombatDropCapacity * kvp.Value)
        )
    member private this._TroopTransportCapability =
        lazy (
            this.TroopTransports
            |> Seq.sumBy (fun kvp -> kvp.Key.TroopTransportCapacity * kvp.Value)
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
                    BuildPoints = kvp.Key.DuraniumCost + kvp.Key.NeutroniumCost + kvp.Key.MercassiumCost
                    Duranium    = kvp.Key.DuraniumCost
                    Neutronium  = kvp.Key.NeutroniumCost
                    Mercassium  = kvp.Key.MercassiumCost
                }
                * kvp.Value
            )
        )
    //#endregion

    //#region Accessors
    member this.BuildCost with get() = this._BuildCost.Value
    member this.Crew with get() = this._Crew.Value
    member this.CombatDropCapability with get() = this._CombatDropCapability.Value
    member this.CryoDropCapability with get() = this._CryoDropCapability.Value
    member this.MaintenanceClass with get() = this._MaintenanceClass.Value
    member this.TotalSize with get() = this._TotalSize.Value
    member this.TroopTransportCapability with get() = this._TroopTransportCapability.Value
    //#endregion

    
