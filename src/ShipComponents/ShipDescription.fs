module ShipDescription

open Fable.Helpers.React
open Fable.Helpers.React.Props

open Types
open Global

type private ShipDescription =
    | Block of ShipDescription list
    | Inline of ShipDescription list
    | Spacer
    | Text of string
    | Label of string
    | Size of float<hs>

let private describe ship =
    Block [ Inline [ Text ship.Name
                     Label "class"
                     Text ship.ShipClass
                     Spacer
                     Size ship.Size
                     Spacer
                   ]
          ]

let rec private render desc =
    match desc with
    | Block c ->
        p [] (List.map render c)
    | Inline c ->
        div [] (List.map render c)
    | Spacer ->
        span [] []
    | Text s ->
        span [] [ str s ]
    | Label s ->
        span [] [ str s ]
    | Size s ->
        span [] [ str << sprintf "%.0f tons" <| toTons s ]

let descriptionBox ship =
    ship
    |> describe
    |> render
