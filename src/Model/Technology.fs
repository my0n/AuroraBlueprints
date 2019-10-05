module Model.Technology

open Model.Measures
open Global

type EngineLevel = int
type EngineEfficiencyLevel = int
type PowerModLevel = int
type PowerBoostLevel = int
type ThermalEfficiencyLevel = int

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

type ThermalEfficiencyTech =
    {
        Level: ThermalEfficiencyLevel
        Name: string
        ThermalEfficiency: float<therm/ep>
        CostMultiplier: float
    }

type TechCategory =
    | DefensiveSystems
    | LogisticsAndGroundCombat
    | PowerAndPropulsion
    | SensorsAndFireControl

type Tech =
    | GeologicalSurveySensors
    | ImprovedGeologicalSensors
    | AdvancedGeologicalSensors
    | PhasedGeologicalSensors
    | GravitationalSurveySensors
    | ImprovedGravitationalSensors
    | AdvancedGravitationalSensors
    | PhasedGravitationalSensors
    | ConventionalArmor
    | DuraniumArmor
    | HighDensityDuraniumArmor
    | CompositeArmor
    | CeramicCompositeArmor
    | LaminateCompositeArmor
    | CompressedCarbonArmor
    | BiphaseCarbideArmor
    | CrystallineCompositeArmor
    | SuperdenseArmor
    | BondedSuperdenseArmor
    | CoherentSuperdenseArmor
    | CollapsiumArmor
    | ImprovedCargoHandlingSystem
    | AdvancedCargoHandlingSystem
    | GravAssistedCargoHandlingSystem
    | PressurizedWaterReactor
    | PebbleBedReactor
    | GasCooledFastReactor
    | StellaratorFusionReactor
    | TokamakFusionReactor
    | MagneticConfinementFusionReactor
    | InertialConfinementFusionReactor
    | SolidcoreAntimatterPowerPlant
    | GascoreAntimatterPowerPlant
    | PlasmacoreAntimatterPowerPlant
    | BeamCoreAntimatterPowerPlant
    | VacuumEnergyPowerPlant

type TechType =
    | Armor of {| Strength: float<armorStrength/hs>; DuraniumRatio: float; NeutroniumRatio: float |}
    | CargoHandling of {| TractorStrength: float |}
    | Reactor of {| PowerOutput: float<power/hs> |}
    | SurveySensor

type TechNode =
    {
        Tech: Tech
        Name: string
        Cost: int<rp>
        Category: TechCategory
        Type: TechType
        Parents: Tech list
    }

let allTechnologies =
    [
        //#region Survey sensors
        {
            Tech = GeologicalSurveySensors
            Name = "Geological Survey Sensors"
            Cost = 1000<rp>
            Category = SensorsAndFireControl
            Type = SurveySensor
            Parents = []
        }
        {
            Tech = ImprovedGeologicalSensors
            Name = "Improved Geological Sensors"
            Cost = 10000<rp>
            Category = SensorsAndFireControl
            Type = SurveySensor
            Parents = [GeologicalSurveySensors]
        }
        {
            Tech = AdvancedGeologicalSensors
            Name = "Advanced Geological Sensors"
            Cost = 35000<rp>
            Category = SensorsAndFireControl
            Type = SurveySensor
            Parents = [ImprovedGeologicalSensors]
        }
        {
            Tech = PhasedGeologicalSensors
            Name = "Phased Geological Sensors"
            Cost = 100000<rp>
            Category = SensorsAndFireControl
            Type = SurveySensor
            Parents = [AdvancedGeologicalSensors]
        }
        {
            Tech = GravitationalSurveySensors
            Name = "Gravitational Survey Sensors"
            Cost = 2000<rp>
            Category = SensorsAndFireControl
            Type = SurveySensor
            Parents = []
        }
        {
            Tech = ImprovedGravitationalSensors
            Name = "Improved Gravitational Sensors"
            Cost = 10000<rp>
            Category = SensorsAndFireControl
            Type = SurveySensor
            Parents = [GravitationalSurveySensors]
        }
        {
            Tech = AdvancedGravitationalSensors
            Name = "Advanced Gravitational Sensors"
            Cost = 35000<rp>
            Category = SensorsAndFireControl
            Type = SurveySensor
            Parents = [ImprovedGravitationalSensors]
        }
        {
            Tech = PhasedGravitationalSensors
            Name = "Phased Gravitational Sensors"
            Cost = 100000<rp>
            Category = SensorsAndFireControl
            Type = SurveySensor
            Parents = [AdvancedGravitationalSensors]
        }
        //#endregion
        //#region Armor
        {
            Tech = ConventionalArmor
            Name = "Conventional Armor"
            Cost = 250<rp>
            Category = DefensiveSystems
            Type = Armor {| Strength = 2.0<armorStrength/hs>; DuraniumRatio = 1.0; NeutroniumRatio = 0.0 |}
            Parents = []
        }
        {
            Tech = DuraniumArmor
            Name = "Duranium Armor"
            Cost = 500<rp>
            Category = DefensiveSystems
            Type = Armor {| Strength = 5.0<armorStrength/hs>; DuraniumRatio = 1.0; NeutroniumRatio = 0.0 |}
            Parents = [ConventionalArmor]
        }
        {
            Tech = HighDensityDuraniumArmor
            Name = "High Density Duranium Armor"
            Cost = 2500<rp>
            Category = DefensiveSystems
            Type = Armor {| Strength = 6.0<armorStrength/hs>; DuraniumRatio = 1.0; NeutroniumRatio = 0.0 |}
            Parents = [DuraniumArmor]
        }
        {
            Tech = CompositeArmor
            Name = "Composite Armor"
            Cost = 5000<rp>
            Category = DefensiveSystems
            Type = Armor {| Strength = 8.0<armorStrength/hs>; DuraniumRatio = 1.0; NeutroniumRatio = 0.0 |}
            Parents = [HighDensityDuraniumArmor]
        }
        {
            Tech = CeramicCompositeArmor
            Name = "Ceramic Composite Armor"
            Cost = 10000<rp>
            Category = DefensiveSystems
            Type = Armor {| Strength = 10.0<armorStrength/hs>; DuraniumRatio = 0.9; NeutroniumRatio = 0.1 |}
            Parents = [CompositeArmor]
        }
        {
            Tech = LaminateCompositeArmor
            Name = "Laminate Composite Armor"
            Cost = 20000<rp>
            Category = DefensiveSystems
            Type = Armor {| Strength = 12.0<armorStrength/hs>; DuraniumRatio = 0.8; NeutroniumRatio = 0.2 |}
            Parents = [CeramicCompositeArmor]
        }
        {
            Tech = CompressedCarbonArmor
            Name = "Compressed Carbon Armor"
            Cost = 40000<rp>
            Category = DefensiveSystems
            Type = Armor {| Strength = 15.0<armorStrength/hs>; DuraniumRatio = 0.7; NeutroniumRatio = 0.3 |}
            Parents = [LaminateCompositeArmor]
        }
        {
            Tech = BiphaseCarbideArmor
            Name = "Biphase Carbide Armor"
            Cost = 80000<rp>
            Category = DefensiveSystems
            Type = Armor {| Strength = 18.0<armorStrength/hs>; DuraniumRatio = 0.6; NeutroniumRatio = 0.4 |}
            Parents = [CompressedCarbonArmor]
        }
        {
            Tech = CrystallineCompositeArmor
            Name = "Crystalline Composite Armor"
            Cost = 150000<rp>
            Category = DefensiveSystems
            Type = Armor {| Strength = 21.0<armorStrength/hs>; DuraniumRatio = 0.5; NeutroniumRatio = 0.5 |}
            Parents = [BiphaseCarbideArmor]
        }
        {
            Tech = SuperdenseArmor
            Name = "Superdense Armor"
            Cost = 300000<rp>
            Category = DefensiveSystems
            Type = Armor {| Strength = 25.0<armorStrength/hs>; DuraniumRatio = 0.4; NeutroniumRatio = 0.6 |}
            Parents = [CrystallineCompositeArmor]
        }
        {
            Tech = BondedSuperdenseArmor
            Name = "Bonded Superdense Armor"
            Cost = 600000<rp>
            Category = DefensiveSystems
            Type = Armor {| Strength = 30.0<armorStrength/hs>; DuraniumRatio = 0.3; NeutroniumRatio = 0.7 |}
            Parents = [SuperdenseArmor]
        }
        {
            Tech = CoherentSuperdenseArmor
            Name = "Coherent Superdense Armor"
            Cost = 1250000<rp>
            Category = DefensiveSystems
            Type = Armor {| Strength = 36.0<armorStrength/hs>; DuraniumRatio = 0.2; NeutroniumRatio = 0.8 |}
            Parents = [BondedSuperdenseArmor]
        }
        {
            Tech = CollapsiumArmor
            Name = "Collapsium Armor"
            Cost = 2500000<rp>
            Category = DefensiveSystems
            Type = Armor {| Strength = 45.0<armorStrength/hs>; DuraniumRatio = 0.1; NeutroniumRatio = 0.9 |}
            Parents = [CoherentSuperdenseArmor]
        }
        //#endregion
        //#region Cargo Handling Systems
        {
            Tech = ImprovedCargoHandlingSystem
            Name = "Improved Cargo Handling System"
            Cost = 10000<rp>
            Category = LogisticsAndGroundCombat
            Type = CargoHandling {| TractorStrength = 10.0 |}
            Parents = []
        }
        {
            Tech = AdvancedCargoHandlingSystem
            Name = "Advanced Cargo Handling System"
            Cost = 40000<rp>
            Category = LogisticsAndGroundCombat
            Type = CargoHandling {| TractorStrength = 20.0 |}
            Parents = [ImprovedCargoHandlingSystem]
        }
        {
            Tech = GravAssistedCargoHandlingSystem
            Name = "Grav-Assisted Cargo Handling System"
            Cost = 150000<rp>
            Category = LogisticsAndGroundCombat
            Type = CargoHandling {| TractorStrength = 40.0 |}
            Parents = [AdvancedCargoHandlingSystem]
        }
        //#endregion
        //#region Reactors
        {
            Tech = PressurizedWaterReactor
            Name = "Pressurized Water Reactor"
            Cost = 1500<rp>
            Category = PowerAndPropulsion
            Type = Reactor {| PowerOutput = 2.0<power/hs> |}
            Parents = []
        }
        {
            Tech = PebbleBedReactor
            Name = "Pebble Bed Reactor"
            Cost = 3000<rp>
            Category = PowerAndPropulsion
            Type = Reactor {| PowerOutput = 3.0<power/hs> |}
            Parents = [PressurizedWaterReactor]
        }
        {
            Tech = GasCooledFastReactor
            Name = "Gas-Cooled Fast Reactor"
            Cost = 6000<rp>
            Category = PowerAndPropulsion
            Type = Reactor {| PowerOutput = 4.5<power/hs> |}
            Parents = [PebbleBedReactor]
        }
        {
            Tech = StellaratorFusionReactor
            Name = "Stellarator Fusion Reactor"
            Cost = 12000<rp>
            Category = PowerAndPropulsion
            Type = Reactor {| PowerOutput = 6.0<power/hs> |}
            Parents = [GasCooledFastReactor]
        }
        {
            Tech = TokamakFusionReactor
            Name = "Tokamak Fusion Reactor"
            Cost = 24000<rp>
            Category = PowerAndPropulsion
            Type = Reactor {| PowerOutput = 8.0<power/hs> |}
            Parents = [StellaratorFusionReactor]
        }
        {
            Tech = MagneticConfinementFusionReactor
            Name = "Magnetic Confinement Fusion Reactor"
            Cost = 45000<rp>
            Category = PowerAndPropulsion
            Type = Reactor {| PowerOutput = 10.0<power/hs> |}
            Parents = [TokamakFusionReactor]
        }
        {
            Tech = InertialConfinementFusionReactor
            Name = "Inertial Confinement Fusion Reactor"
            Cost = 90000<rp>
            Category = PowerAndPropulsion
            Type = Reactor {| PowerOutput = 12.0<power/hs> |}
            Parents = [MagneticConfinementFusionReactor]
        }
        {
            Tech = SolidcoreAntimatterPowerPlant
            Name = "Solid-core Anti-matter Power Plant"
            Cost = 180000<rp>
            Category = PowerAndPropulsion
            Type = Reactor {| PowerOutput = 16.0<power/hs> |}
            Parents = [InertialConfinementFusionReactor]
        }
        {
            Tech = GascoreAntimatterPowerPlant
            Name = "Gas-core Anti-matter Power Plant"
            Cost = 375000<rp>
            Category = PowerAndPropulsion
            Type = Reactor {| PowerOutput = 20.0<power/hs> |}
            Parents = [SolidcoreAntimatterPowerPlant]
        }
        {
            Tech = PlasmacoreAntimatterPowerPlant
            Name = "Plasma-core Anti-matter Power Plant"
            Cost = 750000<rp>
            Category = PowerAndPropulsion
            Type = Reactor {| PowerOutput = 24.0<power/hs> |}
            Parents = [GascoreAntimatterPowerPlant]
        }
        {
            Tech = BeamCoreAntimatterPowerPlant
            Name = "Beam Core Anti-matter Power Plant"
            Cost = 1500000<rp>
            Category = PowerAndPropulsion
            Type = Reactor {| PowerOutput = 32.0<power/hs> |}
            Parents = [PlasmacoreAntimatterPowerPlant]
        }
        {
            Tech = VacuumEnergyPowerPlant
            Name = "Vacuum Energy Power Plant"
            Cost = 3000000<rp>
            Category = PowerAndPropulsion
            Type = Reactor {| PowerOutput = 40.0<power/hs> |}
            Parents = [BeamCoreAntimatterPowerPlant]
        }
        //#endregion
    ]

module Technology =
    //#region Armor
    type ArmorLevel = int
    type ArmorTech =
        {|
            Level: ArmorLevel
            Name: string
            Strength: float<armorStrength/hs>
            DuraniumRatio: float
            NeutroniumRatio: float
            Tech: Tech
        |}
    let private _armor =
        lazy (
            allTechnologies
            |> List.map (fun t ->
                match t.Type with
                | Armor a -> Some {| a with Name = t.Name; Tech = t.Tech |}
                | _ -> None
            )
            |> List.choose id
            |> List.sortBy (fun t -> t.Strength)
            |> List.mapi (fun i t -> {| t with Level = i |})
            |> List.map (fun e -> e.Level, e)
            |> Map.ofSeq
        )
    let armor = _armor.Value
    //#endregion
    //#region Reactors
    type PowerPlantLevel = int
    type PowerPlantTech =
        {|
            Level: ArmorLevel
            Name: string
            PowerOutput: float<power/hs>
            Tech: Tech
        |}
    let private _powerPlant =
        lazy (
            allTechnologies
            |> List.map (fun t ->
                match t.Type with
                | Reactor a -> Some {| a with Name = t.Name; Tech = t.Tech |}
                | _ -> None
            )
            |> List.choose id
            |> List.sortBy (fun t -> t.PowerOutput)
            |> List.mapi (fun i t -> {| t with Level = i |})
            |> List.map (fun e -> e.Level, e)
            |> Map.ofSeq
        )
    let powerPlant = _powerPlant.Value
    //#endregion

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
            let te =
                { te with
                    Name = sprintf "Thermal Reduction: Signature %d%% Normal" <| int (te.ThermalEfficiency * 100.0)
                }
            te.Level, te
        )
        |> Map.ofSeq
