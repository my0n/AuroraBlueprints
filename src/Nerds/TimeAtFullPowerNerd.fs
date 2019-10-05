module Nerds.TimeAtFullPowerNerd

open Nerds.Icon
open Nerds.Common
open Model.Measures

type private NiceTime =
    | Years of float<year>
    | Days of float<day>
    | Hours of float<hr>
    | Minutes of float<min>
    | Seconds of float<s>

let private toNiceTime t =
    let years = mo2year t
    let days = mo2day t
    let hours = day2hr days
    let minutes = hr2min hours
    [
        float years,   Years years
        float days,    Days days
        float hours,   Hours hours
        float minutes, Minutes minutes
    ]
    |> List.tryFind (fun (t, _) -> t > 1.0)
    |> Option.map (fun (_, t) -> t)
    |> Option.defaultValue (Seconds <| min2s minutes)

type TimeAtFullPowerNerd =
    {
        FullPowerTime: float<mo>
    }
    interface INerd with
        member this.Text
            with get() =
                match this.FullPowerTime |> toNiceTime with
                | Years t   -> sprintf "%.1f yr"                       t // lol
                | Days t    -> sprintf "%.0f d"   <| rounduom 1.0<day> t
                | Hours t   -> sprintf "%.0f hr"  <| rounduom 1.0<hr>  t
                | Minutes t -> sprintf "%.0f min" <| rounduom 1.0<min> t
                | Seconds t -> sprintf "%.0f s"   <| rounduom 1.0<s>   t
        member this.Tooltip
            with get() =
                match this.FullPowerTime |> toNiceTime with
                | Years t   -> sprintf "%.1f years"                        t // lol
                | Days t    -> sprintf "%.0f days"    <| rounduom 1.0<day> t
                | Hours t   -> sprintf "%.0f hours"   <| rounduom 1.0<hr>  t
                | Minutes t -> sprintf "%.0f minutes" <| rounduom 1.0<min> t
                | Seconds t -> sprintf "%.0f seconds" <| rounduom 1.0<s>   t
                |> sprintf "%s at full power"
        member this.Icon
            with get() =
                Calendar
        member this.Render
            with get() = true
        member this.Description
            with get() =
                match this.FullPowerTime |> toNiceTime with
                | Years t   -> sprintf "%.1f years"                        t // lol
                | Days t    -> sprintf "%.0f days"    <| rounduom 1.0<day> t
                | Hours t   -> sprintf "%.0f hours"   <| rounduom 1.0<hr>  t
                | Minutes t -> sprintf "%.0f minutes" <| rounduom 1.0<min> t
                | Seconds t -> sprintf "%.0f seconds" <| rounduom 1.0<s>   t
                |> sprintf "%s at full power"
                |> Some
