module Nerds.DeployTimeNerd

open Nerds.Icon
open Nerds.Common
open Model.Measures
open System

type DeployTimeNerd =
    {
        DeployTime: float<mo>
    }
    interface INerd with
        member this.Text
            with get() =
                String.Format("{0}", this.DeployTime)
        member this.Tooltip
            with get() =
                String.Format("{0} months", this.DeployTime)
        member this.Icon
            with get() =
                Calendar
        member this.Render
            with get() = true
        member this.Description
            with get() =
                Some <| String.Format("Intended Deployment Time {0} months", this.DeployTime)
