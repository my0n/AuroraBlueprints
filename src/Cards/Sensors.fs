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

let render (allTechs: AllTechnologies) (tech: GameObjectId list) (ship: Ship) (comp: Sensors) dispatch =
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
                [
                    Bulma.FC.IntInput
                        {
                            Label = Some "Standard Geo"
                            Value = comp.StandardGeo
                            Min = Some 0
                            Max = None
                            Disabled = not <| List.contains allTechs.GeologicalSurveySensors tech
                        }
                        (fun n -> Msg.ReplaceShipComponent (ship, Sensors { comp with StandardGeo = n }) |> dispatch)
                    Bulma.FC.IntInput
                        {
                            Label = Some "Improved Geo"
                            Value = comp.ImprovedGeo
                            Min = Some 0
                            Max = None
                            Disabled = not <| List.contains allTechs.ImprovedGeologicalSurveySensors tech
                        }
                        (fun n -> Msg.ReplaceShipComponent (ship, Sensors { comp with ImprovedGeo = n }) |> dispatch)
                    Bulma.FC.IntInput
                        {
                            Label = Some "Advanced Geo"
                            Value = comp.AdvancedGeo
                            Min = Some 0
                            Max = None
                            Disabled = not <| List.contains allTechs.AdvancedGeologicalSurveySensors tech
                        }
                        (fun n -> Msg.ReplaceShipComponent (ship, Sensors { comp with AdvancedGeo = n }) |> dispatch)
                    Bulma.FC.IntInput
                        {
                            Label = Some "Phased Geo"
                            Value = comp.PhasedGeo
                            Min = Some 0
                            Max = None
                            Disabled = not <| List.contains allTechs.PhasedGeologicalSurveySensors tech
                        }
                        (fun n -> Msg.ReplaceShipComponent (ship, Sensors { comp with PhasedGeo = n }) |> dispatch)
                ]
            Bulma.FC.HorizontalGroup
                None
                [
                    Bulma.FC.IntInput
                        {
                            Label = Some "Standard Grav"
                            Value = comp.StandardGrav
                            Min = Some 0
                            Max = None
                            Disabled = not <| List.contains allTechs.GravitationalSurveySensors tech
                        }
                        (fun n -> Msg.ReplaceShipComponent (ship, Sensors { comp with StandardGrav = n }) |> dispatch)
                    Bulma.FC.IntInput
                        {
                            Label = Some "Improved Grav"
                            Value = comp.ImprovedGrav
                            Min = Some 0
                            Max = None
                            Disabled = not <| List.contains allTechs.ImprovedGravitationalSurveySensors tech
                        }
                        (fun n -> Msg.ReplaceShipComponent (ship, Sensors { comp with ImprovedGrav = n }) |> dispatch)
                    Bulma.FC.IntInput
                        {
                            Label = Some "Advanced Grav"
                            Value = comp.AdvancedGrav
                            Min = Some 0
                            Max = None
                            Disabled = not <| List.contains allTechs.AdvancedGravitationalSurveySensors tech
                        }
                        (fun n -> Msg.ReplaceShipComponent (ship, Sensors { comp with AdvancedGrav = n }) |> dispatch)
                    Bulma.FC.IntInput
                        {
                            Label = Some "Phased Grav"
                            Value = comp.PhasedGrav
                            Min = Some 0
                            Max = None
                            Disabled = not <| List.contains allTechs.PhasedGravitationalSurveySensors tech
                        }
                        (fun n -> Msg.ReplaceShipComponent (ship, Sensors { comp with PhasedGrav = n }) |> dispatch)
                ]
        ]
    let actions =
        [
            "Remove", DangerColor, (fun _ -> Msg.RemoveComponentFromShip (ship, Sensors comp) |> dispatch)
        ]
    shipComponentCard (comp.Id.ToString ()) header form actions
