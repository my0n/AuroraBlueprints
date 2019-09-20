module Nerds.FuelCapacityNerd

open Nerds.Icon
open Nerds.Common
open Model.Measures

type FuelCapacityNerd =
    {
        FuelCapacity: float<kl>
    }
    interface INerd with
        member this.Text
            with get() =
                sprintf "%.0f" this.FuelCapacity
        member this.Tooltip
            with get() =
                sprintf "%.0f kL" this.FuelCapacity
        member this.Icon
            with get() =
                GasPump
        member this.Render
            with get() = true
