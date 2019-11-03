module Comp.TroopTransport

open Global
open System
open Model.BuildCost
open Model.MaintenanceClass
open Model.Measures

type TroopTransport =
    {
        Id: GameObjectId
        
        CompanyTransport: int<comp>
        BattalionTransport: int<comp>
        CompanyDropModule: int<comp>
        BattalionDropModule: int<comp>
        CompanyCryoDropModule: int<comp>
        BattalionCryoDropModule: int<comp>
    }
    static member Zero
        with get() =
            {
                Id = GameObjectId.generate()
                CompanyTransport = 0<comp>
                BattalionTransport = 0<comp>
                CompanyDropModule = 0<comp>
                BattalionDropModule = 0<comp>
                CompanyCryoDropModule = 0<comp>
                BattalionCryoDropModule = 0<comp>
            }
    //#region Calculated Values
    member private this._TotalSize =
        lazy (
            this.CompanyTransport          * 500<ton/comp>
            + this.BattalionTransport      * 2500<ton/comp>
            + this.CompanyDropModule       * 100<ton/comp>
            + this.BattalionDropModule     * 500<ton/comp>
            + this.CompanyCryoDropModule   * 200<ton/comp>
            + this.BattalionCryoDropModule * 1000<ton/comp>
        )
    member private this._Crew =
        lazy (
            this.CompanyTransport          * 3<people/comp>
            + this.BattalionTransport      * 10<people/comp>
            + this.CompanyDropModule       * 1<people/comp>
            + this.BattalionDropModule     * 2<people/comp>
            + this.CompanyCryoDropModule   * 2<people/comp>
            + this.BattalionCryoDropModule * 4<people/comp>
        )
    member private this._CryoDropCapability =
        lazy (
            this.CompanyCryoDropModule     * 1<company/comp>
            + this.BattalionCryoDropModule * 5<company/comp>
        )
    member private this._CombatDropCapability =
        lazy (
            this.CompanyDropModule         * 1<company/comp>
            + this.BattalionDropModule     * 5<company/comp>
        )
    member private this._TroopTransportCapability =
        lazy (
            this.CompanyTransport          * 1<company/comp>
            + this.BattalionTransport      * 5<company/comp>
        )
    member private this._TotalTroopCapacity =
        lazy (
            this.CompanyTransport          * 3<people/comp>
            + this.BattalionTransport      * 10<people/comp>
            + this.CompanyDropModule       * 1<people/comp>
            + this.BattalionDropModule     * 2<people/comp>
            + this.CompanyCryoDropModule   * 2<people/comp>
            + this.BattalionCryoDropModule * 4<people/comp>
        )
    member private this._MaintenanceClass =
        lazy (
            match
                this.CompanyCryoDropModule > 0<comp>
                || this.BattalionCryoDropModule > 0<comp>
                || this.CompanyDropModule > 0<comp>
                || this.BattalionDropModule > 0<comp>
                with
            | true -> Military
            | false -> Commercial
        )
    member private this._BuildCost =
        lazy (
            { TotalBuildCost.Zero with
                BuildPoints = 10.0
                Duranium    = 5.0
                Mercassium  = 5.0
            } * float this.CompanyTransport
            +
            { TotalBuildCost.Zero with
                BuildPoints = 40.0
                Duranium    = 10.0
                Neutronium  = 10.0
                Mercassium  = 20.0
            } * float this.BattalionTransport
            +
            { TotalBuildCost.Zero with
                BuildPoints = 15.0
                Duranium    = 5.0
                Neutronium  = 5.0
                Mercassium  = 5.0
            } * float this.CompanyDropModule
            +
            { TotalBuildCost.Zero with
                BuildPoints = 60.0
                Duranium    = 20.0
                Neutronium  = 20.0
                Mercassium  = 20.0
            } * float this.BattalionDropModule
            +
            { TotalBuildCost.Zero with
                BuildPoints = 30.0
                Duranium    = 10.0
                Neutronium  = 10.0
                Mercassium  = 10.0
            } * float this.CompanyCryoDropModule
            +
            { TotalBuildCost.Zero with
                BuildPoints = 120.0
                Duranium    = 40.0
                Neutronium  = 40.0
                Mercassium  = 40.0
            } * float this.BattalionCryoDropModule
        )
    //#endregion

    //#region Accessors
    member this.BuildCost with get() = this._BuildCost.Value
    member this.Crew with get() = this._Crew.Value
    member this.CombatDropCapability with get() = this._CombatDropCapability.Value
    member this.CryoDropCapability with get() = this._CryoDropCapability.Value
    member this.MaintenanceClass with get() = this._MaintenanceClass.Value
    member this.TotalSize with get() = this._TotalSize.Value
    member this.TotalTroopCapacity with get() = this._TotalTroopCapacity.Value
    member this.TroopTransportCapability with get() = this._TroopTransportCapability.Value
    //#endregion

    
