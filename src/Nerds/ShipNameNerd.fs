module Nerds.ShipNameNerd

open Nerds.Icon
open Nerds.Common
open Model.Measures

type ShipNameNerd =
    {
        ShipName: string
        ShipClass: string
    }
    interface INerd with
        member this.Text
            with get() =
                sprintf "%s class %s" this.ShipName this.ShipClass
        member this.Tooltip
            with get() = ""
        member this.Icon
            with get() =
                NoIcon
        member this.Render
            with get() = true
        member this.Description
            with get() =
                Some <| sprintf "%s class %s" this.ShipName this.ShipClass
