module Global

open System

module List =
    let inline wrap element = [ element ]
    let inline lookup (m: Map<'key, 'value>) =
        List.map (fun key ->
            m.TryFind key
        )
        >> List.choose id

module Map =
    let inline keys m = m |> Map.toList |> List.map (fun (a, b) -> a)
    let inline values m = m |> Map.toList |> List.map (fun (a, b) -> b)
    let inline toListV vfn m =
        m
        |> Map.toList
        |> List.map (fun (k, v) ->
            (k, vfn v)
        )
    
let inline (@+) (l: 'a list) (a: 'a) = l @ [a]
let inline (@+?) (l: 'a list) (a: 'a option) = l @ (match a with Some a -> [a] | None -> [])
let inline (@-) (l: 'a list) (a: 'a) = l |> List.except [a]

let inline (%+) (m: Map<Guid, 'b>) (v: ^b) =
    m |> Map.add ((^b) : (member Guid : Guid) (v)) v
    
let inline (%-) (m: Map<Guid, 'b>) (v: ^b) =
    m |> Map.remove ((^b) : (member Guid : Guid) (v))
