module ShipComponents.Bridge

open ShipComponents.Common

open AppModel.Msg
open Bulma.Form
open Model.Measures
open Model.ShipComponent

let render (comp: Bridge) dispatch =
    let header =
        [
            Name "Bridge"
            Price (comp.Count, comp.BuildCost)
            SizeInt (comp.Count, comp.Size)
            RemoveButton
        ] |> Some
    let form =
        [ HorGrp (None,
                  [ IntInp ({ Label = Some "Count"; Value = comp.Count*1</comp>; Max = None },
                            (fun n -> Msg.ReplaceShipComponent (Bridge { comp with Count = n*1<comp> }) |> dispatch)
                           )
                  ]
                 )
        ]
        |> Bulma.Form.render
    shipComponentCard header
                      form
                      (Some <| Bridge comp)
                      dispatch
