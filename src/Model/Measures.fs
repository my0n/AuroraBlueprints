module Model.Measures

open System

[<Measure>] type ep
[<Measure>] type l
[<Measure>] type kl
[<Measure>] type ton
[<Measure>] type armorStrength
[<Measure>] type hs
[<Measure>] type hsSA
[<Measure>] type km
[<Measure>] type therm
[<Measure>] type comp
[<Measure>] type people
[<Measure>] type year
[<Measure>] type mo
[<Measure>] type day
[<Measure>] type hr
[<Measure>] type min
[<Measure>] type s
[<Measure>] type power
[<Measure>] type company
[<Measure>] type battalion
[<Measure>] type rp
[<Measure>] type cargoCapacity
[<Measure>] type tractorStrength

// fable doesn't support .FloatWithMeasure etc. so that's a box and unbox (parens are important otherwise you get a newobj, call, and callvirt)
let inline int2float (x: int<'t>): float<'t> = unbox (float x)
let inline float2int (x: float<'t>): int<'t> = unbox (int x)
let inline int2int<[<Measure>] 't> (x: int): int<'t> = unbox x
let inline float2float<[<Measure>] 't> (x: float): float<'t> = unbox x

// ceil doesn't support UOM afaik, also I wanted an int
let inline ceiluom (a: float<'t>): int<'t> = unbox (int (ceil (float a)))
let inline flooruom (a: float<'t>): int<'t> = unbox (int (floor (float a)))
let inline powuom (n: float<'u>) (a: float<'t>): float<'t> = unbox (Math.Pow(float a, float n))
let inline rounduom (multiple: float<'t>) (a: float<'t>): float<'t> = Math.Round(a / multiple, MidpointRounding.AwayFromZero) * multiple

// normal conversions
let inline l2kl (liters: float<l>) = liters / 1000.0<l/kl>
let inline kl2l (kiloliters: float<kl>) = kiloliters * 1000.0<l/kl>
let inline mo2year (mo: float<mo>) = mo / 12.0<mo/year>
let inline mo2day (mo: float<mo>) = mo * 30.0<day/mo>
let inline day2mo (day: float<day>) = day / 30.0<day/mo>
let inline day2hr (day: float<day>) = day * 24.0<hr/day>
let inline hr2day (hr: float<hr>) = hr / 60.0<hr/day>
let inline hr2min (hr: float<hr>) = hr * 60.0<min/hr>
let inline min2hr (min: float<min>) = min / 60.0<min/hr>
let inline min2s (min: float<min>) = min * 60.0<s/min>
let inline kphr2kps (kphr: float<km/hr>) = kphr / 60.0<min/hr> / 60.0<s/min>
let inline kps2kphr (kps: float<km/s>) = kps * 60.0<min/hr> * 60.0<s/min>
let inline company2battalion (c: int<company>) = float c / 5.0</battalion>

// hs conversions
let inline hs2ton (hs: float<hs>) = hs * 50.0<ton/hs>
let inline hs2tonint (hs: int<hs>) = hs * 50<ton/hs>
let inline ton2hs (t: float<ton>) = t / 50.0<ton/hs>
let inline ton2hsint (t: int<ton>) = t / 50<ton/hs>

let inline hs2sa (volume: float<hs>) = 4.0 * Math.PI * (powuom (2.0/3.0) ((3.0/(4.0*Math.PI)) * volume)) * 1.0<hsSA/hs>
