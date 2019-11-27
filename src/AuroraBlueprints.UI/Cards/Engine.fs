module Cards.Engine

open Model.Measures

open Global
open System

open State.Msg
open State.UI
open State.Model
open Bulma.Card
open Cards.Common
open Comp.Engine
open Comp.ShipComponent
open Comp.Ship

open Nerds.MaintenanceClassNerd
open Nerds.PriceNerd
open Nerds.SizeNerd
open Nerds.EnginePowerNerd
open Nerds.FuelConsumptionNerd

let render (comp: Engine) (count: int<comp>) (model: State.Model.Model) (ship: Ship) dispatch =
    let currentTech = model.CurrentTechnology
    let allTechs = model.AllTechnologies
    let key = ship.Id.ToString() + comp.Id.ToString()
    let expanded = model |> Model.isExpanded key

    let header =
        [ Name comp.Name
          Nerd { MaintenanceClass = comp.MaintenanceClass }
          Nerd
              { Count = count
                BuildCost = comp.BuildCost }
          Nerd
              { Count = count
                RenderMode = HS
                Size = comp.Size * 50<ton/hs> }
          Nerd
              { Count = count
                EnginePower = comp.EnginePower
                Size = comp.Size * 50<ton/hs>
                Speed = ship.Speed }
          Nerd
              { Count = count
                Consumption = comp.FuelConsumption
                Efficiency = comp.FuelConsumption / comp.EnginePower } ]

    let form =
        [ Bulma.FC.HorizontalGroup None
              [ countField
                  { Value = count
                    Ship = ship
                    Component = Engine comp } dispatch
                nameFields
                    { Name = comp.Name
                      Manufacturer = comp.Manufacturer
                      GeneratedName = comp.GeneratedName
                      OnNameChange = fun n -> Engine { comp with Name = n }
                      OnManufacturerChange = fun n -> Engine { comp with Manufacturer = n } } dispatch ]
          Bulma.FC.HorizontalGroup None
              [ compIntField
                  { Label = "Size"
                    Value = comp.Size
                    Min = Some 0
                    Max = None
                    Disabled = false
                    OnChange = fun n -> Engine { comp with Size = n } } dispatch
                compTechField
                    { CurrentTech = currentTech
                      Label = "Engine Technology"
                      Value = comp.EngineTech
                      Options = allTechs.Engines
                      GetName = fun t -> String.Format("{0} EP - {1}", t.PowerPerHs, t.Name)
                      OnChange = fun n -> Engine { comp with EngineTech = n } } dispatch
                compTechField
                    { CurrentTech = currentTech
                      Label = "Fuel Consumption"
                      Value = comp.EfficiencyTech
                      Options = allTechs.EngineEfficiency
                      GetName = fun t -> sprintf "x%.3f fuel" t.Efficiency
                      OnChange = fun n -> Engine { comp with EfficiencyTech = n } } dispatch
                floatChoiceField
                    { Label = "Engine Power"
                      Value = comp.PowerModTech
                      Options = allTechs.AllPowerMods
                      AvailableOptions = allTechs.UnlockedPowerMods currentTech
                      GetName = fun n -> sprintf "x%.2f EP / x%.3f fuel" n (Math.Pow(n, 2.5))
                      OnChange = fun n -> Engine { comp with PowerModTech = n } } dispatch
                compTechField
                    { CurrentTech = currentTech
                      Label = "Thermal Efficiency"
                      Value = comp.ThermalEfficiencyTech
                      Options = allTechs.EngineThermalEfficiency
                      GetName = fun t -> sprintf "x%.2f therms" t.ThermalEfficiency
                      OnChange = fun n -> Engine { comp with ThermalEfficiencyTech = n } } dispatch ] ]

    let actions =
        [ removeButton
            { Ship = ship
              Component = Engine comp } dispatch ]

    shipComponentCard key header form actions expanded dispatch
