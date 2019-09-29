module Nerds.SpareBerthsNerd

open Nerds.Icon
open Nerds.Common
open Model.Measures
open System

type SpareBerthsNerd =
    {
        SpareBerths: int<people>
    }
    interface INerd with
        member this.Text
            with get() =
                String.Format("{0}", this.SpareBerths)
        member this.Tooltip
            with get() =
                String.Format("{0} spare berths", this.SpareBerths)
        member this.Icon
            with get() =
                UserPlus
        member this.Render
            with get() =
                this.SpareBerths > 0<people>
        member this.Description
            with get() =
                match this.SpareBerths with
                | b when b > 0<people> -> 
                    String.Format("Spare Berths {0}", this.SpareBerths)
                    |> Some
                | _ -> None
