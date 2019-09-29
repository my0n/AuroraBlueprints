module Comp.ShipComponent

open System
open Comp.Bridge
open Comp.Engine
open Comp.FuelStorage
open Comp.PowerPlant
open Comp.Sensors
open Model.MaintenanceClass
open Model.Measures

type ShipComponent =
    | Engine of Engine
    | FuelStorage of FuelStorage
    | Bridge of Bridge
    | Sensors of Sensors
    | PowerPlant of PowerPlant
    member this.Guid
        with get() =
            match this with
            | Bridge c      -> c.Guid
            | Engine c      -> c.Guid
            | FuelStorage c -> c.Guid
            | PowerPlant c  -> c.Guid
            | Sensors c     -> c.Guid
    member this.Name
        with get() =
            match this with
            | Bridge c      -> "Bridge"
            | Engine c      -> c.Name
            | FuelStorage c -> "Fuel Storage"
            | PowerPlant c  -> c.Name
            | Sensors c     -> "Sensors"
    member this.MaintenanceClass
        with get() =
            match this with
            | Engine c      -> c.MaintenanceClass
            | PowerPlant c  -> c.MaintenanceClass
            | Sensors c     -> c.MaintenanceClass
            | _             -> Commercial
    member this.Crew
        with get() =
            match this with
            | Bridge c      -> c.Crew * c.Count
            | Engine c      -> c.Crew * c.Count
            | FuelStorage c -> 0<people>
            | PowerPlant c  -> c.Crew * c.Count
            | Sensors c     -> c.Crew
    member this.Cost
        with get() =
            match this with
            | Bridge c      -> c.BuildCost * c.Count
            | Engine c      -> c.BuildCost * c.Count
            | FuelStorage c -> c.BuildCost
            | PowerPlant c  -> c.BuildCost * c.Count
            | Sensors c     -> c.BuildCost
    member this.TotalSize
        with get() =
            match this with
            | Bridge c      -> c.TotalSize
            | Engine c      -> c.TotalSize
            | FuelStorage c -> c.TotalSize
            | PowerPlant c  -> c.TotalSize
            | Sensors c     -> c.TotalSize
    member this.duplicate =
        match this with
        | Bridge c          -> Bridge { c with Guid = Guid.NewGuid() }
        | Engine c          -> Engine { c with Guid = Guid.NewGuid() }
        | FuelStorage c     -> FuelStorage { c with Guid = Guid.NewGuid() }
        | PowerPlant c      -> PowerPlant { c with Guid = Guid.NewGuid() }
        | Sensors c         -> Sensors { c with Guid = Guid.NewGuid() }
        