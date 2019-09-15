module Cards.Bridge

open App.Msg
open Bulma.Card
open Bulma.Form
open Cards.Common
open Model.Measures
open Comp.Bridge
open Comp.ShipComponent

let render (comp: Bridge) dispatch =
    let header =
        [
            Name "Bridge"
            Price (comp.Count, comp.BuildCost)
            SizeInt (comp.Count, comp.Size)
        ]
    let form =
        [ HorGrp (None,
                  [ IntInp ({ Label = Some "Count"; Value = comp.Count*1</comp>; Max = None },
                            (fun n -> Msg.ReplaceShipComponent (Bridge { comp with Count = n*1<comp> }) |> dispatch)
                           )
                  ]
                 )
        ]
        |> Bulma.Form.render
    let actions =
        [
            "Remove", DangerColor, (fun _ -> Msg.RemoveComponentFromShip <| Bridge comp |> dispatch)
        ]
    shipComponentCard header form actions
