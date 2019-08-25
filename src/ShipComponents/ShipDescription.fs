module ShipComponents.ShipDescription

open Fable.Helpers.React
open Fable.Helpers.React.Props

open Types
open Measures
open Global
open Bulma.Card

type private ShipDescription =
    | Block of ShipDescription list
    | Line of ShipDescription list
    | If of bool * ShipDescription list
    | Space
    | Text of string
    | Label of string
    | Size of float<hs>
    | Crew of int<people>
    | BuildPoints of float

let private describe ship =
    Block [ Line [ Text ship.Name; Label "class"; Text ship.ShipClass; Space; Size ship.Size; Space; Crew ship.Crew; Space; BuildPoints ship.BuildPoints ]
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
        span [ ClassName "ship-description" ] [ str s ]
    | Size s ->
        span [ ClassName "ship-description" ] [ str << sprintf "%.0f tons" <| toTons s ]
    | Crew s ->
        span [ ClassName "ship-description" ] [ str <| sprintf "%d crew" s ]
    | BuildPoints s ->
        span [ ClassName "ship-description" ] [ str <| sprintf "%.0f BP" s ]

let render ship =
    let contents = ship
                   |> describe
                   |> renderDescription
    Bulma.Card.render (Some [ Title "Description" ]) [ contents ]
