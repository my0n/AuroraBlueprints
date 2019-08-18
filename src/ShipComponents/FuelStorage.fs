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
                      [
                        integerInput {
                                      Label = "Tiny"
                                      OnChange = fun n -> Msg.ReplaceShipComponent (ship, FuelStorage { comp with Tiny = n })
                                    }
                                    dispatch
                                    comp.Tiny
                      ]
