module Cards.Sensors

open Global
open System

open App.Msg
open Bulma.Card
open Bulma.Form
open Cards.Common
open Model.Measures
open Comp.Sensors
open Comp.ShipComponent
open Ship

open Nerds.MaintenanceClassNerd
open Nerds.PriceTotalNerd
open Nerds.SizeIntNerd
open Nerds.SensorStrengthNerd

let render (ship: Ship) (comp: Sensors) dispatch =
    let header =
        [
            Name "Sensors"
            Nerd { MaintenanceClass = comp.MaintenanceClass }
            Nerd { TotalBuildCost = comp.BuildCost }
            Nerd { Count = 1<comp>; Size = comp.Size*1</comp> }
            Nerd { Geo = comp.GeoSensorRating; Grav = comp.GravSensorRating }
        ]
    let form =
        [ HorGrp (None,
                  [ IntInp ({ Label = Some "Standard Geo"; Value = comp.StandardGeo*1</comp>; Min = Some 0; Max = None },
                            (fun n -> Msg.ReplaceShipComponent (ship, Sensors { comp with StandardGeo = n*1<comp> }) |> dispatch)
                           )
                    IntInp ({ Label = Some "Improved Geo"; Value = comp.ImprovedGeo*1</comp>; Min = Some 0; Max = None },
                            (fun n -> Msg.ReplaceShipComponent (ship, Sensors { comp with ImprovedGeo = n*1<comp> }) |> dispatch)
                           )
                    IntInp ({ Label = Some "Advanced Geo"; Value = comp.AdvancedGeo*1</comp>; Min = Some 0; Max = None },
                            (fun n -> Msg.ReplaceShipComponent (ship, Sensors { comp with AdvancedGeo = n*1<comp> }) |> dispatch)
                           )
                    IntInp ({ Label = Some "Phased Geo"; Value = comp.PhasedGeo*1</comp>; Min = Some 0; Max = None },
                            (fun n -> Msg.ReplaceShipComponent (ship, Sensors { comp with PhasedGeo = n*1<comp> }) |> dispatch)
                           )
                  ]
                 )
          HorGrp (None,
                  [ IntInp ({ Label = Some "Standard Grav"; Value = comp.StandardGrav*1</comp>; Min = Some 0; Max = None },
                            (fun n -> Msg.ReplaceShipComponent (ship, Sensors { comp with StandardGrav = n*1<comp> }) |> dispatch)
                           )
                    IntInp ({ Label = Some "Improved Grav"; Value = comp.ImprovedGrav*1</comp>; Min = Some 0; Max = None },
                            (fun n -> Msg.ReplaceShipComponent (ship, Sensors { comp with ImprovedGrav = n*1<comp> }) |> dispatch)
                           )
                    IntInp ({ Label = Some "Advanced Grav"; Value = comp.AdvancedGrav*1</comp>; Min = Some 0; Max = None },
                            (fun n -> Msg.ReplaceShipComponent (ship, Sensors { comp with AdvancedGrav = n*1<comp> }) |> dispatch)
                           )
                    IntInp ({ Label = Some "Phased Grav"; Value = comp.PhasedGrav*1</comp>; Min = Some 0; Max = None },
                            (fun n -> Msg.ReplaceShipComponent (ship, Sensors { comp with PhasedGrav = n*1<comp> }) |> dispatch)
                           )
                  ]
                 )
        ]
        |> Bulma.Form.render
    let actions =
        [
            "Remove", DangerColor, (fun _ -> Msg.RemoveComponentFromShip (ship, Sensors comp) |> dispatch)
        ]
    shipComponentCard header form actions
