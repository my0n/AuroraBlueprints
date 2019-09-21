module Nerds.SensorStrengthNerd

open Nerds.Icon
open Nerds.Common

type SensorStrengthNerd =
    {
        Geo: int
        Grav: int
    }
    interface INerd with
        member this.Text
            with get() =
                sprintf "%d/%d" this.Geo this.Grav
        member this.Tooltip
            with get() =
                let geot =
                    match this.Geo with
                    | 0 -> None
                    | _ -> Some <| sprintf "%d geo survey points/hr" this.Geo
                let gravt =
                    match this.Grav with
                    | 0 -> None
                    | _ -> Some <| sprintf "%d grav survey points/hr" this.Grav
                [ geot; gravt ]
                |> List.choose id
                |> String.concat "\r\n"
        member this.Icon
            with get() =
                GlobeAmericas
        member this.Render
            with get() = true
        member this.Description
            with get() = None
