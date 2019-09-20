module Nerds.ArmorStrengthNerd

open Nerds.Icon
open Nerds.Common

type ArmorStrengthNerd =
    {
        Strength: float
    }
    interface INerd with
        member this.Text
            with get() =
                sprintf "%.1f" this.Strength
        member this.Tooltip
            with get() =
                sprintf "%.1f armor strength" this.Strength
        member this.Icon
            with get() =
                Shield
        member this.Render
            with get() = true
