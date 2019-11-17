module Ships.View

open Fable.React
open Fable.React.Props

open Global

open Model.Measures
open App.Model
open App.Msg
open Bulma.Table
open Comp.ShipComponent
open Comp.Ship

let actionBar dispatch =
    div []
        [ Bulma.Button.render "New Ship" (fun _ -> Msg.NewShip |> dispatch)
        ]

let shipInfo dispatch allTechs tech ship =
    match ship with
    | None ->
        div [ ClassName "title is-4" ] [ str "No ship selected." ]
    | Some ship ->
        let shipComponents =
            ship.Components
            |> Map.values
            |> List.map (fun (count, comp) ->
                match comp with
                | Bridge comp         -> Cards.Bridge.render ship count comp dispatch
                | CargoHold comp      -> Cards.CargoHold.render allTechs tech ship comp dispatch
                | Engine comp         -> Cards.Engine.render allTechs tech ship count comp dispatch
                | FuelStorage comp    -> Cards.FuelStorage.render allTechs tech ship comp dispatch
                | Magazine comp       -> Cards.Magazine.render allTechs tech ship count comp dispatch
                | PowerPlant comp     -> Cards.PowerPlant.render allTechs tech ship count comp dispatch
                | Sensors comp        -> Cards.Sensors.render allTechs tech ship comp dispatch
                | TroopTransport comp -> Cards.TroopTransport.render allTechs tech ship comp dispatch
            )

        [
            Cards.Classification.render ship dispatch
            Cards.Armor.render allTechs tech ship dispatch
            Cards.CrewQuarters.render ship dispatch
        ]
        @ shipComponents
        @ [ Cards.ShipDescription.render ship ]
        |> ofList

let root model dispatch =
    let ships = model.AllShips |> Map.values
    let comps = model.AllComponents |> Map.values

    let removeShip ship = Msg.RemoveShip ship |> dispatch
    let selectShip ship = Msg.SelectShip ship |> dispatch

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

    div [ ClassName "columns" ]
        [
            div [ ClassName "column is-2" ]
                [ Bulma.Table.render shipListOptions ships model.CurrentShip selectShip ]
            div [ ClassName "column is-8" ]
                (
                  [ actionBar dispatch ]
                  @+ div [ ClassName "content" ] [ shipInfo dispatch model.AllTechnologies model.CurrentTechnology model.CurrentShip ]
                )
            div [ ClassName "column" ]
                [ Tables.ComponentTable.render comps model dispatch ]
        ]
