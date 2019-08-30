module ShipComponents.FuelStorage

open AppModel.Msg
open Bulma.Card
open Bulma.Form
open Card.Common
open Model.Measures
open Model.ShipComponent

let render (comp: FuelStorage) dispatch =
    let header =
        [
            Name "Fuel Storage"
            TotalPrice comp.BuildCost
            SizeFloat (1<comp>, comp.TotalSize*1.0</comp>)
            FuelCapacity comp.FuelCapacity
        ]
    let form =
        [ HorGrp (None,
                  [ IntInp ({ Label = Some "Tiny"; Value = comp.Tiny*1</comp>; Max = None },
                            (fun n -> Msg.ReplaceShipComponent (FuelStorage { comp with Tiny = n*1<comp> }) |> dispatch)
                           )
                    IntInp ({ Label = Some "Small"; Value = comp.Small*1</comp>; Max = None },
                            (fun n -> Msg.ReplaceShipComponent (FuelStorage { comp with Small = n*1<comp> }) |> dispatch)
                           )
                    IntInp ({ Label = Some "Standard"; Value = comp.Standard*1</comp>; Max = None },
                            (fun n -> Msg.ReplaceShipComponent (FuelStorage { comp with Standard = n*1<comp> }) |> dispatch)
                           )
                    IntInp ({ Label = Some "Large"; Value = comp.Large*1</comp>; Max = None },
                            (fun n -> Msg.ReplaceShipComponent (FuelStorage { comp with Large = n*1<comp> }) |> dispatch)
                           )
                    IntInp ({ Label = Some "Very Large"; Value = comp.VeryLarge*1</comp>; Max = None },
                            (fun n -> Msg.ReplaceShipComponent (FuelStorage { comp with VeryLarge = n*1<comp> }) |> dispatch)
                           )
                    IntInp ({ Label = Some "Ultra Large"; Value = comp.UltraLarge*1</comp>; Max = None },
                            (fun n -> Msg.ReplaceShipComponent (FuelStorage { comp with UltraLarge = n*1<comp> }) |> dispatch)
                           )
                  ]
                 )
        ]
        |> Bulma.Form.render
    let actions =
        [
            "Remove", DangerColor, (fun _ -> Msg.RemoveComponentFromShip <| FuelStorage comp |> dispatch)
        ]
    shipComponentCard header form actions
