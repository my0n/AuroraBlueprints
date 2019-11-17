module Nerds.CryogenicBerthsNerd

open Nerds.Icon
open Nerds.Common
open Model.Measures
open System

type CryogenicBerthsNerd =
    {
        CryogenicBerths: int<people>
    }
    interface INerd with
        member this.Text
            with get() =
                String.Format("{0}", this.CryogenicBerths)
        member this.Tooltip
            with get() =
                String.Format("{0} cryogenic berths", this.CryogenicBerths)
        member this.Icon
            with get() =
                Snowflake
        member this.Render
            with get() =
                this.CryogenicBerths > 0<people>
        member this.Description
            with get() =
                match this.CryogenicBerths with
                | b when b > 0<people> -> 
                    String.Format("Cryogenic Berths {0}", this.CryogenicBerths)
                    |> Some
                | _ -> None
