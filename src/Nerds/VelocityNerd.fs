module Nerds.VelocityNerd

open Nerds.Icon
open Nerds.Common
open Model.Measures

type VelocityNerd =
    {
        Speed: float<km/s>
    }
    interface INerd with
        member this.Text
            with get() =
                sprintf "%.0f" this.Speed
        member this.Tooltip
            with get() =
                sprintf "%.0f km/s" this.Speed
        member this.Icon
            with get() =
                AngleDoubleRight
        member this.Render
            with get() = true
