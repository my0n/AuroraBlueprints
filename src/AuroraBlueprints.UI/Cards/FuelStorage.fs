module Cards.FuelStorage

open Global

open State.Msg
open State.Model
open State.UI
open Bulma.Card
open Cards.Common
open Model.Measures
open Comp.FuelStorage
open Comp.ShipComponent
open Comp.Ship

open Nerds.PriceTotalNerd
open Nerds.SizeNerd
open Nerds.FuelCapacityNerd

let render (comp: FuelStorage) (model: State.Model.Model) (ship: Ship) dispatch =
    let currentTech = model.CurrentTechnology
    let allTechs = model.AllTechnologies
    let key = ship.Id.ToString() + comp.Id.ToString()
    let expanded = model |> Model.isExpanded key

    let header =
        [ Name "Fuel Storage"
          Nerd { TotalBuildCost = comp.BuildCost * 1<comp> }
          Nerd
              { RenderMode = HS
                Count = 1<comp>
                Size = comp.Size }
          Nerd { FuelCapacity = comp.FuelCapacity * 1.0<comp> } ]

    let form =
        [ Bulma.FC.HorizontalGroup None
              (techCountFields
                  { Values = comp.FuelStorages
                    CurrentTech = currentTech
                    AllTechs = allTechs.FuelStorages
                    GetName = fun n -> n.Name
                    OnChange = fun tech n -> FuelStorage { comp with FuelStorages = comp.FuelStorages.Add(tech, n) } }
                   dispatch) ]

    let actions =
        [ removeButton
            { Ship = ship
              Component = FuelStorage comp } dispatch ]

    shipComponentCard key header form actions expanded dispatch
