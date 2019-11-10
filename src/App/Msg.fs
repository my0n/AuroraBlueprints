module App.Msg

open Global

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

    // Component Designs
    | NewComponentDesign of ShipComponent
    | RemoveComponentDesign of ShipComponent
    | ReplaceComponentDesign of ShipComponent

    // Components
    | SaveComponentToDesigns of ShipComponent
    | CopyComponentToShip of Ship * ShipComponent
    | RemoveComponentFromShip of Ship * ShipComponent
    | ReplaceShipComponent of Ship * ShipComponent

    // Technology
    | AddTechnology of GameObjectId
    | RemoveTechnology of GameObjectId
