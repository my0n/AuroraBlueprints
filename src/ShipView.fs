module Ships.View

open Fable.Helpers.React
open Fable.Helpers.React.Props

open Types
open Global
open TableCommon
open SelectableList
open InputComponents

let shipListOptions =
    {
        Columns = [ "Name"; "Weight"; "" ]
        RowRenderer = (fun ship ->
            [
                String ship.Name
                Number (ship.Weight, { Precision = 2 })
                Button { Text = "Delete"; OnClick = Msg.RemoveShip ship }
            ]
        )
        OnSelect = fun ship -> Msg.SelectShip ship
    }

let componentListOptions ship =
    {
        Columns = [ "Name"; "" ]
        RowRenderer = (fun (comp: ShipComponentDesign) ->
            let name =
                match comp.Spec with
                | FuelStorage _ -> "Fuel Storage"

            let onClick =
                match ship with
                | None -> Msg.Noop
                | Some ship -> Msg.CopyComponentToShip (ship, comp.Spec)

            [
                String name
                Button { Text = "Add"; OnClick = onClick }
            ]
        )
        OnSelect = fun comp -> Msg.Noop
    }

let shipComponentCard header weight contents =
    div []
        [
            div [ ClassName "is-4" ] [str header]
        ]

let fuelStorage comp =
    shipComponentCard "Fuel Storage" 20 []

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
            |> List.map (fun comp ->
                match comp with
                | FuelStorage fs ->
                    fuelStorage fs
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
