module Cards.Sensors

open Global

open State.Msg
open State.Model
open State.UI

open Bulma.Card
open Cards.Common
open Comp.Sensors
open Comp.ShipComponent
open Comp.Ship

open Model.Measures

open Nerds.MaintenanceClassNerd
open Nerds.PriceTotalNerd
open Nerds.SizeNerd
open Nerds.SensorStrengthNerd

let render (comp: Sensors) (model: State.Model.Model) (ship: Ship) dispatch =
    let currentTech = model.CurrentTechnology
    let allTechs = model.AllTechnologies
    let key = ship.Id.ToString() + comp.Id.ToString()
    let expanded = model |> Model.isExpanded key

    let header =
        [ Name "Sensors"
          Nerd { MaintenanceClass = comp.MaintenanceClass }
          Nerd { TotalBuildCost = comp.BuildCost * 1<comp> }
          Nerd
              { RenderMode = HS
                Count = 1<comp>
                Size = comp.Size }
          Nerd
              { Geo = comp.GeoSensorRating * 1<comp>
                Grav = comp.GravSensorRating * 1<comp> } ]

    let form =
        [ Bulma.FC.HorizontalGroup None
              (techCountFields
                  { Values = comp.GeoSensors
                    CurrentTech = currentTech
                    AllTechs = allTechs.GeoSensors
                    GetName = fun n -> n.Name
                    OnChange = fun tech n -> Sensors { comp with GeoSensors = comp.GeoSensors.Add(tech, n) } } dispatch)
          Bulma.FC.HorizontalGroup None
              (techCountFields
                  { Values = comp.GravSensors
                    CurrentTech = currentTech
                    AllTechs = allTechs.GravSensors
                    GetName = fun n -> n.Name
                    OnChange = fun tech n -> Sensors { comp with GravSensors = comp.GravSensors.Add(tech, n) } }
                   dispatch) ]

    let actions =
        [ removeButton
            { Ship = ship
              Component = Sensors comp } dispatch ]

    shipComponentCard key header form actions expanded dispatch
