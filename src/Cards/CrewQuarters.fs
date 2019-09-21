module Cards.CrewQuarters

open App.Msg
open Bulma.Form
open Cards.Common
open Model.Measures
open Ship

open Nerds.DeployTimeNerd
open Nerds.PriceTotalNerd
open Nerds.SizeFloatNerd

let render (ship: Ship) dispatch =
    let header =
        [
            Name "Crew Quarters"
            Nerd { TotalBuildCost = ship.CrewQuartersBuildCost }
            Nerd { Count = 1<comp>; Size = ship.CrewQuartersSize*1.0</comp> }
            Nerd { DeployTime = ship.DeployTime }
        ]
    let form =
        [ HorGrp (None,
                  [ FltInp ({ Label = Some "Deployment Time"; Value = ship.DeployTime*1.0</mo> },
                            (fun n -> Msg.ReplaceShip { ship with DeployTime = n*1.0<mo> } |> dispatch)
                           )
                    IntInp ({ Label = Some "Spare Crew Berths"; Value = ship.SpareBerths*1</people>; Min = Some 0; Max = None },
                            (fun n -> Msg.ReplaceShip { ship with SpareBerths = n*1<people> } |> dispatch)
                           )
                  ]
                 )
        ]
        |> Bulma.Form.render
    let actions = []
    shipComponentCard header form actions
