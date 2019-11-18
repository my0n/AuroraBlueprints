module Tables.ShipTable

open Global
open App.Model
open App.Msg
open Bulma.Table
open Fable.React
open Comp.Ship
open Model.Measures

let render (ships: Ship list) model dispatch =
    let removeShip ship = Msg.RemoveShip ship |> dispatch
    let selectShip ship = Msg.SelectShip ship |> dispatch

    let sortedShips =
        ships
        |> List.sortBy (fun ship -> ship.Name.ToLowerInvariant())

    let shipListOptions: ColumnOptions<Ship> list =
        [
            {
                Name = "Name"
                Render = fun ship -> str ship.Name
            }
            {
                Name = "Size (HS)"
                Render = fun ship -> str << sprintf "%.1f" << ton2hs <| int2float ship.Size
            }
            {
                Name = ""
                Render = fun ship ->
                    Bulma.FC.Button
                        "Remove"
                        Bulma.FC.ButtonOpts.Empty
                        (fun _ -> removeShip ship)
            }
        ]
            
    Bulma.Table.render shipListOptions sortedShips model.CurrentShip selectShip
