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
                if this.Range > 14959787.1<km> then // 0.1 AU
                    sprintf "%.1f billion km (%.1f AU)" (this.Range / 1000000000.0) (this.Range / 149597870.7)
                else
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
