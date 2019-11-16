module Tables.ComponentTable

open App.Model
open App.Msg
open Bulma.Table
open Comp.ShipComponent
open Fable.React

let render comps model dispatch =
    let addComponent ship (comp: ShipComponent) =
        match comp.BuiltIn with
        | true -> Msg.CopyComponentToShip (ship, comp)
        | false -> Msg.AddComponentToShip (ship, comp)
        |> dispatch
    let deleteComponent comp = Msg.RemoveComponent comp |> dispatch

    let componentListOptions: ColumnOptions<ShipComponent> list =
        [
            {
                Name = "Name"
                Render = fun comp -> str comp.Name
            }
            {
                Name = ""
                Render = fun comp ->
                    Bulma.FC.Button
                        "Add"
                        Bulma.FC.ButtonOpts.Empty
                        (
                            match model.CurrentShip with
                            | Some ship -> fun _ -> addComponent ship comp
                            | None -> id
                        )
            }
            {
                Name = ""
                Render = fun comp ->
                    Bulma.FC.Button
                        "Delete"
                        (match Model.canDeleteComponent model comp with true -> Bulma.FC.ButtonOpts.Empty | false -> Bulma.FC.ButtonOpts.Disabled)
                        (fun _ -> deleteComponent comp)
            }
        ]

    Bulma.Table.render componentListOptions comps None ignore
