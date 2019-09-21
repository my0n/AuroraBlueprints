module Comp.Sensors

open System
open Model.BuildCost
open Model.MaintenanceClass
open Model.Measures

type Sensors =
    {
        Guid: Guid
        
        StandardGeo: int<comp>
        ImprovedGeo: int<comp>
        AdvancedGeo: int<comp>
        PhasedGeo: int<comp>
        
        StandardGrav: int<comp>
        ImprovedGrav: int<comp>
        AdvancedGrav: int<comp>
        PhasedGrav: int<comp>
    }
    static member Zero
        with get() =
            {
                Guid = Guid.NewGuid()
                StandardGeo = 0<comp>
                ImprovedGeo = 0<comp>
                AdvancedGeo = 0<comp>
                PhasedGeo = 0<comp>
                StandardGrav = 0<comp>
                ImprovedGrav = 0<comp>
                AdvancedGrav = 0<comp>
                PhasedGrav = 0<comp>
            }
    //#region Calculated Values
    member private this._Count =
        lazy (
            this.StandardGeo + this.StandardGrav
            + this.ImprovedGeo + this.ImprovedGrav
            + this.AdvancedGeo + this.AdvancedGrav
            + this.PhasedGeo + this.PhasedGrav
        )
    member private this._Size =
        lazy (
            this.Count * 5<hs/comp>
        )
    member private this._Crew =
        lazy (
            this.Count * 10<people/comp>
        )
    member private this._BuildCost =
        lazy (
            let cost =
                (
                    (this.StandardGeo + this.StandardGrav) * 100
                    + (this.ImprovedGeo + this.ImprovedGrav) * 150
                    + (this.AdvancedGeo + this.AdvancedGrav) * 200
                    + (this.PhasedGeo + this.PhasedGrav) * 300
                ) * 1</comp>
            { TotalBuildCost.Zero with
                BuildPoints = float cost
                Uridium = float cost
            }
        )
    member private this._MaintenanceClass =
        lazy (
            match [ this.StandardGrav; this.ImprovedGrav; this.AdvancedGrav; this.PhasedGrav ]
                  |> List.exists (fun a -> a > 0<comp>) with
            | true -> Military
            | false -> Commercial
        )
    member private this._GeoSensorRating =
        lazy (
            (
                this.StandardGeo
                + this.ImprovedGeo * 2
                + this.AdvancedGeo * 3
                + this.PhasedGeo * 5
            ) * 1</comp>
        )
    member private this._GravSensorRating =
        lazy (
            (
                this.StandardGrav
                + this.ImprovedGrav * 2
                + this.AdvancedGrav * 3
                + this.PhasedGrav * 5
            ) * 1</comp>
        )
    //#endregion

    //#region Accessors
    member private this.Count with get() = this._Count.Value
    member this.Size with get() = this._Size.Value
    member this.Crew with get() = this._Crew.Value
    member this.BuildCost with get() = this._BuildCost.Value
    member this.MaintenanceClass with get() = this._MaintenanceClass.Value
    member this.GeoSensorRating with get() = this._GeoSensorRating.Value
    member this.GravSensorRating with get() = this._GravSensorRating.Value
    //#endregion

    
