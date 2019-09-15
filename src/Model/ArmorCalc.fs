module Model.ArmorCalc

open System
open Model.BuildCost
open Model.Measures
open Model.Technology

type ArmorCalculation =
    {
        Size: float<hs>
        Area: float
        Volume: int<hs>
        Strength: float
        Width: int
        Cost: TotalBuildCost
    }
    static member Zero
        with get() =
            {
                Size = 0.0<hs>
                Area = 0.0
                Volume = 0<hs>
                Strength = 0.0
                Width = 0
                Cost = TotalBuildCost.Zero
            }

let private surfaceArea (volume: float<hs>) =
    4.0 * Math.PI * (powuom (2.0/3.0) ((3.0/(4.0*Math.PI)) * volume))

let private materials costFactor technology =
    { TotalBuildCost.Zero with
        BuildPoints = costFactor
        Duranium = technology.DuraniumRatio * costFactor
        Neutronium = technology.NeutroniumRatio * costFactor
    }

let private increaseCoverage shipSize (technology: ArmorTech) calc layer =
    let area = (int2float calc.Volume |> surfaceArea |> rounduom 10.0<hs>) * 1.0</hs>
    let strength = float layer * area / 4.0
    let size = (strength / technology.Strength) |> rounduom 10.0<hs>
    let volume = ceiluom (shipSize + calc.Size)
    {
        Size = size
        Area = area
        Strength = strength
        Volume = volume
        Width = 0
        Cost = TotalBuildCost.Zero
    }

let rec private addLayer shipSize technology calc layer =
    let applied = increaseCoverage shipSize technology calc layer
    match applied.Size with
    | size when calc.Size = size -> applied
    | _ -> addLayer shipSize technology applied layer

let shipArmor (shipSize: float<hs>) (depth: int) technology =
    let calc =
        match depth with
        | depth when depth < 1 -> ArmorCalculation.Zero
        | _ -> Seq.fold (addLayer shipSize technology) ArmorCalculation.Zero {1 .. depth + 1}

    { calc with
        Cost = materials calc.Area technology
        Width = flooruom (calc.Strength / float depth)
    }
