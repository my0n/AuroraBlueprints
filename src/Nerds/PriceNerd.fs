module Nerds.PriceNerd

open Nerds.Icon
open Nerds.Common
open Model.Measures
open Model.BuildCost

type PriceNerd =
    {
        Count: int<comp>
        BuildCost: BuildCost
    }
    interface INerd with
        member this.Text
            with get() =
                sprintf "%.0f" (this.BuildCost.BuildPoints * int2float this.Count)
        member this.Tooltip
            with get() =
                let bpr a lbl =
                    match a with
                    | 0.0</comp> -> None
                    | a ->
                        (match this.Count with
                            | 1<comp> -> sprintf "%.1f %s" a lbl
                            | _ -> sprintf "%.1f (%.1f) %s" (a * int2float this.Count) a lbl
                        )
                        |> Some
                let hoverText =
                    [
                        bpr this.BuildCost.BuildPoints "build points"
                        bpr this.BuildCost.Boronide "boronide"
                        bpr this.BuildCost.Corbomite "corbomite"
                        bpr this.BuildCost.Duranium "duranium"
                        bpr this.BuildCost.Gallicite "gallicite"
                        bpr this.BuildCost.Mercassium "mercassium"
                        bpr this.BuildCost.Neutronium "neutronium"
                        bpr this.BuildCost.Uridium "uridium"
                    ]
                    |> List.choose id
                    |> String.concat "\r\n"

                match hoverText with
                | "" -> "free"
                | _ -> hoverText
        member this.Icon
            with get() =
                Dollar
        member this.Render
            with get() = true
