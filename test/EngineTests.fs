module Tests

open Expecto

open Model.Measures
open Model.Technology
open Comp.Engine

type EngineTestInfo =
    {
        Size: int<hs/comp>
        EngineTech: EngineTech
        PowerModTech: float
        EfficiencyTech: EngineEfficiencyTech
        ThermalEfficiencyTech: EngineThermalTech

        BuildPoints: float</comp>
        Crew: int<people/comp>
        Thermal: float<therm/comp>
        EP: float<ep/comp>
        Fuel: float<l/comp/hr>
    }

let emptyBasics = {Id = ""; Cost = 0<rp>; Level = 0; Name = ""; Parents = []}

let smallEngine            = 1<hs/comp>
let bigEngine              = 50<hs/comp>

let conventionalEngineTech = EngineTech (emptyBasics, 0.2<ep/hs>)
let photonicEngineTech     = EngineTech (emptyBasics, 100.0<ep/hs>)

let lowPowerMod            = 0.5
let highPowerMod           = 1.25

let lowEfficiency          = EngineEfficiencyTech (emptyBasics, 1.0<l/ep/hr>)
let highEfficiency         = EngineEfficiencyTech (emptyBasics, 0.9<l/ep/hr>)

let normalThermal          = EngineThermalTech (emptyBasics, 1.0<therm/ep>, 1.0)
let goodThermal            = EngineThermalTech (emptyBasics, 0.01<therm/ep>, 4.0)

let testEngineProperties testInfo =
    let engine =
        {
            Id = ""; Name = ""; Manufacturer = ""; BuiltIn = false; Locked = false
            Size = testInfo.Size
            EngineTech = testInfo.EngineTech
            PowerModTech = testInfo.PowerModTech
            EfficiencyTech = testInfo.EfficiencyTech
            ThermalEfficiencyTech = testInfo.ThermalEfficiencyTech
        }
    let name =
        sprintf
            "test engine calculations - %d HS, %.1f ep/hs, x%.3f ep, x%.3f fuel, x%.3f therm"
            testInfo.Size
            testInfo.EngineTech.PowerPerHs
            testInfo.PowerModTech
            testInfo.EfficiencyTech.Efficiency
            testInfo.ThermalEfficiencyTech.ThermalEfficiency
    testCase name <| fun _ ->
        Expect.floatClose
            Accuracy.high
            (float engine.BuildCost.BuildPoints)
            (float testInfo.BuildPoints)
            "build points not equal"
        Expect.equal
            engine.Crew
            testInfo.Crew
            "crew not equal"
        Expect.floatClose
            Accuracy.high
            (float engine.ThermalOutput)
            (float testInfo.Thermal)
            "thermal output not equal"
        Expect.floatClose
            Accuracy.high
            (float engine.EnginePower)
            (float testInfo.EP)
            "engine power not equal"
        Expect.floatClose
            { absolute = 5e-7; relative = 5e-4 } // medium-low accuracy, because of test case #8 (see below)
            (float engine.FuelConsumption)
            (float testInfo.Fuel)
            "fuel consumption not equal"

[<Tests>]
let tests =
    [
        (smallEngine, conventionalEngineTech, lowPowerMod,  lowEfficiency,  normalThermal), (5.0</comp>,    1<people/comp>,  0.1<therm/comp>,    0.1<ep/comp>,    0.0175<l/comp/hr>)
        (smallEngine, conventionalEngineTech, lowPowerMod,  lowEfficiency,  goodThermal  ), (5.0</comp>,    1<people/comp>,  0.001<therm/comp>,  0.1<ep/comp>,    0.0175<l/comp/hr>)
        (smallEngine, photonicEngineTech,     lowPowerMod,  lowEfficiency,  normalThermal), (12.5</comp>,   1<people/comp>,  50.0<therm/comp>,   50.0<ep/comp>,   8.75<l/comp/hr>)
        (smallEngine, photonicEngineTech,     lowPowerMod,  lowEfficiency,  goodThermal  ), (50.0</comp>,   1<people/comp>,  0.5<therm/comp>,    50.0<ep/comp>,   8.75<l/comp/hr>)
        (bigEngine,   conventionalEngineTech, lowPowerMod,  lowEfficiency,  normalThermal), (5.0</comp>,    25<people/comp>, 5.0<therm/comp>,    5.0<ep/comp>,    0.442<l/comp/hr>)
        (bigEngine,   photonicEngineTech,     lowPowerMod,  lowEfficiency,  normalThermal), (625.0</comp>,  25<people/comp>, 2500.0<therm/comp>, 2500.0<ep/comp>, 221.0<l/comp/hr>)
        (bigEngine,   conventionalEngineTech, highPowerMod, lowEfficiency,  normalThermal), (6.25</comp>,   62<people/comp>, 12.5<therm/comp>,   12.5<ep/comp>,   10.91875<l/comp/hr>)
        (bigEngine,   photonicEngineTech,     highPowerMod, lowEfficiency,  normalThermal), (3125.0</comp>, 62<people/comp>, 6250.0<therm/comp>, 6250.0<ep/comp>, 5458.75<l/comp/hr>) // test case #8
        (smallEngine, photonicEngineTech,     highPowerMod, highEfficiency, normalThermal), (62.5</comp>,   1<people/comp>,  125.0<therm/comp>,  125.0<ep/comp>,  194.5625<l/comp/hr>)
        (bigEngine,   photonicEngineTech,     highPowerMod, highEfficiency, normalThermal), (3125.0</comp>, 62<people/comp>, 6250.0<therm/comp>, 6250.0<ep/comp>, 4913.125<l/comp/hr>)
    ]
    |> List.map (fun ((a, b, c, d, e), (f, g, h, i, j)) ->
        {
            Size = a
            EngineTech = b
            PowerModTech = c
            EfficiencyTech = d
            ThermalEfficiencyTech = e

            BuildPoints = f
            Crew = g
            Thermal = h
            EP = i
            Fuel = j
        }
        |> testEngineProperties
    ) 
    |> testList "engine tests"