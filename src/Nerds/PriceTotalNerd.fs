module Nerds.PriceTotalNerd

open Nerds.Icon
open Nerds.Common
open Model.BuildCost

type PriceTotalNerd =
    {
        TotalBuildCost: TotalBuildCost
    }
    interface INerd with
        member this.Text
            with get() =
                sprintf "%.0f" this.TotalBuildCost.BuildPoints
        member this.Tooltip
            with get() =
                let bpr a lbl =
                    match a with
                    | 0.0 -> None
                    | a -> Some <| sprintf "%.1f %s" a lbl
                let hoverText =
                    [
                        bpr this.TotalBuildCost.BuildPoints "build points"
                        bpr this.TotalBuildCost.Boronide "boronide"
                        bpr this.TotalBuildCost.Corbomite "corbomite"
                        bpr this.TotalBuildCost.Duranium "duranium"
                        bpr this.TotalBuildCost.Gallicite "gallicite"
                        bpr this.TotalBuildCost.Mercassium "mercassium"
                        bpr this.TotalBuildCost.Neutronium "neutronium"
                        bpr this.TotalBuildCost.Uridium "uridium"
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
        member this.Description
            with get() =
                sprintf "%.0f BP" this.TotalBuildCost.BuildPoints
                |> Some
