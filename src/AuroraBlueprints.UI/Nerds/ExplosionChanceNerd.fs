module Nerds.ExplosionChanceNerd

open Nerds.Icon
open Nerds.Common

type ExplosionChanceNerd =
    {
        ExplosionChance: float
    }
    interface INerd with
        member this.Text
            with get() =
                sprintf "%.0f%%" (this.ExplosionChance * 100.0)
        member this.Tooltip
            with get() =
                sprintf "%.0f%% explosion chance" (this.ExplosionChance * 100.0)
        member this.Icon
            with get() =
                Bomb
        member this.Render
            with get() = true
        member this.Description
            with get() =
                sprintf "Exp %.0f%%" (this.ExplosionChance * 100.0)
                |> Some
