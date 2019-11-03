module Bulma.Table

open Bulma.Button
open Fable.React
open Fable.React.Props

type RowRenderer =
    | Normal
    | Highlighted

type CellRenderer<'a> =
    | String of ('a -> string)
    | Custom of ('a -> Fable.React.ReactElement)
    | Button of (string * ('a -> unit))

type ColumnOptions<'a> =
    {
        Name: string
        Value: CellRenderer<'a>
    }

let inline private tableHead columns =
    columns
    |> List.map (fun column -> td [] [ str column ])
    |> tr []

let inline private tableBody options data selection onSelect =
    data
    |> List.map (fun datum ->
        let renderer =
            match selection with
            | None -> Normal
            | Some selection ->
                match selection = datum with
                | false -> Normal
                | true -> Highlighted

        let cells =
            options
            |> List.map (fun option ->
                match option.Value with
                | String fn -> td [] [ str <| fn datum ]
                | Custom fn -> td [] [ fn datum ]
                | Button (label, onClick) -> Bulma.Button.render label (fun _ -> onClick datum)
            )

        cells
        |> tr [ classList [ "is-selected", renderer = Highlighted ]
                OnClick (fun event ->
                    event.stopPropagation() |> ignore
                    onSelect datum
                )
              ]
    )
    |> tbody []

let render options data selection onSelect =
    let columns = options |> List.map (fun o -> o.Name)
    table [ ClassName "table" ]
          [ tableHead columns
            tableBody options data selection onSelect
          ]
