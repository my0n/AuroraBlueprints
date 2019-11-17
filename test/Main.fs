module AuroraBlueprints.Tests
open Expecto

[<EntryPoint>]
let main argv =
    let writeResults = TestResults.writeNUnitSummary ("TestResults.xml", "Expecto.Tests")
    let config = defaultConfig.appendSummaryHandler writeResults
    Tests.runTestsInAssembly config argv
