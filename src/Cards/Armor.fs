module Cards.Armor

open Global
open App.Msg
open Cards.Common
open Comp.Ship
open System

open Model.Measures

open Nerds.ArmorSizeNerd
open Nerds.ArmorStrengthNerd
open Nerds.PriceTotalNerd
open Nerds.SizeNerd

let render (tech: Set<Technology.Tech>) (ship: Ship) dispatch =
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
                Bulma.FC.IntInput
                    {
                        Label = Some "Armor Depth"
                        Value = ship.ArmorDepth
                        Min = Some 1
                        Max = None
                        Disabled = false
                    }
                    (fun n -> Msg.ReplaceShip { ship with ArmorDepth = n } |> dispatch)
                Bulma.FC.Select
                    {
                        Label = Some "Armor Technology"
                        Options =
                            Technology.armor
                            |> Map.mapKvp (fun k v ->
                                {|
                                    Key = k
                                    Text = String.Format("{0} ({1:0} strength/HS)", v.Name, v.Strength)
                                    Disallowed = not <| tech.Contains v.Tech
                                |}
                            )
                        Value = ship.ArmorTechnology.Level
                    }
                    (fun n -> Msg.ReplaceShip { ship with ArmorTechnology = Technology.armor.[n] } |> dispatch)
            ]
    let actions = []
    shipComponentCard "armor" header [ form ] actions
