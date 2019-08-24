module ShipComponents.Engine

open Global
open Types
open ShipComponents.Common
open ShipComponent
open Bulma.Form
open Measures
open Technology

let render (comp: Engine) dispatch =
    let header =
        [
            Name "Engine"
            Price (comp.Count, comp.BuildCost)
            SizeInt (comp.Count, comp.Size)
            EnginePower (comp.Count, comp.EnginePower)
            FuelConsumption (comp.Count, comp.FuelConsumption, comp.FuelConsumption / comp.EnginePower)
            RemoveButton
        ] |> Some
    let form =
        [ HorGrp (None,
                  [ IntInp ({ Label = Some "Count"; Value = comp.Count*1</comp>; Max = None },
                            (fun n -> Msg.ReplaceShipComponent (Engine { comp with Count = n*1<comp> }) |> dispatch)
                           )
                    TxtInp ({ Label = Some "Name"; Value = comp.Name },
                            (fun n -> Msg.ReplaceShipComponent (Engine { comp with Name = n }) |> dispatch)
                           )
                    TxtInp ({ Label = Some "Manufacturer"; Value = comp.Manufacturer },
                            (fun n -> Msg.ReplaceShipComponent (Engine { comp with Manufacturer = n }) |> dispatch)
                           )
                  ]
                 )
          HorGrp (None,
                  [ IntInp ({ Label = Some "Size"; Value = comp.Size*1<comp/hs>; Max = Some 50 },
                            (fun n -> Msg.ReplaceShipComponent (Engine { comp with Size = n*1<hs/comp> }) |> dispatch)
                           )
                    Select ({ Label = Some "Engine Techology"; Options = Technology.engine |> Map.toListV (fun v -> v.Name); Value = comp.EngineTech.Level },
                            (fun n -> Msg.ReplaceShipComponent (Engine { comp with EngineTech = Technology.engine.[n] }) |> dispatch)
                           )
                    Select ({ Label = Some "Engine Efficiency"; Options = Technology.engineEfficiency |> Map.toListV (fun v -> sprintf "%.2fx fuel consumption" v.Efficiency); Value = comp.EfficiencyTech.Level },
                            (fun n -> Msg.ReplaceShipComponent (Engine { comp with EfficiencyTech = Technology.engineEfficiency.[n] }) |> dispatch)
                           )
                    Select ({ Label = Some "Engine Power"; Options = Technology.allPowerMods |> Map.toListV (fun v -> sprintf "%.2fx engine power" v.PowerMod); Value = comp.PowerModTech.Level },
                            (fun n -> Msg.ReplaceShipComponent (Engine { comp with PowerModTech = Technology.allPowerMods.[n] }) |> dispatch)
                           )
                  ]
                 )
        ]
        |> Bulma.Form.render
    shipComponentCard header
                      form
                      (Engine comp)
                      dispatch
