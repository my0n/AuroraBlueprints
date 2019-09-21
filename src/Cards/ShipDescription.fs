module Cards.ShipDescription

open Fable.Helpers.React
open Fable.Helpers.React.Props

open Global
open System

open Bulma.Card
open Model.Measures
open Ship
open Model.MaintenanceClass
open Comp.ShipComponent

open Nerds.Common
open Nerds.ArmorSizeNerd
open Nerds.DeployTimeNerd
open Nerds.ShipNameNerd
open Nerds.ThermalSignatureNerd
open Nerds.VelocityNerd

type private SizeOptions =
    | HS
    | Tons

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
    | Block of ShipDescription list
    | Line of ShipDescription list
    | If of bool * ShipDescription list
    | Text of string
    | Label of string
    | Size of SizeOptions * float<hs>
    | People of PeopleOptions * int<people>
    | BP of float
    | Time of TimeOptions * float<mo>
    | FuelCapacity of FuelCapacityOptions * float<kl>
    | Range of RangeOptions * float<km>
    | Nerd of INerd

let private generalOverview (ship: Ship) =
    Block [ Line [ Nerd { ShipName = ship.Name; ShipClass = ship.ShipClass }
                   Size (Tons, ship.Size)
                   People (Crew, ship.Crew)
                   BP ship.BuildCost.BuildPoints
                   Nerd { ThermalSignature = ship.ThermalSignature; EngineCount = ship.EngineCount; EngineContribution = ship.EngineThermalSignatureContribution }
                 ]
            Line [ Nerd { Speed = ship.Speed }
                   Nerd { Width = ship.ArmorWidth; Depth = ship.ArmorDepth }
                 ]
            Line [ Nerd { DeployTime = ship.DeployTime }
                   If (ship.SpareBerths > 0<people>,
                       [ Label "Spare Berths"; People (NoLabel, ship.SpareBerths) ])
                 ]
            Line [ If (ship.CryogenicBerths > 0<people>,
                       [ Label "Cryogenic Berths"; People (NoLabel, ship.CryogenicBerths) ])
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
    |> List.map Line
    |> Block

let private powerAndMaintenanceClass ship =
    let pp =
        ship.Components
        |> Map.values
        |> List.map (fun c ->
            match c with
            | PowerPlant c when c.Count > 0<comp> ->
                [ Text c.Name
                  Text <| sprintf "(%d)" c.Count
                  Label "Total Power Output"
                  Text <| sprintf "%.1f" (c.Power * int2float c.Count)
                  Label "Armour"
                  Text "0"
                  Label "Exp"
                  Text <| sprintf "%.0f%%" (c.PowerBoost.ExplosionChance * 100.0)
                ]
                |> Line |> Some
            | _ -> None
        )
        |> List.choose id

    let maint =
        match ship.MaintenanceClass with
        | Commercial -> "This design is classed as a Commercial Vessel for maintenance purposes"
        | Military -> "This design is classed as a Military Vessel for maintenance purposes"
        |> Text |> List.wrap
        |> Line

    pp @ [ maint ]
    |> Block

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
    |> Block

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

let rec private renderDescription desc =
    match desc with
    | Block c ->
        div [ ClassName "ship-description-block" ] (List.map renderDescription c)
    | Line c ->
        div [] (List.map renderDescription c)
    | If (pred, c) ->
        match pred with
        | true -> div [] (List.map renderDescription c)
        | false -> div [] []
    | Text s ->
        span [ ClassName "ship-description" ] [ str s ]
    | Label s ->
        span [ ClassName "ship-description has-text-light" ] [ str s ]
    | Size (opt, s) ->
        match opt with
        | HS -> span [ ClassName "ship-description" ] [ str <| sprintf "%.0f HS" s ]
        | Tons -> span [ ClassName "ship-description" ] [ str << sprintf "%.0f tons" <| hs2ton s ]
    | People (opt, s) ->
        match opt with
        | NoLabel -> span [ ClassName "ship-description" ] [ str <| sprintf "%d" s ]
        | Crew -> span [ ClassName "ship-description" ] [ str <| sprintf "%d crew" s ]
    | BP s ->
        span [ ClassName "ship-description" ] [ str <| sprintf "%.0f BP" s ]
    | Time (opt, t) -> renderTime opt t
    | FuelCapacity (opt, fc) ->
        match opt with
        | Litres -> span [ ClassName "ship-description" ] [ str << sprintf "%.0f Litres" <| kl2l fc ]
    | Range (opt, r) ->
        match opt with
        | BillionKm -> span [ ClassName "ship-description" ] [ str <| sprintf "%.1f billion km" (r / 1000000000.0) ]
    | Nerd (nerd) ->
        div [ ClassName "ship-description" ] [ nerd |> Nerds.Common.render DescriptiveForm ]

let render ship =
    let contents = ship
                   |> describe
                   |> renderDescription
    Bulma.Card.render {
        HeaderItems = [ Title "Description" ]
        Contents = [ contents ]
        Actions = []
        HasExpanderToggle = true
    }
