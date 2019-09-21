module Cards.Armor

open Global
open App.Msg
open Cards.Common
open Model.Measures
open Comp.Ship
open Model.Technology
open System

open Nerds.ArmorSizeNerd
open Nerds.ArmorStrengthNerd
open Nerds.PriceTotalNerd
open Nerds.SizeFloatNerd

let render (ship: Ship) dispatch =
    let header =
        [
            Name <| sprintf "%s Armor" ship.ArmorTechnology.Name
            Nerd { TotalBuildCost = ship.ArmorBuildCost }
            Nerd { Count = 1<comp>; Size = ship.ArmorSize*1.0</comp> }
            Nerd { Strength = ship.ArmorStrength }
            Nerd { Width = ship.ArmorWidth; Depth = ship.ArmorDepth }
        ]
    let form =
        Bulma.FC.HorizontalGroup
            None
            [
                Bulma.FC.IntInput
                    {
                        Label = Some "Armor Depth"
                        Value = ship.ArmorDepth
                        Min = Some 1
                        Max = None
                    }
                    (fun n -> Msg.ReplaceShip { ship with ArmorDepth = n } |> dispatch)
                Bulma.FC.Select
                    {
                        Label = Some "Armor Technology"
                        Options =
                            Technology.armor
                            |> Map.toListV (fun v -> String.Format("{0} ({1:0} strength/HS)", v.Name, v.Strength))
                        Value = ship.ArmorTechnology.Level
                    }
                    (fun n -> Msg.ReplaceShip { ship with ArmorTechnology = Technology.armor.[n] } |> dispatch)
            ]
    let actions = []
    shipComponentCard header (List.wrap form) actions
