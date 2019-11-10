module Global

open System

type GameObjectId = string
module GameObjectId =
    let generate () = Guid.NewGuid().ToString()

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

let inline (%+) (m: Map<GameObjectId, 'b>) (v: ^b) =
    m |> Map.add ((^b) : (member Id : GameObjectId) (v)) v
    
let inline (%-) (m: Map<GameObjectId, 'b>) (v: ^b) =
    m |> Map.remove ((^b) : (member Id : GameObjectId) (v))

let inline (@%%) (a: Map<'a, 'b>) (b: Map<'a, 'b>) =
    Map.toSeq a
    |> Seq.append (Map.toSeq b)
    |> Map.ofSeq

module String =
    let inline split (ch: char[]) (count: int) (s: string) = s.Split(ch, count)

// lol
let inline (|||>) (a, b, c) f = f a b c
let inline (||||>) (a, b, c, d) f = f a b c d
let inline (|||||>) (a, b, c, d, e) f = f a b c d e
