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
            | Bridge c         -> c.Crew
            | CargoHold c      -> c.Crew
            | Engine c         -> c.Crew
            | FuelStorage c    -> 0<people/comp>
            | Magazine c       -> c.Crew
            | PowerPlant c     -> c.Crew
            | Sensors c        -> c.Crew
            | TroopTransport c -> c.Crew
    member this.Cost
        with get() =
            match this with
            | Bridge c         -> c.BuildCost
            | CargoHold c      -> c.BuildCost
            | Engine c         -> c.BuildCost
            | FuelStorage c    -> c.BuildCost
            | Magazine c       -> c.BuildCost
            | PowerPlant c     -> c.BuildCost
            | Sensors c        -> c.BuildCost
            | TroopTransport c -> c.BuildCost
    member this.Size
        with get() =
            match this with
            | Bridge c         -> c.Size * 50<ton/hs>
            | CargoHold c      -> c.Size
            | Engine c         -> c.Size * 50<ton/hs>
            | FuelStorage c    -> c.Size
            | Magazine c       -> c.Size * 50<ton/hs>
            | PowerPlant c     -> c.Size * 50.0<ton/hs> |> float2int
            | Sensors c        -> c.Size
            | TroopTransport c -> c.Size
    /// Is this component locked?
    member this.Locked
        with get() =
            match this with
            | Bridge c         -> c.Locked
            | CargoHold c      -> c.Locked
            | Engine c         -> c.Locked
            | FuelStorage c    -> c.Locked
            | Magazine c       -> c.Locked
            | PowerPlant c     -> c.Locked
            | Sensors c        -> c.Locked
            | TroopTransport c -> c.Locked
    /// Is this component a built-in base component, used for the sole purpose of
    /// populating the defaults of duplicated components?
    member this.BuiltIn
        with get() =
            match this with
            | Bridge c         -> c.BuiltIn
            | CargoHold c      -> c.BuiltIn
            | Engine c         -> c.BuiltIn
            | FuelStorage c    -> c.BuiltIn
            | Magazine c       -> c.BuiltIn
            | PowerPlant c     -> c.BuiltIn
            | Sensors c        -> c.BuiltIn
            | TroopTransport c -> c.BuiltIn
    /// Is this component composed of many, smaller components?
    member this.Composite
        with get() =
            match this with
            | Bridge c         -> false
            | CargoHold c      -> true
            | Engine c         -> false
            | FuelStorage c    -> true
            | Magazine c       -> false
            | PowerPlant c     -> false
            | Sensors c        -> true
            | TroopTransport c -> true
    member this.WithLocked (locked) =
        match this with
        | Bridge c             -> Bridge { c with Locked = locked }
        | CargoHold c          -> CargoHold { c with Locked = locked }
        | Engine c             -> Engine { c with Locked = locked }
        | FuelStorage c        -> FuelStorage { c with Locked = locked }
        | Magazine c           -> Magazine { c with Locked = locked }
        | PowerPlant c         -> PowerPlant { c with Locked = locked }
        | Sensors c            -> Sensors { c with Locked = locked }
        | TroopTransport c     -> TroopTransport { c with Locked = locked }
    member this.duplicate =
        match this with
        | Bridge c             -> Bridge { c with Id = GameObjectId.generate(); BuiltIn = false }
        | CargoHold c          -> CargoHold { c with Id = GameObjectId.generate(); BuiltIn = false }
        | Engine c             -> Engine { c with Id = GameObjectId.generate(); BuiltIn = false }
        | FuelStorage c        -> FuelStorage { c with Id = GameObjectId.generate(); BuiltIn = false }
        | Magazine c           -> Magazine { c with Id = GameObjectId.generate(); BuiltIn = false }
        | PowerPlant c         -> PowerPlant { c with Id = GameObjectId.generate(); BuiltIn = false }
        | Sensors c            -> Sensors { c with Id = GameObjectId.generate(); BuiltIn = false }
        | TroopTransport c     -> TroopTransport { c with Id = GameObjectId.generate(); BuiltIn = false }