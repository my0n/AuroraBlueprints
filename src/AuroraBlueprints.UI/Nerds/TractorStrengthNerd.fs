module Nerds.TractorStrengthNerd

open System
open System.Globalization

open Nerds.Icon
open Nerds.Common
open Model.Measures

let inline private getLoadingTimeStr t =
    let ts = TimeSpan.FromHours(float t)
    let ts = ts.Subtract << TimeSpan.FromMilliseconds <| float ts.Milliseconds
    let ts = ts.ToString("G", CultureInfo.InvariantCulture)
    ts.Split('.').[0]

type TractorStrengthNerd =
    {
        TractorStrength: int<tractorStrength>
        LoadTime: float<hr>
    }
    interface INerd with
        member this.Text
            with get() =
                sprintf "%s" (getLoadingTimeStr this.LoadTime)
        member this.Tooltip
            with get() =
                sprintf "%s loading time (dd:hh:mm:ss)\r\n%d tractor strength" (getLoadingTimeStr this.LoadTime) this.TractorStrength
        member this.Icon
            with get() =
                PeopleCarry
        member this.Render
            with get() = true
        member this.Description
            with get() =
                match this.TractorStrength with
                | t when t > 1<tractorStrength> ->
                    sprintf "Cargo Handling Multiplier %d" t
                    |> Some
                | _ -> None
