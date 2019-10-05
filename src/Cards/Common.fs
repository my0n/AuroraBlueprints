module Cards.Common

open Global

open Fable.Helpers.React
open Fable.Helpers.React.Props
open Bulma.Card
open Nerds.Common

type ShipComponentCardHeaderItem =
    | Name of string
    | Nerd of INerd

let renderNerd (nerd: INerd) =
    div [ HTMLAttr.Title nerd.Tooltip ]
        [ str nerd.Text
          i [ ClassName << Nerds.Icon.render <| nerd.Icon ]
            []
        ]

let inline private renderHeader header: CardHeaderElement list =
    header
    |> List.map (fun h ->
        match h with
        | Name name -> Title name
        | Nerd nerd ->
            match nerd.Render with
            | false -> NoRender
            | true ->
                nerd
                |> renderNerd
                |> Info
    )

let shipComponentCard key header contents actions =
    Bulma.Card.render {
        key = key
        HeaderItems = renderHeader header
        Contents = contents
        Actions = actions
        HasExpanderToggle = true
    }
