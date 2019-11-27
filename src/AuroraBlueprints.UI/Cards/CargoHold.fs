module Cards.CargoHold

open State.Model
open State.UI
open Cards.Common
open Comp.CargoHold
open Comp.ShipComponent
open Comp.Ship

open Model.Measures

open Nerds.CargoCapacityNerd
open Nerds.PriceTotalNerd
open Nerds.SizeNerd
open Nerds.TractorStrengthNerd

let render (comp: CargoHold) (model: State.Model.Model) (ship: Ship) dispatch =
    let currentTech = model.CurrentTechnology
    let allTechs = model.AllTechnologies
    let key = ship.Id.ToString() + comp.Id.ToString()
    let expanded = model |> Model.isExpanded key

    let header =
        [ Name "Cargo Hold"
          Nerd { TotalBuildCost = comp.BuildCost * 1<comp> }
          Nerd
              { RenderMode = HS
                Count = 1<comp>
                Size = comp.Size }
          Nerd { CargoCapacity = comp.CargoCapacity * 1<comp> }
          Nerd
              { TractorStrength = comp.TractorStrength * 1<comp>
                LoadTime = ship.LoadTime } ]

    let form =
        [ Bulma.FC.HorizontalGroup None
              (techCountFields
                  { Values = comp.CargoHolds
                    CurrentTech = currentTech
                    AllTechs = allTechs.CargoHolds
                    GetName = fun n -> n.Name
                    OnChange = fun tech n -> CargoHold { comp with CargoHolds = comp.CargoHolds.Add(tech, n) } }
                   dispatch)
          Bulma.FC.HorizontalGroup None
              (techCountFields
                  { Values = comp.CargoHandlingSystems
                    CurrentTech = currentTech
                    AllTechs = allTechs.CargoHandling
                    GetName = fun n -> n.Name
                    OnChange =
                        fun tech n ->
                            CargoHold { comp with CargoHandlingSystems = comp.CargoHandlingSystems.Add(tech, n) } }
                   dispatch) ]

    let actions =
        [ removeButton
            { Ship = ship
              Component = CargoHold comp } dispatch ]

    shipComponentCard key header form actions expanded dispatch
