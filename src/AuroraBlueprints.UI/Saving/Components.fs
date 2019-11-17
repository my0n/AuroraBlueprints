module Saving.Components

open Global
open Model.Measures
open Model.Technology
open LocalStorage
open Thoth.Json

let inline serializeTechCount map =
    map
    |> Map.mapKvp (fun k v ->
        ((^key) : (member Id : GameObjectId) (k)), v
    )
    |> Map

let inline deserializeTech (techs: #TechBase list) (key: string) =
    techs
    |> List.find (fun t -> t.Id = key)

let inline deserializeTechCount (techs: #TechBase list) (map: Map<string, int>) =
    map
    |> Map.mapKvp (fun k v ->
        techs
        |> List.find (fun t -> t.Id = k),
        v * 1<comp>
    )
    |> Map

//#region Bridge
let serializeBridge (c: Comp.Bridge.Bridge) =
    "bridge-1",
    {|
        Locked = c.Locked
    |}
    |> fun c -> Encode.Auto.toString (0, c)

let deserializeBridge1 id techs : Decoder<Comp.Bridge.Bridge> =
    Decode.map
        (fun locked ->
            {
                Id = id
                Locked = locked
                BuiltIn = false
            }
        )
        (Decode.field "Locked" Decode.bool)
//#endregion

//#region Cargo Hold
let serializeCargoHold (c: Comp.CargoHold.CargoHold) =
    "cargo-1",
    {|
        Locked = c.Locked
        Holds = serializeTechCount c.CargoHolds
        Handlers = serializeTechCount c.CargoHandlingSystems
    |}
    |> fun c -> Encode.Auto.toString (0, c)

let deserializeCargo1 id (techs: AllTechnologies) : Decoder<Comp.CargoHold.CargoHold> =
    Decode.map3
        (fun locked holds handlers ->
            {
                Id = id
                Locked = locked
                BuiltIn = false
                CargoHolds = deserializeTechCount techs.CargoHolds holds
                CargoHandlingSystems = deserializeTechCount techs.CargoHandling handlers
            }
        )
        (Decode.field "Locked" Decode.bool)
        (Decode.field "Holds" <| Decode.dict Decode.int)
        (Decode.field "Handlers" <| Decode.dict Decode.int)
//#endregion

//#region Engine
let serializeEngine (c: Comp.Engine.Engine) =
    "engine-1",
    {|
        Locked = c.Locked
        Name = c.Name
        Mfr = c.Manufacturer
        Size = c.Size
        Eff = c.EfficiencyTech.Id
        Tech = c.EngineTech.Id
        Pwr = c.PowerModTech
        Thm = c.ThermalEfficiencyTech.Id
    |}
    |> fun c -> Encode.Auto.toString (0, c)

let deserializeEngine1 id (techs: AllTechnologies) : Decoder<Comp.Engine.Engine> =
    Decode.map8
        (fun locked name mfr size eff tech pwr thm ->
            {
                Id = id
                Locked = locked
                BuiltIn = false
                Name = name
                Manufacturer = mfr
                Size = size * 1<hs/comp>
                EfficiencyTech = deserializeTech techs.EngineEfficiency eff
                EngineTech = deserializeTech techs.Engines tech
                PowerModTech = pwr
                ThermalEfficiencyTech = deserializeTech techs.EngineThermalEfficiency thm
            }
        )
        (Decode.field "Locked" Decode.bool)
        (Decode.field "Name" Decode.string)
        (Decode.field "Mfr" Decode.string)
        (Decode.field "Size" Decode.int)
        (Decode.field "Eff" Decode.string)
        (Decode.field "Tech" Decode.string)
        (Decode.field "Pwr" Decode.float)
        (Decode.field "Thm" Decode.string)
//#endregion

//#region Fuel Storage
let serializeFuelStorage (c: Comp.FuelStorage.FuelStorage) =
    "fuel-1",
    {|
        Locked = c.Locked
        Fuels = serializeTechCount c.FuelStorages
    |}
    |> fun c -> Encode.Auto.toString (0, c)

let deserializeFuelStorage1 id (techs: AllTechnologies) : Decoder<Comp.FuelStorage.FuelStorage> =
    Decode.map2
        (fun locked fuels ->
            {
                Id = id
                Locked = locked
                BuiltIn = false
                FuelStorages = deserializeTechCount techs.FuelStorages fuels
            }
        )
        (Decode.field "Locked" Decode.bool)
        (Decode.field "Fuels" <| Decode.dict Decode.int)
//#endregion

//#region Magazine
let serializeMagazine (c: Comp.Magazine.Magazine) =
    "magazine-1",
    {|
        Name = c.Name
        Locked = c.Locked
        Mfr = c.Manufacturer
        Size = c.Size
        HTK = c.HTK
        Armor = c.Armor.Id
        Ejc = c.Ejection.Id
        Feed = c.FeedSystem.Id
    |}
    |> fun c -> Encode.Auto.toString (0, c)

let deserializeMagazine1 id (techs: AllTechnologies) : Decoder<Comp.Magazine.Magazine> =
    Decode.map8
        (fun locked name mfr size htk armor ejc feed ->
            {
                Id = id
                Locked = locked
                BuiltIn = false
                Name = name
                Manufacturer = mfr
                Size = size * 1<hs/comp>
                HTK = htk
                Armor = deserializeTech techs.Armor armor
                Ejection = deserializeTech techs.MagazineEjection ejc
                FeedSystem = deserializeTech techs.MagazineEfficiency feed
            }
        )
        (Decode.field "Locked" Decode.bool)
        (Decode.field "Name" Decode.string)
        (Decode.field "Mfr" Decode.string)
        (Decode.field "Size" Decode.int)
        (Decode.field "HTK" Decode.int)
        (Decode.field "Armor" Decode.string)
        (Decode.field "Ejc" Decode.string)
        (Decode.field "Feed" Decode.string)
//#endregion

//#region Power Plant
let serializePowerPlant (c: Comp.PowerPlant.PowerPlant) =
    "power-1",
    {|
        Locked = c.Locked
        Name = c.Name
        Mfr = c.Manufacturer
        Size = c.Size
        Pwr = c.PowerBoost.Id
        Tech = c.Technology.Id
    |}
    |> fun c -> Encode.Auto.toString (0, c)

let deserializePowerPlant1 id (techs: AllTechnologies) : Decoder<Comp.PowerPlant.PowerPlant> =
    Decode.map6
        (fun locked name mfr size pwr tech ->
            {
                Id = id
                Locked = locked
                BuiltIn = false
                Name = name
                Manufacturer = mfr
                Size = size * 1.0<hs/comp>
                PowerBoost = deserializeTech techs.ReactorsPowerBoost pwr
                Technology = deserializeTech techs.Reactors tech
            }
        )
        (Decode.field "Locked" Decode.bool)
        (Decode.field "Name" Decode.string)
        (Decode.field "Mfr" Decode.string)
        (Decode.field "Size" Decode.float)
        (Decode.field "Pwr" Decode.string)
        (Decode.field "Tech" Decode.string)
//#endregion

//#region Sensors
let serializeSensors (c: Comp.Sensors.Sensors) =
    "sensor-1",
    {|
        Locked = c.Locked
        Geo = serializeTechCount c.GeoSensors
        Grav = serializeTechCount c.GravSensors
    |}
    |> fun c -> Encode.Auto.toString (0, c)

let deserializeSensors1 id (techs: AllTechnologies) : Decoder<Comp.Sensors.Sensors> =
    Decode.map3
        (fun locked geo grav ->
            {
                Id = id
                Locked = locked
                BuiltIn = false
                GeoSensors = deserializeTechCount techs.GeoSensors geo
                GravSensors = deserializeTechCount techs.GravSensors grav
            }
        )
        (Decode.field "Locked" Decode.bool)
        (Decode.field "Geo" <| Decode.dict Decode.int)
        (Decode.field "Grav" <| Decode.dict Decode.int)
//#endregion

//#region Troop Transports
let serializeTroopTransport (c: Comp.TroopTransport.TroopTransport) =
    "troop-1",
    {|
        Locked = c.Locked
        Troop = serializeTechCount c.TroopTransports
    |}
    |> fun c -> Encode.Auto.toString (0, c)

let deserializeTroopTransports1 id (techs: AllTechnologies) : Decoder<Comp.TroopTransport.TroopTransport> =
    Decode.map2
        (fun locked troop ->
            {
                Id = id
                Locked = locked
                BuiltIn = false
                TroopTransports = deserializeTechCount techs.TroopTransports troop
            }
        )
        (Decode.field "Locked" Decode.bool)
        (Decode.field "Troop" <| Decode.dict Decode.int)
//#endregion

let serialize (c: Comp.ShipComponent.ShipComponent) =
    let (version, serialized) =
        match c with
        | Comp.ShipComponent.Bridge c         -> serializeBridge c
        | Comp.ShipComponent.CargoHold c      -> serializeCargoHold c
        | Comp.ShipComponent.Engine c         -> serializeEngine c
        | Comp.ShipComponent.FuelStorage c    -> serializeFuelStorage c
        | Comp.ShipComponent.Magazine c       -> serializeMagazine c
        | Comp.ShipComponent.PowerPlant c     -> serializePowerPlant c
        | Comp.ShipComponent.Sensors c        -> serializeSensors c
        | Comp.ShipComponent.TroopTransport c -> serializeTroopTransport c
    "comp", version, c.Id, serialized

let deserialize (techs: AllTechnologies) version key str =
    let applyDeserialization fn fout =
        Decode.fromString (fn key techs) str |> Result.map fout

    match version with
    | "bridge-1" -> applyDeserialization deserializeBridge1 Comp.ShipComponent.Bridge
    | "cargo-1" -> applyDeserialization deserializeCargo1 Comp.ShipComponent.CargoHold
    | "engine-1" -> applyDeserialization deserializeEngine1 Comp.ShipComponent.Engine
    | "fuel-1" -> applyDeserialization deserializeFuelStorage1 Comp.ShipComponent.FuelStorage
    | "magazine-1" -> applyDeserialization deserializeMagazine1 Comp.ShipComponent.Magazine
    | "power-1" -> applyDeserialization deserializePowerPlant1 Comp.ShipComponent.PowerPlant
    | "sensor-1" -> applyDeserialization deserializeSensors1 Comp.ShipComponent.Sensors
    | "troop-1" -> applyDeserialization deserializeTroopTransports1 Comp.ShipComponent.TroopTransport
    | s -> Error <| sprintf "Invalid version %s" s
    |> function
    | Ok s -> Success s 
    | Error s -> Failure s
