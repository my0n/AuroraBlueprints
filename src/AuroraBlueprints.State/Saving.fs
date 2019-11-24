module State.Saving

open Comp.Ship
open Comp.ShipComponent
open Global

open Model.Technology

type Storage =
    {
        LoadComponents: AllTechnologies -> ShipComponent list
        SaveComponent: ShipComponent -> unit
        DeleteComponent: ShipComponent -> unit
        LoadShips: AllTechnologies -> Map<GameObjectId, ShipComponent> -> Ship list
        SaveShip: Ship -> unit
        DeleteShip: Ship -> unit
        LoadCurrentTechnology: unit -> GameObjectId list
        SaveCurrentTechnology: GameObjectId list -> unit
    }
