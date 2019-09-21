module Nerds.MaintenanceClassNerd

open Model.MaintenanceClass
open Nerds.Icon
open Nerds.Common

type MaintenanceClassNerd =
    {
        MaintenanceClass: MaintenanceClass
    }
    interface INerd with
        member this.Text
            with get() = ""
        member this.Tooltip
            with get() =
                match this.MaintenanceClass with
                | Commercial -> ""
                | Military -> "This component is classified as a military component for maintenance purposes."
        member this.Icon
            with get() =
                Shield
        member this.Render
            with get() =
                match this.MaintenanceClass with
                | Commercial -> false
                | Military -> true
        member this.Description
            with get() = None
