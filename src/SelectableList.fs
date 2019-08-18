module SelectableList

open Fable.Helpers.React
open Fable.Helpers.React.Props

open Global
open TableCommon
open Types

type SelectableListOptions<'a> =
    {
        Columns: string list
        RowRenderer: 'a -> CellRenderer list
        OnSelect: 'a -> Msg
    }

let private tableBody options dispatch rows selection =
    rows
    |> Seq.map (fun row ->
        (row, match selection with None -> false | Some s -> row = s)
    )
    |> Seq.map (fun (row, selected) ->
        row
        |> options.RowRenderer
        |> Seq.map (tableCell dispatch)
        |> Seq.map List.wrap
        |> Seq.map (td [])
        |> tr [
                classList [ "is-selected", selected ]
                OnClick (fun event ->
                    event.stopPropagation() |> ignore
                    options.OnSelect row |> dispatch
                )
              ]
    )
    |> tbody []

let selectableList options dispatch rows selection =
    table [ ClassName "table" ]
          [
            tableHead options.Columns
            tableBody options dispatch rows selection
          ]
