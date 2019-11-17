module Nerds.ComponentArmorNerd

open Nerds.Icon
open Nerds.Common

type ComponentArmorNerd =
    {
        Depth: int
    }
    interface INerd with
        member this.Text
            with get() =
                sprintf "%d" this.Depth
        member this.Tooltip
            with get() =
                sprintf "%d armor" this.Depth
        member this.Icon
            with get() =
                Shield
        member this.Render
            with get() = true
        member this.Description
            with get() =
                sprintf "Armour %d" this.Depth
                |> Some
