module Ships.View

open Fable.Helpers.React
open Fable.Helpers.React.Props

open Types
open Global
open TableCommon
open SelectableList
open ShipComponent

let shipListOptions: SelectableListOptions<Ship> =
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
            let name = comp.Name

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
                | FuelStorage comp -> ShipComponents.FuelStorage.render comp dispatch
                | Engine comp ->      ShipComponents.Engine.render comp dispatch
                | Bridge comp ->      ShipComponents.Bridge.render comp dispatch
            )

        [ ShipComponents.Classification.render ship dispatch ]
        @ shipComponents
        @+ ShipComponents.ShipDescription.render ship

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
