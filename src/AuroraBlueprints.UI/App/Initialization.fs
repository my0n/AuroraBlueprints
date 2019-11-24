module App.Model.Initialization

open Global
open Model.Technology
open GameInfo

open Saving.LocalStorage

module Model =

    /// <returns>The updated model</returns>
    let initializeModel (allTechs: AllTechnologies) (gameInfo: GameInfo) model =
        let currentTechnology =
            load "ct" Saving.Technology.deserialize
            |> Seq.tryHead
            |> Option.defaultValue NotFound
            |> function
                | Success ct -> ct
                | NotFound -> []
                | Failure _ -> []

        let customComponents =
            load "comp" (Saving.Components.deserialize allTechs)
            |> Seq.map (function
                | Success comp -> Some comp
                | NotFound -> None
                | Failure _ -> None
            )
            |> Seq.choose id
            |> Seq.map (fun comp -> comp.Id, comp)
            |> Map.ofSeq

        let customShips =
            load "ship" (Saving.Ships.deserialize (Map.values customComponents) allTechs)
            |> Seq.map (function
                | Success ship -> Some ship
                | NotFound -> None
                | Failure _ -> None
            )
            |> Seq.choose id
            |> Seq.map (fun ship -> ship.Id, ship)
            |> Map.ofSeq

        { model with
            AllTechnologies = allTechs
            CurrentTechnology = currentTechnology
            AllComponents =
                Map.empty
                %+ Comp.ShipComponent.Bridge         ({ Comp.Bridge.Bridge.Zero with BuiltIn = true })
                %+ Comp.ShipComponent.CargoHold      ({ Comp.CargoHold.CargoHold.Zero with BuiltIn = true })
                %+ Comp.ShipComponent.Engine         ({ Comp.Engine.engine allTechs with Name = "Engine"; BuiltIn = true })
                %+ Comp.ShipComponent.FuelStorage    ({ Comp.FuelStorage.FuelStorage.Zero with BuiltIn = true })
                %+ Comp.ShipComponent.Magazine       ({ Comp.Magazine.magazine allTechs with Name = "Magazine"; BuiltIn = true })
                %+ Comp.ShipComponent.PowerPlant     ({ Comp.PowerPlant.powerPlant allTechs with Name = "Power Plant"; BuiltIn = true })
                %+ Comp.ShipComponent.Sensors        ({ Comp.Sensors.Sensors.Zero with BuiltIn = true })
                %+ Comp.ShipComponent.TroopTransport ({ Comp.TroopTransport.TroopTransport.Zero with BuiltIn = true })
                %@ customComponents
            AllShips = customShips
            Presets = gameInfo.Presets
            FullyInitialized = true
        }
