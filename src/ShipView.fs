module Ships.View

open Fable.Helpers.React
open Fable.Helpers.React.Props

open Types
open Global
open TableCommon
open SelectableList

let gridOptions =
    {
        Columns = [ "Name"; "Weight"; "" ]
        RowRenderer = (fun ship ->
            [
                String ship.Name
                Number (ship.Weight, { Precision = 2 })
                Button { Text = "Delete"; OnClick = Msg.RemoveShip ship.Guid }
            ]
        )
        OnSelect = fun ship -> Msg.SelectShip ship
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

let shipInfo ship =
    match ship with
    | None ->
        [ div [ ClassName "title is-4" ] [ str "No ship selected." ] ]
    | Some ship ->
        let shipComponents =
            ship.Components
            |> List.map (fun comp ->
                match comp with
                | FuelStorage ->
                    fuelStorage comp
            )

        [ div [ ClassName "title is-4" ] [ str (ship.Guid.ToString()) ] ]
        @ shipComponents

let root model dispatch =
    let ships = Map.values model.Ships

    div [ ClassName "columns" ]
        [
            div [ ClassName "column is-3" ]
                [ selectableList gridOptions dispatch ships model.CurrentShip ]
            div [ ClassName "column" ]
                (
                  [ actionBar dispatch ]
                  @+ div [ ClassName "content" ] (shipInfo model.CurrentShip)
                )
        ]
