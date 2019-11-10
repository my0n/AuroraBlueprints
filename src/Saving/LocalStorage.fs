module Saving.LocalStorage

open Global

open Browser
open Thoth.Json

let inline toKey prefix version key = prefix + "|" + version + "|" + key

let inline save prefix key version serialized =
    let serialized = Encode.Auto.toString (0, serialized)
    localStorage.setItem(toKey prefix version key, serialized)

type LoadedData<'t> =
    | Success of 't
    | NotFound
    | Failure of string

let inline load prefix deserializer =
    Seq.init localStorage.length id
    |> Seq.map (
        float
        >> localStorage.key
        >> String.split [|'|'|] 3
    )
    |> Seq.filter (function
        | [|a; _; _|] -> a = prefix
        | _ -> false
    )
    |> Seq.map (function
        | [|a; b; c|] ->
            toKey a b c
            |> localStorage.getItem
            |> deserializer b c
        | _ -> failwith "filter didn't filter!"
    )