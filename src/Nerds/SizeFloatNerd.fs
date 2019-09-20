module Nerds.SizeFloatNerd

open Nerds.Icon
open Nerds.Common
open Model.Measures

type SizeFloatNerd =
    {
        Size: float<hs/comp>
        Count: int<comp>
    }
    interface INerd with
        member this.Text
            with get() =
                sprintf "%.1f" (this.Size * int2float this.Count)
        member this.Tooltip
            with get() =
                match this.Count with
                | 1<comp> | 0<comp> -> sprintf "%.1f HS" (this.Size * int2float this.Count)
                | _ -> sprintf "%.1f (%.1f) HS" (this.Size * int2float this.Count) this.Size
        member this.Icon
            with get() =
                Weight
        member this.Render
            with get() = true
