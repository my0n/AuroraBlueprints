module Comp.Sensors

open Global
open Model.BuildCost
open Model.MaintenanceClass
open Model.Measures

type Sensors =
    {
        Id: GameObjectId
        
        GeoSensors: Map<Technology.GeoSensorTech, int<comp>>
        GravSensors: Map<Technology.GravSensorTech, int<comp>>
    }
    static member Zero
        with get() =
            {
                Id = GameObjectId.generate()
                GeoSensors = Map.empty
                GravSensors = Map.empty
            }
    //#region Calculated Values
    member private this._TotalSize =
        lazy (
            (
                this.GeoSensors
                |> Seq.sumBy (fun kvp -> kvp.Key.HsPerComp * kvp.Value)
            )
            +
            (
                this.GravSensors
                |> Seq.sumBy (fun kvp -> kvp.Key.HsPerComp * kvp.Value)
            )
            |> hs2tonint
        )
    member private this._Crew =
        lazy (
            (
                this.GeoSensors
                |> Seq.sumBy (fun kvp -> kvp.Key.CrewPerComp * kvp.Value)
            )
            +
            (
                this.GravSensors
                |> Seq.sumBy (fun kvp -> kvp.Key.CrewPerComp * kvp.Value)
            )
        )
    member private this._BuildCost =
        lazy (
            (
                this.GeoSensors
                |> Seq.sumBy (fun kvp ->
                    { TotalBuildCost.Zero with
                        BuildPoints = kvp.Key.UridiumCost * int2float kvp.Value
                        Uridium = kvp.Key.UridiumCost * int2float kvp.Value
                    }
                )
            )
            +
            (
                this.GravSensors
                |> Seq.sumBy (fun kvp ->
                    { TotalBuildCost.Zero with
                        BuildPoints = kvp.Key.UridiumCost * int2float kvp.Value
                        Uridium = kvp.Key.UridiumCost * int2float kvp.Value
                    }
                )
            )
        )
    member private this._MaintenanceClass =
        lazy (
            match Map.values this.GravSensors
                  |> List.exists (fun a -> a > 0<comp>) with
            | true -> Military
            | false -> Commercial
        )
    member private this._GeoSensorRating =
        lazy (
            (
                this.GeoSensors
                |> Seq.sumBy (fun kvp -> kvp.Key.SensorRating * kvp.Value)
            )
        )
    member private this._GravSensorRating =
        lazy (
            (
                this.GravSensors
                |> Seq.sumBy (fun kvp -> kvp.Key.SensorRating * kvp.Value)
            )
        )
    //#endregion

    //#region Accessors
    member this.BuildCost with get() = this._BuildCost.Value
    member this.Crew with get() = this._Crew.Value
    member this.GeoSensorRating with get() = this._GeoSensorRating.Value
    member this.GravSensorRating with get() = this._GravSensorRating.Value
    member this.MaintenanceClass with get() = this._MaintenanceClass.Value
    member this.TotalSize with get() = this._TotalSize.Value
    //#endregion

    
