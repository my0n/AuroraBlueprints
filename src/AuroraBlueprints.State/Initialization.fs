module State.Initialization

open Global
open Model.Technology
open State.Msg
open State.Model

open State.Saving

module Model =

    /// <returns>The updated model</returns>
    let initializeModel (allTechs: AllTechnologies) (presets: Preset list) (storage: Storage) model =
        let currentTechnology =
            storage.LoadCurrentTechnology ()

        let customComponents =
            storage.LoadComponents allTechs
            |> List.map (fun comp -> comp.Id, comp)
            |> Map.ofList

        let customShips =
            storage.LoadShips allTechs customComponents
            |> List.map (fun ship -> ship.Id, ship)
            |> Map.ofList

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
            Presets = presets
            FullyInitialized = true
        }
