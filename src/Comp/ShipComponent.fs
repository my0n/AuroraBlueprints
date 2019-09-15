module Comp.ShipComponent

open System
open Comp.Bridge
open Comp.Engine
open Comp.FuelStorage
open Comp.Sensors

type ShipComponent =
    | Engine of Engine
    | FuelStorage of FuelStorage
    | Bridge of Bridge
    | Sensors of Sensors
    member this.Guid
        with get() =
            match this with
            | Engine c -> c.Guid
            | FuelStorage c -> c.Guid
            | Bridge c -> c.Guid
            | Sensors c -> c.Guid
    member this.Name
        with get() =
            match this with
            | Engine c -> c.Name
            | FuelStorage c -> "Fuel Storage"
            | Bridge c -> "Bridge"
            | Sensors c -> "Sensors"
    member this.duplicate =
        match this with
        | Engine c -> Engine { c with Guid = Guid.NewGuid() }
        | FuelStorage c -> FuelStorage { c with Guid = Guid.NewGuid() }
        | Bridge c -> Bridge { c with Guid = Guid.NewGuid() }
        | Sensors c -> Sensors { c with Guid = Guid.NewGuid() }
    member this.calculate =
        match this with
        | Engine c -> Engine c.calculate
        | FuelStorage c -> FuelStorage c.calculate
        | Bridge c -> Bridge c
        | Sensors c -> Sensors c.calculate
