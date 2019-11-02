module TechView

open Fable.Helpers.React
open Fable.Helpers.React.Props

open System
open Global

open App.Model
open App.Msg
open Bulma.FC

let techList header techsToDisplay currentTechs dispatch =
    let l =
        techsToDisplay
        |> List.map (fun (tech: #Technology.TechBase) ->
            let researched =
                currentTechs
                |> List.contains tech.Id
            let available =
                researched 
                || tech.Parents
                |> List.forall (fun parent ->
                    List.contains parent currentTechs
                )
            let text = tech.Name

            let opts =
                {
                    Disabled = not available
                    Checked = researched
                    Label = text
                    Unselectable = true
                }

            Bulma.FC.Checkbox opts (fun value ->
                Fable.Import.Browser.console.log (value)
                match value with
                | true ->  AddTechnology tech.Id
                | false -> RemoveTechnology tech.Id
                |> dispatch
            )
            |> List.wrap
            |> div [ Key (tech.Id.ToString()) ]
        )
        |> ofList
        |> List.wrap
        |> ul []

    div []
        [
            br []
            div [ ClassName "title is-unselectable is-4" ] [ str header ]
            l
        ]

let root model dispatch =
    div [ ClassName "columns" ]
        [
            div [ ClassName "column is-2" ] []
            div [ ClassName "column is-3" ]
                [
                    techList "Armor" model.AllTechnologies.Armor model.CurrentTechnology dispatch
                    techList "Cargo Handling" model.AllTechnologies.CargoHandling model.CurrentTechnology dispatch
                    techList "Geo Sensors" model.AllTechnologies.GeoSensors model.CurrentTechnology dispatch
                    techList "Grav Sensors" model.AllTechnologies.GravSensors model.CurrentTechnology dispatch
                    techList "Magazine Efficiency" model.AllTechnologies.MagazineEfficiency model.CurrentTechnology dispatch
                    techList "Magazine Ejection" model.AllTechnologies.MagazineEjection model.CurrentTechnology dispatch
                ]
            div [ ClassName "column is-3" ]
                [
                    techList "Reactors" model.AllTechnologies.Reactors model.CurrentTechnology dispatch
                    techList "Reactor Power Boost" model.AllTechnologies.ReactorsPowerBoost model.CurrentTechnology dispatch
                    techList "Engine Thermal Efficiency" model.AllTechnologies.EngineThermalEfficiency model.CurrentTechnology dispatch
                ]
            div [ ClassName "column is-3" ]
                [
                    techList "Engines" model.AllTechnologies.Engines model.CurrentTechnology dispatch
                    techList "Engine Power Mod" model.AllTechnologies.EnginePowerMod model.CurrentTechnology dispatch
                    techList "Engine Efficiency" model.AllTechnologies.EngineEfficiency model.CurrentTechnology dispatch
                ]
        ]
