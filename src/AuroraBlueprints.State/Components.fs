module State.Components

open Global

open Comp.Ship
open Comp.ShipComponent
open State.Model
open State.Ships

module Model =

    /// <returns>Whether the component can be deleted or not</returns>
    let canDeleteComponent (comp: ShipComponent) model =
        not comp.BuiltIn
        &&
        not (
            model.AllShips
            |> Map.values
            |> Seq.exists (fun ship ->
                ship.Components.ContainsKey comp.Id
            )
        )

    /// <returns>
    /// * The updated model
    /// * The newly duplicated component
    /// </returns>
    let duplicateComponent (comp: ShipComponent) model =
        let comp' =
            match comp with
            | Bridge c         -> Bridge { c with Id = GameObjectId.generate(); BuiltIn = false }
            | CargoHold c      -> CargoHold { c with Id = GameObjectId.generate(); BuiltIn = false }
            | Engine c         -> Engine { c with Id = GameObjectId.generate(); BuiltIn = false }
            | FuelStorage c    -> FuelStorage { c with Id = GameObjectId.generate(); BuiltIn = false }
            | Magazine c       -> Magazine { c with Id = GameObjectId.generate(); BuiltIn = false }
            | PowerPlant c     -> PowerPlant { c with Id = GameObjectId.generate(); BuiltIn = false }
            | Sensors c        -> Sensors { c with Id = GameObjectId.generate(); BuiltIn = false }
            | TroopTransport c -> TroopTransport { c with Id = GameObjectId.generate(); BuiltIn = false }
            
        { model with
            AllComponents = model.AllComponents.Add(comp'.Id, comp')
            PendingSaves = model.PendingSaves @ [ SetComponent comp' ]
        },
        comp'
        
    /// <returns>
    /// * The updated model
    /// * The list of updated ships
    /// </returns>
    let updateComponent (comp: ShipComponent) model =
        { model with
            AllComponents = model.AllComponents.Add(comp.Id, comp)
            PendingSaves = model.PendingSaves @ [ SetComponent comp ]
        }
        |> Model.reloadComponentInShips comp

    /// <returns>
    /// * The updated model
    /// * A bool indicating if the component was removed
    /// </returns>
    let removeComponent (comp: ShipComponent) model =
        if canDeleteComponent comp model then
            { model with
                AllComponents = model.AllComponents.Remove(comp.Id)
                PendingSaves = model.PendingSaves @ [ RemoveComponent comp ]
            },
            true
        else
            model,
            false
