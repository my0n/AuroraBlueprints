module Saving.LocalStorage

open Global

open Browser

let inline toKey prefix version key = prefix + "|" + version + "|" + key

let inline save prefix version key serialized =
    localStorage.setItem(toKey prefix version key, serialized)

let inline delete prefix version key _ =
    localStorage.removeItem(toKey prefix version key)

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