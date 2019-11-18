module ShipView.SidePanel

open Global
open App.Model

let render model dispatch =
    let ships = model.AllShips |> Map.values
    let comps = model.AllComponents |> Map.values

    let tabContents =
        [
            ("Ships", fun _ -> Tables.ShipTable.render ships model dispatch)
            ("Components", fun _ -> Tables.ComponentTable.render comps model dispatch)
        ]
    Bulma.Tabs.container tabContents
