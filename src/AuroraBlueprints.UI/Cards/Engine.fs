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

let render (allTechs: AllTechnologies) (tech: GameObjectId list) (ship: Ship) (count: int<comp>) (comp: Engine) dispatch =
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
                    boundTechField tech
                        "Engine Technology"
                        allTechs.Engines
                        (fun t -> String.Format("{0} EP - {1}", t.PowerPerHs, t.Name))
                        comp.EngineTech
                        (fun n -> Msg.UpdateComponent (Engine { comp with EngineTech = n }) |> dispatch)
                    boundTechField tech
                        "Fuel Consumption"
                        allTechs.EngineEfficiency
                        (fun t -> sprintf "x%.3f fuel" (t.Efficiency / 1000.0))
                        comp.EfficiencyTech
                        (fun n -> Msg.UpdateComponent (Engine { comp with EfficiencyTech = n }) |> dispatch)
                    boundFloatChoiceField (allTechs.UnlockedPowerMods tech) ship dispatch
                        "Engine Power"
                        allTechs.AllPowerMods
                        comp.PowerModTech
                        (fun n -> sprintf "x%.2f EP / x%.3f fuel" n (Math.Pow(n, 2.5)))
                        (fun n -> Engine { comp with PowerModTech = n })
                    boundTechField tech
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
    shipComponentCard (comp.Id.ToString ()) header form actions
