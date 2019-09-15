module Comp.ShipComponent

open System
open Comp.Bridge
open Comp.Engine
open Comp.FuelStorage
open Comp.Sensors
open Model.MaintenanceClass
open Model.Measures

type ShipComponent =
    | Engine of Engine
    | FuelStorage of FuelStorage
    | Bridge of Bridge
    | Sensors of Sensors
    member this.Guid
        with get() =
            match this with
            | Bridge c      -> c.Guid
            | Engine c      -> c.Guid
            | FuelStorage c -> c.Guid
            | Sensors c     -> c.Guid
    member this.Name
        with get() =
            match this with
            | Bridge c      -> "Bridge"
            | Engine c      -> c.Name
            | FuelStorage c -> "Fuel Storage"
            | Sensors c     -> "Sensors"
    member this.MaintenanceClass
        with get() =
            match this with
            | Engine c      -> c.MaintenenceClass
            | Sensors c     -> c.MaintenenceClass
            | _             -> Commercial
    member this.Crew
        with get() =
            match this with
            | Bridge c      -> c.Crew * c.Count
            | Engine c      -> c.Crew * c.Count
            | FuelStorage c -> 0<people>
            | Sensors c     -> c.Crew
    member this.Cost
        with get() =
            match this with
            | Bridge c      -> c.BuildCost * c.Count
            | Engine c      -> c.BuildCost * c.Count
            | FuelStorage c -> c.BuildCost
            | Sensors c     -> c.BuildCost
    member this.Size
        with get() =
            match this with
            | Bridge c      -> int2float c.Size * int2float c.Count
            | Engine c      -> int2float c.Size * int2float c.Count
            | FuelStorage c -> c.TotalSize
            | Sensors c     -> int2float c.Size
    member this.duplicate =
        match this with
        | Bridge c          -> Bridge { c with Guid = Guid.NewGuid() }
        | Engine c          -> Engine { c with Guid = Guid.NewGuid() }
        | FuelStorage c     -> FuelStorage { c with Guid = Guid.NewGuid() }
        | Sensors c         -> Sensors { c with Guid = Guid.NewGuid() }
    member this.calculate =
        match this with
        | Bridge c          -> Bridge c
        | Engine c          -> Engine c.calculate
        | FuelStorage c     -> FuelStorage c.calculate
        | Sensors c         -> Sensors c.calculate
