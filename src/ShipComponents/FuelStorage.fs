module ShipComponents.FuelStorage

open Types
open ShipComponents.Common
open Bulma.Form
open ShipComponent

let render (comp: FuelStorage) dispatch =
    let header =
        [
            Name "Fuel Storage"
            Price comp.BuildCost
            Size comp.TotalSize
            FuelCapacity comp.FuelCapacity
            RemoveButton
        ] |> Some
    let form =
        [ HorGrp (None,
                  [ IntInp ({ Label = Some "Tiny"; Value = comp.Tiny },
                            (fun n -> Msg.ReplaceShipComponent (FuelStorage { comp with Tiny = n }) |> dispatch)
                           )
                    IntInp ({ Label = Some "Small"; Value = comp.Small },
                            (fun n -> Msg.ReplaceShipComponent (FuelStorage { comp with Small = n }) |> dispatch)
                           )
                    IntInp ({ Label = Some "Standard"; Value = comp.Standard },
                            (fun n -> Msg.ReplaceShipComponent (FuelStorage { comp with Standard = n }) |> dispatch)
                           )
                    IntInp ({ Label = Some "Large"; Value = comp.Large },
                            (fun n -> Msg.ReplaceShipComponent (FuelStorage { comp with Large = n }) |> dispatch)
                           )
                    IntInp ({ Label = Some "Very Large"; Value = comp.VeryLarge },
                            (fun n -> Msg.ReplaceShipComponent (FuelStorage { comp with VeryLarge = n }) |> dispatch)
                           )
                    IntInp ({ Label = Some "Ultra Large"; Value = comp.UltraLarge },
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
