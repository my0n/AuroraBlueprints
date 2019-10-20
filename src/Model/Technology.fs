module Technology

open System

open Model.Measures
open Global

type TechCategory =
    | DefensiveSystems
    | LogisticsAndGroundCombat
    | MissilesAndKineticWeapons
    | PowerAndPropulsion
    | SensorsAndFireControl

[<AbstractClass>]
type TechBase(name: string, cost: int<rp>) =
    let guid = Guid.NewGuid ()
    let mutable parents: TechBase list = []
    let mutable level: int = 0
    member this.Guid with get() = guid
    member this.Name with get() = name
    member this.Cost with get() = cost
    member this.Parents with get() = parents
    member this.Level with get() = level
    abstract member Category: TechCategory with get
    // cannot figure out how to hide this functionality to only the file
    member this.__SetNewParents value =
        parents <- value
        this
    member this.__SetNewLevel value =
        level <- value
        this
    member this.__SetNewParentsAndLevel p l =
        parents <- p
        level <- l
        this

type ArmorTech(name, cost, strength, duraniumRatio, neutroniumRatio) =
    inherit TechBase(name, cost)
    override this.Category = DefensiveSystems
    member val Strength: float<armorStrength/hs> = strength with get
    member val DuraniumRatio: float = duraniumRatio with get
    member val NeutroniumRatio: float = neutroniumRatio with get

type CargoHandlingTech(name, cost, tractorStrength) =
    inherit TechBase(name, cost)
    override this.Category = LogisticsAndGroundCombat
    member val TractorStrength: float = tractorStrength with get

type EngineTech(name, cost, powerPerHs) =
    inherit TechBase(name, cost)
    override this.Category = PowerAndPropulsion
    member val PowerPerHs: float<ep/hs> = powerPerHs with get

type EngineBoostTech(name, cost, powerMod) =
    inherit TechBase(name, cost)
    override this.Category = PowerAndPropulsion
    member val PowerMod: float = powerMod with get

type EngineBoostUnlockTech(name, cost, unlocked) =
    inherit TechBase(name, cost)
    override this.Category = PowerAndPropulsion
    member val UnlockedPowerMods: float list = unlocked with get

type EngineEffTech(name, cost, efficiency) =
    inherit TechBase(name, cost)
    override this.Category = PowerAndPropulsion
    member val Efficiency: float<kl/hr/ep> = efficiency with get

type MagazineEfficiencyTech(name, cost, ammoDensity) =
    inherit TechBase(name, cost)
    override this.Category = MissilesAndKineticWeapons
    member val AmmoDensity: float<ammo/hs> = ammoDensity with get

type MagazineEjectionTech(name, cost, ejectionChance) =
    inherit TechBase(name, cost)
    override this.Category = MissilesAndKineticWeapons
    member val EjectionChance: float = ejectionChance with get

type ReactorTech(name, cost, powerOutput) =
    inherit TechBase(name, cost)
    override this.Category = PowerAndPropulsion
    member val PowerOutput: float<power/hs> = powerOutput with get

type EngineThermalTech(name, cost, thermalEfficiency, costMultiplier) =
    inherit TechBase(name, cost)
    override this.Category = PowerAndPropulsion
    member val ThermalEfficiency: float<therm/ep> = thermalEfficiency with get
    member val CostMultiplier: float = costMultiplier with get

type ReactorBoostTech(name, cost, powerBoost, explosionChance) =
    inherit TechBase(name, cost)
    override this.Category = PowerAndPropulsion
    member val PowerBoost: float = powerBoost with get
    member val ExplosionChance: float = explosionChance with get

type GravSensorTech(name, cost) =
    inherit TechBase(name, cost)
    override this.Category = SensorsAndFireControl

type GeoSensorTech(name, cost) =
    inherit TechBase(name, cost)
    override this.Category = SensorsAndFireControl

let private applyChainTechnologies techs =
    techs
    |> List.scani (fun i prev (current: 'a :> TechBase) ->
        let parents =
            match prev with
            | None -> []
            | Some a -> [a]
        Some <| current.__SetNewParentsAndLevel parents i
    ) None

let private chainTechnologies techs =
    applyChainTechnologies techs |> ignore
    techs

let geologicalSensors = GeoSensorTech ("Geological Survey Sensors", 1000<rp>)
let improvedGeologicalSensors = GeoSensorTech ("Improved Geological Sensors", 10000<rp>)
let advancedGeologicalSensors = GeoSensorTech ("Advanced Geological Sensors", 35000<rp>)
let phasedGeologicalSensors = GeoSensorTech ("Phased Geological Sensors", 100000<rp>)
let geoSensors =
    [
        geologicalSensors; improvedGeologicalSensors; advancedGeologicalSensors; phasedGeologicalSensors
    ]
    |> chainTechnologies
let gravitationalSensors = GravSensorTech ("Gravitational Survey Sensors", 2000<rp>)
let improvedGravitationalSensors = GravSensorTech ("Improved Gravitational Sensors", 10000<rp>)
let advancedGravitationalSensors = GravSensorTech ("Advanced Gravitational Sensors", 35000<rp>)
let phasedGravitationalSensors = GravSensorTech ("Phased Gravitational Sensors", 100000<rp>)
let gravSensors =
    [
        gravitationalSensors; improvedGravitationalSensors; advancedGravitationalSensors; phasedGravitationalSensors
    ]
    |> chainTechnologies
let improvedCargoHandling =
    CargoHandlingTech ("Improved Cargo Handling System", 10000<rp>,
        tractorStrength = 10.0
    )
let advancedCargoHandling =
    CargoHandlingTech ("Advanced Cargo Handling System", 40000<rp>,
        tractorStrength = 20.0
    )
let gravAssistedCargoHandling =
    CargoHandlingTech ("Grav-Assisted Cargo Handling System", 150000<rp>,
        tractorStrength = 40.0
    )
let cargoHandling =
    [
        improvedCargoHandling; advancedCargoHandling; gravAssistedCargoHandling 
    ]
    |> chainTechnologies
let armor =
    [
        ArmorTech ("Conventional Armor", 250<rp>,
            strength = 2.0<armorStrength/hs>,
            duraniumRatio = 1.0,
            neutroniumRatio = 0.0
        )
        ArmorTech ("Duranium Armor", 500<rp>,
            strength = 5.0<armorStrength/hs>,
            duraniumRatio = 1.0,
            neutroniumRatio = 0.0
        )
        ArmorTech ("High Density Duranium Armor", 2500<rp>,
            strength = 6.0<armorStrength/hs>,
            duraniumRatio = 1.0,
            neutroniumRatio = 0.0
        )
        ArmorTech ("Composite Armor", 5000<rp>,
            strength = 8.0<armorStrength/hs>,
            duraniumRatio = 1.0,
            neutroniumRatio = 0.0
        )
        ArmorTech ("Ceramic Composite Armor", 10000<rp>,
            strength = 10.0<armorStrength/hs>,
            duraniumRatio = 0.9,
            neutroniumRatio = 0.1
        )
        ArmorTech ("Laminate Composite Armor", 20000<rp>,
            strength = 12.0<armorStrength/hs>,
            duraniumRatio = 0.8,
            neutroniumRatio = 0.2
        )
        ArmorTech ("Compressed Carbon Armor", 40000<rp>,
            strength = 15.0<armorStrength/hs>,
            duraniumRatio = 0.7,
            neutroniumRatio = 0.3
        )
        ArmorTech ("Biphase Carbide Armor", 80000<rp>,
            strength = 18.0<armorStrength/hs>,
            duraniumRatio = 0.6,
            neutroniumRatio = 0.4
        )
        ArmorTech ("Crystalline Composite Armor", 150000<rp>,
            strength = 21.0<armorStrength/hs>,
            duraniumRatio = 0.5,
            neutroniumRatio = 0.5
        )
        ArmorTech ("Superdense Armor", 300000<rp>,
            strength = 25.0<armorStrength/hs>,
            duraniumRatio = 0.4,
            neutroniumRatio = 0.6
        )
        ArmorTech ("Bonded Superdense Armor", 600000<rp>,
            strength = 30.0<armorStrength/hs>,
            duraniumRatio = 0.3,
            neutroniumRatio = 0.7
        )
        ArmorTech ("Coherent Superdense Armor", 1250000<rp>,
            strength = 36.0<armorStrength/hs>,
            duraniumRatio = 0.2,
            neutroniumRatio = 0.8
        )
        ArmorTech ("Collapsium Armor", 2500000<rp>,
            strength = 45.0<armorStrength/hs>,
            duraniumRatio = 0.1,
            neutroniumRatio = 0.9
        )
    ]
    |> chainTechnologies
let reactors =
    [
        ReactorTech ("Pressurized Water Reactor", 1500<rp>,
            powerOutput = 2.0<power/hs>
        )
        ReactorTech ("Pebble Bed Reactor", 3000<rp>,
            powerOutput = 3.0<power/hs>
        )
        ReactorTech ("Gas-Cooled Fast Reactor", 6000<rp>,
            powerOutput = 4.5<power/hs>
        )
        ReactorTech ("Stellarator Fusion Reactor", 12000<rp>,
            powerOutput = 6.0<power/hs>
        )
        ReactorTech ("Tokamak Fusion Reactor", 24000<rp>,
            powerOutput = 8.0<power/hs>
        )
        ReactorTech ("Magnetic Confinement Fusion Reactor", 45000<rp>,
            powerOutput = 10.0<power/hs>
        )
        ReactorTech ("Inertial Confinement Fusion Reactor", 90000<rp>,
            powerOutput = 12.0<power/hs>
        )
        ReactorTech ("Solid-core Anti-matter Power Plant", 180000<rp>,
            powerOutput = 16.0<power/hs>
        )
        ReactorTech ("Gas-core Anti-matter Power Plant", 375000<rp>,
            powerOutput = 20.0<power/hs>
        )
        ReactorTech ("Plasma-core Anti-matter Power Plant", 750000<rp>,
            powerOutput = 24.0<power/hs>
        )
        ReactorTech ("Beam Core Anti-matter Power Plant", 1500000<rp>,
            powerOutput = 32.0<power/hs>
        )
        ReactorTech ("Vacuum Energy Power Plant", 3000000<rp>,
            powerOutput = 40.0<power/hs>
        )
    ]
    |> chainTechnologies
let engine=
    [
        EngineTech ("Conventional Engine", 500<rp>,
            powerPerHs = 0.2<ep/hs>
        )
        EngineTech ("Nuclear Thermal Engine", 2500<rp>,
            powerPerHs = 5.0<ep/hs>
        )
        EngineTech ("Nuclear Pulse Engine", 5000<rp>,
            powerPerHs = 8.0<ep/hs>
        )
        EngineTech ("Ion Engine", 10000<rp>,
            powerPerHs = 12.0<ep/hs>
        )
        EngineTech ("Magneto-Plasma Engine", 20000<rp>,
            powerPerHs = 16.0<ep/hs>
        )
        EngineTech ("Internal Confinement Fusion Engine", 40000<rp>,
            powerPerHs = 20.0<ep/hs>
        )
        EngineTech ("Magnetic Confinement Fusion Engine", 80000<rp>,
            powerPerHs = 25.0<ep/hs>
        )
        EngineTech ("Inertial Confinement Fusion Engine", 150000<rp>,
            powerPerHs = 32.0<ep/hs>
        )
        EngineTech ("Solid-Core Anti-matter Engine", 300000<rp>,
            powerPerHs = 40.0<ep/hs>
        )
        EngineTech ("Gas-Core Anti-matter Engine", 600000<rp>,
            powerPerHs = 50.0<ep/hs>
        )
        EngineTech ("Plasma-Core Anti-matter Engine", 1250000<rp>,
            powerPerHs = 60.0<ep/hs>
        )
        EngineTech ("Beam-Core Anti-matter Engine", 2500000<rp>,
            powerPerHs = 80.0<ep/hs>
        )
        EngineTech ("Photonic Engine", 5000000<rp>,
            powerPerHs = 100.0<ep/hs>
        )
    ]
    |> chainTechnologies
    |> List.map2 (fun reactor engine ->
        let engine' = engine :> TechBase
        match reactor with
        | None -> engine'
        | Some reactor -> engine'.__SetNewParents (engine'.Parents @ [reactor])
        |> ignore
        engine
    ) ([None] @ (List.map Some reactors))
let engineEfficiency =
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
    |> List.map (fun (eff, cost) ->
        let eff = float eff / 1000.0
        EngineEffTech (String.Format("Fuel Consumption: {0}% Litres per Engine Power Hour", eff), cost * 1<rp>,
            efficiency = eff * 1.0<kl/hr/ep>
        )
    )
    |> chainTechnologies
let feedEfficiency =
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
    |> List.map (fun (eff, cost) ->
        MagazineEfficiencyTech (sprintf "Magazine Feed System Efficiency - %d%%" eff, cost * 1<rp>,
            ammoDensity = 20.0<ammo/hs> * float eff / 100.0
        )
    )
    |> chainTechnologies
let ejectionChance =
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
    |> List.map (fun (chance, cost) ->
        MagazineEjectionTech (sprintf "Magazine Ejection System - %d%% Chance" chance, cost * 1<rp>,
            ejectionChance = float chance / 100.0
        )
    )
    |> chainTechnologies
let highPowerMod =
    [
        "1",    [1.0],                                          1
        "1.25", [1.05;1.1;1.15;1.2;1.25],                       1000
        "1.5",  [1.3;1.35;1.4;1.45;1.5],                        2000
        "1.75", [1.55;1.6;1.65;1.7;1.75],                       4000
        "2",    [1.8;1.85;1.9;1.95;2.0],                        8000
        "2.5",  [2.05;2.1;2.25;2.3;2.35;2.4;2.45;2.5],          15000
        "3",    [2.55;2.6;2.65;2.7;2.75;2.8;2.85;2.9;2.95;3.0], 30000
    ]
    |> List.map (fun (id, unlocked, cost) ->
        EngineBoostUnlockTech (sprintf "Maximum Engine Power Modifier x%s" id, cost * 1<rp>,
            unlocked = unlocked
        )
    )
    |> chainTechnologies
let lowPowerMod =
    [
        "0.5",  [0.95;0.9;0.85;0.8;0.75;0.7;0.65;0.6;0.55;0.5], 2
        "0.4",  [0.45;0.4],                                     1000
        "0.3",  [0.35;0.3],                                     2000
        "0.25", [0.25],                                         4000
        "0.2",  [0.2],                                          8000
        "0.15", [0.15],                                         15000
        "0.1",  [0.1],                                          30000
    ]
    |> List.map (fun (id, unlocked, cost) ->
        EngineBoostUnlockTech (sprintf "Minimum Engine Power Modifier x%s" id, cost * 1<rp>,
            unlocked = unlocked
        )
    )
    |> chainTechnologies
let allPowerMods =
    highPowerMod @ lowPowerMod
    |> List.collect (fun tech ->
        tech.UnlockedPowerMods
        |> List.map (fun unlocked ->
            let engineBoostTech =
                EngineBoostTech (String.Format("Engine Power Modifier x{0}", unlocked), tech.Cost,
                    powerMod = unlocked
                )
            engineBoostTech.__SetNewParents [tech] |> ignore
            engineBoostTech
        )
    )
let noPowerMod =
    allPowerMods
    |> List.pick (fun tech ->
        match tech.PowerMod with
        | 1.0 -> Some tech
        | _ -> None
    )
let unlockedPowerMods current =
    allPowerMods
    |> List.filter (fun value ->
        current
        |> List.contains value.Parents.[0]
    )
//#region Reactor Power Boost
let powerBoost =
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
    |> List.map (fun (boost, explosionChance, cost) ->
        let name =
            match boost with
            | 0 -> "No Power Boost"
            | _ -> String.Format("Reactor Power Boost {0}%, Explosion {1}%", boost, explosionChance)
        ReactorBoostTech (name, cost * 1<rp>,
            powerBoost = float boost / 100.0,
            explosionChance = float explosionChance / 100.0
        )
    )
    |> chainTechnologies
//#endregion
//#region Thermal Efficiency
let thermalEfficiency =
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
    |> List.map (fun (signature, costMultiplier, cost) ->
        EngineThermalTech (sprintf "Thermal Reduction: Signature %d%% Normal" signature, cost * 1<rp>,
            thermalEfficiency = 1.0<therm/ep> * float signature / 100.0,
            costMultiplier = costMultiplier
        )
    )
    |> chainTechnologies
//#endregion

let allTechnologies =
    let toTbList l = l |> List.map (fun a -> a :> TechBase)
    toTbList armor
    @ toTbList cargoHandling
    @ toTbList gravSensors
    @ toTbList geoSensors
    @ toTbList reactors
    @ toTbList highPowerMod
    @ toTbList lowPowerMod
    @ toTbList powerBoost
    @ toTbList engine
    @ toTbList engineEfficiency
    @ toTbList thermalEfficiency
    @ toTbList feedEfficiency
    @ toTbList ejectionChance
