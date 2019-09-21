module Nerds.ArmorSizeNerd

open Nerds.Icon
open Nerds.Common

type ArmorSizeNerd =
    {
        Width: int
        Depth: int
    }
    interface INerd with
        member this.Text
            with get() =
                sprintf "%d×%d" this.Depth this.Width
        member this.Tooltip
            with get() =
                sprintf "%d rows deep\n%d columns wide" this.Depth this.Width
        member this.Icon
            with get() =
                ThLarge
        member this.Render
            with get() = true
        member this.Description
            with get() = Some <| sprintf "Armour %d-%d" this.Depth this.Width
