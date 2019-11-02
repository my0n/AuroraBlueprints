module Cards.Engine

open Model.Measures

open Global
open System

open App.Msg
open Bulma.Card
open Cards.Common
open Comp.Engine
open Comp.ShipComponent
open Comp.Ship

open Nerds.MaintenanceClassNerd
open Nerds.PriceNerd
open Nerds.SizeNerd
open Nerds.EnginePowerNerd
open Nerds.FuelConsumptionNerd

open Technology

let render (allTechs: AllTechnologies) (tech: GameObjectId list) (ship: Ship) (comp: Engine) dispatch =
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
                            Disabled = false
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
                            Disabled = false
                        }
                        (fun n -> Msg.ReplaceShipComponent (ship, Engine { comp with Size = n }) |> dispatch)
                    boundTechField tech ship dispatch
                        "Engine Technology"
                        allTechs.Engines
                        comp.EngineTech
                        (fun n -> Engine { comp with EngineTech = n })
                    boundTechField tech ship dispatch
                        "Engine Efficiency"
                        allTechs.EngineEfficiency
                        comp.EfficiencyTech
                        (fun n -> Engine { comp with EfficiencyTech = n })
                    boundFloatChoiceField (allTechs.UnlockedPowerMods tech) ship dispatch
                        "Engine Power"
                        allTechs.AllPowerMods
                        comp.PowerModTech
                        (fun n -> sprintf "Engine Power x%.2f" n)
                        (fun n -> Engine { comp with PowerModTech = n })
                    boundTechField tech ship dispatch
                        "Thermal Efficiency"
                        allTechs.EngineThermalEfficiency
                        comp.ThermalEfficiencyTech
                        (fun n -> Engine { comp with ThermalEfficiencyTech = n })
                ]
        ]
    let actions =
        [
            "Remove", DangerColor, (fun _ -> Msg.RemoveComponentFromShip (ship, Engine comp) |> dispatch)
        ]
    shipComponentCard (comp.Id.ToString ()) header form actions
