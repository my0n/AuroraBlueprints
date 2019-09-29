module Cards.Engine

open Model.Measures

open Global
open System

open App.Msg
open Bulma.Card
open Cards.Common
open Model.Technology
open Comp.Engine
open Comp.ShipComponent
open Comp.Ship

open Nerds.MaintenanceClassNerd
open Nerds.PriceNerd
open Nerds.SizeNerd
open Nerds.EnginePowerNerd
open Nerds.FuelConsumptionNerd

let render (ship: Ship) (comp: Engine) dispatch =
    let header =
        [
            Name comp.Name
            Nerd { MaintenanceClass = comp.MaintenanceClass }
            Nerd { Count = comp.Count; BuildCost = comp.BuildCost }
            Nerd { RenderMode = HS; Count = comp.Count; Size = comp.Size * 50<ton/hs> }
            Nerd { Count = comp.Count; EnginePower = comp.EnginePower; Size = comp.Size * 50<ton/hs>; Speed = ship.Speed }
            Nerd { Count = comp.Count; Consumption = comp.FuelConsumption; Efficiency = comp.FuelConsumption / comp.EnginePower }
        ]
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
                        (fun n -> Msg.ReplaceShipComponent (ship, Engine { comp with Count = n }) |> dispatch)
                    Bulma.FC.WithLabel
                        "Name"
                        [
                            Bulma.FC.AddonGroup
                                [
                                    Bulma.FC.TextInput
                                        comp.Name
                                        (fun n -> Msg.ReplaceShipComponent (ship, Engine { comp with Name = n }) |> dispatch)
                                    Bulma.FC.Button
                                        "Generate"
                                        (fun _ -> Msg.ReplaceShipComponent (ship, Engine { comp with Name = comp.GeneratedName }) |> dispatch)
                                ]
                        ]
                    Bulma.FC.WithLabel
                        "Manufacturer"
                        [
                            Bulma.FC.TextInput
                                comp.Manufacturer
                                (fun n -> Msg.ReplaceShipComponent (ship, Engine { comp with Manufacturer = n }) |> dispatch)
                        ]
                ]
            Bulma.FC.HorizontalGroup
                None
                [
                    Bulma.FC.IntInput
                        {
                            Label = Some "Size"
                            Value = comp.Size
                            Min = Some 1
                            Max = Some 50
                        }
                        (fun n -> Msg.ReplaceShipComponent (ship, Engine { comp with Size = n }) |> dispatch)
                    Bulma.FC.Select
                        {
                            Label = Some "Engine Technology"
                            Options =
                                Technology.engine
                                |> Map.toListV (fun v -> String.Format("{0} ({1:0} EP/HS)", v.Name, v.PowerPerHs))
                            Value = comp.EngineTech.Level
                        }
                        (fun n -> Msg.ReplaceShipComponent (ship, Engine { comp with EngineTech = Technology.engine.[n] }) |> dispatch)
                    Bulma.FC.Select
                        {
                            Label = Some "Engine Efficiency"
                            Options =
                                Technology.engineEfficiency
                                |> Map.toListV (fun v -> sprintf "%.2fx fuel consumption" v.Efficiency)
                            Value = comp.EfficiencyTech.Level
                        }
                        (fun n -> Msg.ReplaceShipComponent (ship, Engine { comp with EfficiencyTech = Technology.engineEfficiency.[n] }) |> dispatch)
                    Bulma.FC.Select
                        {
                            Label = Some "Engine Power"
                            Options =
                                Technology.allPowerMods
                                |> Map.toListV (fun v -> sprintf "%.2fx engine power" v.PowerMod)
                            Value = comp.PowerModTech.Level
                        }
                        (fun n -> Msg.ReplaceShipComponent (ship, Engine { comp with PowerModTech = Technology.allPowerMods.[n] }) |> dispatch)
                    Bulma.FC.Select
                        {
                            Label = Some "Thermal Efficiency"
                            Options =
                                Technology.thermalEfficiency
                                |> Map.toListV (fun v -> v.Name)
                            Value = comp.ThermalEfficiencyTech.Level
                        }
                        (fun n -> Msg.ReplaceShipComponent (ship, Engine { comp with ThermalEfficiencyTech = Technology.thermalEfficiency.[n] }) |> dispatch)
                ]
        ]
    let actions =
        [
            "Remove", DangerColor, (fun _ -> Msg.RemoveComponentFromShip (ship, Engine comp) |> dispatch)
        ]
    shipComponentCard header form actions
