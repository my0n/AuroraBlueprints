module File.CsvReader

open Fetch
open Model.Measures
open Model.Technology
open System

let readCsv file consumeFn =
    fetch file []
    |> Promise.bind (fun res ->
        res.text ()
    )
    |> Promise.map (fun text ->
        text.Split([|'\n'|])
        |> Array.map (fun line ->
            line.Split([|','|])
        )
        |> Array.skip 1
        |> Seq.map consumeFn
    )


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
let private readTechCsv file generateFn =
    readCsv file (fun line -> generateFn (parseBasics line) line)
    |> Promise.map Seq.cast<TechBase>
    
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
                    efficiency = Convert.ToDouble line.[5] * 1.0<l/hr/ep>
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
            "data/tech-fuel-storage.csv"
            (fun basics line ->
                FuelStorageTech (basics,
                    fuelCapacity = Convert.ToInt32 line.[5] * 1<l/comp>,
                    hsPerComp = Convert.ToDouble line.[6] * 1.0<hs/comp>,
                    duraniumCost = Convert.ToDouble line.[7] * 1.0</comp>,
                    boronideCost = Convert.ToDouble line.[8] * 1.0</comp>
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
        readTechCsv
            "data/tech-troop-transports.csv"
            (fun basics line ->
                TroopTransportTech (basics,
                    isMilitary = (line.[5].ToLower() = "true"),
                    troopTransportCapacity = Convert.ToInt32 line.[6] * 1<company/comp>,
                    combatDropCapacity = Convert.ToInt32 line.[7] * 1<company/comp>,
                    cryoDropCapacity = Convert.ToInt32 line.[8] * 1<company/comp>,
                    crewPerComp = Convert.ToInt32 line.[9] * 1<people/comp>,
                    hsPerComp = Convert.ToDouble line.[10] * 1.0<hs/comp>,
                    duraniumCost = Convert.ToDouble line.[11] * 1.0</comp>,
                    mercassiumCost = Convert.ToDouble line.[12] * 1.0</comp>,
                    neutroniumCost = Convert.ToDouble line.[13] * 1.0</comp>
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