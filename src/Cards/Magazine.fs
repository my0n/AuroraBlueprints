module Cards.Magazine

open Cards.Common
open Model.Measures
open Comp.Ship
open Comp.ShipComponent
open Comp.Magazine

open Nerds.MagazineCapacityNerd
open Nerds.PriceNerd
open Nerds.SizeNerd

let render (tech: Set<Technology.Tech>) (ship: Ship) (comp: Magazine) dispatch =
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
                    boundTechField tech ship dispatch
                        "Armor"
                        Technology.armor
                        comp.Armor
                        (fun n -> Magazine { comp with Armor = n })
                    boundTechField tech ship dispatch
                        "Feed System"
                        Technology.feedEfficiency
                        comp.FeedSystem
                        (fun n -> Magazine { comp with FeedSystem = n })
                    boundTechField tech ship dispatch
                        "Ejection"
                        Technology.ejectionChance
                        comp.Ejection
                        (fun n -> Magazine { comp with Ejection = n })
                ]
        ]
    let actions = []
    shipComponentCard (comp.Guid.ToString ()) header form actions
