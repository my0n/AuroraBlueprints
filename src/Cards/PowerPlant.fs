module Cards.PowerPlant

open App.Msg
open Bulma.Card
open Bulma.Form
open Cards.Common
open Model.Measures
open Comp.PowerPlant
open Comp.ShipComponent
open Ship
open Model.Technology
open System
open Global

let render (ship: Ship) (comp: PowerPlant) dispatch =
    let header =
        [
            Name comp.Name
            Price (comp.Count, comp.BuildCost)
            SizeFloat (comp.Count, comp.Size)
            PowerProduction (comp.Count, comp.Power)
        ]

    let sizeOptions =
        [ 0.1 .. 0.1 .. 0.9] @ [1.0 .. 30.0]

    let form =
        [ HorGrp (None,
                  [ IntInp ({ Label = Some "Count"; Value = comp.Count*1</comp>; Min = Some 0; Max = None },
                            (fun n -> Msg.ReplaceShipComponent (ship, PowerPlant { comp with Count = n*1<comp> }) |> dispatch)
                           )
                    TxtInp ({ Label = Some "Name"; Value = comp.Name },
                            (fun n -> Msg.ReplaceShipComponent (ship, PowerPlant { comp with Name = n }) |> dispatch)
                           )
                    TxtInp ({ Label = Some "Manufacturer"; Value = comp.Manufacturer },
                            (fun n -> Msg.ReplaceShipComponent (ship, PowerPlant { comp with Manufacturer = n }) |> dispatch)
                           )
                  ]
                 )
          HorGrp (None,
                  [ Select ({ Label = Some "Size"
                              Options =
                                sizeOptions
                                |> List.mapi (fun i o -> i, sprintf "%.1f" o)
                              Value =
                                sizeOptions
                                |> List.tryFindIndex (fun o -> o = comp.Size * 1.0<comp/hs>)
                                |> Option.defaultValue 1
                            },
                            (fun n -> Msg.ReplaceShipComponent (ship, PowerPlant { comp with Size = sizeOptions.[n] * 1.0<hs/comp> }) |> dispatch)
                           )
                    Select ({ Label = Some "Power Plant Technology"; Options = Technology.powerPlant |> Map.toListV (fun v -> String.Format("{0} ({1} power/HS)", v.Name, v.PowerOutput)); Value = comp.Technology.Level },
                            (fun n -> Msg.ReplaceShipComponent (ship, PowerPlant { comp with Technology = Technology.powerPlant.[n] }) |> dispatch)
                           )
                    Select ({ Label = Some "Power Boost"; Options = Technology.powerBoost |> Map.toListV (fun v -> sprintf "Reactor Power Boost %d%%, Explosion %d%%" (int (v.PowerBoost * 100.0)) (int (v.ExplosionChance * 100.0))); Value = comp.PowerBoost.Level },
                            (fun n -> Msg.ReplaceShipComponent (ship, PowerPlant { comp with PowerBoost = Technology.powerBoost.[n] }) |> dispatch)
                           )
                  ]
                 )
        ]
        |> Bulma.Form.render
    let actions =
        [
            "Remove", DangerColor, (fun _ -> Msg.RemoveComponentFromShip (ship, PowerPlant comp) |> dispatch)
        ]
    shipComponentCard header form actions
