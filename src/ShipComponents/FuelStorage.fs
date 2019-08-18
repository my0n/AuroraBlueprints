module ShipComponents.FuelStorage

open Fable.Helpers.React
open Fable.Helpers.React.Props

open Types
open ShipComponents.Common
open InputComponents

let fuelStorage dispatch (ship: Ship) (comp: FuelStorage) =
    shipComponentCard [
                        Name "Fuel Storage"
                        Size comp.Size
                        FuelCapacity comp.FuelCapacity
                      ]
                      [ horizontalGroup None
                                        [ integerInput {
                                                         Label = "Tiny"
                                                         OnChange = fun n -> Msg.ReplaceShipComponent (ship, FuelStorage { comp with Tiny = n })
                                                       }
                                                       dispatch
                                                       comp.Tiny
                                          integerInput {
                                                         Label = "Small"
                                                         OnChange = fun n -> Msg.ReplaceShipComponent (ship, FuelStorage { comp with Small = n })
                                                       }
                                                       dispatch
                                                       comp.Small
                                          integerInput {
                                                         Label = "Standard"
                                                         OnChange = fun n -> Msg.ReplaceShipComponent (ship, FuelStorage { comp with Standard = n })
                                                       }
                                                       dispatch
                                                       comp.Standard
                                          integerInput {
                                                         Label = "Large"
                                                         OnChange = fun n -> Msg.ReplaceShipComponent (ship, FuelStorage { comp with Large = n })
                                                       }
                                                       dispatch
                                                       comp.Large
                                          integerInput {
                                                         Label = "Very Large"
                                                         OnChange = fun n -> Msg.ReplaceShipComponent (ship, FuelStorage { comp with VeryLarge = n })
                                                       }
                                                       dispatch
                                                       comp.VeryLarge
                                          integerInput {
                                                         Label = "Ultra Large"
                                                         OnChange = fun n -> Msg.ReplaceShipComponent (ship, FuelStorage { comp with UltraLarge = n })
                                                       }
                                                       dispatch
                                                       comp.UltraLarge
                                        ]
                      ]
