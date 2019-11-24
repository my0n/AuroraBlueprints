module Cards.CrewQuarters

open App.Msg
open App.Model
open Bulma.FC
open Cards.Common
open Model.Measures
open Comp.Ship

open Nerds.DeployTimeNerd
open Nerds.PriceTotalNerd
open Nerds.SizeNerd

let render (model: App.Model.Model) (ship: Ship) dispatch =
    let key = ship.Id.ToString() + "crewquarters"
    let expanded = model |> Model.isExpanded key

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
    shipComponentCard key header form actions expanded dispatch
