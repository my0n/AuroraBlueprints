module Cards.FuelStorage

open App.Msg
open Bulma.Card
open Cards.Common
open Model.Measures
open Comp.FuelStorage
open Comp.ShipComponent
open Comp.Ship

open Nerds.PriceTotalNerd
open Nerds.SizeNerd
open Nerds.FuelCapacityNerd

let render (ship: Ship) (comp: FuelStorage) dispatch =
    let header =
        [
            Name "Fuel Storage"
            Nerd { TotalBuildCost = comp.BuildCost }
            Nerd { RenderMode = HS; Count = 1<comp>; Size = comp.TotalSize*1</comp> }
            Nerd { FuelCapacity = comp.FuelCapacity }
        ]
    let form =
        [
            Bulma.FC.HorizontalGroup
                None
                [
                    Bulma.FC.IntInput
                        {
                            Label = Some "Tiny"
                            Value = comp.Tiny
                            Min = Some 0
                            Max = None
                        }
                        (fun n -> Msg.ReplaceShipComponent (ship, FuelStorage { comp with Tiny = n }) |> dispatch)
                    Bulma.FC.IntInput
                        {
                            Label = Some "Small"
                            Value = comp.Small
                            Min = Some 0
                            Max = None
                        }
                        (fun n -> Msg.ReplaceShipComponent (ship, FuelStorage { comp with Small = n }) |> dispatch)
                    Bulma.FC.IntInput
                        {
                            Label = Some "Standard"
                            Value = comp.Standard
                            Min = Some 0
                            Max = None
                        }
                        (fun n -> Msg.ReplaceShipComponent (ship, FuelStorage { comp with Standard = n }) |> dispatch)
                    Bulma.FC.IntInput
                        {
                            Label = Some "Large"
                            Value = comp.Large
                            Min = Some 0
                            Max = None
                        }
                        (fun n -> Msg.ReplaceShipComponent (ship, FuelStorage { comp with Large = n }) |> dispatch)
                    Bulma.FC.IntInput
                        {
                            Label = Some "Very Large"
                            Value = comp.VeryLarge
                            Min = Some 0
                            Max = None
                        }
                        (fun n -> Msg.ReplaceShipComponent (ship, FuelStorage { comp with VeryLarge = n }) |> dispatch)
                    Bulma.FC.IntInput
                        {
                            Label = Some "Ultra Large"
                            Value = comp.UltraLarge
                            Min = Some 0
                            Max = None
                        }
                        (fun n -> Msg.ReplaceShipComponent (ship, FuelStorage { comp with UltraLarge = n }) |> dispatch)
                ]
        ]
    let actions =
        [
            "Remove", DangerColor, (fun _ -> Msg.RemoveComponentFromShip (ship, FuelStorage comp) |> dispatch)
        ]
    shipComponentCard (comp.Guid.ToString ()) header form actions
