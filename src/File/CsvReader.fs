module File.CsvReader

open Fable.PowerPack.Fetch
open Fable.PowerPack

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
        |> Seq.toList
    )
