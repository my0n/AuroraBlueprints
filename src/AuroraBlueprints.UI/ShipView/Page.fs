module ShipView.Page

open Fable.React
open Fable.React.Props

open Global

open App.Model
open App.Msg
open Comp.ShipComponent
open Comp.Ship

let actionBar ship dispatch =
    Bulma.FC.HorizontalGroup
        None
        [
            Bulma.FC.AddonGroup
                [
                    Bulma.FC.Button
                        "New Ship"
                        Bulma.FC.ButtonOpts.Empty
                        (fun _ -> Msg.NewShip |> dispatch)
                    Bulma.FC.Button
                        "Clone Ship"
                        Bulma.FC.ButtonOpts.Empty
                        (fun _ -> 
                            match ship with
                            | None -> ()
                            | Some ship' ->
                                Msg.DuplicateShip ship'
                                |> dispatch
                        )
                    Bulma.FC.Spacer
                ]
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
    div [ ClassName "columns" ]
        [
            div [ ClassName "column is-3" ]
                [ SidePanel.render model dispatch ]
            div [ ClassName "column" ]
                (
                  [ actionBar model.CurrentShip dispatch ]
                  @+ div [ ClassName "content" ] [ shipInfo dispatch model.AllTechnologies model.CurrentTechnology model.CurrentShip ]
                )
        ]
