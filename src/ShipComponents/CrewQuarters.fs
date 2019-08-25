module ShipComponents.CrewQuarters

open Global
open Types
open ShipComponents.Common
open ShipComponent
open Bulma.Form
open Measures
open Technology
open System

let render (ship: Ship) dispatch =
    let header =
        [
            Name "Crew Quarters"
            TotalPrice ship.CrewQuartersBuildCost
            SizeFloat (1<comp>, ship.CrewQuartersSize*1.0</comp>)
        ] |> Some
    let form =
        [ HorGrp (None,
                  [ FltInp ({ Label = Some "Deployment Time"; Value = ship.DeployTime*1.0</mo> },
                            (fun n -> Msg.ReplaceShip { ship with DeployTime = n*1.0<mo> } |> dispatch)
                           )
                    IntInp ({ Label = Some "Spare Crew Berths"; Value = ship.SpareBerths*1</people>; Max = None },
                            (fun n -> Msg.ReplaceShip { ship with SpareBerths = n*1<people> } |> dispatch)
                           )
                  ]
                 )
        ]
        |> Bulma.Form.render
    shipComponentCard header
                      form
                      None
                      dispatch
