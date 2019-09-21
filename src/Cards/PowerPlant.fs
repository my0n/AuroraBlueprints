module Cards.PowerPlant

open App.Msg
open Bulma.Card
open Cards.Common
open Model.Measures
open Comp.PowerPlant
open Comp.ShipComponent
open Comp.Ship
open Model.Technology
open System
open Global

open Nerds.PriceNerd
open Nerds.SizeFloatNerd
open Nerds.PowerProductionNerd

let render (ship: Ship) (comp: PowerPlant) dispatch =
    let header =
        [
            Name comp.Name
            Nerd { Count = comp.Count; BuildCost = comp.BuildCost }
            Nerd { Count = comp.Count; Size = comp.Size }
            Nerd { Count = comp.Count; PowerOutput = comp.Power }
        ]

    let sizeOptions =
        [ 0.1 .. 0.1 .. 0.9] @ [1.0 .. 30.0]

    let form =
        [
            Bulma.FC.HorizontalGroup
                None
                [
                    Bulma.FC.IntInput
                        {
                            Label = Some "Count"
                            Value = comp.Count
                            Min = Some 0
                            Max = None
                        }
                        (fun n -> Msg.ReplaceShipComponent (ship, PowerPlant { comp with Count = n }) |> dispatch)
                    Bulma.FC.WithLabel
                        "Name"
                        [
                            Bulma.FC.AddonGroup
                                [
                                    Bulma.FC.TextInput
                                        comp.Name
                                        (fun n -> Msg.ReplaceShipComponent (ship, PowerPlant { comp with Name = n }) |> dispatch)
                                    Bulma.FC.Button
                                        "Generate"
                                        (fun _ -> Msg.ReplaceShipComponent (ship, PowerPlant { comp with Name = comp.GeneratedName }) |> dispatch)
                                ]
                        ]
                    Bulma.FC.WithLabel
                        "Manufacturer"
                        [
                            Bulma.FC.TextInput
                                comp.Manufacturer
                                (fun n -> Msg.ReplaceShipComponent (ship, PowerPlant { comp with Manufacturer = n }) |> dispatch)
                        ]
                ]
            Bulma.FC.HorizontalGroup
                None
                [
                    Bulma.FC.Select
                        {
                            Label = Some "Size"
                            Options =
                                sizeOptions
                                |> List.mapi (fun i o -> i, sprintf "%.1f" o)
                            Value =
                                sizeOptions
                                |> List.tryFindIndex (fun o -> o = comp.Size * 1.0<comp/hs>)
                                |> Option.defaultValue 1
                        }
                        (fun n -> Msg.ReplaceShipComponent (ship, PowerPlant { comp with Size = sizeOptions.[n] * 1.0<hs/comp> }) |> dispatch)
                    Bulma.FC.Select
                        {
                            Label = Some "Power Plant Technology"
                            Options =
                                Technology.powerPlant
                                |> Map.toListV (fun v -> String.Format("{0} ({1} power/HS)", v.Name, v.PowerOutput))
                            Value = comp.Technology.Level
                        }
                        (fun n -> Msg.ReplaceShipComponent (ship, PowerPlant { comp with Technology = Technology.powerPlant.[n] }) |> dispatch)
                    Bulma.FC.Select
                        {
                            Label = Some "Power Boost"
                            Options =
                                Technology.powerBoost
                                |> Map.toListV (fun v -> sprintf "Reactor Power Boost %d%%, Explosion %d%%" (int (v.PowerBoost * 100.0)) (int (v.ExplosionChance * 100.0)))
                            Value = comp.PowerBoost.Level
                        }
                        (fun n -> Msg.ReplaceShipComponent (ship, PowerPlant { comp with PowerBoost = Technology.powerBoost.[n] }) |> dispatch)
                ]
        ]
    let actions =
        [
            "Remove", DangerColor, (fun _ -> Msg.RemoveComponentFromShip (ship, PowerPlant comp) |> dispatch)
        ]
    shipComponentCard header form actions
