module App.Msg

open System

open Comp.Ship
open Comp.ShipComponent

type Msg =
    | Noop

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
    | RegisterTechnologies of Technology.TechBase list
    | AddTechnology of Guid
    | RemoveTechnology of Guid
