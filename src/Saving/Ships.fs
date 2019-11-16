module Saving.Ships

open Global
open Model.Measures
open LocalStorage
open Thoth.Json

open Comp.Ship
open Comp.ShipComponent

let serialize (ship: Ship) =
    let serialized =
        {|
            Name = ship.Name
            ShipClass = ship.ShipClass
            ArmorDepth = ship.ArmorDepth
            ArmorTechnology = ship.ArmorTechnology.Id
            Components =
                ship.Components
                |> Map.map (fun _ (count, _) -> count)
            SpareBerths = ship.SpareBerths
            CryogenicBerths = ship.CryogenicBerths
            DeployTime = ship.DeployTime
        |}
        |> fun c -> Encode.Auto.toString (0, c)

    "ship", "1", ship.Id, serialized

let deserializeShip1 id (comps: ShipComponent list) (techs: Technology.AllTechnologies) =
    Decode.object
        (fun get ->
            {
                Id = id
                Name = get.Required.Field "Name" Decode.string
                ShipClass = get.Required.Field "ShipClass" Decode.string
                ArmorDepth = get.Required.Field "ArmorDepth" Decode.int
                ArmorTechnology =
                    techs.Armor
                    |> List.find (fun t ->
                        t.Id = get.Required.Field "ArmorTechnology" Decode.string
                    )
                Components =
                    Decode.dict Decode.int
                    |> get.Required.Field "Components"
                    |> Map.map (fun key count ->
                        count * 1<comp>,
                        comps |> List.find (fun comp -> comp.Id = key)
                    )
                SpareBerths = get.Required.Field "SpareBerths" Decode.int * 1<people>
                CryogenicBerths = get.Required.Field "CryogenicBerths" Decode.int * 1<people>
                DeployTime = get.Required.Field "DeployTime" Decode.float * 1.0<mo>
            } : Ship
        )

let deserialize (comps: ShipComponent list) (techs: Technology.AllTechnologies) version key str =
    let applyDeserialization fn =
        Decode.fromString (fn key comps techs) str

    match version with
    | "1" -> applyDeserialization deserializeShip1
    | s -> Error <| sprintf "Invalid version %s" s
    |> function
    | Ok s -> Success s 
    | Error s -> Failure s