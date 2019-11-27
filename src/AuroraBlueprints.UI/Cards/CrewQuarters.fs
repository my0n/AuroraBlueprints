module Cards.CrewQuarters

open State.UI
open Cards.Common
open Model.Measures
open Comp.Ship

open Nerds.DeployTimeNerd
open Nerds.PriceTotalNerd
open Nerds.SizeNerd

let render (model: State.Model.Model) (ship: Ship) dispatch =
    let key = ship.Id.ToString() + "crewquarters"
    let expanded = model |> Model.isExpanded key

    let header =
        [ Name "Crew Quarters"
          Nerd { TotalBuildCost = ship.CrewQuartersBuildCost }
          Nerd
              { RenderMode = HS
                Count = 1<comp>
                Size = ship.CrewQuartersSize * 1<1/comp> }
          Nerd { DeployTime = ship.DeployTime } ]
    let form =
        [ Bulma.FC.HorizontalGroup None
              [ shipFloatField
                  { Label = "Deployment Time"
                    Value = ship.DeployTime
                    OnChange = fun n -> { ship with DeployTime = n } } dispatch
                shipIntField
                    { Label = "Spare Crew Berths"
                      Value = ship.SpareBerths
                      Min = Some 0
                      Max = None
                      Disabled = false
                      OnChange = fun n -> { ship with SpareBerths = n } } dispatch ] ]

    let actions = []
    shipComponentCard key header form actions expanded dispatch
