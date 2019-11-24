module Cards.Armor

open App.Model.UI
open App.Msg
open Cards.Common
open Comp.Ship

open Model.Measures

open Nerds.ArmorSizeNerd
open Nerds.ArmorStrengthNerd
open Nerds.PriceTotalNerd
open Nerds.SizeNerd

let render (model: App.Model.Model) (ship: Ship) dispatch =
    let currentTech = model.CurrentTechnology
    let allTechs = model.AllTechnologies
    let key = ship.Id.ToString() + "armor"
    let expanded = model |> Model.isExpanded key

    let header =
        [
            Name ship.ArmorTechnology.Name
            Nerd { TotalBuildCost = ship.ArmorBuildCost }
            Nerd { RenderMode = HS; Count = 1<comp>; Size = ship.ArmorSize*1</comp> }
            Nerd { Strength = ship.ArmorStrength }
            Nerd { Width = ship.ArmorWidth; Depth = ship.ArmorDepth }
        ]
    let form =
        Bulma.FC.HorizontalGroup
            None
            [
                boundShipIntField dispatch
                    "Armor Depth"
                    (Some 1, None)
                    ship.ArmorDepth
                    (fun n -> { ship with ArmorDepth = n })
                boundTechField currentTech
                    "Armor Technology"
                    allTechs.Armor
                    (fun t -> t.Name)
                    ship.ArmorTechnology
                    (fun n -> Msg.ReplaceShip { ship with ArmorTechnology = n } |> dispatch)
            ]
    let actions = []
    shipComponentCard key header [ form ] actions expanded dispatch
