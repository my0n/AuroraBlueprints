module TableCommon

open Fable.Helpers.React
open Fable.Helpers.React.Props

open Global
open Types

type NumberOptions =
    {
        Precision: int
    }

type ButtonOptions =
    {
        Text: string
        OnClick: Msg
    }

type CellRenderer =
    | String of string
    | Integer of int
    | Number of double * NumberOptions
    | Button of ButtonOptions

let tableHead columns =
    columns
    |> Seq.map (fun title -> th [] [str title])
    |> tr []
    |> List.wrap
    |> thead []

let tableCell dispatch cell =
    match cell with
    | String value ->
        str value
    | Integer value ->
        str <| sprintf "%d" value
    | Number (value, options) ->
        str <| sprintf "%.2f" value
    | Button options ->
        p [ ClassName "control" ]
          [
              div [ ClassName "button"
                    OnClick (fun event -> dispatch options.OnClick)
                  ]
                  [ str options.Text ]
          ]
