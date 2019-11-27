module Cards.PowerPlant

open Model.Measures
open State.Msg
open State.Model
open State.UI
open Bulma.Card
open Cards.Common
open Comp.PowerPlant
open Comp.ShipComponent
open Comp.Ship
open System
open Global

open Nerds.PriceNerd
open Nerds.SizeNerd
open Nerds.PowerProductionNerd

let render (comp: PowerPlant) (count: int<comp>) (model: State.Model.Model) (ship: Ship) dispatch =
    let currentTech = model.CurrentTechnology
    let allTechs = model.AllTechnologies
    let key = ship.Id.ToString() + comp.Id.ToString()
    let expanded = model |> Model.isExpanded key

    let header =
        [ Name comp.Name
          Nerd
              { Count = count
                BuildCost = comp.BuildCost }
          Nerd
              { Count = count
                RenderMode = HS
                Size = float2int <| comp.Size * 50.0<ton/hs> }
          Nerd
              { Count = count
                PowerOutput = comp.Power } ]

    let sizeOptions = [ 0.1 .. 0.1 .. 0.9 ] @ [ 1.0 .. 1.0 .. 30.0 ] |> List.map (fun o -> o * 1.0<hs/comp>)

    let form =
        [ Bulma.FC.HorizontalGroup None
              [ countField
                  { Value = count
                    Ship = ship
                    Component = PowerPlant comp } dispatch
                nameFields
                    { Name = comp.Name
                      Manufacturer = comp.Manufacturer
                      GeneratedName = comp.GeneratedName
                      OnNameChange = fun n -> PowerPlant { comp with Name = n }
                      OnManufacturerChange = fun n -> PowerPlant { comp with Manufacturer = n } } dispatch ]
          Bulma.FC.HorizontalGroup None
              [ floatChoiceField
                  { Label = "Size"
                    Value = comp.Size
                    Options = sizeOptions
                    AvailableOptions = sizeOptions
                    GetName = fun n -> sprintf "%.1f" n
                    OnChange = fun n -> PowerPlant { comp with Size = n } } dispatch
                compTechField
                    { CurrentTech = currentTech
                      Label = "Power Plant Technology"
                      Value = comp.Technology
                      Options = allTechs.Reactors
                      GetName = fun t -> String.Format("{0} power - {1}", t.PowerOutput, t.Name)
                      OnChange = fun n -> PowerPlant { comp with Technology = n } } dispatch
                compTechField
                    { CurrentTech = currentTech
                      Label = "Power Boost"
                      Value = comp.PowerBoost
                      Options = allTechs.ReactorsPowerBoost
                      GetName = fun t -> t.Name
                      OnChange = fun n -> PowerPlant { comp with PowerBoost = n } } dispatch ] ]

    let actions =
        [ removeButton
            { Ship = ship
              Component = PowerPlant comp } dispatch ]

    shipComponentCard key header form actions expanded dispatch
