module TechView

open Fable.Helpers.React
open Fable.Helpers.React.Props

open System
open Global

open App.Model
open App.Msg
open Bulma.FC

let techs (allTechs: Technology.AllTechnologies) (current: Guid list) dispatch =
    allTechs.Technologies
    |> Map.values
    |> List.map (fun tech ->
        let researched =
            current
            |> List.contains tech.Guid
        let available =
            researched 
            || tech.Parents
            |> List.forall (fun parent ->
                current
                |> List.contains parent
            )
        let text = tech.Name

        let opts =
            {
                Disabled = not available
                Checked = researched
                Label = text
            }

        Bulma.FC.Checkbox opts (fun value ->
            match value with
            | true ->  AddTechnology tech.Guid
            | false -> RemoveTechnology tech.Guid
            |> dispatch
        )
        |> List.wrap
        |> div [ Key (tech.Guid.ToString()) ]
    )
    |> ofList
    |> List.wrap
    |> ul []

let root model dispatch =
    div [ ClassName "columns" ]
        [
            div [ ClassName "column is-2" ] [ ]
            div [ ClassName "column is-8" ] [ techs model.AllTechnologies model.CurrentTechnology dispatch ]
            div [ ClassName "column" ] [ ]
        ]
