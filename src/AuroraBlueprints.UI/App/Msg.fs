module App.Msg

open Global
open Comp.Ship
open Comp.ShipComponent
open Model.Measures
open Model.Technology

type Msg =
    | Noop

    // Initialization
    | InitializeGame of AllTechnologies * GameInfo.GameInfo
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
