module Cards.ShipDescription

open Fable.Helpers.React
open Fable.Helpers.React.Props

open Global
open System

open Bulma.Card
open Model.Measures
open Comp.Ship
open Model.MaintenanceClass
open Comp.ShipComponent

open Nerds.Common
open Nerds.ArmorSizeNerd
open Nerds.CryogenicBerthsNerd
open Nerds.DeployTimeNerd
open Nerds.PriceTotalNerd
open Nerds.ShipNameNerd
open Nerds.SizeNerd
open Nerds.SpareBerthsNerd
open Nerds.ThermalSignatureNerd
open Nerds.TotalCrewNerd
open Nerds.TroopTransportNerd
open Nerds.VelocityNerd

type private PeopleOptions =
    | NoLabel
    | Crew

type private FuelCapacityOptions =
    | Litres

type private RangeOptions =
    | BillionKm

type private TimeOptions =
    | Months
    | Adaptive

type private ShipDescription =
    | If of bool * ShipDescription list
    | Text of string
    | Label of string
    | Time of TimeOptions * float<mo>
    | FuelCapacity of FuelCapacityOptions * float<kl>
    | Range of RangeOptions * float<km>
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
            Nerd { CryogenicBerths = ship.CryogenicBerths }
        ]
    ]

let private fuelCapAndRange (ship: Ship) =
    let fc =
        match ship.FuelCapacity with
        | fc when fc > 0.0<kl> -> Some [ Label "Fuel Capacity"; FuelCapacity (Litres, ship.FuelCapacity) ]
        | _ -> None
    let range =
        match ship.HasEngines && ship.FuelCapacity > 0.0<kl> with
        | true -> Some [ Label "Range"; Range (BillionKm, ship.FuelRange) ]
        | false -> Some [ Label "Range"; Text "N/A" ]
    let fullPowerTime =
        match ship.HasEngines && ship.FuelCapacity > 0.0<kl> with
        | true -> Some [ Time (Adaptive, ship.FullPowerTime) ]
        | false -> None
    
    [ fc; range; fullPowerTime ]
    |> List.choose id

let private powerAndMaintenanceClass ship =
    let pp =
        ship.Components
        |> Map.values
        |> List.map (fun c ->
            match c with
            | PowerPlant c when c.Count > 0<comp> ->
                [
                    Text c.Name
                    Text <| sprintf "(%d)" c.Count
                    Label "Total Power Output"
                    Text <| sprintf "%.1f" (c.Power * int2float c.Count)
                    Label "Armour"
                    Text "0"
                    Label "Exp"
                    Text <| sprintf "%.0f%%" (c.PowerBoost.ExplosionChance * 100.0)
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

        (match ship.FuelCapacity > 0.0<kl> || ship.HasEngines with
         | true -> Some fuelCapAndRange
         | false -> None
        )

        Some powerAndMaintenanceClass
    ]
    |> List.choose id
    |> List.map (fun a -> a ship)

let private renderTime opt t =
    match opt with
    | Adaptive ->
        let years = mo2year t
        let days = mo2day t
        let hours = day2hr days
        let minutes = hr2min hours
        let seconds = min2s minutes
        [ (float years,   sprintf "%.1f years" years)
          (float days,    sprintf "%.0f days"    <| rounduom 1.0<day> days)
          (float hours,   sprintf "%.0f hours"   <| rounduom 1.0<hr>  hours)
          (float minutes, sprintf "%.0f minutes" <| rounduom 1.0<min> minutes)
          (float seconds, sprintf "%.0f seconds" <| rounduom 1.0<s>   seconds)
        ]
        |> List.tryFind (fun (t, _) -> t > 1.0)
        |> Option.defaultValue (float seconds, sprintf "%.0f seconds" <| rounduom 1.0<s> seconds)
        |> (fun (_, s) -> sprintf "%s at full power" s)
        |> str |> List.wrap
        |> span [ ClassName "ship-description" ]
    | Months -> span [ ClassName "ship-description" ] [ str <| String.Format("{0} months", t) ]


let private renderNerd (nerd: INerd) =
    nerd.Description
    |> Option.map (fun d ->
        div [ HTMLAttr.Title nerd.Tooltip ] [ str d ]
    )

let rec private renderDescription desc =
    match desc with
    | If (pred, c) ->
        match pred with
        | true ->
            div [] (List.map renderDescription c |> List.choose id)
            |> Some
        | false -> None
    | Text s ->
        span [ ClassName "ship-description" ] [ str s ]
        |> Some
    | Label s ->
        span [ ClassName "ship-description has-text-light" ] [ str s ]
        |> Some
    | Time (opt, t) ->
        renderTime opt t
        |> Some
    | FuelCapacity (opt, fc) ->
        match opt with
        | Litres -> span [ ClassName "ship-description" ] [ str << sprintf "%.0f Litres" <| kl2l fc ]
        |> Some
    | Range (opt, r) ->
        match opt with
        | BillionKm -> span [ ClassName "ship-description" ] [ str <| sprintf "%.1f billion km" (r / 1000000000.0) ]
        |> Some
    | Nerd nerd ->
        nerd
        |> renderNerd
        |> Option.map (fun nerd ->
            div [ ClassName "ship-description" ] [ nerd ]
        )

let render ship =
    let contents =
        let renderLine = List.map renderDescription >> List.choose id >> div []
        let renderBlock = List.map renderLine >> div [ ClassName "ship-description-block" ]
        let renderDescription = List.map renderBlock >> div [ ClassName "ship-description-block" ]

        ship
        |> describe
        |> renderDescription
    Bulma.Card.render {
        HeaderItems = [ Title "Description" ]
        Contents = [ contents ]
        Actions = []
        HasExpanderToggle = true
    }
