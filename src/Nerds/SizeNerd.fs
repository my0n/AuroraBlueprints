module Nerds.SizeNerd

open Nerds.Icon
open Nerds.Common
open Model.Measures

type SizeNerdRenderMode =
    | Tons
    | HS

type SizeNerd =
    {
        RenderMode: SizeNerdRenderMode
        Size: int<ton/comp>
        Count: int<comp>
    }
    interface INerd with
        member this.Text
            with get() =
                sprintf "%.1f" << ton2hs <| int2float (this.Size * this.Count)
        member this.Tooltip
            with get() =
                match this.Count with
                | 1<comp> | 0<comp> -> sprintf "%.1f HS" << ton2hs <| int2float (this.Size * this.Count)
                | _ -> sprintf "%.1f (%.1f) HS" (ton2hs <| int2float (this.Size * this.Count)) (ton2hs <| (int2float this.Size * 1.0<comp>))
        member this.Icon
            with get() =
                Weight
        member this.Render
            with get() = true
        member this.Description
            with get() =
                match this.RenderMode with
                | Tons -> sprintf "%d tons" (this.Size * this.Count)
                | HS   -> sprintf "%.1f HS" << ton2hs <| int2float (this.Size * this.Count)
                |> Some
