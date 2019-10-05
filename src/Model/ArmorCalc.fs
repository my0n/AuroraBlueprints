module Model.ArmorCalc

open System
open Model.BuildCost
open Model.Measures
open Model.Technology

type ArmorCalculation =
    {
        Size: float<hs>
        Area: float<hsSA>
        Volume: int<hs>
        Strength: float<armorStrength>
        Width: int
    }
    static member Zero
        with get() =
            {
                Size = 0.0<hs>
                Area = 0.0<hsSA>
                Volume = 0<hs>
                Strength = 0.0<armorStrength>
                Width = 0
            }

let private increaseCoverage shipSize (technology: Technology.ArmorTech) calc layer =
    let area =
        int2float calc.Volume
        |> hs2sa
        |> rounduom 10.0<hsSA>
    let strength =
        float layer
        * area
        / 4.0<hsSA/armorStrength>
    let size =
        strength
        / technology.Strength
        |> rounduom 10.0<hs>
    let volume =
        shipSize
        + calc.Size
        |> ceiluom
    {
        Size = size
        Area = area
        Strength = strength
        Volume = volume
        Width = 0
    }

let rec private addLayer shipSize technology calc layer =
    let applied = increaseCoverage shipSize technology calc layer
    match applied.Size with
    | size when calc.Size = size -> applied
    | _ -> addLayer shipSize technology applied layer

let shipArmor (shipSize: int<ton>) (depth: int) technology =
    let shipSize = ton2hs <| int2float shipSize
    let calc =
        match depth with
        | depth when depth < 1 -> ArmorCalculation.Zero
        | _ -> Seq.fold (addLayer shipSize technology) ArmorCalculation.Zero {1 .. depth + 1}

    { calc with
        Width = flooruom (calc.Strength * 1.0</armorStrength> / float depth)
    }
