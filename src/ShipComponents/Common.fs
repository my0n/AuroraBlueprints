module ShipComponents.Common

open Measures
open Types
open Global

open Bulma.Card
open BuildCost

type ShipComponentCardHeaderItem =
    | Name of string
    | Size of float<hs>
    | FuelCapacity of float<kl>
    | EnginePower of float<ep>
    | Velocity of float<km/s>
    | Price of BuildCost
    | RemoveButton

let inline private renderHeader header comp dispatch: CardHeaderElement list option =
    header
    |> Option.bind (
        List.map (fun h ->
            match h with
            | Name name -> Title name
            | Size size -> Info (sprintf "%.1f" size, sprintf "%.1f HS" size, Weight)
            | FuelCapacity fc -> Info (sprintf "%.1f" fc, sprintf "%.1f kL" fc, GasPump)
            | EnginePower e -> Info (sprintf "%.1f" e, sprintf "%.1f EP" e, AngleDoubleRight)
            | Velocity e -> Info (sprintf "%.0f" e, sprintf "%.0f km/s" e, AngleDoubleRight)
            | Price b ->
                let hoverText =
                    [
                        (match b.Gallicite with 0.0 -> None | a -> Some <| sprintf "%.1f build points" a)
                        (match b.Gallicite with 0.0 -> None | a -> Some <| sprintf "%.0f gallicite" a)
                    ]
                    |> List.choose id
                    |> String.concat "\r\n"
                Info (sprintf "%.0f" b.BuildPoints, (match hoverText with "" -> "free" | _ -> hoverText), Dollar)
            | RemoveButton -> Button (Close, (fun _ -> Msg.RemoveComponentFromShip comp |> dispatch))
        )
        >> Some
    )

let shipComponentCard header contents comp dispatch =
    Bulma.Card.render (renderHeader header comp dispatch) contents
