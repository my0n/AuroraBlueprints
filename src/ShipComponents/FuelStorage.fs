module ShipComponents.FuelStorage

open Types
open ShipComponents.Common
open InputComponents
open ShipComponent

let fuelStorage (comp: FuelStorage) dispatch =
    shipComponentCard [
                        Name "Fuel Storage"
                        Size comp.Size
                        FuelCapacity comp.FuelCapacity
                        RemoveButton
                      ]
                      [ horizontalGroup None
                                        [
                                          integerInput {
                                                         Label = "Tiny"
                                                         OnChange = fun n -> Msg.ReplaceShipComponent (FuelStorage { comp with Tiny = n })
                                                       }
                                                       dispatch
                                                       comp.Tiny
                                          integerInput {
                                                         Label = "Small"
                                                         OnChange = fun n -> Msg.ReplaceShipComponent (FuelStorage { comp with Small = n })
                                                       }
                                                       dispatch
                                                       comp.Small
                                          integerInput {
                                                         Label = "Standard"
                                                         OnChange = fun n -> Msg.ReplaceShipComponent (FuelStorage { comp with Standard = n })
                                                       }
                                                       dispatch
                                                       comp.Standard
                                          integerInput {
                                                         Label = "Large"
                                                         OnChange = fun n -> Msg.ReplaceShipComponent (FuelStorage { comp with Large = n })
                                                       }
                                                       dispatch
                                                       comp.Large
                                          integerInput {
                                                         Label = "Very Large"
                                                         OnChange = fun n -> Msg.ReplaceShipComponent (FuelStorage { comp with VeryLarge = n })
                                                       }
                                                       dispatch
                                                       comp.VeryLarge
                                          integerInput {
                                                         Label = "Ultra Large"
                                                         OnChange = fun n -> Msg.ReplaceShipComponent (FuelStorage { comp with UltraLarge = n })
                                                       }
                                                       dispatch
                                                       comp.UltraLarge
                                        ]
                      ]
                      (FuelStorage comp)
                      dispatch
