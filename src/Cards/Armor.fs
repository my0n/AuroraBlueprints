module Cards.Armor

open App.Msg
open Bulma.Form
open Cards.Common
open Model.Measures
open Ship
open Model.Technology
open Global
open System

let render (ship: Ship) dispatch =
    let header =
        [
            Name <| sprintf "%s Armor" ship.ArmorTechnology.Name
            TotalPrice ship.ArmorBuildCost
            SizeFloat (1<comp>, ship.ArmorSize*1.0</comp>)
            ArmorStrength ship.ArmorStrength
            ArmorSize (ship.ArmorWidth, ship.ArmorDepth)
        ]
    let form =
        [ HorGrp (None,
                  [ IntInp ({ Label = Some "Armor Depth"; Value = ship.ArmorDepth; Min = Some 1; Max = None },
                            (fun n -> Msg.ReplaceShip { ship with ArmorDepth = n } |> dispatch)
                           )
                    Select ({ Label = Some "Armor Technology"; Options = Technology.armor |> List.map (fun v -> v.Level, String.Format("{0} ({1:0} strength/HS)", v.Name, v.Strength)); Value = ship.ArmorTechnology.Level },
                            (fun n -> Msg.ReplaceShip { ship with ArmorTechnology = Technology.armor.[n] } |> dispatch)
                           )
                  ]
                 )
        ]
        |> Bulma.Form.render
    let actions = []
    shipComponentCard header form actions
