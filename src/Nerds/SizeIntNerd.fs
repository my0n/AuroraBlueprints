module Nerds.SizeIntNerd

open Nerds.Icon
open Nerds.Common
open Model.Measures

type SizeIntNerd =
    {
        Size: int<hs/comp>
        Count: int<comp>
    }
    interface INerd with
        member this.Text
            with get() =
                sprintf "%d" (this.Size * this.Count)
        member this.Tooltip
            with get() =
                match this.Count with
                | 1<comp> | 0<comp> -> sprintf "%d HS" (this.Size * this.Count)
                | _ -> sprintf "%d (%d) HS" (this.Size * this.Count) this.Size
        member this.Icon
            with get() =
                Weight
        member this.Render
            with get() = true
