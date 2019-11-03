module Technology

open System

open File.CsvReader
open Model.Measures
open Global

type ParsedBasics =
    {
        Id: GameObjectId
        Name: string
        Cost: int<rp>
        Level: int
        Parents: GameObjectId list
    }

let private parseList (convert: string -> 'a) (cell: string) =
    match cell with
    | "" -> []
    | str ->
        str.Split ([|';'|])
        |> Array.map convert
        |> Array.toList

let private parseBasics (line: string[]) =
    {
        Id = line.[0]
        Name = line.[1]
        Cost = Convert.ToInt32 line.[2] * 1<rp>
        Level = Convert.ToInt32 line.[3]
        Parents = parseList id line.[4]
    }

type TechCategory =
    | DefensiveSystems
    | LogisticsAndGroundCombat
    | MissilesAndKineticWeapons
    | PowerAndPropulsion
    | SensorsAndFireControl

[<AbstractClass>]
type TechBase(basics: ParsedBasics) =
    member this.Id with get() = basics.Id
    member this.Name with get() = basics.Name
    member this.Cost with get() = basics.Cost
    member this.Parents with get() = basics.Parents
    member this.Level with get() = basics.Level
    abstract member Category: TechCategory with get
    interface IComparable with
        member this.CompareTo obj =
            match obj with
            | :? TechBase as other -> compare this.Id other.Id
            | _ -> invalidArg "other" "not a TechBase"
    override this.Equals obj =
        match obj with
        | :? TechBase as other -> this.Id = other.Id
        | _ -> false
    override this.GetHashCode() =
        hash this.Id

type ArmorTech(basics, strength, duraniumRatio, neutroniumRatio) =
    inherit TechBase(basics)
    override this.Category = DefensiveSystems
    member val Strength: float<armorStrength/hs> = strength with get
    member val DuraniumRatio: float = duraniumRatio with get
    member val NeutroniumRatio: float = neutroniumRatio with get

type CargoHoldTech(basics, cargoCapacity, crewPerComp, hsPerComp, duraniumCost) =
    inherit TechBase(basics)
    override this.Category = LogisticsAndGroundCombat
    member val CargoCapacity: int<cargoCapacity/comp> = cargoCapacity with get
    member val CrewPerComp: int<people/comp> = crewPerComp with get
    member val HsPerComp: int<hs/comp> = hsPerComp with get
    member val DuraniumCost: float</comp> = duraniumCost with get

type CargoHandlingTech(basics, tractorStrength, crewPerComp, hsPerComp, duraniumCost, mercassiumCost) =
    inherit TechBase(basics)
    override this.Category = LogisticsAndGroundCombat
    member val TractorStrength: int<tractorStrength/comp> = tractorStrength with get
    member val CrewPerComp: int<people/comp> = crewPerComp with get
    member val HsPerComp: int<hs/comp> = hsPerComp with get
    member val DuraniumCost: float</comp> = duraniumCost with get
    member val MercassiumCost: float</comp> = mercassiumCost with get

type EngineTech(basics, powerPerHs) =
    inherit TechBase(basics)
    override this.Category = PowerAndPropulsion
    member val PowerPerHs: float<ep/hs> = powerPerHs with get

type EngineBoostTech(basics, powerMod) =
    inherit TechBase(basics)
    override this.Category = PowerAndPropulsion
    member val PowerMod: float = powerMod with get

type EngineBoostUnlockTech(basics, unlocked) =
    inherit TechBase(basics)
    override this.Category = PowerAndPropulsion
    member val UnlockedPowerMods: float list = unlocked with get

type EngineEfficiencyTech(basics, efficiency) =
    inherit TechBase(basics)
    override this.Category = PowerAndPropulsion
    member val Efficiency: float<kl/hr/ep> = efficiency with get

type MagazineEfficiencyTech(basics, ammoDensity) =
    inherit TechBase(basics)
    override this.Category = MissilesAndKineticWeapons
    member val AmmoDensity: float<ammo/hs> = ammoDensity with get

type MagazineEjectionTech(basics, ejectionChance) =
    inherit TechBase(basics)
    override this.Category = MissilesAndKineticWeapons
    member val EjectionChance: float = ejectionChance with get

type ReactorTech(basics, powerOutput) =
    inherit TechBase(basics)
    override this.Category = PowerAndPropulsion
    member val PowerOutput: float<power/hs> = powerOutput with get

type EngineThermalTech(basics, thermalEfficiency, costMultiplier) =
    inherit TechBase(basics)
    override this.Category = PowerAndPropulsion
    member val ThermalEfficiency: float<therm/ep> = thermalEfficiency with get
    member val CostMultiplier: float = costMultiplier with get

type ReactorBoostTech(basics, powerBoost, explosionChance) =
    inherit TechBase(basics)
    override this.Category = PowerAndPropulsion
    member val PowerBoost: float = powerBoost with get
    member val ExplosionChance: float = explosionChance with get

type GravSensorTech(basics, sensorRating, hsPerComp, crewPerComp, uridiumCost) =
    inherit TechBase(basics)
    override this.Category = SensorsAndFireControl
    member val SensorRating: int</comp> = sensorRating with get
    member val HsPerComp: int<hs/comp> = hsPerComp with get
    member val CrewPerComp: int<people/comp> = crewPerComp with get
    member val UridiumCost: float</comp> = uridiumCost with get

type GeoSensorTech(basics, sensorRating, hsPerComp, crewPerComp, uridiumCost) =
    inherit TechBase(basics)
    override this.Category = SensorsAndFireControl
    member val SensorRating: int</comp> = sensorRating with get
    member val HsPerComp: int<hs/comp> = hsPerComp with get
    member val CrewPerComp: int<people/comp> = crewPerComp with get
    member val UridiumCost: float</comp> = uridiumCost with get

let private readTechCsv file generateFn =
    readCsv file (fun line -> generateFn (parseBasics line) line)
    |> Promise.map Seq.cast<TechBase>

let rec private researchedParents (allTechs: Map<GameObjectId, TechBase>) researchedTechs unprocessed processed =
    match unprocessed with
    | [] -> processed
    | x::xs ->
        let tech = allTechs.[x]
        match List.contains tech researchedTechs with
        | false -> researchedParents allTechs researchedTechs xs processed
        | true -> researchedParents allTechs researchedTechs (xs @ tech.Parents) (x::processed)

let inline private techsOfType<'a> techs =
    techs
    |> Map.values
    |> Seq.cast<obj>
    |> Seq.filter (fun x -> x :? 'a)
    |> Seq.cast<'a>
    |> Seq.toList

type AllTechnologies =
    {
        Technologies: Map<GameObjectId, TechBase>
    }
    member this.Item with get identifier = this.Technologies.[identifier]

    member private this._Children =
        lazy (
            this.Technologies
            |> Map.map (fun parentKey _ ->
                this.Technologies
                |> Map.filter (fun _ childValue ->
                    List.contains parentKey childValue.Parents
                )
                |> Map.keys
            )
        )
    member this.GetParents identifier    = this.Technologies.[identifier].Parents
    member this.GetChildren identifier   = this._Children.Value.[identifier]
    member this.GetAllChildren onlyCheck identifier =
        let rec parents unprocessed processed =
            match unprocessed with
            | [] -> processed
            | x::xs ->
                let moreChildren = 
                    Set.intersect
                        (Set.ofList <| this.GetChildren x)
                        (Set.ofList onlyCheck)
                    |> Set.toList
                parents (xs @ moreChildren) (x::processed)
        parents (this.GetChildren identifier) []

    member this.Armor                   = this.Technologies |> techsOfType<ArmorTech>              |> List.sortBy (fun tech -> tech.Level)
    member this.CargoHandling           = this.Technologies |> techsOfType<CargoHandlingTech>      |> List.sortBy (fun tech -> tech.Level)
    member this.CargoHolds              = this.Technologies |> techsOfType<CargoHoldTech>          |> List.sortBy (fun tech -> tech.Level)
    member this.Engines                 = this.Technologies |> techsOfType<EngineTech>             |> List.sortBy (fun tech -> tech.Level)
    member this.EngineEfficiency        = this.Technologies |> techsOfType<EngineEfficiencyTech>   |> List.sortBy (fun tech -> tech.Level)
    member this.EnginePowerMod          = this.Technologies |> techsOfType<EngineBoostUnlockTech>  |> List.sortBy (fun tech -> tech.Level)
    member this.EngineThermalEfficiency = this.Technologies |> techsOfType<EngineThermalTech>      |> List.sortBy (fun tech -> tech.Level)
    member this.GeoSensors              = this.Technologies |> techsOfType<GeoSensorTech>          |> List.sortBy (fun tech -> tech.Level)
    member this.GravSensors             = this.Technologies |> techsOfType<GravSensorTech>         |> List.sortBy (fun tech -> tech.Level)
    member this.MagazineEfficiency      = this.Technologies |> techsOfType<MagazineEfficiencyTech> |> List.sortBy (fun tech -> tech.Level)
    member this.MagazineEjection        = this.Technologies |> techsOfType<MagazineEjectionTech>   |> List.sortBy (fun tech -> tech.Level)
    member this.Reactors                = this.Technologies |> techsOfType<ReactorTech>            |> List.sortBy (fun tech -> tech.Level)
    member this.ReactorsPowerBoost      = this.Technologies |> techsOfType<ReactorBoostTech>       |> List.sortBy (fun tech -> tech.Level)

    member this.DefaultArmor             = this.Armor.[0]
    member this.DefaultEngine            = this.Engines.[0]
    member this.DefaultEngineEfficiency  = this.EngineEfficiency.[0]
    member this.DefaultThermalEfficiency = this.EngineThermalEfficiency.[0]
    member this.DefaultFeedEfficiency    = this.MagazineEfficiency.[0]
    member this.DefaultEjectionChance    = this.MagazineEjection.[0]
    member this.DefaultReactor           = this.Reactors.[0]
    member this.DefaultPowerBoost        = this.ReactorsPowerBoost.[0]

    member this.AllPowerMods =
        this.EnginePowerMod
        |> List.collect (fun tech -> tech.UnlockedPowerMods)

    member this.UnlockedPowerMods (researchedTechs: GameObjectId list) =
        this.EnginePowerMod
        |> List.filter (fun tech -> List.contains tech.Id researchedTechs)
        |> List.collect (fun tech -> tech.UnlockedPowerMods)

    member this.DefaultPowerMod =
        (
            this.EnginePowerMod
            |> List.find (fun tech -> tech.Level = 1)
        ).UnlockedPowerMods.[0]

let allTechnologies: Fable.Core.JS.Promise<AllTechnologies> =
    [
        readTechCsv
            "data/tech-armor.csv"
            (fun basics line ->
                ArmorTech (basics,
                    strength = Convert.ToDouble line.[5] * 1.0<armorStrength/hs>,
                    duraniumRatio = Convert.ToDouble line.[6],
                    neutroniumRatio = Convert.ToDouble line.[7]
                )
            )
        readTechCsv
            "data/tech-geo-sensors.csv"
            (fun basics line ->
                GeoSensorTech (basics,
                    sensorRating = Convert.ToInt32 line.[5] * 1</comp>,
                    crewPerComp = Convert.ToInt32 line.[6] * 1<people/comp>,
                    hsPerComp = Convert.ToInt32 line.[7] * 1<hs/comp>,
                    uridiumCost = Convert.ToDouble line.[8] * 1.0</comp>
                )
            )
        readTechCsv
            "data/tech-grav-sensors.csv"
            (fun basics line ->
                GravSensorTech (basics,
                    sensorRating = Convert.ToInt32 line.[5] * 1</comp>,
                    crewPerComp = Convert.ToInt32 line.[6] * 1<people/comp>,
                    hsPerComp = Convert.ToInt32 line.[7] * 1<hs/comp>,
                    uridiumCost = Convert.ToDouble line.[8] * 1.0</comp>
                )
            )
        readTechCsv
            "data/tech-cargo-handling.csv"
            (fun basics line ->
                CargoHandlingTech (basics,
                    tractorStrength = Convert.ToInt32 line.[5] * 1<tractorStrength/comp>,
                    crewPerComp = Convert.ToInt32 line.[6] * 1<people/comp>,
                    hsPerComp = Convert.ToInt32 line.[7] * 1<hs/comp>,
                    duraniumCost = Convert.ToDouble line.[8] * 1.0</comp>,
                    mercassiumCost = Convert.ToDouble line.[9] * 1.0</comp>
                )
            )
        readTechCsv
            "data/tech-cargo-holds.csv"
            (fun basics line ->
                CargoHoldTech (basics,
                    cargoCapacity = Convert.ToInt32 line.[5] * 1<cargoCapacity/comp>,
                    crewPerComp = Convert.ToInt32 line.[6] * 1<people/comp>,
                    hsPerComp = Convert.ToInt32 line.[7] * 1<hs/comp>,
                    duraniumCost = Convert.ToDouble line.[8] * 1.0</comp>
                )
            )
        readTechCsv
            "data/tech-engines.csv"
            (fun basics line ->
                EngineTech (basics,
                    powerPerHs = Convert.ToDouble line.[5] * 1.0<ep/hs>
                )
            )
        readTechCsv
            "data/tech-engine-efficiency.csv"
            (fun basics line ->
                EngineEfficiencyTech (basics,
                    efficiency = Convert.ToDouble line.[5] * 1000.0<kl/hr/ep>
                )
            )
        readTechCsv
            "data/tech-engine-power-mod.csv"
            (fun basics line ->
                EngineBoostUnlockTech (basics,
                    unlocked = parseList Convert.ToDouble line.[5]
                )
            )
        readTechCsv
            "data/tech-engine-thermal-efficiency.csv"
            (fun basics line ->
                EngineThermalTech (basics,
                    thermalEfficiency = Convert.ToDouble line.[5] * 1.0<therm/ep>,
                    costMultiplier = Convert.ToDouble line.[6]
                )
            )
        readTechCsv
            "data/tech-magazine-efficiency.csv"
            (fun basics line ->
                MagazineEfficiencyTech (basics,
                    ammoDensity = Convert.ToDouble line.[5] * 20.0<ammo/hs>
                )
            )
        readTechCsv
            "data/tech-magazine-ejection.csv"
            (fun basics line ->
                MagazineEjectionTech (basics,
                    ejectionChance = Convert.ToDouble line.[5]
                )
            )
        readTechCsv
            "data/tech-reactors.csv"
            (fun basics line ->
                ReactorTech (basics,
                    powerOutput = Convert.ToDouble line.[5] * 1.0<power/hs>
                )
            )
        readTechCsv
            "data/tech-reactors-power-boost.csv"
            (fun basics line ->
                ReactorBoostTech (basics,
                    powerBoost = Convert.ToDouble line.[5],
                    explosionChance = Convert.ToDouble line.[6]
                )
            )
    ]
    |> Promise.Parallel
    |> Promise.map (fun techs ->
        {
            Technologies =
                techs
                |> Seq.collect Seq.toList
                |> Seq.map (fun tech -> (tech.Id, tech))
                |> Map.ofSeq
        }
    )