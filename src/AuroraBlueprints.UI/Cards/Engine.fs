module Cards.Engine

open Model.Measures

open Global
open System

open State.Msg
open State.UI
open State.Model
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

let render (comp: Engine) (count: int<comp>) (model: State.Model.Model) (ship: Ship) dispatch =
    let currentTech = model.CurrentTechnology
    let allTechs = model.AllTechnologies
    let key = ship.Id.ToString() + comp.Id.ToString()
    let expanded = model |> Model.isExpanded key

    let header =
        [
            Name comp.Name
            Nerd { MaintenanceClass = comp.MaintenanceClass }
            Nerd { Count = count; BuildCost = comp.BuildCost }
            Nerd { Count = count; RenderMode = HS; Size = comp.Size * 50<ton/hs> }
            Nerd { Count = count; EnginePower = comp.EnginePower; Size = comp.Size * 50<ton/hs>; Speed = ship.Speed }
            Nerd { Count = count; Consumption = comp.FuelConsumption; Efficiency = comp.FuelConsumption / comp.EnginePower }
        ]
    let form =
        [
            Bulma.FC.HorizontalGroup
                None
                [
                    boundCountField ship (Engine comp) dispatch
                        "Count"
                        count
                    Bulma.FC.WithLabel
                        "Name"
                        [
                            Bulma.FC.AddonGroup
                                [
                                    Bulma.FC.TextInput
                                        comp.Name
                                        (fun n -> Msg.UpdateComponent (Engine { comp with Name = n }) |> dispatch)
                                    Bulma.FC.Button
                                        "Generate"
                                        Bulma.FC.ButtonOpts.Empty
                                        (fun _ -> Msg.UpdateComponent (Engine { comp with Name = comp.GeneratedName }) |> dispatch)
                                ]
                        ]
                    Bulma.FC.WithLabel
                        "Manufacturer"
                        [
                            Bulma.FC.TextInput
                                comp.Manufacturer
                                (fun n -> Msg.UpdateComponent (Engine { comp with Manufacturer = n }) |> dispatch)
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
                        (fun n -> Msg.UpdateComponent (Engine { comp with Size = n }) |> dispatch)
                    boundTechField currentTech
                        "Engine Technology"
                        allTechs.Engines
                        (fun t -> String.Format("{0} EP - {1}", t.PowerPerHs, t.Name))
                        comp.EngineTech
                        (fun n -> Msg.UpdateComponent (Engine { comp with EngineTech = n }) |> dispatch)
                    boundTechField currentTech
                        "Fuel Consumption"
                        allTechs.EngineEfficiency
                        (fun t -> sprintf "x%.3f fuel" t.Efficiency)
                        comp.EfficiencyTech
                        (fun n -> Msg.UpdateComponent (Engine { comp with EfficiencyTech = n }) |> dispatch)
                    boundFloatChoiceField (allTechs.UnlockedPowerMods currentTech) ship dispatch
                        "Engine Power"
                        allTechs.AllPowerMods
                        comp.PowerModTech
                        (fun n -> sprintf "x%.2f EP / x%.3f fuel" n (Math.Pow(n, 2.5)))
                        (fun n -> Engine { comp with PowerModTech = n })
                    boundTechField currentTech
                        "Thermal Efficiency"
                        allTechs.EngineThermalEfficiency
                        (fun t -> sprintf "x%.2f therms" t.ThermalEfficiency)
                        comp.ThermalEfficiencyTech
                        (fun n -> Msg.UpdateComponent (Engine { comp with ThermalEfficiencyTech = n }) |> dispatch)
                ]
        ]
    let actions =
        [
            "Remove", DangerColor, (fun _ -> Msg.RemoveComponentFromShip (ship, Engine comp) |> dispatch)
        ]
    shipComponentCard key header form actions expanded dispatch
