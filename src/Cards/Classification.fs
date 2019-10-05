module Cards.Classification

open Global
open App.Msg
open Comp.Ship

let render (ship: Ship) dispatch =
    let form =
        Bulma.FC.HorizontalGroup
            None
            [
                Bulma.FC.WithLabel
                    "Name"
                    [
                        Bulma.FC.TextInput
                            ship.Name
                            (fun n -> Msg.ShipUpdateName (ship, n) |> dispatch)
                    ]
                Bulma.FC.WithLabel
                    "Class"
                    [
                        Bulma.FC.TextInput
                            ship.ShipClass
                            (fun n -> Msg.ShipUpdateClass (ship, n) |> dispatch)
                    ]                    
            ]

    Bulma.Card.render {
        key = "classification"
        HeaderItems = []
        Contents = [ form ]
        Actions = []
        HasExpanderToggle = false
    }
