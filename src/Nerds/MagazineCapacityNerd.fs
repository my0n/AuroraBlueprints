module Nerds.MagazineCapacityNerd

open Model.Measures
open Nerds.Icon
open Nerds.Common

type MagazineCapacityNerd =
    {
        Count: int<comp>
        Ammo: int<ammo/comp>
    }
    interface INerd with
        member this.Text
            with get() =
                sprintf "%d" (this.Ammo * this.Count)
        member this.Tooltip
            with get() =
                match this.Count with
                | 1<comp> | 0<comp> -> sprintf "%d magazine capacity" (this.Ammo * this.Count)
                | _ -> sprintf "%d (%d) magazine capacity" (this.Ammo * this.Count) this.Ammo
        member this.Icon
            with get() =
                Boxes
        member this.Render
            with get() = true
        member this.Description
            with get() =
                match this.Ammo with
                | 0<ammo/comp> -> None
                | _ ->
                    sprintf "Magazine %d" (this.Ammo * this.Count)
                    |> Some
