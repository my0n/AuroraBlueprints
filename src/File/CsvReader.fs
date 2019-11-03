module File.CsvReader

open Fetch

let readCsv file consumeFn =
    fetch file [ Cache RequestCache.Nostore ]
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
