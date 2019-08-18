module Global

type Page =
    | Ships

module List =
    let wrap element = [ element ]

module Map =
    let keys m = m |> Map.toSeq |> Seq.map (fun (a, b) -> a)
    let values m = m |> Map.toSeq |> Seq.map (fun (a, b) -> b)

let toHash page =
    match page with
    | Ships -> "#ships"

let inline (@+) (l: 'a list) (a: 'a) = l @ [a]

let inline (%+) (m: Map<'a, 'b>) (k: 'a, v: 'b) =
    m |> Map.add k v
    
let inline (%-) (m: Map<'a, 'b>) (k: 'a) =
    m |> Map.remove k
