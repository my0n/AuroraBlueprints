module Ships.View

open Fable.Helpers.React
open Fable.Helpers.React.Props

open Ships.Types
open Global
open TableCommon
open SelectableList

let gridOptions =
    {
        Columns = [ "Name"; "Weight" ]
        RowRenderer = (fun ship ->
            [
                String ship.Name
                Number (ship.Weight, { Precision = 2 })
            ]
        )
    }

let shipComponentCard header weight contents =
    div []
        [
            div [ClassName "is-4"] [str header]
        ]

let fuelStorage comp =
    shipComponentCard "Fuel Storage" 20 []

let shipInfo ship =
    let shipComponents =
        ship.Components
        |> List.map (fun comp ->
            match comp with
            | FuelStorage ->
                fuelStorage comp
        )

    div [ ClassName "content" ]
        (
          [ div [] [str ship.Name] ]
          @ shipComponents
        )

let root (model: Ships.Types.Model) =
    div [ ClassName "columns" ]
        [
            div [ ClassName "column is-3" ]
                [ selectableList gridOptions model.Ships model.CurrentShip ]
            div [ ClassName "column" ]
                (match model.CurrentShip with None -> [ div [ ClassName "title is-4" ] [str "No ship selected."] ] | Some ship -> [ shipInfo ship ])
        ]
