module Nerds.EnginePowerNerd

open Nerds.Icon
open Nerds.Common
open Model.Measures

type EnginePowerNerd =
    {
        Count: int<comp>
        EnginePower: float<ep/comp>
        Size: int<hs/comp>
        Speed: float<km/s>
    }
    interface INerd with
        member this.Text
            with get() =
                sprintf "%.0f" this.Speed
        member this.Tooltip
            with get() =
                let spdstr = sprintf "%.0f km/s" this.Speed
                let epstr =
                    match this.Count with
                    | 1<comp> | 0<comp> -> sprintf "%.1f EP" (this.EnginePower * int2float this.Count)
                    | _ -> sprintf "%.1f (%.1f) EP" (this.EnginePower * int2float this.Count) this.EnginePower
                let ephrstr = sprintf "%.1f EP/HS" (this.EnginePower / int2float this.Size)
                [ spdstr; epstr; ephrstr ] |> String.concat "\r\n"
        member this.Icon
            with get() =
                AngleDoubleRight
        member this.Render
            with get() = true
