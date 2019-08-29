module ShipComponents.ShipDescription

open Fable.Helpers.React
open Fable.Helpers.React.Props

open Types
open Measures
open Global
open Bulma.Card
open System

type private SizeOptions =
    | HS
    | Tons

type private PeopleOptions =
    | NoLabel
    | Crew

type private ShipDescription =
    | Block of ShipDescription list
    | Line of ShipDescription list
    | If of bool * ShipDescription list
    | Space
    | Text of string
    | Label of string
    | Size of SizeOptions * float<hs>
    | People of PeopleOptions * int<people>
    | BP of float
    | Time of float<mo>

let private describe ship =
    Block [ Line [ Text ship.Name; Label "class"; Text ship.ShipClass
                   Space
                   Size (Tons, ship.Size)
                   Space
                   People (Crew, ship.Crew)
                   Space
                   BP ship.BuildCost.BuildPoints
                 ]
            Line [ Label "Intended Deployment Time"; Time ship.DeployTime
                   Space
                   If (ship.SpareBerths > 0<people>, [ Label "Spare Berths"; People (NoLabel, ship.SpareBerths) ])
                 ]
            Line [ If (ship.CryogenicBerths > 0<people>, [ Label "Cryogenic Berths"; People (NoLabel, ship.CryogenicBerths) ])
                 ]
          ]

let rec private renderDescription desc =
    match desc with
    | Block c ->
        div [] (List.map renderDescription c)
    | Line c ->
        div [] (List.map renderDescription c)
    | If (pred, c) ->
        match pred with
        | true -> div [] (List.map renderDescription c)
        | false -> div [] []
    | Space ->
        span [ ClassName "spacer" ] []
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
    | Time t ->
        span [ ClassName "ship-description" ] [ str <| String.Format("{0} months", t) ]

let render ship =
    let contents = ship
                   |> describe
                   |> renderDescription
    Bulma.Card.render (Some [ Title "Description" ]) [ contents ]
