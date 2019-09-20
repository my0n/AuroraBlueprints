module Cards.Bridge

open App.Msg
open Bulma.Card
open Bulma.Form
open Cards.Common
open Model.Measures
open Comp.Bridge
open Comp.ShipComponent
open Ship

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
        [ HorGrp (None,
                  [ IntInp ({ Label = Some "Count"; Value = comp.Count*1</comp>; Min = Some 0; Max = None },
                            (fun n -> Msg.ReplaceShipComponent (ship, Bridge { comp with Count = n*1<comp> }) |> dispatch)
                           )
                  ]
                 )
        ]
        |> Bulma.Form.render
    let actions =
        [
            "Remove", DangerColor, (fun _ -> Msg.RemoveComponentFromShip (ship, Bridge comp) |> dispatch)
        ]
    shipComponentCard header form actions
