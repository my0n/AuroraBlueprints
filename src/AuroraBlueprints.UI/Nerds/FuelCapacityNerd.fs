module Nerds.FuelCapacityNerd

open Nerds.Icon
open Nerds.Common
open Model.Measures

type FuelCapacityNerd =
    {
        FuelCapacity: float<l>
    }
    interface INerd with
        member this.Text
            with get() =
                sprintf "%.0f" this.FuelCapacity
        member this.Tooltip
            with get() =
                sprintf "%.0f L" this.FuelCapacity
        member this.Icon
            with get() =
                GasPump
        member this.Render
            with get() = true
        member this.Description
            with get() =
                sprintf "Fuel Capacity %.0f Litres" this.FuelCapacity
                |> Some
