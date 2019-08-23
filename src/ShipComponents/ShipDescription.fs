module ShipDescription

open Fable.Helpers.React
open Fable.Helpers.React.Props

open Types
open Global

type private ShipDescription =
    | Block of ShipDescription list
    | Line of ShipDescription list
    | If of bool * ShipDescription list
    | Space
    | Text of string
    | Label of string
    | Size of float<hs>

let private describe ship =
    Block [ Line [ Text ship.Name; Label "class"; Text ship.ShipClass; Space; Size ship.Size; Space ]
          ]

let rec private render desc =
    match desc with
    | Block c ->
        div [] (List.map render c)
    | Line c ->
        div [] (List.map render c)
    | If (pred, c) ->
        match pred with
        | true -> div [] (List.map render c)
        | false -> div [] []
    | Space ->
        span [ ClassName "spacer" ] []
    | Text s ->
        span [ ClassName "ship-description" ] [ str s ]
    | Label s ->
        span [ ClassName "ship-description" ] [ str s ]
    | Size s ->
        span [ ClassName "ship-description" ] [ str << sprintf "%.0f tons" <| toTons s ]

let descriptionBox ship =
    ship
    |> describe
    |> render
