module Nerds.ThermalSignatureNerd

open Nerds.Icon
open Nerds.Common
open Model.Measures

type ThermalSignatureNerd =
    {
        ThermalSignature: float<therm>
        EngineCount: int<comp>
        EngineContribution: float<therm/comp>
    }
    interface INerd with
        member this.Text
            with get() =
                sprintf "%.0f" this.ThermalSignature
        member this.Tooltip
            with get() =
                let total = sprintf "%.0f thermal signature" this.ThermalSignature
                let fromEngine =
                    match this.EngineCount with
                    | 1<comp> -> sprintf "%.1f from engine" this.EngineContribution
                    | _ -> sprintf "%.1f (%.1f) from engines" (this.EngineContribution * int2float this.EngineCount) this.EngineContribution 
                [ total; fromEngine ]
                |> String.concat "\r\n"
        member this.Icon
            with get() =
                FireAlt
        member this.Render
            with get() = true
        member this.Description
            with get() = Some <| sprintf "Signature %.0f" this.ThermalSignature
