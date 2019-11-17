module Cards.Sensors

open Global

open App.Msg
open Bulma.Card
open Cards.Common
open Comp.Sensors
open Comp.ShipComponent
open Comp.Ship

open Model.Measures

open Nerds.MaintenanceClassNerd
open Nerds.PriceTotalNerd
open Nerds.SizeNerd
open Nerds.SensorStrengthNerd

open Model.Technology

let render (allTechs: AllTechnologies) (currentTech: GameObjectId list) (ship: Ship) (comp: Sensors) dispatch =
    let header =
        [
            Name "Sensors"
            Nerd { MaintenanceClass = comp.MaintenanceClass }
            Nerd { TotalBuildCost = comp.BuildCost * 1<comp> }
            Nerd { RenderMode = HS; Count = 1<comp>; Size = comp.Size }
            Nerd { Geo = comp.GeoSensorRating * 1<comp>; Grav = comp.GravSensorRating * 1<comp> }
        ]
    let form =
        [
            Bulma.FC.HorizontalGroup
                None
                (
                    allTechs.GeoSensors
                    |> List.map (fun tech ->
                        Bulma.FC.IntInput
                            {
                                Label = Some tech.Name
                                Value =
                                    comp.GeoSensors
                                    |> Map.tryFind tech
                                    |> Option.defaultValue 0<comp>
                                Min = Some 0
                                Max = None
                                Disabled = not <| List.contains tech.Id currentTech
                            }
                            (fun n ->
                                Msg.UpdateComponent
                                    (
                                        Sensors
                                            { comp with
                                                GeoSensors = comp.GeoSensors.Add (tech, n)
                                            }
                                    )
                                |> dispatch
                            )
                    )
                )
            Bulma.FC.HorizontalGroup
                None
                (
                    allTechs.GravSensors
                    |> List.map (fun tech ->
                        Bulma.FC.IntInput
                            {
                                Label = Some tech.Name
                                Value =
                                    comp.GravSensors
                                    |> Map.tryFind tech
                                    |> Option.defaultValue 0<comp>
                                Min = Some 0
                                Max = None
                                Disabled = not <| List.contains tech.Id currentTech
                            }
                            (fun n ->
                                Msg.UpdateComponent
                                    (
                                        Sensors
                                            { comp with
                                                GravSensors = comp.GravSensors.Add (tech, n)
                                            }
                                    )
                                |> dispatch
                            )
                    )
                )
        ]
    let actions =
        [
            "Remove", DangerColor, (fun _ -> Msg.RemoveComponentFromShip (ship, Sensors comp) |> dispatch)
        ]
    shipComponentCard (comp.Id.ToString ()) header form actions
