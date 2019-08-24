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
            Price (1<comp>, comp.BuildCost)
            SizeFloat (1<comp>, comp.TotalSize*1.0</comp>)
            FuelCapacity comp.FuelCapacity
            RemoveButton
        ] |> Some
    let form =
        [ HorGrp (None,
                  [ IntInp ({ Label = Some "Tiny"; Value = comp.Tiny; Max = None },
                            (fun n -> Msg.ReplaceShipComponent (FuelStorage { comp with Tiny = n }) |> dispatch)
                           )
                    IntInp ({ Label = Some "Small"; Value = comp.Small; Max = None },
                            (fun n -> Msg.ReplaceShipComponent (FuelStorage { comp with Small = n }) |> dispatch)
                           )
                    IntInp ({ Label = Some "Standard"; Value = comp.Standard; Max = None },
                            (fun n -> Msg.ReplaceShipComponent (FuelStorage { comp with Standard = n }) |> dispatch)
                           )
                    IntInp ({ Label = Some "Large"; Value = comp.Large; Max = None },
                            (fun n -> Msg.ReplaceShipComponent (FuelStorage { comp with Large = n }) |> dispatch)
                           )
                    IntInp ({ Label = Some "Very Large"; Value = comp.VeryLarge; Max = None },
                            (fun n -> Msg.ReplaceShipComponent (FuelStorage { comp with VeryLarge = n }) |> dispatch)
                           )
                    IntInp ({ Label = Some "Ultra Large"; Value = comp.UltraLarge; Max = None },
                            (fun n -> Msg.ReplaceShipComponent (FuelStorage { comp with UltraLarge = n }) |> dispatch)
                           )
                  ]
                 )
        ]
        |> Bulma.Form.render
    shipComponentCard header
                      form
                      (FuelStorage comp)
                      dispatch
