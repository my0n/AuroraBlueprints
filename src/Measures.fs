module Measures

[<Measure>] type ep
[<Measure>] type l
[<Measure>] type kl
[<Measure>] type ton
[<Measure>] type hs
[<Measure>] type hr
[<Measure>] type s
[<Measure>] type km
let literToKiloliterConversion = 1000.0<l/kl>
let tonToHSConversion = 50.0<ton/hs>
let inline toKiloliters (liters: float<l>) = liters / literToKiloliterConversion
let inline toTons (hs: float<hs>) = hs * tonToHSConversion

// fable doesn't support .FloatWithMeasure etc. so that's a box and unbox (parens are important otherwise you get a newobj, call, and callvirt lmao)
let inline int2float (x: int<'t>): float<'t> = unbox (float x)
let inline float2int (x: float<'t>): int<'t> = unbox (int x)
