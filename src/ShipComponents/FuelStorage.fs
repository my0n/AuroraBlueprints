module ShipComponents.FuelStorage

open Types
open ShipComponents.Common
open Bulma.Form
open ShipComponent
open Measures

let render (comp: FuelStorage) dispatch =
    let header =
        [
            Name "Fuel Storage"
            TotalPrice comp.BuildCost
            SizeFloat (1<comp>, comp.TotalSize*1.0</comp>)
            FuelCapacity comp.FuelCapacity
            RemoveButton
        ] |> Some
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
    shipComponentCard header
                      form
                      (Some <| FuelStorage comp)
                      dispatch
