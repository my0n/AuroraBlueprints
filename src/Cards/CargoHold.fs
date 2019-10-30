module Cards.CargoHold

open Global
open System

open App.Msg
open Bulma.Card
open Cards.Common
open Comp.CargoHold
open Comp.ShipComponent
open Comp.Ship

open Model.Measures

open Nerds.CargoCapacityNerd
open Nerds.PriceTotalNerd
open Nerds.SizeNerd
open Nerds.TractorStrengthNerd

open Technology

let render (allTechs: AllTechnologies) (tech: Guid list) (ship: Ship) (comp: CargoHold) dispatch =
    let header =
        [
            Name "Cargo Hold"
            Nerd { TotalBuildCost = comp.BuildCost }
            Nerd { RenderMode = HS; Count = 1<comp>; Size = comp.TotalSize*1</comp> }
            Nerd { CargoCapacity = comp.CargoCapacity }
            Nerd { TractorStrength = comp.TractorStrength; LoadTime = ship.LoadTime }
        ]
    let form =
        [
            Bulma.FC.HorizontalGroup
                None
                [
                    Bulma.FC.IntInput
                        {
                            Label = Some "Tiny Cargo Hold"
                            Value = comp.Tiny
                            Min = Some 0
                            Max = None
                            Disabled = false
                        }
                        (fun n -> Msg.ReplaceShipComponent (ship, CargoHold { comp with Tiny = n }) |> dispatch)
                    Bulma.FC.IntInput
                        {
                            Label = Some "Small Cargo Hold"
                            Value = comp.Small
                            Min = Some 0
                            Max = None
                            Disabled = false
                        }
                        (fun n -> Msg.ReplaceShipComponent (ship, CargoHold { comp with Small = n }) |> dispatch)
                    Bulma.FC.IntInput
                        {
                            Label = Some "Standard Cargo Hold"
                            Value = comp.Standard
                            Min = Some 0
                            Max = None
                            Disabled = false
                        }
                        (fun n -> Msg.ReplaceShipComponent (ship, CargoHold { comp with Standard = n }) |> dispatch)
                ]
            Bulma.FC.HorizontalGroup
                None
                [
                    Bulma.FC.IntInput
                        {
                            Label = Some "Cargo Handling System"
                            Value = comp.CargoHandlingSystem
                            Min = Some 0
                            Max = None
                            Disabled = false
                        }
                        (fun n -> Msg.ReplaceShipComponent (ship, CargoHold { comp with CargoHandlingSystem = n }) |> dispatch)
                    Bulma.FC.IntInput
                        {
                            Label = Some "Improved Cargo Handling System"
                            Value = comp.ImprovedCargoHandlingSystem
                            Min = Some 0
                            Max = None
                            Disabled = not <| List.contains allTechs.ImprovedCargoHandlingSystem tech
                        }
                        (fun n -> Msg.ReplaceShipComponent (ship, CargoHold { comp with ImprovedCargoHandlingSystem = n }) |> dispatch)
                    Bulma.FC.IntInput
                        {
                            Label = Some "Advanced Cargo Handling System"
                            Value = comp.AdvancedCargoHandlingSystem
                            Min = Some 0
                            Max = None
                            Disabled = not <| List.contains allTechs.AdvancedCargoHandlingSystem tech
                        }
                        (fun n -> Msg.ReplaceShipComponent (ship, CargoHold { comp with AdvancedCargoHandlingSystem = n }) |> dispatch)
                    Bulma.FC.IntInput
                        {
                            Label = Some "Grav-Assisted Cargo Handling System"
                            Value = comp.GravAssistedCargoHandlingSystem
                            Min = Some 0
                            Max = None
                            Disabled = not <| List.contains allTechs.GravAssistedCargoHandlingSystem tech
                        }
                        (fun n -> Msg.ReplaceShipComponent (ship, CargoHold { comp with GravAssistedCargoHandlingSystem = n }) |> dispatch)
                ]
        ]
    let actions =
        [
            "Remove", DangerColor, (fun _ -> Msg.RemoveComponentFromShip (ship, CargoHold comp) |> dispatch)
        ]
    shipComponentCard (comp.Guid.ToString ()) header form actions
