module ShipComponents.Sensors

open Global
open Types
open ShipComponents.Common
open ShipComponent
open Bulma.Form
open Measures
open Technology
open System

let render (comp: Sensors) dispatch =
    let header =
        [
            Name "Sensors"
            MaintenanceClass comp.MaintenenceClass
            Price (1<comp>, comp.BuildCost)
            SizeInt (1<comp>, comp.Size*1</comp>)
            SensorStrength (comp.GeoSensorRating, comp.GravSensorRating)
            RemoveButton
        ] |> Some
    let form =
        [ HorGrp (None,
                  [ IntInp ({ Label = Some "Standard Geo"; Value = comp.StandardGeo*1</comp>; Max = None },
                            (fun n -> Msg.ReplaceShipComponent (Sensors { comp with StandardGeo = n*1<comp> }) |> dispatch)
                           )
                    IntInp ({ Label = Some "Improved Geo"; Value = comp.ImprovedGeo*1</comp>; Max = None },
                            (fun n -> Msg.ReplaceShipComponent (Sensors { comp with ImprovedGeo = n*1<comp> }) |> dispatch)
                           )
                    IntInp ({ Label = Some "Advanced Geo"; Value = comp.AdvancedGeo*1</comp>; Max = None },
                            (fun n -> Msg.ReplaceShipComponent (Sensors { comp with AdvancedGeo = n*1<comp> }) |> dispatch)
                           )
                    IntInp ({ Label = Some "Phased Geo"; Value = comp.PhasedGeo*1</comp>; Max = None },
                            (fun n -> Msg.ReplaceShipComponent (Sensors { comp with PhasedGeo = n*1<comp> }) |> dispatch)
                           )
                  ]
                 )
          HorGrp (None,
                  [ IntInp ({ Label = Some "Standard Grav"; Value = comp.StandardGrav*1</comp>; Max = None },
                            (fun n -> Msg.ReplaceShipComponent (Sensors { comp with StandardGrav = n*1<comp> }) |> dispatch)
                           )
                    IntInp ({ Label = Some "Improved Grav"; Value = comp.ImprovedGrav*1</comp>; Max = None },
                            (fun n -> Msg.ReplaceShipComponent (Sensors { comp with ImprovedGrav = n*1<comp> }) |> dispatch)
                           )
                    IntInp ({ Label = Some "Advanced Grav"; Value = comp.AdvancedGrav*1</comp>; Max = None },
                            (fun n -> Msg.ReplaceShipComponent (Sensors { comp with AdvancedGrav = n*1<comp> }) |> dispatch)
                           )
                    IntInp ({ Label = Some "Phased Grav"; Value = comp.PhasedGrav*1</comp>; Max = None },
                            (fun n -> Msg.ReplaceShipComponent (Sensors { comp with PhasedGrav = n*1<comp> }) |> dispatch)
                           )
                  ]
                 )
        ]
        |> Bulma.Form.render
    shipComponentCard header
                      form
                      (Sensors comp)
                      dispatch
