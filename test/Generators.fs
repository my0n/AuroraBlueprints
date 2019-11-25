module Tests.Generators

open Global
open Comp.ShipComponent
open Comp.Engine
open Model.Measures
open Model.Technology

//#region Technologies

let ``engine technologies`` =
    seq {
        EngineTech ({Id = GameObjectId.generate(); Cost = 1000<rp>; Level = 1; Name = "low tech engine"; Parents = []}, 0.2<ep/hs>)
        EngineTech ({Id = GameObjectId.generate(); Cost = 1000000<rp>; Level = 2; Name = "high tech engine"; Parents = []}, 100.0<ep/hs>)
    }

let ``power mod technologies`` =
    seq {
        0.5
        1.25
    }

let ``engine efficiency technologies`` =
    seq {
        EngineEfficiencyTech ({Id = GameObjectId.generate(); Cost = 1000<rp>; Level = 1; Name = "low tech engine eff"; Parents = []}, 1.0<l/ep/hr>)
        EngineEfficiencyTech ({Id = GameObjectId.generate(); Cost = 1000000<rp>; Level = 2; Name = "high tech engine eff"; Parents = []}, 0.1<l/ep/hr>)
    }

let ``thermal efficiency technologies`` =
    seq {
        EngineThermalTech ({Id = GameObjectId.generate(); Cost = 1000<rp>; Level = 1; Name = "low tech thermal eff"; Parents = []}, 1.0<therm/ep>, 1.0)
        EngineThermalTech ({Id = GameObjectId.generate(); Cost = 1000000<rp>; Level = 2; Name = "high tech therma eff"; Parents = []}, 0.01<therm/ep>, 4.0)
    }

//#endregion

//#region Components

let components =
    seq {
        ShipComponent.FuelStorage
            {
                Id = "fuel storage built in"
                Locked = false
                BuiltIn = true
                FuelStorages = Map.empty
            }

        ShipComponent.FuelStorage
            {
                Id = "fuel storage not built in"
                Locked = false
                BuiltIn = false
                FuelStorages = Map.empty
            }

        for engineTech in ``engine technologies`` do
        for powerModTech in ``power mod technologies`` do
        for engineEffTech in ``engine efficiency technologies`` do
        for thermalEffTech in ``thermal efficiency technologies`` do
        for engineSize in [1<hs/comp>; 50<hs/comp>] do
            let name =
                sprintf
                    "engine %d HS, %.1f ep/hs, x%.3f ep, x%.3f fuel, x%.3f therm"
                    engineSize
                    engineTech.PowerPerHs
                    powerModTech
                    engineEffTech.Efficiency
                    thermalEffTech.ThermalEfficiency
                |> String.replace "." ","

            ShipComponent.Engine
                {
                    Id = name
                    Locked = false
                    BuiltIn = false
                    Name = "Engine"
                    Manufacturer = "Aurora Industries"
                    EngineTech = engineTech
                    PowerModTech = powerModTech
                    EfficiencyTech = engineEffTech
                    ThermalEfficiencyTech = thermalEffTech
                    Size = engineSize
                }
            
    }

//#endregion

