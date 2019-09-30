module Cards.TroopTransport

open Global
open System

open App.Msg
open Bulma.Card
open Cards.Common
open Model.Measures
open Comp.TroopTransport
open Comp.ShipComponent
open Comp.Ship

open Nerds.MaintenanceClassNerd
open Nerds.PriceTotalNerd
open Nerds.SizeNerd
open Nerds.TroopTransportNerd

let render (ship: Ship) (comp: TroopTransport) dispatch =
    let header =
        [
            Name "Troop Transport"
            Nerd { MaintenanceClass = comp.MaintenanceClass }
            Nerd { TotalBuildCost = comp.BuildCost }
            Nerd { RenderMode = HS; Count = 1<comp>; Size = comp.TotalSize*1</comp> }
            Nerd { CryoDrop = comp.CryoDropCapability; CombatDrop = comp.CombatDropCapability; TroopTransport = comp.TroopTransportCapability  }
        ]
    let form =
        [
            Bulma.FC.HorizontalGroup
                None
                [
                    Bulma.FC.IntInput
                        {
                            Label = Some "Company Troop Transport"
                            Value = comp.CompanyTransport
                            Min = Some 0
                            Max = None
                        }
                        (fun n -> Msg.ReplaceShipComponent (ship, TroopTransport { comp with CompanyTransport = n }) |> dispatch)
                    Bulma.FC.IntInput
                        {
                            Label = Some "Battalion Troop Transport"
                            Value = comp.BattalionTransport
                            Min = Some 0
                            Max = None
                        }
                        (fun n -> Msg.ReplaceShipComponent (ship, TroopTransport { comp with BattalionTransport = n }) |> dispatch)
                    Bulma.FC.IntInput
                        {
                            Label = Some "Company Combat Drop"
                            Value = comp.CompanyDropModule
                            Min = Some 0
                            Max = None
                        }
                        (fun n -> Msg.ReplaceShipComponent (ship, TroopTransport { comp with CompanyDropModule = n }) |> dispatch)
                    Bulma.FC.IntInput
                        {
                            Label = Some "Battalion Combat Drop"
                            Value = comp.BattalionDropModule
                            Min = Some 0
                            Max = None
                        }
                        (fun n -> Msg.ReplaceShipComponent (ship, TroopTransport { comp with BattalionDropModule = n }) |> dispatch)
                    Bulma.FC.IntInput
                        {
                            Label = Some "Company Cryo Drop"
                            Value = comp.CompanyCryoDropModule
                            Min = Some 0
                            Max = None
                        }
                        (fun n -> Msg.ReplaceShipComponent (ship, TroopTransport { comp with CompanyCryoDropModule = n }) |> dispatch)
                    Bulma.FC.IntInput
                        {
                            Label = Some "Battalion Cryo Drop"
                            Value = comp.BattalionCryoDropModule
                            Min = Some 0
                            Max = None
                        }
                        (fun n -> Msg.ReplaceShipComponent (ship, TroopTransport { comp with BattalionCryoDropModule = n }) |> dispatch)
                ]
        ]
    let actions =
        [
            "Remove", DangerColor, (fun _ -> Msg.RemoveComponentFromShip (ship, TroopTransport comp) |> dispatch)
        ]
    shipComponentCard header form actions
