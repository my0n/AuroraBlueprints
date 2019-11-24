module Tables.ComponentTable

open State.Components
open State.Model
open State.Msg
open Bulma.Table
open Comp.ShipComponent
open Fable.React

let render (comps: ShipComponent list) model dispatch =
    let groupedComps =
        comps
        |> List.filter (fun comp ->
            comp.BuiltIn
            || comp.Locked
            || not comp.Composite
        )
        |> List.groupBy (function
            | comp when comp.BuiltIn -> "Built-In Components"
            | Engine _               -> "Engines"
            | Magazine _             -> "Magazines"
            | PowerPlant _           -> "Power Plants"
            | _                      -> "Other"
        )
        |> List.sortBy (fun (name, _) -> name)
        |> List.map (fun (name, comps) ->
            name,
            comps
            |> List.sortBy (fun comp ->
                comp.Name.ToLowerInvariant()
            )
        )

    let addComponent ship (comp: ShipComponent) =
        match comp.BuiltIn with
        | true -> Msg.CopyComponentToShip (ship, comp)
        | false -> Msg.AddComponentToShip (ship, comp)
        |> dispatch
    let deleteComponent comp = Msg.RemoveComponent comp |> dispatch

    groupedComps
    |> List.map (fun (name, comps) ->
        let componentListOptions: ColumnOptions<ShipComponent> list =
            [
                {
                    Name = name
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
                            (match Model.canDeleteComponent comp model with true -> Bulma.FC.ButtonOpts.Empty | false -> Bulma.FC.ButtonOpts.Disabled)
                            (fun _ -> deleteComponent comp)
                }
            ]
            
        Bulma.Table.render componentListOptions comps None ignore
    )
    |> fragment []
