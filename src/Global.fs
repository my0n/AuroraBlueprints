module Global

open System

type Page =
    | Ships

module List =
    let wrap element = [ element ]
    let lookup (m: Map<'key, 'value>) =
        List.map (fun key ->
            m.TryFind key
        )
        >> List.choose id

module Map =
    let keys m = m |> Map.toList |> List.map (fun (a, b) -> a)
    let values m = m |> Map.toList |> List.map (fun (a, b) -> b)

let toHash page =
    match page with
    | Ships -> "#ships"
    
let inline (@+) (l: 'a list) (a: 'a) = l @ [a]
let inline (@+?) (l: 'a list) (a: 'a option) = l @ (match a with Some a -> [a] | None -> [])
let inline (@-) (l: 'a list) (a: 'a) = l |> List.except [a]

let inline (%+) (m: Map<Guid, 'b>) (v: ^b) =
    m |> Map.add ((^b) : (member Guid : Guid) (v)) v
    
let inline (%-) (m: Map<Guid, 'b>) (v: ^b) =
    m |> Map.remove ((^b) : (member Guid : Guid) (v))
