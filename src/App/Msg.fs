module App.Msg

open Global

open Model.Measures

open Comp.Ship
open Comp.ShipComponent

type Msg =
    | Noop

    // Initialization
    | InitializeGame of Technology.AllTechnologies * Model.GameInfo.GameInfo
    | ApplyPreset of string

    // Ships
    | NewShip
    | RemoveShip of Ship
    | ReplaceShip of Ship
    | SelectShip of Ship
    | ShipUpdateName of Ship * string
    | ShipUpdateClass of Ship * string

    // Components
    | AddComponentToShip of Ship * ShipComponent
    | CopyComponentToShip of Ship * ShipComponent
    | UpdateComponentInShip of Ship * ShipComponent
    | RemoveComponentFromShip of Ship * ShipComponent
    | SetComponentCount of Ship * ShipComponent * int<comp>
    | UpdateComponent of ShipComponent
    | RemoveComponent of ShipComponent
    | LockComponent of ShipComponent
    | UnlockComponent of ShipComponent

    // Technology
    | AddTechnology of GameObjectId
    | RemoveTechnology of GameObjectId
