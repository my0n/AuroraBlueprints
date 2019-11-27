module Cards.Magazine

open System

open Global

open State.Msg
open State.Model
open State.UI

open Cards.Common
open Model.Measures
open Comp.Ship
open Comp.ShipComponent
open Comp.Magazine

open Nerds.MagazineCapacityNerd
open Nerds.PriceNerd
open Nerds.SizeNerd

let render (comp: Magazine) (count: int<comp>) (model: State.Model.Model) (ship: Ship) dispatch =
    let currentTech = model.CurrentTechnology
    let allTechs = model.AllTechnologies
    let key = ship.Id.ToString() + comp.Id.ToString()
    let expanded = model |> Model.isExpanded key

    let header =
        [ Name comp.Name
          Nerd
              { Ammo = comp.Capacity
                Count = count }
          Nerd
              { BuildCost = comp.BuildCost
                Count = count }
          Nerd
              { RenderMode = HS
                Count = count
                Size = float2int <| int2float comp.Size * 50.0<ton/hs> } ]
    let form =
        [ Bulma.FC.HorizontalGroup None
              [ countField
                  { Value = count
                    Ship = ship
                    Component = Magazine comp } dispatch
                nameFields
                    { Name = comp.Name
                      Manufacturer = comp.Manufacturer
                      GeneratedName = comp.GeneratedName
                      OnNameChange = fun n -> Magazine { comp with Name = n }
                      OnManufacturerChange = fun n -> Magazine { comp with Manufacturer = n } } dispatch ]
          Bulma.FC.HorizontalGroup None
              [ compIntField
                  { Label = "HTK"
                    Value = comp.HTK
                    Min = Some 1
                    Max = Some 10
                    Disabled = false
                    OnChange = fun n -> Magazine { comp with HTK = n } } dispatch
                compIntField
                    { Label = "Size"
                      Value = comp.Size
                      Min = Some 1
                      Max = Some 30
                      Disabled = false
                      OnChange = fun n -> Magazine { comp with Size = n } } dispatch
                compTechField
                    { CurrentTech = currentTech
                      Label = "Armor"
                      Value = comp.Armor
                      Options = allTechs.Armor
                      GetName = fun t -> t.Name
                      OnChange = fun n -> Magazine { comp with Armor = n } } dispatch
                compTechField
                    { CurrentTech = currentTech
                      Label = "Feed System"
                      Value = comp.FeedSystem
                      Options = allTechs.MagazineEfficiency
                      GetName = fun t -> sprintf "x%.2f ammo" (t.AmmoDensity / 20.0)
                      OnChange = fun n -> Magazine { comp with FeedSystem = n } } dispatch
                compTechField
                    { CurrentTech = currentTech
                      Label = "Ejection"
                      Value = comp.Ejection
                      Options = allTechs.MagazineEjection
                      GetName = fun t -> String.Format("{0}% ejection chance", t.EjectionChance * 100.0)
                      OnChange = fun n -> Magazine { comp with Ejection = n } } dispatch ] ]

    let actions =
        [ "Remove", Bulma.Card.DangerColor,
          (fun _ -> State.Msg.RemoveComponentFromShip(ship, Magazine comp) |> dispatch) ]
    shipComponentCard key header form actions expanded dispatch
