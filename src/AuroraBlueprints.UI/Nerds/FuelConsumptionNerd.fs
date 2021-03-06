module Nerds.FuelConsumptionNerd

open Nerds.Icon
open Nerds.Common
open Model.Measures

type FuelConsumptionNerd =
    {
        Count: int<comp>
        Consumption: float<l/hr/comp>
        Efficiency: float<l/hr/ep>
    }
    interface INerd with
        member this.Text
            with get() =
                sprintf "%.2f" (this.Consumption * int2float this.Count)
        member this.Tooltip
            with get() =
                let klhr =
                    match this.Count with
                    | 1<comp> | 0<comp> -> sprintf "%0.2f l/hr" (this.Consumption * int2float this.Count)
                    | _ -> sprintf "%0.2f (%0.2f) l/hr" (this.Consumption * int2float this.Count) this.Consumption
                let klhrep = sprintf "%0.4f l/hr/EP" this.Efficiency
                [ klhr; klhrep ] |> String.concat "\r\n"
        member this.Icon
            with get() =
                Tachometer
        member this.Render
            with get() = true
        member this.Description
            with get() = None
