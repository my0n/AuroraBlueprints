module Cards.Bridge

open Model.Measures

open Global
open App.Msg
open Bulma.Card
open Bulma.FC
open Cards.Common
open Comp.Bridge
open Comp.ShipComponent
open Comp.Ship

open Nerds.PriceNerd
open Nerds.SizeNerd

let render (ship: Ship) (comp: Bridge) dispatch =
    let header =
        [
            Name "Bridge"
            Nerd { Count = comp.Count; BuildCost = comp.BuildCost }
            Nerd { RenderMode = HS; Count = comp.Count; Size = comp.Size * 50<ton/hs> }
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
    shipComponentCard (comp.Guid.ToString ()) header [ form ] actions
