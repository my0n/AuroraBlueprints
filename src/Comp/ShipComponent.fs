module Comp.ShipComponent

open Global
open Comp.Bridge
open Comp.CargoHold
open Comp.Engine
open Comp.FuelStorage
open Comp.Magazine
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
    | Magazine of Magazine
    | PowerPlant of PowerPlant
    | Sensors of Sensors
    | TroopTransport of TroopTransport
    member this.Id
        with get() =
            match this with
            | Bridge c         -> c.Id
            | CargoHold c      -> c.Id
            | Engine c         -> c.Id
            | FuelStorage c    -> c.Id
            | Magazine c       -> c.Id
            | PowerPlant c     -> c.Id
            | Sensors c        -> c.Id
            | TroopTransport c -> c.Id
    member this.Name
        with get() =
            match this with
            | Bridge c         -> "Bridge"
            | CargoHold c      -> "Cargo Hold"
            | Engine c         -> c.Name
            | FuelStorage c    -> "Fuel Storage"
            | Magazine c       -> c.Name
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
            | Magazine c       -> c.Crew * c.Count
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
            | Magazine c       -> c.BuildCost * c.Count
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
            | Magazine c       -> c.TotalSize
            | PowerPlant c     -> c.TotalSize
            | Sensors c        -> c.TotalSize
            | TroopTransport c -> c.TotalSize
    member this.duplicate =
        match this with
        | Bridge c             -> Bridge { c with Id = GameObjectId.generate() }
        | CargoHold c          -> CargoHold { c with Id = GameObjectId.generate() }
        | Engine c             -> Engine { c with Id = GameObjectId.generate() }
        | FuelStorage c        -> FuelStorage { c with Id = GameObjectId.generate() }
        | Magazine c           -> Magazine { c with Id = GameObjectId.generate() }
        | PowerPlant c         -> PowerPlant { c with Id = GameObjectId.generate() }
        | Sensors c            -> Sensors { c with Id = GameObjectId.generate() }
        | TroopTransport c     -> TroopTransport { c with Id = GameObjectId.generate() }
        