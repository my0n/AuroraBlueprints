module Ships.View

open Fable.Helpers.React
open Fable.Helpers.React.Props

open Global

open App.Model
open App.Msg
open Bulma.Button
open Bulma.Table
open Comp.ShipComponent
open Ship
open Fable.Import.React

let actionBar dispatch =
    div []
        [ Bulma.Button.render "New Ship" (fun _ -> Msg.NewShip |> dispatch)
        ]

let shipInfo dispatch ship =
    match ship with
    | None ->
        [ div [ ClassName "title is-4" ] [ str "No ship selected." ] ]
    | Some ship ->
        let shipComponents =
            ship.Components
            |> Map.values
            |> List.map (fun comp ->
                match comp with
                | FuelStorage comp -> Cards.FuelStorage.render ship comp dispatch
                | Engine comp      -> Cards.Engine.render ship comp dispatch
                | Bridge comp      -> Cards.Bridge.render ship comp dispatch
                | Sensors comp     -> Cards.Sensors.render ship comp dispatch
                | PowerPlant comp  -> Cards.PowerPlant.render ship comp dispatch
            )

        [ Cards.Classification.render ship dispatch
          Cards.Armor.render ship dispatch
          Cards.CrewQuarters.render ship dispatch
        ]
        @ shipComponents
        @ [ Cards.ShipDescription.render ship ]

let root model dispatch =
    let ships = model.AllShips |> Map.values
    let comps = model.AllComponents |> Map.values

    let removeShip ship = Msg.RemoveShip ship |> dispatch
    let selectShip ship = Msg.SelectShip ship |> dispatch
    let addComponent ship comp = Msg.CopyComponentToShip (ship, comp) |> dispatch

    let shipListOptions: ColumnOptions<Ship> list =
        [
            {
                Name = "Name"
                Value = String (fun ship -> ship.Name)
            }
            {
                Name = "Size (HS)"
                Value = String (fun ship -> sprintf "%.1f" ship.Size)
            }
            {
                Name = ""
                Value = Button ("Remove", removeShip)
            }
        ]

    let componentListOptions: ColumnOptions<ShipComponent> list =
        [
            {
                Name = "Name"
                Value = String (fun comp -> comp.Name)
            }
            {
                Name = ""
                Value = Button ("Add", fun comp ->
                    match model.CurrentShip with
                    | Some ship -> addComponent ship comp
                    | None -> ()
                )
            }
        ]

    div [ ClassName "columns" ]
        [
            div [ ClassName "column is-2" ]
                [ Bulma.Table.render shipListOptions ships model.CurrentShip selectShip ]
            div [ ClassName "column is-8" ]
                (
                  [ actionBar dispatch ]
                  @+ div [ ClassName "content" ] (shipInfo dispatch model.CurrentShip)
                )
            div [ ClassName "column" ]
                [ Bulma.Table.render componentListOptions comps None (fun _ -> ()) ]
        ]
