module SelectableList

open Fable.Helpers.React
open Fable.Helpers.React.Props

open Global
open TableCommon

type SelectableListOptions<'a> =
    {
        Columns: string list
        RowRenderer: 'a -> CellRenderer list
    }

let private tableBody rowFn dispatch rows selection =
    rows
    |> Seq.map (fun row ->
        (row, match selection with None -> false | Some s -> row = s)
    )
    |> Seq.map (fun (row, selected) ->
        row
        |> rowFn
        |> Seq.map (tableCell dispatch)
        |> Seq.map List.wrap
        |> Seq.map (td [])
        |> tr [ classList [ "is-selected", selected ] ]
    )
    |> tbody []

let selectableList options dispatch rows selection =
    table [ ClassName "table" ]
          [
            tableHead options.Columns
            tableBody options.RowRenderer dispatch rows selection
          ]
