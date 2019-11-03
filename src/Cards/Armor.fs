module Cards.Armor

open Global
open App.Msg
open Cards.Common
open Comp.Ship

open Model.Measures

open Nerds.ArmorSizeNerd
open Nerds.ArmorStrengthNerd
open Nerds.PriceTotalNerd
open Nerds.SizeNerd

open Technology

let render (allTechs: AllTechnologies) (tech: GameObjectId list) (ship: Ship) dispatch =
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
                boundTechField tech
                    "Armor Technology"
                    allTechs.Armor
                    ship.ArmorTechnology
                    (fun n -> Msg.ReplaceShip { ship with ArmorTechnology = n } |> dispatch)
            ]
    let actions = []
    shipComponentCard "armor" header [ form ] actions
