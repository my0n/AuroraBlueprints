module Model.Technology

open Model.Measures
open Global

type ArmorLevel = int
type EngineLevel = int
type EngineEfficiencyLevel = int
type PowerModLevel = int
type PowerBoostLevel = int
type ThermalEfficiencyLevel = int

type ArmorTech =
    {
        Level: ArmorLevel
        Name: string
        Strength: float<armorStrength/hs>
        DuraniumRatio: float
        NeutroniumRatio: float
    }

type EngineTech =
    {
        Level: EngineLevel
        Name: string
        PowerPerHs: float<ep/hs>
    }

type EngineEfficiencyTech =
    {
        Level: EngineEfficiencyLevel
        Efficiency: float<kl/hr/ep>
    }

type PowerModTech =
    {
        Level: PowerModLevel
        PowerMod: float
    }

type PowerBoostTech =
    {
        Level: PowerBoostLevel
        PowerBoost: float
        ExplosionChance: float
    }

type PowerPlantTech =
    {
        Level: PowerBoostLevel
        Name: string
        PowerOutput: float<power/hs>
    }

type ThermalEfficiencyTech =
    {
        Level: ThermalEfficiencyLevel
        Name: string
        ThermalEfficiency: float<therm/ep>
        CostMultiplier: float
    }

module Technology =
    let armor =
        [
            { Level = 0;  Name = "Conventional";          Strength = 2.0<armorStrength/hs>;  DuraniumRatio = 1.0; NeutroniumRatio = 0.0; }
            { Level = 1;  Name = "Duranium";              Strength = 5.0<armorStrength/hs>;  DuraniumRatio = 1.0; NeutroniumRatio = 0.0; }
            { Level = 2;  Name = "High Density Duranium"; Strength = 6.0<armorStrength/hs>;  DuraniumRatio = 1.0; NeutroniumRatio = 0.0; }
            { Level = 3;  Name = "Composite";             Strength = 8.0<armorStrength/hs>;  DuraniumRatio = 1.0; NeutroniumRatio = 0.0; }
            { Level = 4;  Name = "Ceramic Composite";     Strength = 10.0<armorStrength/hs>; DuraniumRatio = 0.9; NeutroniumRatio = 0.1; }
            { Level = 5;  Name = "Laminate Composite";    Strength = 12.0<armorStrength/hs>; DuraniumRatio = 0.8; NeutroniumRatio = 0.2; }
            { Level = 6;  Name = "Compressed Carbon";     Strength = 15.0<armorStrength/hs>; DuraniumRatio = 0.7; NeutroniumRatio = 0.3; }
            { Level = 7;  Name = "Biphase Carbide";       Strength = 18.0<armorStrength/hs>; DuraniumRatio = 0.6; NeutroniumRatio = 0.4; }
            { Level = 8;  Name = "Crystalline Composite"; Strength = 21.0<armorStrength/hs>; DuraniumRatio = 0.5; NeutroniumRatio = 0.5; }
            { Level = 9;  Name = "Superdense";            Strength = 25.0<armorStrength/hs>; DuraniumRatio = 0.4; NeutroniumRatio = 0.6; }
            { Level = 10; Name = "Bonded Superdense";     Strength = 30.0<armorStrength/hs>; DuraniumRatio = 0.3; NeutroniumRatio = 0.7; }
            { Level = 11; Name = "Coherent Superdense";   Strength = 36.0<armorStrength/hs>; DuraniumRatio = 0.2; NeutroniumRatio = 0.8; }
            { Level = 12; Name = "Collapsium";            Strength = 45.0<armorStrength/hs>; DuraniumRatio = 0.1; NeutroniumRatio = 0.9; }
        ]
        |> List.map (fun e -> e.Level, e) |> Map.ofSeq

    let engine =
        [
            { Level = 0;  Name = "Conventional";                PowerPerHs = 0.2<ep/hs> }
            { Level = 1;  Name = "Nuclear Thermal";             PowerPerHs = 5.0<ep/hs> }
            { Level = 2;  Name = "Nuclear Pulse";               PowerPerHs = 8.0<ep/hs> }
            { Level = 3;  Name = "Ion";                         PowerPerHs = 12.0<ep/hs> }
            { Level = 4;  Name = "Magneto-Plasma";              PowerPerHs = 16.0<ep/hs> }
            { Level = 5;  Name = "Internal Confinement Fusion"; PowerPerHs = 20.0<ep/hs> }
            { Level = 6;  Name = "Magnetic Confinement Fusion"; PowerPerHs = 25.0<ep/hs> }
            { Level = 7;  Name = "Inertial Confinement Fusion"; PowerPerHs = 32.0<ep/hs> }
            { Level = 8;  Name = "Solid-Core Anti-matter";      PowerPerHs = 40.0<ep/hs> }
            { Level = 9;  Name = "Gas-Core Anti-matter";        PowerPerHs = 50.0<ep/hs> }
            { Level = 10; Name = "Plasma-Core Anti-matter";     PowerPerHs = 60.0<ep/hs> }
            { Level = 11; Name = "Beam-Core Anti-matter";       PowerPerHs = 80.0<ep/hs> }
            { Level = 12; Name = "Photonic";                    PowerPerHs = 100.0<ep/hs> }
        ]
        |> List.map (fun e -> e.Level, e) |> Map.ofSeq

    let engineEfficiency =
        [
            { Level = 0;  Efficiency = 1.0<kl/hr/ep> }
            { Level = 1;  Efficiency = 0.9<kl/hr/ep> }
            { Level = 2;  Efficiency = 0.8<kl/hr/ep> }
            { Level = 3;  Efficiency = 0.7<kl/hr/ep> }
            { Level = 4;  Efficiency = 0.6<kl/hr/ep> }
            { Level = 5;  Efficiency = 0.5<kl/hr/ep> }
            { Level = 6;  Efficiency = 0.4<kl/hr/ep> }
            { Level = 7;  Efficiency = 0.3<kl/hr/ep> }
            { Level = 8;  Efficiency = 0.25<kl/hr/ep> }
            { Level = 9;  Efficiency = 0.2<kl/hr/ep> }
            { Level = 10; Efficiency = 0.16<kl/hr/ep> }
            { Level = 11; Efficiency = 0.125<kl/hr/ep> }
            { Level = 12; Efficiency = 0.1<kl/hr/ep> }
        ]
        |> List.map (fun e -> e.Level, e) |> Map.ofSeq

    let highPowerMod =
        [
            { Level = 0;  PowerMod = 1.0 }
            { Level = 1;  PowerMod = 1.05 }
            { Level = 2;  PowerMod = 1.1 }
            { Level = 3;  PowerMod = 1.15 }
            { Level = 4;  PowerMod = 1.2 }
            { Level = 5;  PowerMod = 1.25 }
            { Level = 6;  PowerMod = 1.3 }
            { Level = 7;  PowerMod = 1.35 }
            { Level = 8;  PowerMod = 1.4 }
            { Level = 9;  PowerMod = 1.45 }
            { Level = 10; PowerMod = 1.5 }
            { Level = 11; PowerMod = 1.55 }
            { Level = 12; PowerMod = 1.6 }
            { Level = 13; PowerMod = 1.65 }
            { Level = 14; PowerMod = 1.7 }
            { Level = 15; PowerMod = 1.75 }
            { Level = 16; PowerMod = 1.8 }
            { Level = 17; PowerMod = 1.85 }
            { Level = 18; PowerMod = 1.9 }
            { Level = 19; PowerMod = 1.95 }
            { Level = 20; PowerMod = 2.0 }
            { Level = 21; PowerMod = 2.05 }
            { Level = 22; PowerMod = 2.1 }
            { Level = 23; PowerMod = 2.25 }
            { Level = 24; PowerMod = 2.3 }
            { Level = 25; PowerMod = 2.35 }
            { Level = 26; PowerMod = 2.4 }
            { Level = 27; PowerMod = 2.45 }
            { Level = 28; PowerMod = 2.5 }
            { Level = 29; PowerMod = 2.55 }
            { Level = 30; PowerMod = 2.6 }
            { Level = 31; PowerMod = 2.65 }
            { Level = 32; PowerMod = 2.7 }
            { Level = 33; PowerMod = 2.75 }
            { Level = 34; PowerMod = 2.8 }
            { Level = 35; PowerMod = 2.85 }
            { Level = 36; PowerMod = 2.9 }
            { Level = 37; PowerMod = 2.95 }
            { Level = 38; PowerMod = 3.0 }
        ]
        |> List.map (fun e -> e.Level, e) |> Map.ofSeq

    let lowPowerMod =
        [
            { Level = -1;  PowerMod = 0.95 }
            { Level = -2;  PowerMod = 0.9 }
            { Level = -3;  PowerMod = 0.85 }
            { Level = -4;  PowerMod = 0.8 }
            { Level = -5;  PowerMod = 0.75 }
            { Level = -6;  PowerMod = 0.7 }
            { Level = -7;  PowerMod = 0.65 }
            { Level = -8;  PowerMod = 0.6 }
            { Level = -9;  PowerMod = 0.55 }
            { Level = -10; PowerMod = 0.5 }
            { Level = -11; PowerMod = 0.45 }
            { Level = -12; PowerMod = 0.4 }
            { Level = -13; PowerMod = 0.35 }
            { Level = -14; PowerMod = 0.3 }
            { Level = -15; PowerMod = 0.25 }
            { Level = -16; PowerMod = 0.2 }
            { Level = -17; PowerMod = 0.15 }
            { Level = -18; PowerMod = 0.1 }
        ]
        |> List.map (fun e -> e.Level, e) |> Map.ofSeq

    let allPowerMods =
        let al = highPowerMod |> Map.toList
        let bl = lowPowerMod |> Map.toList
        al @ bl |> Map.ofList

    let powerBoost =
        [
            { Level = 0; PowerBoost = 0.00; ExplosionChance = 0.00 }
            { Level = 1; PowerBoost = 0.05; ExplosionChance = 0.07 }
            { Level = 2; PowerBoost = 0.10; ExplosionChance = 0.10 }
            { Level = 3; PowerBoost = 0.15; ExplosionChance = 0.12 }
            { Level = 4; PowerBoost = 0.20; ExplosionChance = 0.16 }
            { Level = 5; PowerBoost = 0.25; ExplosionChance = 0.20 }
            { Level = 6; PowerBoost = 0.30; ExplosionChance = 0.25 }
            { Level = 7; PowerBoost = 0.40; ExplosionChance = 0.30 }
            { Level = 8; PowerBoost = 0.50; ExplosionChance = 0.35 }
        ]
        |> List.map (fun e -> e.Level, e) |> Map.ofSeq

    let powerPlant =
        [
            { Level = 0;  Name = "Pressurized Water Reactor";           PowerOutput = 2.0<power/hs> }
            { Level = 1;  Name = "Pebble Bed Reactor";                  PowerOutput = 3.0<power/hs> }
            { Level = 2;  Name = "Gas-Cooled Fast Reactor";             PowerOutput = 4.5<power/hs> }
            { Level = 3;  Name = "Stellarator Fusion Reactor";          PowerOutput = 6.0<power/hs> }
            { Level = 4;  Name = "Tokamak Fusion Reactor";              PowerOutput = 8.0<power/hs> }
            { Level = 5;  Name = "Magnetic Confinement Fusion Reactor"; PowerOutput = 10.0<power/hs> }
            { Level = 6;  Name = "Inertial Confinement Fusion Reactor"; PowerOutput = 12.0<power/hs> }
            { Level = 7;  Name = "Solid-core Anti-matter Power Plant";  PowerOutput = 16.0<power/hs> }
            { Level = 8;  Name = "Gas-core Anti-matter Power Plant";    PowerOutput = 20.0<power/hs> }
            { Level = 9;  Name = "Plasma-core Anti-matter Power Plant"; PowerOutput = 24.0<power/hs> }
            { Level = 10; Name = "Beam Core Anti-matter Power Plant";   PowerOutput = 32.0<power/hs> }
            { Level = 11; Name = "Vacuum Energy Power Plant";           PowerOutput = 40.0<power/hs> }
        ]
        |> List.map (fun e -> e.Level, e) |> Map.ofSeq

    let thermalEfficiency =
        [
            { Level = 0;  ThermalEfficiency = 1.00<therm/ep>; Name = ""; CostMultiplier = 1.00 }
            { Level = 1;  ThermalEfficiency = 0.75<therm/ep>; Name = ""; CostMultiplier = 1.25 }
            { Level = 2;  ThermalEfficiency = 0.50<therm/ep>; Name = ""; CostMultiplier = 1.50 }
            { Level = 3;  ThermalEfficiency = 0.35<therm/ep>; Name = ""; CostMultiplier = 1.75 }
            { Level = 4;  ThermalEfficiency = 0.25<therm/ep>; Name = ""; CostMultiplier = 2.00 }
            { Level = 5;  ThermalEfficiency = 0.16<therm/ep>; Name = ""; CostMultiplier = 2.25 }
            { Level = 6;  ThermalEfficiency = 0.12<therm/ep>; Name = ""; CostMultiplier = 2.50 }
            { Level = 7;  ThermalEfficiency = 0.08<therm/ep>; Name = ""; CostMultiplier = 2.75 }
            { Level = 8;  ThermalEfficiency = 0.06<therm/ep>; Name = ""; CostMultiplier = 3.00 }
            { Level = 9;  ThermalEfficiency = 0.04<therm/ep>; Name = ""; CostMultiplier = 3.25 }
            { Level = 10; ThermalEfficiency = 0.03<therm/ep>; Name = ""; CostMultiplier = 3.50 }
            { Level = 11; ThermalEfficiency = 0.02<therm/ep>; Name = ""; CostMultiplier = 3.75 }
            { Level = 12; ThermalEfficiency = 0.01<therm/ep>; Name = ""; CostMultiplier = 4.00 }
        ]
        |> List.map (fun te ->
            { te with
                Name = sprintf "Thermal Reduction: Signature %d%% Normal" <| int (te.ThermalEfficiency * 100.0)
            }
        )
        |> List.map (fun e -> e.Level, e) |> Map.ofSeq
