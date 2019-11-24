module Cards.TroopTransport

open Global

open App.Msg
open App.Model.UI

open Bulma.Card
open Cards.Common
open Model.Measures
open Comp.TroopTransport
open Comp.ShipComponent
open Comp.Ship

open Nerds.MaintenanceClassNerd
open Nerds.PriceTotalNerd
open Nerds.SizeNerd
open Nerds.TroopTransportNerd

let render (comp: TroopTransport) (model: App.Model.Model) (ship: Ship) dispatch =
    let currentTech = model.CurrentTechnology
    let allTechs = model.AllTechnologies
    let key = ship.Id.ToString() + comp.Id.ToString()
    let expanded = model |> Model.isExpanded key

    let header =
        [
            Name "Troop Transport"
            Nerd { MaintenanceClass = comp.MaintenanceClass }
            Nerd { TotalBuildCost = comp.BuildCost * 1<comp> }
            Nerd { RenderMode = HS; Count = 1<comp>; Size = comp.Size }
            Nerd { CryoDrop = comp.CryoDropCapability * 1<comp>; CombatDrop = comp.CombatDropCapability * 1<comp>; TroopTransport = comp.TroopTransportCapability * 1<comp> }
        ]
    let form =
        [
            Bulma.FC.HorizontalGroup
                None
                (
                    allTechs.TroopTransports
                    |> List.map (fun tech ->
                        Bulma.FC.IntInput
                            {
                                Label = Some tech.Name
                                Value =
                                    comp.TroopTransports
                                    |> Map.tryFind tech
                                    |> Option.defaultValue 0<comp>
                                Min = Some 0
                                Max = None
                                Disabled = not <| List.contains tech.Id currentTech
                            }
                            (fun n ->
                                Msg.UpdateComponent
                                    (
                                        TroopTransport
                                            { comp with
                                                TroopTransports = comp.TroopTransports.Add (tech, n)
                                            }
                                    )
                                |> dispatch
                            )
                    )
                )
        ]
    let actions =
        [
            "Remove", DangerColor, (fun _ -> Msg.RemoveComponentFromShip (ship, TroopTransport comp) |> dispatch)
        ]
    shipComponentCard key header form actions expanded dispatch
