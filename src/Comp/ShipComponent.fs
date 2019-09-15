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
    member this.ShipGuid
        with get() =
            match this with
            | Engine c -> c.ShipGuid
            | FuelStorage c -> c.ShipGuid
            | Bridge c -> c.ShipGuid
            | Sensors c -> c.ShipGuid
    member this.Name
        with get() =
            match this with
            | Engine c -> c.Name
            | FuelStorage c -> "Fuel Storage"
            | Bridge c -> "Bridge"
            | Sensors c -> "Sensors"
    member this.duplicate (shipGuid: Guid) =
        match this with
        | Engine c -> Engine { c with Guid = Guid.NewGuid(); ShipGuid = shipGuid }
        | FuelStorage c -> FuelStorage { c with Guid = Guid.NewGuid(); ShipGuid = shipGuid }
        | Bridge c -> Bridge { c with Guid = Guid.NewGuid(); ShipGuid = shipGuid }
        | Sensors c -> Sensors { c with Guid = Guid.NewGuid(); ShipGuid = shipGuid }
    member this.calculate =
        match this with
        | Engine c -> Engine c.calculate
        | FuelStorage c -> FuelStorage c.calculate
        | Bridge c -> Bridge c
        | Sensors c -> Sensors c.calculate