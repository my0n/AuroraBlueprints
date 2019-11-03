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

open Technology

let render (allTechs: AllTechnologies) (currentTech: GameObjectId list) (ship: Ship) (comp: Sensors) dispatch =
    let header =
        [
            Name "Sensors"
            Nerd { MaintenanceClass = comp.MaintenanceClass }
            Nerd { TotalBuildCost = comp.BuildCost }
            Nerd { RenderMode = HS; Count = 1<comp>; Size = comp.TotalSize*1</comp> }
            Nerd { Geo = comp.GeoSensorRating; Grav = comp.GravSensorRating }
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
                                Msg.ReplaceShipComponent
                                    (
                                        ship,
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
                                Msg.ReplaceShipComponent
                                    (
                                        ship,
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
