module State.Ships

open Global

open Comp.Ship
open Comp.ShipComponent
open Model.Names
open Model.Measures
open State.Model

module Model =

    /// <returns>The updated model</returns>
    let setCurrentShip ship model =
        { model with
            CurrentShip =
                model.AllShips
                |> Map.tryFind ship.Id
        }

    /// <returns>
    /// * The updated model
    /// * The newly created ship
    /// </returns>
    let createNewShip model =
        let ship = Comp.Ship.ship model.AllTechnologies
        { model with
            AllShips = model.AllShips %+ ship
            PendingSaves = model.PendingSaves @ [ SetShip ship ]
        },
        ship

    /// <returns>
    /// * The updated model
    /// * The newly duplicated ship
    /// </returns>
    let duplicateShip ship model =
        let ship' =
            { ship with
                Id = GameObjectId.generate ()
                Name = nameOfCopy ship.Name
            }
        { model with
            AllShips = model.AllShips %+ ship'
            PendingSaves = model.PendingSaves @ [ SetShip ship' ]
        },
        ship'

    /// <returns>The updated model</returns>
    let removeShip ship model =
        let orNoneIf pred inp =
            inp |> Option.bind (fun a -> match pred a with true -> None | false -> inp)

        { model with
            AllShips = model.AllShips %- ship
            CurrentShip = model.CurrentShip |> orNoneIf (fun s -> s.Id = ship.Id)
            PendingSaves = model.PendingSaves @ [ RemoveShip ship ]
        }

    /// <returns>The updated model</returns>
    let replaceShip ship model =
        let orOtherIf pred other inp =
            inp |> Option.bind (fun a -> match pred a with true -> Some other | false -> inp)

        { model with
            AllShips = model.AllShips %+ ship
            CurrentShip = model.CurrentShip |> orOtherIf (fun s -> s.Id = ship.Id) ship
            PendingSaves = model.PendingSaves @ [ SetShip ship ]
        }

    /// <returns>Whether the component can be added to the ship</returns>
    let canAddComponentToShip (comp: ShipComponent) ship =
        not comp.BuiltIn

    /// <returns>
    /// * The model
    /// * The current count of the given component
    /// </returns>
    let getComponentCount (comp: ShipComponent) (ship: Ship) model =
        model,
        ship.Components
        |> Map.tryFind comp.Id
        |> Option.map (fun (count, _) -> count)
        |> Option.defaultValue 0<comp>

    /// <returns>
    /// * The updated model
    /// * The updated ship
    /// </returns>
    let setComponentCount count (comp: ShipComponent) ship model =
        if canAddComponentToShip comp ship then
            let count' =
                match comp.Composite with
                | true -> 1<comp>
                | false -> count

            let ship' =
                { ship with
                    Components =
                        ship.Components.Add(
                            comp.Id,
                            (count', comp)
                        )
                }

            replaceShip ship' model, ship'
        else
            model, ship
    
    /// <returns>
    /// * The updated model
    /// * A bool indicating whether the ship was updated or not
    /// </returns>
    let updateComponentInShip (comp: ShipComponent) ship model =
        ship.Components
        |> Map.tryFind comp.Id
        |> function
        | None -> model, false
        | Some (count, _) ->
            replaceShip
                { ship with
                    Components =
                        ship.Components.Add(
                            comp.Id,
                            (count, comp)
                        )
                }
                model
            , true

    /// <returns>
    /// * The updated model
    /// * The list of updated ships
    /// </returns>
    let reloadComponentInShips (comp: ShipComponent) model =
        model.AllShips
        |> Map.values
        |> List.fold (fun (model', updatedShips) ship ->
            let (model'', shipUpdated) = updateComponentInShip comp ship model'
            (model'', if shipUpdated then ship::updatedShips else updatedShips)
        ) (model, [])

    /// <returns>
    /// * The updated model
    /// * The updated ship
    /// * A bool indicating whether the component was removed due to a cleanup or not
    /// </returns>
    let removeComponentFromShip (comp: ShipComponent) ship model =
        let ship' =
            { ship with
                Components =
                    ship.Components.Remove comp.Id
            }
            
        replaceShip ship' model,
        (ship', not comp.Locked && comp.Composite)
