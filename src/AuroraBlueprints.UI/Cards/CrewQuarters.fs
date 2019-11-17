module Cards.CrewQuarters

open Global
open App.Msg
open Bulma.FC
open Cards.Common
open Model.Measures
open Comp.Ship

open Nerds.DeployTimeNerd
open Nerds.PriceTotalNerd
open Nerds.SizeNerd

let render (ship: Ship) dispatch =
    let header =
        [
            Name "Crew Quarters"
            Nerd { TotalBuildCost = ship.CrewQuartersBuildCost }
            Nerd { RenderMode = HS; Count = 1<comp>; Size = ship.CrewQuartersSize*1</comp> }
            Nerd { DeployTime = ship.DeployTime }
        ]
    let form =
        [
            Bulma.FC.HorizontalGroup
                None
                [
                    Bulma.FC.FloatInput
                        {
                            Label = Some "Deployment Time"
                            Value = ship.DeployTime
                        }
                        (fun n -> Msg.ReplaceShip { ship with DeployTime = n } |> dispatch)
                    Bulma.FC.IntInput
                        {
                            Label = Some "Spare Crew Berths"
                            Value = ship.SpareBerths
                            Min = Some 0
                            Max = None
                            Disabled = false
                        }
                        (fun n -> Msg.ReplaceShip { ship with SpareBerths = n } |> dispatch)
                ]
        ]
    let actions = []
    shipComponentCard "crewquarters" header form actions
