module Nerds.CargoCapacityNerd

open Nerds.Icon
open Nerds.Common
open Model.Measures

type CargoCapacityNerd =
    {
        CargoCapacity: int<cargoCapacity>
    }
    interface INerd with
        member this.Text
            with get() =
                sprintf "%d" this.CargoCapacity
        member this.Tooltip
            with get() =
                sprintf "%d cargo capacity" this.CargoCapacity
        member this.Icon
            with get() =
                Boxes
        member this.Render
            with get() = true
        member this.Description
            with get() =
                match this.CargoCapacity with
                | c when c > 0<cargoCapacity> ->
                    sprintf "Cargo %d" this.CargoCapacity
                    |> Some
                | _ -> None
