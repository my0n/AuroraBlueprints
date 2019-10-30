module Technology

open System

open File.CsvReader
open Model.Measures
open Global
open Fable.PowerPack

type ParsedBasics =
    {
        Guid: Guid
        Name: string
        Cost: int<rp>
        Level: int
        Parents: Guid list
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
        Guid = Guid.Parse line.[0]
        Name = line.[1]
        Cost = Convert.ToInt32 line.[2] * 1<rp>
        Level = Convert.ToInt32 line.[3]
        Parents = parseList Guid.Parse line.[4]
    }

let private readTechCsv file generateFn =
    readCsv file (fun line -> generateFn (parseBasics line) line)

type TechCategory =
    | DefensiveSystems
    | LogisticsAndGroundCombat
    | MissilesAndKineticWeapons
    | PowerAndPropulsion
    | SensorsAndFireControl

[<AbstractClass>]
type TechBase(basics: ParsedBasics) =
    member this.Guid with get() = basics.Guid
    member this.Name with get() = basics.Name
    member this.Cost with get() = basics.Cost
    member this.Parents with get() = basics.Parents
    member this.Level with get() = basics.Level
    abstract member Category: TechCategory with get

type ArmorTech(basics, strength, duraniumRatio, neutroniumRatio) =
    inherit TechBase(basics)
    override this.Category = DefensiveSystems
    member val Strength: float<armorStrength/hs> = strength with get
    member val DuraniumRatio: float = duraniumRatio with get
    member val NeutroniumRatio: float = neutroniumRatio with get

type CargoHandlingTech(basics, tractorStrength) =
    inherit TechBase(basics)
    override this.Category = LogisticsAndGroundCombat
    member val TractorStrength: float = tractorStrength with get

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

type GravSensorTech(basics) =
    inherit TechBase(basics)
    override this.Category = SensorsAndFireControl

type GeoSensorTech(basics) =
    inherit TechBase(basics)
    override this.Category = SensorsAndFireControl

let rec private researchedParents (allTechs: Map<Guid, TechBase>) researchedTechs unprocessed processed =
    match unprocessed with
    | [] -> processed
    | x::xs ->
        let tech = allTechs.[x]
        match List.contains tech researchedTechs with
        | false -> researchedParents allTechs researchedTechs xs processed
        | true -> researchedParents allTechs researchedTechs (xs @ tech.Parents) (x::processed)

type AllTechnologies =
    {
        Technologies: Map<Guid, TechBase>
    }
    member this.Item with get guid = this.Technologies.[guid]

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
    member this.GetParents  guid = this.Technologies.[guid].Parents
    member this.GetChildren guid = this._Children.Value.[guid]

    member private this.TechsOfType<'a> () =
        this.Technologies
        |> Map.values
        |> Seq.ofType<'a>
        |> Seq.toList

    member this.Armor                    = this.TechsOfType<ArmorTech>()              |> List.sortBy (fun tech -> tech.Level)
    member this.CargoHandling            = this.TechsOfType<CargoHandlingTech>()      |> List.sortBy (fun tech -> tech.Level)
    member this.Engines                  = this.TechsOfType<EngineTech>()             |> List.sortBy (fun tech -> tech.Level)
    member this.EngineEfficiency         = this.TechsOfType<EngineEfficiencyTech>()   |> List.sortBy (fun tech -> tech.Level)
    member this.EnginePowerMod           = this.TechsOfType<EngineBoostUnlockTech>()  |> List.sortBy (fun tech -> tech.Level)
    member this.EngineThermalEfficiency  = this.TechsOfType<EngineThermalTech>()      |> List.sortBy (fun tech -> tech.Level)
    member this.GeoSensors               = this.TechsOfType<GeoSensorTech>()          |> List.sortBy (fun tech -> tech.Level)
    member this.GravSensors              = this.TechsOfType<GravSensorTech>()         |> List.sortBy (fun tech -> tech.Level)
    member this.MagazineEfficiency       = this.TechsOfType<MagazineEfficiencyTech>() |> List.sortBy (fun tech -> tech.Level)
    member this.MagazineEjection         = this.TechsOfType<MagazineEjectionTech>()   |> List.sortBy (fun tech -> tech.Level)
    member this.Reactors                 = this.TechsOfType<ReactorTech>()            |> List.sortBy (fun tech -> tech.Level)
    member this.ReactorsPowerBoost       = this.TechsOfType<ReactorBoostTech>()       |> List.sortBy (fun tech -> tech.Level)

    member this.DefaultArmor             = this.Armor.[0]
    member this.DefaultEngine            = this.Engines.[0]
    member this.DefaultEngineEfficiency  = this.EngineEfficiency.[0]
    member this.DefaultThermalEfficiency = this.EngineThermalEfficiency.[0]
    member this.DefaultFeedEfficiency    = this.MagazineEfficiency.[0]
    member this.DefaultEjectionChance    = this.MagazineEjection.[0]
    member this.DefaultReactor           = this.Reactors.[0]
    member this.DefaultPowerBoost        = this.ReactorsPowerBoost.[0]

    member this.ImprovedCargoHandlingSystem = Guid "D9E7C6FB-D00C-424D-AC8E-EC8FFBCC5FE5"
    member this.AdvancedCargoHandlingSystem = Guid "D5EAB881-B970-45C9-8518-AB3D0B2FF2A4"
    member this.GravAssistedCargoHandlingSystem = Guid "D425D3A3-8818-4491-A25B-7C4B108939E9"

    member this.GeologicalSurveySensors = Guid "A2D19D2F-EF64-4DFC-B4B2-8838EEDAAC50"
    member this.ImprovedGeologicalSurveySensors = Guid "E77F9805-35E0-4E97-B399-32F00BA52563"
    member this.AdvancedGeologicalSurveySensors = Guid "723ECE17-627E-4CDB-B0B8-D46A60F6FA23"
    member this.PhasedGeologicalSurveySensors = Guid "8B8001E6-F983-4229-9892-B82363D73C49"

    member this.GravitationalSurveySensors = Guid "558A6A1F-2CEB-41A7-867D-2EA00447B9B7"
    member this.ImprovedGravitationalSurveySensors = Guid "CB243D80-E18E-4173-B89D-745DE66F7846"
    member this.AdvancedGravitationalSurveySensors = Guid "E4BB5DD3-5801-4D17-8770-60ABE08A7496"
    member this.PhasedGravitationalSurveySensors = Guid "8A72916C-3957-4002-98B3-BB3FD04BE35A"

    member this.AllPowerMods =
        this.EnginePowerMod
        |> List.collect (fun tech -> tech.UnlockedPowerMods)

    member this.UnlockedPowerMods (researchedTechs: Guid list) =
        this.EnginePowerMod
        |> List.filter (fun tech -> List.contains tech.Guid researchedTechs)
        |> List.collect (fun tech -> tech.UnlockedPowerMods)

    member this.DefaultPowerMod =
        (
            this.EnginePowerMod
            |> List.find (fun tech -> tech.Level = 1)
        ).UnlockedPowerMods.[0]

let allTechnologies: Fable.Import.JS.Promise<AllTechnologies> =
    Promise.cache <| fun _ ->
        promise {
            let! armor =
                readTechCsv
                    "data/tech-armor.csv"
                    (fun basics line ->
                        ArmorTech (basics,
                            strength = Convert.ToDouble line.[5] * 1.0<armorStrength/hs>,
                            duraniumRatio = Convert.ToDouble line.[6],
                            neutroniumRatio = Convert.ToDouble line.[7]
                        )
                    )
            let! geoSensors =
                readTechCsv
                    "data/tech-geo-sensors.csv"
                    (fun basics line -> GeoSensorTech basics)
            let! gravSensors =
                readTechCsv
                    "data/tech-grav-sensors.csv"
                    (fun basics line -> GravSensorTech basics)
            let! cargoHandling =
                readTechCsv
                    "data/tech-cargo-handling.csv"
                    (fun basics line ->
                        CargoHandlingTech (basics,
                            tractorStrength = Convert.ToDouble line.[5]
                        )
                    )
            let! engine =
                readTechCsv
                    "data/tech-engines.csv"
                    (fun basics line ->
                        EngineTech (basics,
                            powerPerHs = Convert.ToDouble line.[5] * 1.0<ep/hs>
                        )
                    )
            let! engineEfficiency =
                readTechCsv
                    "data/tech-engine-efficiency.csv"
                    (fun basics line ->
                        EngineEfficiencyTech (basics,
                            efficiency = Convert.ToDouble line.[5] * 1000.0<kl/hr/ep>
                        )
                    )
            let! enginePowerMod =
                readTechCsv
                    "data/tech-engine-power-mod.csv"
                    (fun basics line ->
                        EngineBoostUnlockTech (basics,
                            unlocked = parseList Convert.ToDouble line.[5]
                        )
                    )
            let! engineThermalEfficiency =
                readTechCsv
                    "data/tech-engine-thermal-efficiency.csv"
                    (fun basics line ->
                        EngineThermalTech (basics,
                            thermalEfficiency = Convert.ToDouble line.[5] * 1.0<therm/ep>,
                            costMultiplier = Convert.ToDouble line.[6]
                        )
                    )
            let! magazineEfficiency =
                readTechCsv
                    "data/tech-magazine-efficiency.csv"
                    (fun basics line ->
                        MagazineEfficiencyTech (basics,
                            ammoDensity = Convert.ToDouble line.[5] * 20.0<ammo/hs>
                        )
                    )
            let! magazineEjection =
                readTechCsv
                    "data/tech-magazine-ejection.csv"
                    (fun basics line ->
                        MagazineEjectionTech (basics,
                            ejectionChance = Convert.ToDouble line.[5]
                        )
                    )
            let! reactors =
                readTechCsv
                    "data/tech-reactors.csv"
                    (fun basics line ->
                        ReactorTech (basics,
                            powerOutput = Convert.ToDouble line.[5] * 1.0<power/hs>
                        )
                    )
            let! reactorsPowerBoost =
                readTechCsv
                    "data/tech-reactors-power-boost.csv"
                    (fun basics line ->
                        ReactorBoostTech (basics,
                            powerBoost = Convert.ToDouble line.[5],
                            explosionChance = Convert.ToDouble line.[6]
                        )
                    )
            return (
                {
                    Technologies =
                        [
                            Seq.cast<TechBase> armor
                            Seq.cast<TechBase> cargoHandling
                            Seq.cast<TechBase> engine
                            Seq.cast<TechBase> engineEfficiency
                            Seq.cast<TechBase> enginePowerMod
                            Seq.cast<TechBase> engineThermalEfficiency
                            Seq.cast<TechBase> geoSensors
                            Seq.cast<TechBase> gravSensors
                            Seq.cast<TechBase> magazineEfficiency
                            Seq.cast<TechBase> magazineEjection
                            Seq.cast<TechBase> reactors
                            Seq.cast<TechBase> reactorsPowerBoost
                        ]
                        |> List.collect Seq.toList
                        |> List.map (fun tech -> (tech.Guid, tech))
                        |> Map.ofList
                }
            )
        }