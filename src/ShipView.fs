module Ships.View

open Fable.Helpers.React
open Fable.Helpers.React.Props

open Types
open Global
open TableCommon
open SelectableList
open InputComponents
open ShipDescription

let shipListOptions =
    {
        Columns = [ "Name"; "Weight"; "" ]
        RowRenderer = (fun ship ->
            [
                String ship.Name
                Size ship.Size
                Button { Text = "Delete"; OnClick = Msg.RemoveShip ship }
            ]
        )
        OnSelect = fun ship -> Msg.SelectShip ship
    }

let componentListOptions ship =
    {
        Columns = [ "Name"; "" ]
        RowRenderer = (fun (comp: ShipComponent) ->
            let name =
                match comp with
                | FuelStorage _ -> "Fuel Storage"

            let onClick =
                match ship with
                | None -> Msg.Noop
                | Some ship -> Msg.CopyComponentToShip (ship, comp)

            [
                String name
                Button { Text = "Add"; OnClick = onClick }
            ]
        )
        OnSelect = fun comp -> Msg.Noop
    }

let actionButton name callback =
    p [ ClassName "control" ]
      [
          div [ ClassName "button"
                OnClick callback
              ]
              [ str name ]
      ]

let actionBar dispatch =
    div []
        [
            actionButton "New Ship" (fun event -> Msg.NewShip |> dispatch)
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
                | FuelStorage fs ->
                    ShipComponents.FuelStorage.fuelStorage fs dispatch
            )

        [ div [ ClassName "title is-4" ]
              [
                textInput {
                            Label = "Name"
                            OnChange = fun newName -> Msg.ShipUpdateName (ship, newName)
                          } dispatch ship.Name
              ]
        ]
        @ shipComponents
        @+ descriptionBox ship

let root model dispatch =
    let ships = Map.values model.AllShips

    div [ ClassName "columns" ]
        [
            div [ ClassName "column is-2" ]
                [ selectableList shipListOptions dispatch ships model.CurrentShip ]
            div [ ClassName "column is-8" ]
                (
                  [ actionBar dispatch ]
                  @+ div [ ClassName "content" ] (shipInfo dispatch model.CurrentShip)
                )
            div [ ClassName "column" ]
                [ selectableList (componentListOptions model.CurrentShip) dispatch (Map.values model.AllComponents) None ]
        ]
