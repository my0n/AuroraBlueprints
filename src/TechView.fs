module TechView

open Fable.Helpers.React
open Fable.Helpers.React.Props

open System
open Global

open App.Model
open App.Msg
open Bulma.FC
open Bulma.Card

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

    div [ ClassName "content" ]
        [
            h4 [ ClassName "title is-unselectable is-4" ] [ str header ]
            l
        ]

let defensiveSystemsCard model dispatch =
    {
        key = "DefensiveSystems"
        HeaderItems = [ Title "Defensive Systems" ]
        Contents =
            [
                div [ ClassName "columns" ]
                    [
                        div [ ClassName "column is-4" ]
                            [
                                techList "Armor" model.AllTechnologies.Armor model.CurrentTechnology dispatch
                            ]
                    ]
            ]
        Actions = []
        HasExpanderToggle = true
    }

let powerAndPropulsionCard model dispatch =
    {
        key = "PowerAndPropulsion"
        HeaderItems = [ Title "Power and Propulsion" ]
        Contents =
            [
                div [ ClassName "columns" ]
                    [
                        div [ ClassName "column is-4" ]
                            [
                                techList "Reactors" model.AllTechnologies.Reactors model.CurrentTechnology dispatch
                                techList "Reactor Power Boost" model.AllTechnologies.ReactorsPowerBoost model.CurrentTechnology dispatch
                            ]
                        div [ ClassName "column is-4" ]
                            [
                                techList "Engines" model.AllTechnologies.Engines model.CurrentTechnology dispatch
                                techList "Engine Efficiency" model.AllTechnologies.EngineEfficiency model.CurrentTechnology dispatch
                            ]
                        div [ ClassName "column is-4" ]
                            [
                                techList "Engine Power Mod" model.AllTechnologies.EnginePowerMod model.CurrentTechnology dispatch
                                techList "Engine Thermal Efficiency" model.AllTechnologies.EngineThermalEfficiency model.CurrentTechnology dispatch
                            ]
                    ]
            ]
        Actions = []
        HasExpanderToggle = true
    }

let logisticsAndGroundCombatCard model dispatch =
    {
        key = "LogisticsAndGroundCombat"
        HeaderItems = [ Title "Logistics and Ground Combat" ]
        Contents =
            [
                div [ ClassName "columns" ]
                    [
                        div [ ClassName "column is-4" ]
                            [
                                techList "Cargo Holds" model.AllTechnologies.CargoHolds model.CurrentTechnology dispatch
                            ]
                        div [ ClassName "column is-4" ]
                            [
                                techList "Cargo Handling" model.AllTechnologies.CargoHandling model.CurrentTechnology dispatch
                            ]
                    ]
            ]
        Actions = []
        HasExpanderToggle = true
    }

let missilesAndKineticWeaponsCard model dispatch =
    {
        key = "MissilesAndKineticWeapons"
        HeaderItems = [ Title "Missiles and Kinetic Weapons" ]
        Contents =
            [
                div [ ClassName "columns" ]
                    [
                        div [ ClassName "column is-4" ]
                            [
                                techList "Magazine Efficiency" model.AllTechnologies.MagazineEfficiency model.CurrentTechnology dispatch
                            ]
                        div [ ClassName "column is-4" ]
                            [
                                techList "Magazine Ejection" model.AllTechnologies.MagazineEjection model.CurrentTechnology dispatch
                            ]
                    ]
            ]
        Actions = []
        HasExpanderToggle = true
    }

let sensorsAndFireControlCard model dispatch =
    {
        key = "SensorsAndFireControl"
        HeaderItems = [ Title "Sensors and Fire Control" ]
        Contents =
            [
                div [ ClassName "columns" ]
                    [
                        div [ ClassName "column is-4" ]
                            [
                                techList "Geo Sensors" model.AllTechnologies.GeoSensors model.CurrentTechnology dispatch
                            ]
                        div [ ClassName "column is-4" ]
                            [
                                techList "Grav Sensors" model.AllTechnologies.GravSensors model.CurrentTechnology dispatch
                            ]
                    ]
            ]
        Actions = []
        HasExpanderToggle = true
    }

let root model dispatch =
    div [ ClassName "columns" ]
        [
            div [ ClassName "column is-2" ] []
            div [ ClassName "column" ]
                (
                    [
                        defensiveSystemsCard
                        logisticsAndGroundCombatCard
                        powerAndPropulsionCard
                        missilesAndKineticWeaponsCard
                        sensorsAndFireControlCard
                    ]
                    |> List.map (fun propsFn -> Bulma.Card.render <| propsFn model dispatch)
                )
            div [ ClassName "column is-2" ] []
        ]
