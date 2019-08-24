module ShipComponents.Engine

open Types
open ShipComponents.Common
open ShipComponent
open Bulma.Form
open Measures

let render (comp: Engine) dispatch =
    let header =
        [
            Name "Engine"
            Price comp.BuildCost
            Size comp.TotalSize
            EnginePower comp.EnginePower
            RemoveButton
        ] |> Some
    let form =
        [ HorGrp (None,
                  [ IntInp ({ Label = Some "Count"; Value = comp.Count },
                            (fun n -> Msg.ReplaceShipComponent (Engine { comp with Count = n }) |> dispatch)
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
                  [ IntInp ({ Label = Some "Size"; Value = comp.Size*1</hs> },
                            (fun n -> Msg.ReplaceShipComponent (Engine { comp with Size = n*1<hs> }) |> dispatch)
                           )
                  ]
                 )
        ]
        |> Bulma.Form.render
    shipComponentCard header
                      form
                      (Engine comp)
                      dispatch
