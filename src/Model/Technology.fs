module Model.Technology

open Model.Measures
open Global

type EngineLevel = int
type EngineEfficiencyLevel = int
type PowerModLevel = int

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

module Technology =
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
