module TechView

open Fable.Helpers.React
open Fable.Helpers.React.Props

open Global

open Model.Measures
open App.Model
open App.Msg
open Bulma.Button
open Bulma.Table
open Bulma.FC
open Comp.ShipComponent
open Comp.Ship
open Fable.Import.React

let changeTech value tech =
    match value with
    | true ->
        AddTechnology tech
    | false ->
        RemoveTechnology tech

let techs (all: Technology.TechNode list) current dispatch =
    all
    |> List.map (fun tech ->
        let researched =
            current
            |> Set.contains tech.Tech
        let available =
            researched 
            || tech.Parents
            |> List.forall (fun parent ->
                current
                |> Set.contains parent
            )
        let text = tech.Name

        let opts =
            {
                Disabled = not available
                Checked = researched
                Label = text
            }

        Bulma.FC.Checkbox opts (fun value -> changeTech value tech.Tech |> dispatch)
        |> List.wrap
        |> div [ Key (tech.Tech.ToString()) ]
    )
    |> ofList
    |> List.wrap
    |> ul []

let root model dispatch =
    div [ ClassName "columns" ]
        [
            div [ ClassName "column is-2" ] [ ]
            div [ ClassName "column is-8" ] [ techs Technology.allTechnologies model.CurrentTechnology dispatch ]
            div [ ClassName "column" ] [ ]
        ]
