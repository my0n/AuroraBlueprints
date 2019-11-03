module Cards.Magazine

open Global

open Cards.Common
open Model.Measures
open Comp.Ship
open Comp.ShipComponent
open Comp.Magazine

open Nerds.MagazineCapacityNerd
open Nerds.PriceNerd
open Nerds.SizeNerd

open Technology

let render (allTechs: AllTechnologies) (tech: GameObjectId list) (ship: Ship) (comp: Magazine) dispatch =
    let header =
        [
            Name comp.Name
            Nerd { BuildCost = comp.BuildCost; Count = comp.Count }
            Nerd { RenderMode = HS; Count = comp.Count; Size = float2int <| int2float comp.Size * 50.0<ton/hs> }
        ]
    let form =
        [
            Bulma.FC.HorizontalGroup
                None
                [
                    boundIntField ship dispatch
                        "Count"
                        (Some 0, None)
                        comp.Count
                        (fun n -> Magazine { comp with Count = n })
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
                    boundTechField tech
                        "Armor"
                        allTechs.Armor
                        comp.Armor
                        (fun n -> App.Msg.ReplaceShipComponent (ship, Magazine { comp with Armor = n }) |> dispatch)
                    boundTechField tech
                        "Feed System"
                        allTechs.MagazineEfficiency
                        comp.FeedSystem
                        (fun n -> App.Msg.ReplaceShipComponent (ship, Magazine { comp with FeedSystem = n }) |> dispatch)
                    boundTechField tech
                        "Ejection"
                        allTechs.MagazineEjection
                        comp.Ejection
                        (fun n -> App.Msg.ReplaceShipComponent (ship, Magazine { comp with Ejection = n }) |> dispatch)
                ]
        ]
    let actions = []
    shipComponentCard (comp.Id.ToString ()) header form actions
