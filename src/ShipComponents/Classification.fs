module ShipComponents.Classification

open AppModel.Msg
open Bulma.Form
open Model.Ship

let render (ship: Ship) dispatch =
    let form =
        [ HorGrp (None,
                  [ TxtInp ({ Label = None; Value = ship.Name },
                            (fun n -> Msg.ShipUpdateName (ship, n) |> dispatch)
                           )
                    TxtInp ({ Label = None; Value = ship.ShipClass },
                            (fun n -> Msg.ShipUpdateClass (ship, n) |> dispatch)
                           )
                  ]
                 )
        ]
        |> Bulma.Form.render

    Bulma.Card.render None form