module Comp.ShipComponent

open System
open Comp.Bridge
open Comp.CargoHold
open Comp.Engine
open Comp.FuelStorage
open Comp.PowerPlant
open Comp.Sensors
open Comp.TroopTransport
open Model.MaintenanceClass
open Model.Measures

type ShipComponent =
    | Bridge of Bridge
    | CargoHold of CargoHold
    | Engine of Engine
    | FuelStorage of FuelStorage
    | PowerPlant of PowerPlant
    | Sensors of Sensors
    | TroopTransport of TroopTransport
    member this.Guid
        with get() =
            match this with
            | Bridge c         -> c.Guid
            | CargoHold c      -> c.Guid
            | Engine c         -> c.Guid
            | FuelStorage c    -> c.Guid
            | PowerPlant c     -> c.Guid
            | Sensors c        -> c.Guid
            | TroopTransport c -> c.Guid
    member this.Name
        with get() =
            match this with
            | Bridge c         -> "Bridge"
            | CargoHold c      -> "Cargo Hold"
            | Engine c         -> c.Name
            | FuelStorage c    -> "Fuel Storage"
            | PowerPlant c     -> c.Name
            | Sensors c        -> "Sensors"
            | TroopTransport c -> "Troop Transport"
    member this.MaintenanceClass
        with get() =
            match this with
            | Engine c         -> c.MaintenanceClass
            | PowerPlant c     -> c.MaintenanceClass
            | Sensors c        -> c.MaintenanceClass
            | TroopTransport c -> c.MaintenanceClass
            | _                -> Commercial
    member this.Crew
        with get() =
            match this with
            | Bridge c         -> c.Crew * c.Count
            | CargoHold c      -> c.Crew
            | Engine c         -> c.Crew * c.Count
            | FuelStorage c    -> 0<people>
            | PowerPlant c     -> c.Crew * c.Count
            | Sensors c        -> c.Crew
            | TroopTransport c -> c.Crew
    member this.Cost
        with get() =
            match this with
            | Bridge c         -> c.BuildCost * c.Count
            | CargoHold c      -> c.BuildCost
            | Engine c         -> c.BuildCost * c.Count
            | FuelStorage c    -> c.BuildCost
            | PowerPlant c     -> c.BuildCost * c.Count
            | Sensors c        -> c.BuildCost
            | TroopTransport c -> c.BuildCost
    member this.TotalSize
        with get() =
            match this with
            | Bridge c         -> c.TotalSize
            | CargoHold c      -> c.TotalSize
            | Engine c         -> c.TotalSize
            | FuelStorage c    -> c.TotalSize
            | PowerPlant c     -> c.TotalSize
            | Sensors c        -> c.TotalSize
            | TroopTransport c -> c.TotalSize
    member this.duplicate =
        match this with
        | Bridge c             -> Bridge { c with Guid = Guid.NewGuid() }
        | CargoHold c          -> CargoHold { c with Guid = Guid.NewGuid() }
        | Engine c             -> Engine { c with Guid = Guid.NewGuid() }
        | FuelStorage c        -> FuelStorage { c with Guid = Guid.NewGuid() }
        | PowerPlant c         -> PowerPlant { c with Guid = Guid.NewGuid() }
        | Sensors c            -> Sensors { c with Guid = Guid.NewGuid() }
        | TroopTransport c     -> TroopTransport { c with Guid = Guid.NewGuid() }
        