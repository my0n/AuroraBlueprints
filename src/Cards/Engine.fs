module Cards.Engine

open Global
open System

open App.Msg
open Bulma.Card
open Bulma.Form
open Cards.Common
open Model.Measures
open Model.Technology
open Comp.Engine
open Comp.ShipComponent
open Ship

let render (ship: Ship) (comp: Engine) dispatch =
    let header =
        [
            Name comp.Name
            MaintenanceClass comp.MaintenenceClass
            Price (comp.Count, comp.BuildCost)
            SizeInt (comp.Count, comp.Size)
            EnginePower (comp.Count, comp.EnginePower, comp.Size, ship.Speed)
            FuelConsumption (comp.Count, comp.FuelConsumption, comp.FuelConsumption / comp.EnginePower)
        ]
    let form =
        [ HorGrp (None,
                  [ IntInp ({ Label = Some "Count"; Value = comp.Count*1</comp>; Min = Some 0; Max = None },
                            (fun n -> Msg.ReplaceShipComponent (ship, Engine { comp with Count = n*1<comp> }) |> dispatch)
                           )
                    TxtInp ({ Label = Some "Name"; Value = comp.Name },
                            (fun n -> Msg.ReplaceShipComponent (ship, Engine { comp with Name = n }) |> dispatch)
                           )
                    TxtInp ({ Label = Some "Manufacturer"; Value = comp.Manufacturer },
                            (fun n -> Msg.ReplaceShipComponent (ship, Engine { comp with Manufacturer = n }) |> dispatch)
                           )
                  ]
                 )
          HorGrp (None,
                  [ IntInp ({ Label = Some "Size"; Value = comp.Size*1<comp/hs>; Min = Some 0; Max = Some 50 },
                            (fun n -> Msg.ReplaceShipComponent (ship, Engine { comp with Size = n*1<hs/comp> }) |> dispatch)
                           )
                    Select ({ Label = Some "Engine Technology"; Options = Technology.engine |> Map.toListV (fun v -> String.Format("{0} ({1:0} EP/HS)", v.Name, v.PowerPerHs)); Value = comp.EngineTech.Level },
                            (fun n -> Msg.ReplaceShipComponent (ship, Engine { comp with EngineTech = Technology.engine.[n] }) |> dispatch)
                           )
                    Select ({ Label = Some "Engine Efficiency"; Options = Technology.engineEfficiency |> Map.toListV (fun v -> sprintf "%.2fx fuel consumption" v.Efficiency); Value = comp.EfficiencyTech.Level },
                            (fun n -> Msg.ReplaceShipComponent (ship, Engine { comp with EfficiencyTech = Technology.engineEfficiency.[n] }) |> dispatch)
                           )
                    Select ({ Label = Some "Engine Power"; Options = Technology.allPowerMods |> Map.toListV (fun v -> sprintf "%.2fx engine power" v.PowerMod); Value = comp.PowerModTech.Level },
                            (fun n -> Msg.ReplaceShipComponent (ship, Engine { comp with PowerModTech = Technology.allPowerMods.[n] }) |> dispatch)
                           )
                  ]
                 )
        ]
        |> Bulma.Form.render
    let actions =
        [
            "Remove", DangerColor, (fun _ -> Msg.RemoveComponentFromShip (ship, Engine comp) |> dispatch)
        ]
    shipComponentCard header form actions
