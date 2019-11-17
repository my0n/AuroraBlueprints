module Cards.ShipDescription

open Fable.React
open Fable.React.Props

open Global

open Bulma.Card
open Model.Measures
open Comp.Ship
open Model.MaintenanceClass
open Comp.ShipComponent

open Nerds.Common
open Nerds.ArmorSizeNerd
open Nerds.CargoCapacityNerd
open Nerds.ComponentArmorNerd
open Nerds.CryogenicBerthsNerd
open Nerds.DeployTimeNerd
open Nerds.ExplosionChanceNerd
open Nerds.FuelCapacityNerd
open Nerds.MagazineCapacityNerd
open Nerds.PowerProductionNerd
open Nerds.PriceTotalNerd
open Nerds.ShipNameNerd
open Nerds.ShipRangeNerd
open Nerds.SizeNerd
open Nerds.SpareBerthsNerd
open Nerds.ThermalSignatureNerd
open Nerds.TimeAtFullPowerNerd
open Nerds.TotalCrewNerd
open Nerds.TractorStrengthNerd
open Nerds.TroopTransportNerd
open Nerds.VelocityNerd

type private ShipDescription =
    | Text of string
    | Nerd of INerd

let private generalOverview (ship: Ship) =
    [
        [
            Nerd { ShipName = ship.Name; ShipClass = ship.ShipClass }
            Nerd { RenderMode = Tons; Count = 1<comp>; Size = ship.Size*1</comp> }
            Nerd { TotalCrew = ship.Crew }
            Nerd { TotalBuildCost = ship.BuildCost }
            Nerd { ThermalSignature = ship.ThermalSignature; EngineCount = ship.EngineCount; EngineContribution = ship.EngineThermalSignatureContribution }
        ]
        [
            Nerd { Speed = ship.Speed }
            Nerd { Width = ship.ArmorWidth; Depth = ship.ArmorDepth }
        ]
        [
            Nerd { DeployTime = ship.DeployTime }
            Nerd { SpareBerths = ship.SpareBerths }
        ]
        [
            Nerd { TroopTransport = ship.TroopTransportCapability; CombatDrop = 0<company>;                CryoDrop = 0<company> }
            Nerd { TroopTransport = 0<company>;                    CombatDrop = ship.CombatDropCapability; CryoDrop = 0<company> }
            Nerd { TroopTransport = 0<company>;                    CombatDrop = 0<company>;                CryoDrop = ship.CryoDropCapability }
            Nerd { Ammo = ship.TotalAmmoCapacity / 1<comp>; Count = 1<comp> }
            Nerd { CargoCapacity = ship.CargoCapacity }
            Nerd { CryogenicBerths = ship.CryogenicBerths }
            Nerd { TractorStrength = ship.CargoHandlingMultiplier; LoadTime = ship.LoadTime }
        ]
    ]

let private fuelCapAndRange (ship: Ship) =
    let canMove = ship.HasEngines && ship.FuelCapacity > 0.0<l>
    [
        (match ship.FuelCapacity with
         | fc when fc > 0.0<l> ->
            Nerd { FuelCapacity = ship.FuelCapacity } |> Some
         | _ ->
            None
        )

        (match canMove with
         | true ->
            Nerd { Range = ship.FuelRange } |> Some
         | false ->
            Text "Range N/A" |> Some
        )

        (match canMove with
         | true ->
            Nerd { FullPowerTime = ship.FullPowerTime } |> Some
         | false ->
            None
        )
    ]
    |> List.choose id
    |> List.map List.wrap

let private powerAndMaintenanceClass ship =
    let pp =
        ship.Components
        |> Map.values
        |> List.map (function
            | count, PowerPlant c when count > 0<comp> ->
                [
                    Text c.Name
                    Text <| sprintf "(%d)" count
                    Nerd { Count = count; PowerOutput = c.Power }
                    Nerd { Depth = 0 }
                    Nerd { ExplosionChance = c.PowerBoost.ExplosionChance }
                ]
                |> Some
            | _ -> None
        )
        |> List.choose id

    let maint =
        match ship.MaintenanceClass with
        | Commercial -> "This design is classed as a Commercial Vessel for maintenance purposes"
        | Military -> "This design is classed as a Military Vessel for maintenance purposes"
        |> Text
        |> List.wrap

    pp @ [ maint ]

let private describe (ship: Ship) =
    [
        Some generalOverview

        (match ship.FuelCapacity > 0.0<l> || ship.HasEngines with
         | true -> Some fuelCapAndRange
         | false -> None
        )

        Some powerAndMaintenanceClass
    ]
    |> List.choose id
    |> List.map (fun a -> a ship)

let private renderNerd (nerd: INerd) =
    nerd.Description
    |> Option.map (fun d ->
        div [ HTMLAttr.Title nerd.Tooltip ] [ str d ]
    )

let render ship =
    let contents =
        let renderDescription desc =
            match desc with
            | Text s ->
                span [ ClassName "ship-description" ] [ str s ]
                |> Some
            | Nerd nerd ->
                nerd
                |> renderNerd
                |> Option.map (fun nerd ->
                    div [ ClassName "ship-description" ] [ nerd ]
                )
        let renderLine = List.map renderDescription >> List.choose id >> div []
        let renderBlock = List.map renderLine >> div [ ClassName "ship-description-block" ]
        let renderContents = List.map renderBlock >> div [ ClassName "ship-description-block" ]

        ship
        |> describe
        |> renderContents
    Bulma.Card.render {
        key = "description"
        HeaderItems = [ Title "Description" ]
        Contents = [ contents ]
        Actions = []
        HasExpanderToggle = true
    }
