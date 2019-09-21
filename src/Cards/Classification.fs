module Cards.Classification

open App.Msg
open Bulma.Card
open Bulma.Form
open Comp.Ship

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

    Bulma.Card.render {
        HeaderItems = []
        Contents = form
        Actions = []
        HasExpanderToggle = false
    }
