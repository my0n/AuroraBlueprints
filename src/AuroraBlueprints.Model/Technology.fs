module Model.Technology

open System

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
            | _ -> invalidArg "obj" "not a TechBase"
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
    member val Efficiency: float<l/hr/ep> = efficiency with get

type FuelStorageTech(basics, fuelCapacity, hsPerComp, duraniumCost, boronideCost) =
    inherit TechBase(basics)
    override this.Category = LogisticsAndGroundCombat
    member val FuelCapacity: int<l/comp> = fuelCapacity with get
    member val HsPerComp: float<hs/comp> = hsPerComp with get
    member val DuraniumCost: float</comp> = duraniumCost with get
    member val BoronideCost: float</comp> = boronideCost with get

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

type TroopTransportTech(basics, isMilitary, troopTransportCapacity, combatDropCapacity, cryoDropCapacity, crewPerComp, hsPerComp, duraniumCost, mercassiumCost, neutroniumCost) =
    inherit TechBase(basics)
    override this.Category = LogisticsAndGroundCombat
    member val IsMilitary: bool = isMilitary with get
    member val TroopTransportCapacity: int<company/comp> = troopTransportCapacity with get
    member val CombatDropCapacity: int<company/comp> = combatDropCapacity with get
    member val CryoDropCapacity: int<company/comp> = cryoDropCapacity with get
    member val CrewPerComp: int<people/comp> = crewPerComp with get
    member val HsPerComp: float<hs/comp> = hsPerComp with get
    member val DuraniumCost: float</comp> = duraniumCost with get
    member val MercassiumCost: float</comp> = mercassiumCost with get
    member val NeutroniumCost: float</comp> = neutroniumCost with get

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
    member this.FuelStorages            = this.Technologies |> techsOfType<FuelStorageTech>        |> List.sortBy (fun tech -> tech.Level)
    member this.GeoSensors              = this.Technologies |> techsOfType<GeoSensorTech>          |> List.sortBy (fun tech -> tech.Level)
    member this.GravSensors             = this.Technologies |> techsOfType<GravSensorTech>         |> List.sortBy (fun tech -> tech.Level)
    member this.MagazineEfficiency      = this.Technologies |> techsOfType<MagazineEfficiencyTech> |> List.sortBy (fun tech -> tech.Level)
    member this.MagazineEjection        = this.Technologies |> techsOfType<MagazineEjectionTech>   |> List.sortBy (fun tech -> tech.Level)
    member this.Reactors                = this.Technologies |> techsOfType<ReactorTech>            |> List.sortBy (fun tech -> tech.Level)
    member this.ReactorsPowerBoost      = this.Technologies |> techsOfType<ReactorBoostTech>       |> List.sortBy (fun tech -> tech.Level)
    member this.TroopTransports         = this.Technologies |> techsOfType<TroopTransportTech>     |> List.sortBy (fun tech -> tech.Level)

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
        |> List.sort

    member this.UnlockedPowerMods (researchedTechs: GameObjectId list) =
        this.EnginePowerMod
        |> List.filter (fun tech -> List.contains tech.Id researchedTechs)
        |> List.collect (fun tech -> tech.UnlockedPowerMods)

    member this.DefaultPowerMod =
        (
            this.EnginePowerMod
            |> List.find (fun tech -> tech.Level = 1)
        ).UnlockedPowerMods.[0]