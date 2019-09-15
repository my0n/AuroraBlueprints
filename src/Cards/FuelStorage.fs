module Cards.FuelStorage

open App.Msg
open Bulma.Card
open Bulma.Form
open Cards.Common
open Model.Measures
open Comp.FuelStorage
open Comp.ShipComponent
open Ship

let render (ship: Ship) (comp: FuelStorage) dispatch =
    let header =
        [
            Name "Fuel Storage"
            TotalPrice comp.BuildCost
            SizeFloat (1<comp>, comp.TotalSize*1.0</comp>)
            FuelCapacity comp.FuelCapacity
        ]
    let form =
        [ HorGrp (None,
                  [ IntInp ({ Label = Some "Tiny"; Value = comp.Tiny*1</comp>; Min = Some 0; Max = None },
                            (fun n -> Msg.ReplaceShipComponent (ship, FuelStorage { comp with Tiny = n*1<comp> }) |> dispatch)
                           )
                    IntInp ({ Label = Some "Small"; Value = comp.Small*1</comp>; Min = Some 0; Max = None },
                            (fun n -> Msg.ReplaceShipComponent (ship, FuelStorage { comp with Small = n*1<comp> }) |> dispatch)
                           )
                    IntInp ({ Label = Some "Standard"; Value = comp.Standard*1</comp>; Min = Some 0; Max = None },
                            (fun n -> Msg.ReplaceShipComponent (ship, FuelStorage { comp with Standard = n*1<comp> }) |> dispatch)
                           )
                    IntInp ({ Label = Some "Large"; Value = comp.Large*1</comp>; Min = Some 0; Max = None },
                            (fun n -> Msg.ReplaceShipComponent (ship, FuelStorage { comp with Large = n*1<comp> }) |> dispatch)
                           )
                    IntInp ({ Label = Some "Very Large"; Value = comp.VeryLarge*1</comp>; Min = Some 0; Max = None },
                            (fun n -> Msg.ReplaceShipComponent (ship, FuelStorage { comp with VeryLarge = n*1<comp> }) |> dispatch)
                           )
                    IntInp ({ Label = Some "Ultra Large"; Value = comp.UltraLarge*1</comp>; Min = Some 0; Max = None },
                            (fun n -> Msg.ReplaceShipComponent (ship, FuelStorage { comp with UltraLarge = n*1<comp> }) |> dispatch)
                           )
                  ]
                 )
        ]
        |> Bulma.Form.render
    let actions =
        [
            "Remove", DangerColor, (fun _ -> Msg.RemoveComponentFromShip (ship, FuelStorage comp) |> dispatch)
        ]
    shipComponentCard header form actions
