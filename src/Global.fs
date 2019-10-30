module Global

open System
open Fable.PowerPack

module List =
    let inline wrap element = [ element ]
    let inline lookup (m: Map<'key, 'value>) =
        List.map m.TryFind
        >> List.choose id
    let inline tryFindMap (fn: 't -> 'u option) =
        Seq.map fn
        >> Seq.choose id
        >> Seq.tryFind (fun _ -> true)
    let inline withPrev l =
        l
        |> List.mapi (fun i a ->
            match i with
            | 0 -> (None, a)
            | _ -> (Some l.[i - 1], a)
        )
    let inline scani folder state list =
        list
        |> List.scan (fun (i, prev) current ->
            (i + 1, folder i prev current)
        ) (0, state)
        |> List.map (fun (i, c) -> c)

module Seq =
    let inline ofType<'a> items =
        items
        |> Seq.cast<obj>
        |> Seq.filter (fun x -> x :? 'a)
        |> Seq.cast<'a>

module Map =
    let inline keys m = m |> Map.toList |> List.map (fun (a, b) -> a)
    let inline values m = m |> Map.toList |> List.map (fun (a, b) -> b)
    let inline toListV vfn m =
        m
        |> Map.toList
        |> List.map (fun (k, v) ->
            (k, vfn v)
        )
    let inline mapKvp fn = Map.toSeq >> Seq.map (fun (k, v) -> fn k v) >> Seq.toList

let inline toTuple a b = (a, b)

let inline (@+) (l: 'a list) (a: 'a) = l @ [a]
let inline (@+?) (l: 'a list) (a: 'a option) = l @ (match a with Some a -> [a] | None -> [])
let inline (@-) (l: 'a list) (a: 'a) = l |> List.except [a]

let inline (%+) (m: Map<Guid, 'b>) (v: ^b) =
    m |> Map.add ((^b) : (member Guid : Guid) (v)) v
    
let inline (%-) (m: Map<Guid, 'b>) (v: ^b) =
    m |> Map.remove ((^b) : (member Guid : Guid) (v))

let inline (@%%) (a: Map<'a, 'b>) (b: Map<'a, 'b>) =
    Map.toSeq a
    |> Seq.append (Map.toSeq b)
    |> Map.ofSeq

module Promise =
    let cache (fn: unit -> Fable.Import.JS.Promise<'a>): Fable.Import.JS.Promise<'a> =
        let cached = ref None
        match cached.Value with
        | Some a ->
            promise {
                return a
            }
        | None ->
            promise {
                let! a = fn ()
                cached := Some a
                return a
            }
    let memoize (fn: 'a -> Fable.Import.JS.Promise<'b>) key: Fable.Import.JS.Promise<'b> =
        let cached = ref Map.empty
        match cached.Value.TryFind key with
        | Some a ->
            promise {
                return a
            }
        | None ->
            promise {
                let! a = fn key
                cached := cached.Value.Add (key, a)
                return a
            }
