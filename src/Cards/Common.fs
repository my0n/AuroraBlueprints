module Cards.Common

open Global

open Bulma.Card
open Model.BuildCost
open Model.Measures
open Model.MaintenanceClass

type ShipComponentCardHeaderItem =
    | Name of string
    | SizeInt of int<comp> * int<hs/comp>
    | SizeFloat of int<comp> * float<hs/comp>
    | FuelCapacity of float<kl>
    | EnginePower of int<comp> * float<ep/comp> * int<hs/comp> * float<km/s>
    | Velocity of float<km/s>
    | FuelConsumption of int<comp> * float<kl/hr/comp> * float<kl/hr/ep>
    | Price of int<comp> * BuildCost
    | TotalPrice of TotalBuildCost
    | SensorStrength of int * int
    | MaintenanceClass of MaintenanceClass
    | ArmorStrength of float
    | ArmorSize of int * int
    | PowerProduction of int<comp> * float<power/comp>

let inline private renderHeader header: CardHeaderElement list =
    header
    |> List.map (fun h ->
        match h with
        | Name name -> Title name
        | SizeInt (count, size) ->
            let hoverText =
                match count with
                | 1<comp> | 0<comp> -> sprintf "%d HS" (size * count)
                | _ -> sprintf "%d (%d) HS" (size * count) size
            Info (sprintf "%d" (size * count), hoverText, Weight)

        | SizeFloat (count, size) ->
            let hoverText =
                match count with
                | 1<comp> | 0<comp> -> sprintf "%.1f HS" (size * int2float count)
                | _ -> sprintf "%.1f (%.1f) HS" (size * int2float count) size
            Info (sprintf "%.1f" (size * int2float count), hoverText, Weight)

        | FuelCapacity fc ->
            Info (sprintf "%.0f" fc, sprintf "%.0f kL" fc, GasPump)

        | EnginePower (count, e, sz, spd) ->
            let spdstr = sprintf "%.0f km/s" spd
            let epstr =
                match count with
                | 1<comp> | 0<comp> -> sprintf "%.1f EP" (e * int2float count)
                | _ -> sprintf "%.1f (%.1f) EP" (e * int2float count) e
            let ephrstr = sprintf "%.1f EP/HS" (e / int2float sz)
            let hoverText = [ spdstr; epstr; ephrstr ] |> String.concat "\r\n"
            Info (sprintf "%.0f" spd, hoverText, AngleDoubleRight)

        | Velocity e ->
            Info (sprintf "%.0f" e, sprintf "%.0f km/s" e, AngleDoubleRight)

        | FuelConsumption (count, a, b) ->
            let klhr =
                match count with
                | 1<comp> | 0<comp> -> sprintf "%0.2f kl/hr" (a * int2float count)
                | _ -> sprintf "%0.2f (%0.2f) kl/hr" (a * int2float count) a
            let klhrep = sprintf "%0.4f kl/hr/EP" b
            let hoverText = [ klhr; klhrep ] |> String.concat "\r\n"
            Info (sprintf "%.2f" (a * int2float count), hoverText, Tachometer)
                
        | Price (count, b) ->
            let bpr a lbl =
                match a with
                | 0.0</comp> -> None
                | a ->
                    (match count with
                        | 1<comp> -> sprintf "%.1f %s" a lbl
                        | _ -> sprintf "%.1f (%.1f) %s" (a * int2float count) a lbl
                    )
                    |> Some
            let hoverText =
                [
                    bpr b.BuildPoints "build points"
                    bpr b.Boronide "boronide"
                    bpr b.Corbomite "corbomite"
                    bpr b.Duranium "duranium"
                    bpr b.Gallicite "gallicite"
                    bpr b.Mercassium "mercassium"
                    bpr b.Neutronium "neutronium"
                    bpr b.Uridium "uridium"
                ]
                |> List.choose id
                |> String.concat "\r\n"
            Info (sprintf "%.0f" (b.BuildPoints * int2float count), (match hoverText with "" -> "free" | _ -> hoverText), Dollar)

        | TotalPrice b ->
            let bpr a lbl =
                match a with
                | 0.0 -> None
                | a -> Some <| sprintf "%.1f %s" a lbl
            let hoverText =
                [
                    bpr b.BuildPoints "build points"
                    bpr b.Boronide "boronide"
                    bpr b.Corbomite "corbomite"
                    bpr b.Duranium "duranium"
                    bpr b.Gallicite "gallicite"
                    bpr b.Mercassium "mercassium"
                    bpr b.Neutronium "neutronium"
                    bpr b.Uridium "uridium"
                ]
                |> List.choose id
                |> String.concat "\r\n"
            Info (sprintf "%.0f" b.BuildPoints, (match hoverText with "" -> "free" | _ -> hoverText), Dollar)

        | SensorStrength (geo, grav) ->
            let geot =
                match geo with
                | 0 -> None
                | _ -> Some <| sprintf "%d geo survey points/hr" geo
            let gravt =
                match grav with
                | 0 -> None
                | _ -> Some <| sprintf "%d grav survey points/hr" grav
            let hoverText =
                [ geot; gravt ]
                |> List.choose id
                |> String.concat "\r\n"
            Info (sprintf "%d/%d" geo grav, hoverText, GlobeAmericas)

        | MaintenanceClass (maint) ->
            match maint with
            | Commercial -> NoRender
            | Military -> Info ("", "This component is classified as a military component for maintenance purposes.", Shield)

        | ArmorStrength (strength) ->
            Info (sprintf "%.1f" strength, sprintf "%.1f armor strength" strength, Shield)

        | ArmorSize (depth, width) ->
            Info (sprintf "%d×%d" depth width, sprintf "%d rows deep\n%d columns wide" depth width, ThLarge)

        | PowerProduction (count, pow) ->
            let hoverText =
                match count with
                | 1<comp> | 0<comp> -> sprintf "%.1f power generated" (pow * int2float count)
                | _ -> sprintf "%.1f (%.1f) power generated" (pow * int2float count) pow
            Info (sprintf "%.1f" (pow * int2float count), hoverText, Bolt)
    )

let shipComponentCard header contents actions =
    Bulma.Card.render {
        HeaderItems = renderHeader header
        Contents = contents
        Actions = actions
        HasExpanderToggle = true
    }
