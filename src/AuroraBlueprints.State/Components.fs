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
        let comp' = comp.duplicate
        { model with
            AllComponents = model.AllComponents.Add(comp.Id, comp)
            PendingSaves = model.PendingSaves @ [ SetComponent comp ]
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
