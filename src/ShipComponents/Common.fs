module ShipComponents.Common

open Fable.Helpers.React
open Fable.Helpers.React.Props

open Global

type ShipComponentCardHeaderItem =
    | Name of string
    | Size of float<hs>
    | FuelCapacity of float<l>

let shipComponentCard header contents =
    let headerChunks =
        header
        |> List.map (fun h ->
            match h with
            | Name name ->
                div [ ClassName "is-size-4 level-item level-left" ] [ str name ]
            | Size size ->
                div [ ClassName "level-item level-right" ] [ str <| sprintf "%.1f HS" size ]
            | FuelCapacity fc ->
                div [ ClassName "level-item level-right" ] [ str <| sprintf "%.1f kL" (toKiloliters fc) ]
        )

    div []
        [ div [ ClassName "level" ] headerChunks
          div [] contents
        ]
