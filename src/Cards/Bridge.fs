module Cards.Bridge

open Global
open App.Msg
open Bulma.Card
open Bulma.FC
open Cards.Common
open Comp.Bridge
open Comp.ShipComponent
open Comp.Ship

open Nerds.PriceNerd
open Nerds.SizeIntNerd

let render (ship: Ship) (comp: Bridge) dispatch =
    let header =
        [
            Name "Bridge"
            Nerd { Count = comp.Count; BuildCost = comp.BuildCost }
            Nerd { Count = comp.Count; Size = comp.Size }
        ]
    let form =
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
                    (fun n -> Msg.ReplaceShipComponent (ship, Bridge { comp with Count = n }) |> dispatch)
            ]
    let actions =
        [
            "Remove", DangerColor, (fun _ -> Msg.RemoveComponentFromShip (ship, Bridge comp) |> dispatch)
        ]
    shipComponentCard header (List.wrap form) actions
