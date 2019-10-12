module Technology

open System

open Model.Measures
open Global

let private chainTechnologies idFn =
    List.withPrev
    >> List.map (fun (prev, current) ->
        let parents =
            match prev with
            | None -> []
            | Some a -> [idFn a]
        idFn current, parents, current
    )

type TechCategory =
    | DefensiveSystems
    | LogisticsAndGroundCombat
    | MisslesAndKineticWeapons
    | PowerAndPropulsion
    | SensorsAndFireControl

type TechType =
    | Armor of {| Strength: float<armorStrength/hs>; DuraniumRatio: float; NeutroniumRatio: float |}
    | CargoHandling of {| TractorStrength: float |}
    | Engine of {| PowerPerHs: float<ep/hs> |}
    | EngineBoost of {| UnlockedPowerMods: float list |}
    | EngineEff of {| Efficiency: float<kl/hr/ep> |}
    | EngineThermal of {| ThermalEfficiency: float<therm/ep>; CostMultiplier: float |}
    | MagazineEfficiency of {| AmmoDensity: float<ammo/hs> |}
    | MagazineEjection of {| EjectionChance: float |}
    | Reactor of {| PowerOutput: float<power/hs> |}
    | ReactorBoost of {| PowerBoost: float; ExplosionChance: float |}
    | SurveySensor

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
    | ConventionalEngine
    | NuclearThermalEngine
    | NuclearPulseEngine
    | IonEngine
    | MagnetoPlasmaEngine
    | InternalConfinementFusionEngine
    | MagneticConfinementFusionEngine
    | InertialConfinementFusionEngine
    | SolidCoreAntimatterEngine
    | GasCoreAntimatterEngine
    | PlasmaCoreAntimatterEngine
    | BeamCoreAntimatterEngine
    | PhotonicEngine
    | FeedSystemEfficiency of int
    | EjectionSystem of int
    | EngineEfficiency of int
    | EnginePowerBoost of string
    | ReactorPowerBoost of int
    | EngineThermalEfficiency of int

type TechNode =
    {
        Tech: Tech
        Name: string
        Cost: int<rp>
        Category: TechCategory
        Type: TechType
        Parents: Tech list
    }

//#region Survey Sensors
let geoSensorsTree =
    [
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
    ]
let gravSensorsTree =
    [
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
    ]
//#endregion
//#region Cargo Handling Systems
let cargoHandlingTree =
    [
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
    ]
//#endregion
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
let armorTree =
    [
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
    ]
let private _armor =
    lazy (
        armorTree
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
//#region Engine
let engineTree =
    [
        {
            Tech = ConventionalEngine
            Name = "Conventional Engine"
            Cost = 500<rp>
            Category = PowerAndPropulsion
            Type = Engine {| PowerPerHs = 0.2<ep/hs> |}
            Parents = []
        }
        {
            Tech = NuclearThermalEngine
            Name = "Nuclear Thermal Engine"
            Cost = 2500<rp>
            Category = PowerAndPropulsion
            Type = Engine {| PowerPerHs = 5.0<ep/hs> |}
            Parents = [ConventionalEngine; PressurizedWaterReactor]
        }
        {
            Tech = NuclearPulseEngine
            Name = "Nuclear Pulse Engine"
            Cost = 5000<rp>
            Category = PowerAndPropulsion
            Type = Engine {| PowerPerHs = 8.0<ep/hs> |}
            Parents = [NuclearThermalEngine; PebbleBedReactor]
        }
        {
            Tech = IonEngine
            Name = "Ion Engine"
            Cost = 10000<rp>
            Category = PowerAndPropulsion
            Type = Engine {| PowerPerHs = 12.0<ep/hs> |}
            Parents = [NuclearPulseEngine; GasCooledFastReactor]
        }
        {
            Tech = MagnetoPlasmaEngine
            Name = "Magneto-Plasma Engine"
            Cost = 20000<rp>
            Category = PowerAndPropulsion
            Type = Engine {| PowerPerHs = 16.0<ep/hs> |}
            Parents = [IonEngine; StellaratorFusionReactor]
        }
        {
            Tech = InternalConfinementFusionEngine
            Name = "Internal Confinement Fusion Engine"
            Cost = 40000<rp>
            Category = PowerAndPropulsion
            Type = Engine {| PowerPerHs = 20.0<ep/hs> |}
            Parents = [MagnetoPlasmaEngine; TokamakFusionReactor]
        }
        {
            Tech = MagneticConfinementFusionEngine
            Name = "Magnetic Confinement Fusion Engine"
            Cost = 80000<rp>
            Category = PowerAndPropulsion
            Type = Engine {| PowerPerHs = 25.0<ep/hs> |}
            Parents = [InternalConfinementFusionEngine; MagneticConfinementFusionReactor]
        }
        {
            Tech = InertialConfinementFusionEngine
            Name = "Inertial Confinement Fusion Engine"
            Cost = 150000<rp>
            Category = PowerAndPropulsion
            Type = Engine {| PowerPerHs = 32.0<ep/hs> |}
            Parents = [MagneticConfinementFusionEngine; InertialConfinementFusionReactor]
        }
        {
            Tech = SolidCoreAntimatterEngine
            Name = "Solid-Core Anti-matter Engine"
            Cost = 300000<rp>
            Category = PowerAndPropulsion
            Type = Engine {| PowerPerHs = 40.0<ep/hs> |}
            Parents = [InertialConfinementFusionEngine; SolidcoreAntimatterPowerPlant]
        }
        {
            Tech = GasCoreAntimatterEngine
            Name = "Gas-Core Anti-matter Engine"
            Cost = 600000<rp>
            Category = PowerAndPropulsion
            Type = Engine {| PowerPerHs = 50.0<ep/hs> |}
            Parents = [SolidCoreAntimatterEngine; GascoreAntimatterPowerPlant]
        }
        {
            Tech = PlasmaCoreAntimatterEngine
            Name = "Plasma-Core Anti-matter Engine"
            Cost = 1250000<rp>
            Category = PowerAndPropulsion
            Type = Engine {| PowerPerHs = 60.0<ep/hs> |}
            Parents = [GasCoreAntimatterEngine; PlasmacoreAntimatterPowerPlant]
        }
        {
            Tech = BeamCoreAntimatterEngine
            Name = "Beam-Core Anti-matter Engine"
            Cost = 2500000<rp>
            Category = PowerAndPropulsion
            Type = Engine {| PowerPerHs = 80.0<ep/hs> |}
            Parents = [PlasmaCoreAntimatterEngine; BeamCoreAntimatterPowerPlant]
        }
        {
            Tech = PhotonicEngine
            Name = "Photonic Engine"
            Cost = 5000000<rp>
            Category = PowerAndPropulsion
            Type = Engine {| PowerPerHs = 100.0<ep/hs> |}
            Parents = [BeamCoreAntimatterEngine; VacuumEnergyPowerPlant]
        }
    ]
type EngineLevel = int
type EngineTech =
    {|
        Level: EngineLevel
        Name: string
        PowerPerHs: float<ep/hs>
        Tech: Tech
    |}
let private _engine =
    lazy (
        engineTree
        |> List.map (fun t ->
            match t.Type with
            | Engine a -> Some {| a with Name = t.Name; Tech = t.Tech |}
            | _ -> None
        )
        |> List.choose id
        |> List.sortBy (fun t -> t.PowerPerHs)
        |> List.mapi (fun i t -> {| t with Level = i |})
        |> List.map (fun e -> e.Level, e)
        |> Map.ofSeq
    )
let engine = _engine.Value
//#endregion
//#region Engine Efficiency
let engineEfficiencyTree =
    [
        1000, 1
        900, 1000
        800, 2000
        700, 4000
        600, 8000
        500, 15000
        400, 30000
        300, 60000
        250, 120000
        200, 250000
        160, 500000
        125, 1000000
        100, 2000000
    ]
    |> chainTechnologies (fun (id, _) -> EngineEfficiency id)
    |> List.map (fun (tech, parents, (eff, rp)) ->
        let eff = float eff / 1000.0
        {
            Tech = tech
            Name = String.Format("Fuel Consumption: {0}% Litres per Engine Power Hour", eff)
            Cost = rp * 1<rp>
            Category = PowerAndPropulsion
            Type = EngineEff {| Efficiency = eff * 1.0<kl/hr/ep> |}
            Parents = parents
        }
    )
type EngineEfficiencyLevel = int
type EngineEfficiencyTech =
    {|
        Level: EngineEfficiencyLevel
        Name: string
        Efficiency: float<kl/hr/ep>
        Tech: Tech
    |}
let private _engineEfficiency =
    lazy (
        engineEfficiencyTree
        |> List.map (fun t ->
            match t.Type with
            | EngineEff a -> Some {| a with Name = t.Name; Tech = t.Tech |}
            | _ -> None
        )
        |> List.choose id
        |> List.sortByDescending (fun t -> t.Efficiency)
        |> List.mapi (fun i t -> {| t with Level = i |})
        |> List.map (fun e -> e.Level, e)
        |> Map.ofSeq
    )
let engineEfficiency = _engineEfficiency.Value
//#endregion
//#region Reactors
let reactorsTree =
    [
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
    ]
type PowerPlantLevel = int
type PowerPlantTech =
    {|
        Level: PowerPlantLevel
        Name: string
        PowerOutput: float<power/hs>
        Tech: Tech
    |}
let private _powerPlant =
    lazy (
        reactorsTree
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
//#region Magazine Feed System Efficiency
let feedEfficiencyTree =
    [
        75, 1000
        80, 2000
        85, 4000
        90, 8000
        92, 15000
        94, 30000
        96, 60000
        98, 125000
        99, 250000
    ]
    |> chainTechnologies (fun (id, _) -> FeedSystemEfficiency id)
    |> List.map (fun (tech, parents, (chance, rp)) ->
        {
            Tech = tech
            Name = sprintf "Magazine Feed System Efficiency - %d%%" chance
            Cost = rp * 1<rp>
            Category = MisslesAndKineticWeapons
            Type = MagazineEfficiency {| AmmoDensity = 20.0<ammo/hs> * float chance / 100.0 |}
            Parents = parents
        }
    )
type MagazineFeedEfficiencyLevel = int
type MagazineFeedEfficiencyTech =
    {|
        Level: MagazineFeedEfficiencyLevel
        Name: string
        AmmoDensity: float<ammo/hs>
        Tech: Tech
    |}
let private _feedEfficiency =
    lazy (
        feedEfficiencyTree
        |> List.map (fun t ->
            match t.Type with
            | MagazineEfficiency a -> Some {| a with Name = t.Name; Tech = t.Tech |}
            | _ -> None
        )
        |> List.choose id
        |> List.sortBy (fun t -> t.AmmoDensity)
        |> List.mapi (fun i t -> {| t with Level = i |})
        |> List.map (fun e -> e.Level, e)
        |> Map.ofSeq
    )
let feedEfficiency = _feedEfficiency.Value
//#endregion
//#region Magazine Ejection System
let ejectionChanceTree =
    [
        70, 500
        80, 1000
        85, 2000
        90, 4000
        93, 8000
        95, 15000
        97, 30000
        98, 60000
        99, 120000
    ]
    |> chainTechnologies (fun (id, _) -> EjectionSystem id)
    |> List.map (fun (tech, parents, (chance, rp)) ->
        {
            Tech = tech
            Name = sprintf "Magazine Ejection System - %d%% Chance" chance
            Cost = rp * 1<rp>
            Category = MisslesAndKineticWeapons
            Type = MagazineEjection {| EjectionChance = float chance / 100.0 |}
            Parents = parents
        }
    )
type MagazineEjectionChanceLevel = int
type MagazineEjectionChanceTech =
    {|
        Level: MagazineEjectionChanceLevel
        Name: string
        EjectionChance: float
        Tech: Tech
    |}
let private _ejectionChance =
    lazy (
        ejectionChanceTree
        |> List.map (fun t ->
            match t.Type with
            | MagazineEjection a -> Some {| a with Name = t.Name; Tech = t.Tech |}
            | _ -> None
        )
        |> List.choose id
        |> List.sortBy (fun t -> t.EjectionChance)
        |> List.mapi (fun i t -> {| t with Level = i |})
        |> List.map (fun e -> e.Level, e)
        |> Map.ofSeq
    )
let ejectionChance = _ejectionChance.Value
//#endregion
//#region Engine Power Mod
let highPowerModTree =
    [
        "1",    [1.0],                                          1
        "1.25", [1.05;1.1;1.15;1.2;1.25],                       1000
        "1.5",  [1.3;1.35;1.4;1.45;1.5],                        2000
        "1.75", [1.55;1.6;1.65;1.7;1.75],                       4000
        "2",    [1.8;1.85;1.9;1.95;2.0],                        8000
        "2.5",  [2.05;2.1;2.25;2.3;2.35;2.4;2.45;2.5],          15000
        "3",    [2.55;2.6;2.65;2.7;2.75;2.8;2.85;2.9;2.95;3.0], 30000
    ]
    |> chainTechnologies (fun (id, _, _) -> EnginePowerBoost id)
    |> List.map (fun (tech, parents, (id, unlocked, rp)) ->
        {
            Tech = tech
            Name = sprintf "Maximum Engine Power Modifier x%s" id
            Cost = rp * 1<rp>
            Category = PowerAndPropulsion
            Type = EngineBoost {| UnlockedPowerMods = unlocked |}
            Parents = parents
        }
    )
let lowPowerModTree =
    [
        "0.5",  [0.95;0.9;0.85;0.8;0.75;0.7;0.65;0.6;0.55;0.5], 2
        "0.4",  [0.45;0.4],                                     1000
        "0.3",  [0.35;0.3],                                     2000
        "0.25", [0.25],                                         4000
        "0.2",  [0.2],                                          8000
        "0.15", [0.15],                                         15000
        "0.1",  [0.1],                                          30000
    ]
    |> chainTechnologies (fun (id, _, _) -> EnginePowerBoost id)
    |> List.map (fun (tech, parents, (id, unlocked, rp)) ->
        {
            Tech = tech
            Name = sprintf "Minimum Engine Power Modifier x%s" id
            Cost = rp * 1<rp>
            Category = PowerAndPropulsion
            Type = EngineBoost {| UnlockedPowerMods = unlocked |}
            Parents = parents
        }
    )
type PowerModLevel = int
type PowerModTech =
    {|
        Level: PowerModLevel
        Name: string
        PowerMod: float
        Tech: Tech
    |}
let private getEngineBoosts =
    List.map (fun t ->
        match t.Type with
        | EngineBoost a -> Some {| a with Name = t.Name; Tech = t.Tech |}
        | _ -> None
    )
    >> List.choose id
let private _highPowerMod = lazy (getEngineBoosts highPowerModTree)
let private _lowPowerMod = lazy (getEngineBoosts lowPowerModTree)
let highPowerMod = _highPowerMod.Value
let lowPowerMod = _lowPowerMod.Value
let private _allPowerMods =
    lazy (
        highPowerMod @ lowPowerMod
        |> List.collect (fun tech -> 
            tech.UnlockedPowerMods
            |> List.map (fun unlocked ->
                {|
                    Name = String.Format("Engine Power Modifier x{0}", unlocked)
                    PowerMod = unlocked
                    Tech = tech.Tech
                |}
            )
        )
        |> List.sortBy (fun t -> t.Name)
        |> List.mapi (fun i t -> {| t with Level = i |})
        |> List.map (fun e -> e.Level, e)
        |> Map.ofSeq
    )
let allPowerMods = _allPowerMods.Value
let noPowerMod =
    allPowerMods
    |> Map.pick (fun _ powerMod ->
        match powerMod.Tech with
        | EnginePowerBoost "1" -> Some powerMod
        | _ -> None
    )
let unlockedPowerMods current =
    allPowerMods
    |> Map.filter (fun _ value ->
        current
        |> List.contains value.Tech
    )
//#endregion
//#region Reactor Power Boost
let powerBoostTree =
    [
        0, 0, 1000
        5, 7, 1000
        10, 10, 2000
        15, 12, 3000
        20, 16, 5000
        25, 20, 7500
        30, 25, 12500
        40, 30, 25000
        50, 35, 50000
    ]
    |> chainTechnologies (fun (id, _, _) -> ReactorPowerBoost id)
    |> List.map (fun (tech, parents, (boost, explosionChance, rp)) ->
        let name =
            match boost with
            | 0 -> "No Power Boost"
            | _ -> String.Format("Reactor Power Boost {0}%, Explosion {1}%", boost, explosionChance)
        {
            Tech = tech
            Name = name
            Cost = rp * 1<rp>
            Category = PowerAndPropulsion
            Type = ReactorBoost {| PowerBoost = float boost / 100.0; ExplosionChance = float explosionChance / 100.0 |}
            Parents = parents
        }
    )
type PowerBoostLevel = int
type PowerBoostTech =
    {|
        Level: PowerBoostLevel
        Name: string
        ExplosionChance: float
        PowerBoost: float
        Tech: Tech
    |}
let private _powerBoost =
    lazy (
        powerBoostTree
        |> List.map (fun t ->
            match t.Type with
            | ReactorBoost a -> Some {| a with Name = t.Name; Tech = t.Tech |}
            | _ -> None
        )
        |> List.choose id
        |> List.sortBy (fun t -> t.Name)
        |> List.mapi (fun i t -> {| t with Level = i |})
        |> List.map (fun e -> e.Level, e)
        |> Map.ofSeq
    )
let powerBoost = _powerBoost.Value
//#endregion
//#region Thermal Efficiency
let thermalEfficiencyTree =
    [
        100, 1.00, 1
        75, 1.25, 1500
        50, 1.50, 3000
        35, 1.75, 6000
        25, 2.00, 12000
        16, 2.25, 25000
        12, 2.50, 50000
        8, 2.75, 100000
        6, 3.00, 200000
        4, 3.25, 400000
        3, 3.50, 750000
        2, 3.75, 1500000
        1, 4.00, 2500000
    ]
    |> chainTechnologies (fun (id, _, _) -> ReactorPowerBoost id)
    |> List.map (fun (tech, parents, (signature, costMultiplier, rp)) ->
        {
            Tech = tech
            Name = sprintf "Thermal Reduction: Signature %d%% Normal" signature
            Cost = rp * 1<rp>
            Category = PowerAndPropulsion
            Type = EngineThermal {| ThermalEfficiency = 1.0<therm/ep> * float signature / 100.0; CostMultiplier = costMultiplier |}
            Parents = parents
        }
    )
type ThermalEfficiencyLevel = int
type ThermalEfficiencyTech =
    {|
        Level: ThermalEfficiencyLevel
        Name: string
        ThermalEfficiency: float<therm/ep>
        CostMultiplier: float
        Tech: Tech
    |}
let private _thermalEfficiency =
    lazy (
        thermalEfficiencyTree
        |> List.map (fun t ->
            match t.Type with
            | EngineThermal a -> Some {| a with Name = t.Name; Tech = t.Tech |}
            | _ -> None
        )
        |> List.choose id
        |> List.sortByDescending (fun t -> t.ThermalEfficiency)
        |> List.mapi (fun i t -> {| t with Level = i |})
        |> List.map (fun e -> e.Level, e)
        |> Map.ofSeq
    )
let thermalEfficiency = _thermalEfficiency.Value
//#endregion

let allTechnologies =
    armorTree
    @ cargoHandlingTree
    @ gravSensorsTree
    @ geoSensorsTree
    @ reactorsTree
    @ highPowerModTree
    @ lowPowerModTree
    @ powerBoostTree
    @ engineTree
    @ engineEfficiencyTree
    @ thermalEfficiencyTree
    @ feedEfficiencyTree
    @ ejectionChanceTree
