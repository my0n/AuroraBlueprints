module TechView

open Fable.React
open Fable.React.Props

open Global

open App.Model
open App.Model.UI
open App.Msg
open Bulma.FC
open Bulma.Card

open Model.Technology

let techList header techsToDisplay currentTechs dispatch =
    let l =
        techsToDisplay
        |> List.map (fun (tech: #TechBase) ->
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
            h4 [ ClassName "title is-unselectable is-5" ] [ str header ]
            l
        ]

let defensiveSystemsCard model dispatch =
    "DefensiveSystems",
    "Defensive Systems",
    [
        div [ ClassName "columns" ]
            [
                div [ ClassName "column is-4" ]
                    [
                        techList "Armor" model.AllTechnologies.Armor model.CurrentTechnology dispatch
                    ]
            ]
    ]
let powerAndPropulsionCard model dispatch =
    "PowerAndPropulsion",
    "Power and Propulsion",
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
let logisticsAndGroundCombatCard model dispatch =
    "LogisticsAndGroundCombat",
    "Logistics and Ground Combat",
    [
        div [ ClassName "columns" ]
            [
                div [ ClassName "column is-4" ]
                    [
                        techList "Cargo Holds" model.AllTechnologies.CargoHolds model.CurrentTechnology dispatch
                        techList "Cargo Handling" model.AllTechnologies.CargoHandling model.CurrentTechnology dispatch
                    ]
                div [ ClassName "column is-4" ]
                    [
                        techList "Fuel Storage" model.AllTechnologies.FuelStorages model.CurrentTechnology dispatch
                    ]
                div [ ClassName "column is-4" ]
                    [
                        techList "Troop Transports" model.AllTechnologies.TroopTransports model.CurrentTechnology dispatch
                    ]
            ]
    ]

let missilesAndKineticWeaponsCard model dispatch =
    "MissilesAndKineticWeapons",
    "Missiles and Kinetic Weapons",
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
let sensorsAndFireControlCard model dispatch =
    "SensorsAndFireControl",
    "Sensors and Fire Control",
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

let root model dispatch =
    div [ ClassName "columns" ]
        [
            div [ ClassName "column is-2" ] []
            div [ ClassName "column" ]
                (
                    [
                        defensiveSystemsCard
                        logisticsAndGroundCombatCard
                        missilesAndKineticWeaponsCard
                        powerAndPropulsionCard
                        sensorsAndFireControlCard
                    ]
                    |> List.map (fun propsFn ->
                        let (key, name, contents) = propsFn model dispatch
                        let props =
                            Bulma.Card.CardProps
                                (
                                    name = key,
                                    headerItems = [ Title name ],
                                    contents = contents,
                                    expander =
                                        {
                                            IsExpanded = model |> Model.isExpanded key
                                            OnExpanderToggled = (fun expanded ->
                                                App.Msg.SetSectionExpanded (key, expanded)
                                                |> dispatch
                                            )
                                        }
                                )
                        Bulma.Card.render props
                    )
                )
            div [ ClassName "column is-2" ] []
        ]
