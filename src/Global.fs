module Global

open System

[<Measure>] type l
[<Measure>] type kl
[<Measure>] type ton
[<Measure>] type hs
let literToKiloliterConversion = 1000.0<l/kl>
let tonToHSConversion = 50.0<ton/hs>
let toKiloliters (liters: float<l>) = literToKiloliterConversion / liters
let toHS (tons: float<ton>) = tonToHSConversion / tons

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
let inline (@-) (l: 'a list) (a: 'a) = l |> List.except [a]

let inline (%+) (m: Map<'a, 'b>) (v: ^b) =
    m |> Map.add ((^b) : (member Guid : Guid) (v)) v
    
let inline (%-) (m: Map<'a, 'b>) (v: ^b) =
    m |> Map.remove ((^b) : (member Guid : Guid) (v))
