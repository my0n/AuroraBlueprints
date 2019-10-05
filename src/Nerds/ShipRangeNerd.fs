module Nerds.ShipRangeNerd

open Nerds.Icon
open Nerds.Common
open Model.Measures

type ShipRangeNerd =
    {
        Range: float<km>
    }
    interface INerd with
        member this.Text
            with get() =
                sprintf "%.1f" (this.Range / 1000000000.0)
        member this.Tooltip
            with get() =
                sprintf "%.1f billion km" (this.Range / 1000000000.0)
        member this.Icon
            with get() =
                NoIcon
        member this.Render
            with get() = true
        member this.Description
            with get() =
                sprintf "%.1f billion km" (this.Range / 1000000000.0)
                |> Some
