module Nerds.MagazineCapacityNerd

open Model.Measures
open Nerds.Icon
open Nerds.Common

type MagazineCapacityNerd =
    {
        Count: int<comp>
        PowerOutput: float<power/comp>
    }
    interface INerd with
        member this.Text
            with get() =
                sprintf "%.1f" (this.PowerOutput * int2float this.Count)
        member this.Tooltip
            with get() =
                match this.Count with
                | 1<comp> | 0<comp> -> sprintf "%.1f power generated" (this.PowerOutput * int2float this.Count)
                | _ -> sprintf "%.1f (%.1f) power generated" (this.PowerOutput * int2float this.Count) this.PowerOutput
        member this.Icon
            with get() =
                Bolt
        member this.Render
            with get() = true
        member this.Description
            with get() =
                sprintf "Total Power Output %.1f" (this.PowerOutput * int2float this.Count)
                |> Some
