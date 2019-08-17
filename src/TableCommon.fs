module TableCommon

open Fable.Helpers.React

open Global

type NumberOptions =
    {
        Precision: int
    }

type CellRenderer =
    | String of string
    | Integer of int
    | Number of double * NumberOptions

let tableHead columns =
    columns
    |> Seq.map (fun title -> th [] [str title])
    |> tr []
    |> List.wrap
    |> thead []

let tableCell cell =
    match cell with
    | String value ->
        str value
    | Integer value ->
        str <| sprintf "%d" value
    | Number (value, options) ->
        str <| sprintf "%.*f" options.Precision value
