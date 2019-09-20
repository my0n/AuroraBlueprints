module Cards.Common

open Global

open Bulma.Card
open Nerds.Common

type ShipComponentCardHeaderItem =
    | Name of string
    | Nerd of INerd

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
                |> Nerds.Common.render
                |> Info
    )

let shipComponentCard header contents actions =
    Bulma.Card.render {
        HeaderItems = renderHeader header
        Contents = contents
        Actions = actions
        HasExpanderToggle = true
    }
