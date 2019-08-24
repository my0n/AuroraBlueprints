module ShipComponents.Common

open Measures
open Types
open Global

open Bulma.Card
open BuildCost

type ShipComponentCardHeaderItem =
    | Name of string
    | SizeInt of int<comp> * int<hs/comp>
    | SizeFloat of int<comp> * float<hs/comp>
    | FuelCapacity of float<kl>
    | EnginePower of int<comp> * float<ep/comp>
    | Velocity of float<km/s>
    | FuelConsumption of int<comp> * float<kl/hr/comp> * float<kl/hr/ep>
    | Price of int<comp> * BuildCost
    | RemoveButton

let inline private renderHeader header comp dispatch: CardHeaderElement list option =
    header
    |> Option.bind (
        List.map (fun h ->
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
            | EnginePower (count, e) ->
                let hoverText =
                    match count with
                    | 1<comp> | 0<comp> -> sprintf "%.1f EP" (e * int2float count)
                    | _ -> sprintf "%.1f (%.1f) EP" (e * int2float count) e
                    
                Info (sprintf "%.1f" (e * int2float count), hoverText, AngleDoubleRight)
            | Velocity e ->
                Info (sprintf "%.0f" e, sprintf "%.0f km/s" e, AngleDoubleRight)
            | FuelConsumption (count, a, b) ->
                let klhr =
                    match count with
                    | 1<comp> | 0<comp> -> sprintf "%0.2f kl/hr" (a * int2float count)
                    | _ -> sprintf "%0.2f (%0.2f) kl/hr" (a * int2float count) a
                let hoverText =
                    [
                        klhr
                        sprintf "%0.2f kl/hr/ep" b
                    ] |> String.concat "\r\n"
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
                        bpr b.Gallicite "gallicite"
                    ]
                    |> List.choose id
                    |> String.concat "\r\n"
                Info (sprintf "%.0f" (b.BuildPoints * int2float count), (match hoverText with "" -> "free" | _ -> hoverText), Dollar)
            | RemoveButton -> Button (Close, (fun _ -> Msg.RemoveComponentFromShip comp |> dispatch))
        )
        >> Some
    )

let shipComponentCard header contents comp dispatch =
    Bulma.Card.render (renderHeader header comp dispatch) contents
