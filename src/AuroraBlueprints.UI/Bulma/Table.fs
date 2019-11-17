module Bulma.Table

open Fable.React
open Fable.React.Props

type RowRenderer =
    | Normal
    | Highlighted

type ColumnOptions<'a> =
    {
        Name: string
        Render: 'a -> Fable.React.ReactElement
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

        options
        |> List.map (fun option ->
            td [] [ option.Render datum ]
        )
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
