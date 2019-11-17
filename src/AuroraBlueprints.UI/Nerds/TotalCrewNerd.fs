module Nerds.TotalCrewNerd

open Nerds.Icon
open Nerds.Common
open Model.Measures
open System

type TotalCrewNerd =
    {
        TotalCrew: int<people>
    }
    interface INerd with
        member this.Text
            with get() =
                String.Format("{0}", this.TotalCrew)
        member this.Tooltip
            with get() =
                String.Format("{0} crew members", this.TotalCrew)
        member this.Icon
            with get() =
                Users
        member this.Render
            with get() = true
        member this.Description
            with get() =
                Some <| String.Format("{0} crew", this.TotalCrew)
