module Cards.Magazine

open System

open Global

open App.Msg
open App.Model

open Cards.Common
open Model.Measures
open Comp.Ship
open Comp.ShipComponent
open Comp.Magazine

open Nerds.MagazineCapacityNerd
open Nerds.PriceNerd
open Nerds.SizeNerd

let render (comp: Magazine) (count: int<comp>) (model: App.Model.Model) (ship: Ship) dispatch =
    let currentTech = model.CurrentTechnology
    let allTechs = model.AllTechnologies
    let key = ship.Id.ToString() + comp.Id.ToString()
    let expanded = model |> Model.isExpanded key

    let header =
        [
            Name comp.Name
            Nerd { Ammo = comp.Capacity; Count = count }
            Nerd { BuildCost = comp.BuildCost; Count = count }
            Nerd { RenderMode = HS; Count = count; Size = float2int <| int2float comp.Size * 50.0<ton/hs> }
        ]
    let form =
        [
            Bulma.FC.HorizontalGroup
                None
                [
                    boundCountField ship (Magazine comp) dispatch
                        "Count"
                        count
                    boundNameField ship dispatch
                        comp.Name
                        comp.GeneratedName
                        (fun n -> Magazine { comp with Name = n })
                    boundStringField ship dispatch
                        "Manufacturer"
                        comp.Manufacturer
                        (fun n -> Magazine { comp with Manufacturer = n })
                ]
            Bulma.FC.HorizontalGroup
                None
                [
                    boundIntField ship dispatch
                        "HTK"
                        (Some 1, Some 10)
                        comp.HTK
                        (fun n -> Magazine { comp with HTK = n })
                    boundIntField ship dispatch
                        "Size"
                        (Some 1, Some 30)
                        comp.Size
                        (fun n -> Magazine { comp with Size = n })
                    boundTechField currentTech
                        "Armor"
                        allTechs.Armor
                        (fun t -> t.Name)
                        comp.Armor
                        (fun n -> Msg.UpdateComponent (Magazine { comp with Armor = n }) |> dispatch)
                    boundTechField currentTech
                        "Feed System"
                        allTechs.MagazineEfficiency
                        (fun t -> sprintf "x%.2f ammo" (t.AmmoDensity / 20.0))
                        comp.FeedSystem
                        (fun n -> Msg.UpdateComponent (Magazine { comp with FeedSystem = n }) |> dispatch)
                    boundTechField currentTech
                        "Ejection"
                        allTechs.MagazineEjection
                        (fun t -> String.Format("{0}% ejection chance", t.EjectionChance * 100.0))
                        comp.Ejection
                        (fun n -> Msg.UpdateComponent (Magazine { comp with Ejection = n }) |> dispatch)
                ]
        ]
    let actions =
        [
            "Remove", Bulma.Card.DangerColor, (fun _ -> App.Msg.RemoveComponentFromShip (ship, Magazine comp) |> dispatch)
        ]
    shipComponentCard key header form actions expanded dispatch
