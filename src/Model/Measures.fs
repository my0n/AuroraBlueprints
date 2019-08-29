module Model.Measures

open System

[<Measure>] type ep
[<Measure>] type l
[<Measure>] type kl
[<Measure>] type ton
[<Measure>] type hs
[<Measure>] type hr
[<Measure>] type s
[<Measure>] type km
[<Measure>] type comp
[<Measure>] type people
[<Measure>] type mo
let literToKiloliterConversion = 1000.0<l/kl>
let tonToHSConversion = 50.0<ton/hs>
let inline toKiloliters (liters: float<l>) = liters / literToKiloliterConversion
let inline hs2ton (hs: float<hs>) = hs * tonToHSConversion
let inline ton2hs (t: float<ton>) = t / tonToHSConversion

// fable doesn't support .FloatWithMeasure etc. so that's a box and unbox (parens are important otherwise you get a newobj, call, and callvirt lmao)
let inline int2float (x: int<'t>): float<'t> = unbox (float x)
let inline float2int (x: float<'t>): int<'t> = unbox (int x)

// ceil doesn't support UOM afaik, also I wanted an int
let inline ceiluom (a: float<'t>): int<'t> = unbox (int (ceil (float a)))
let inline flooruom (a: float<'t>): int<'t> = unbox (int (floor (float a)))
let inline powuom (n: float<'u>) (a: float<'t>): float<'t> = unbox (Math.Pow(float a, float n))
let inline rounduom (multiple: float<'t>) (a: float<'t>): float<'t> = Math.Round(a / multiple, MidpointRounding.AwayFromZero) * multiple
