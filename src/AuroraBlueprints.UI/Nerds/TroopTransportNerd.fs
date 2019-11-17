module Nerds.TroopTransportNerd

open Model.Measures
open Nerds.Icon
open Nerds.Common

type TroopTransportNerd =
    {
        CryoDrop: int<company>
        CombatDrop: int<company>
        TroopTransport: int<company>
    }
    interface INerd with
        member this.Text
            with get() =
                sprintf "%d/%d/%d" this.TroopTransport this.CombatDrop this.CryoDrop
        member this.Tooltip
            with get() =
                let troop =
                    match this.TroopTransport with
                    | 0<company> -> None
                    | _ -> Some << sprintf "%.1f troop capacity (battalions)" <| company2battalion this.TroopTransport
                let combat =
                    match this.CombatDrop with
                    | 0<company> -> None
                    | _ -> Some << sprintf "%.1f combat drop capacity (battalions)" <| company2battalion this.CombatDrop
                let cryo =
                    match this.CryoDrop with
                    | 0<company> -> None
                    | _ -> Some << sprintf "%.1f cryo drop capacity (battalions)" <| company2battalion this.CryoDrop
                [ troop; combat; cryo ]
                |> List.choose id
                |> String.concat "\r\n"
        member this.Icon
            with get() =
                ParachuteBox
        member this.Render
            with get() = true
        member this.Description
            with get() =
                let troop =
                    match this.TroopTransport with
                    | 0<company> -> None
                    | _ -> Some << sprintf "Troop Capacity: %.1f Battalions" <| company2battalion this.TroopTransport
                let combat =
                    match this.CombatDrop with
                    | 0<company> -> None
                    | _ -> Some << sprintf "Drop Capacity: %.1f Battalions" <| company2battalion this.CombatDrop
                let cryo =
                    match this.CryoDrop with
                    | 0<company> -> None
                    | _ -> Some << sprintf "Cryo Drop Capacity: %.1f Battalions" <| company2battalion this.CryoDrop
                match
                    [ troop; combat; cryo ]
                    |> List.choose id
                    |> String.concat " "
                    with
                | "" -> None
                | a -> Some a
