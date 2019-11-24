module ShipView.Page

open Fable.React
open Fable.React.Props

open Global

open State.Model
open State.Msg
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

let shipInfo dispatch model ship =
    match ship with
    | None ->
        div [ ClassName "title is-4" ] [ str "No ship selected." ]
    | Some ship ->
        let shipComponents =
            ship.Components
            |> Map.values
            |> List.map (fun (count, comp) ->
                (
                    match comp with
                    | Bridge comp         -> Cards.Bridge.render comp count
                    | CargoHold comp      -> Cards.CargoHold.render comp
                    | Engine comp         -> Cards.Engine.render comp count
                    | FuelStorage comp    -> Cards.FuelStorage.render comp
                    | Magazine comp       -> Cards.Magazine.render comp count
                    | PowerPlant comp     -> Cards.PowerPlant.render comp count
                    | Sensors comp        -> Cards.Sensors.render comp
                    | TroopTransport comp -> Cards.TroopTransport.render comp
                ) model ship dispatch
            )

        [
            Cards.Classification.render ship dispatch
            Cards.Armor.render model ship dispatch
            Cards.CrewQuarters.render model ship dispatch
        ]
        @ shipComponents
        @ [ Cards.ShipDescription.render ship dispatch ]
        |> ofList

let root model dispatch =
    div [ ClassName "columns" ]
        [
            div [ ClassName "column is-3" ]
                [ SidePanel.render model dispatch ]
            div [ ClassName "column" ]
                (
                  [ actionBar model.CurrentShip dispatch ]
                  @+ div [ ClassName "content" ] [ shipInfo dispatch model model.CurrentShip ]
                )
        ]
