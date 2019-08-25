module BuildCost

open Measures

type TotalBuildCost =
    {
        BuildPoints: float
        Duranium: float
        Corbomite: float
        Gallicite: float
        Boronide: float
        Uridium: float
        Mercassium: float
    }
    static member (+) (a: TotalBuildCost, b: TotalBuildCost) =
        {
            BuildPoints = a.BuildPoints + b.BuildPoints
            Duranium = a.Duranium + b.Duranium
            Corbomite = a.Corbomite + b.Corbomite
            Gallicite = a.Gallicite + b.Gallicite
            Boronide = a.Boronide + b.Boronide
            Uridium = a.Uridium + b.Uridium
            Mercassium = a.Mercassium + b.Mercassium
        }
    static member Zero
        with get() =
            {
                BuildPoints = 0.0
                Duranium = 0.0
                Corbomite = 0.0
                Gallicite = 0.0
                Boronide = 0.0
                Uridium = 0.0
                Mercassium = 0.0
            }

type BuildCost =
    {
        BuildPoints: float</comp>
        Duranium: float</comp>
        Corbomite: float</comp>
        Gallicite: float</comp>
        Boronide: float</comp>
        Uridium: float</comp>
        Mercassium: float</comp>
    }
    static member Zero
        with get() =
            {
                BuildPoints = 0.0</comp>
                Duranium = 0.0</comp>
                Corbomite = 0.0</comp>
                Gallicite = 0.0</comp>
                Boronide = 0.0</comp>
                Uridium = 0.0</comp>
                Mercassium = 0.0</comp>
            }
    static member (*) (a: BuildCost, b: int<comp>): TotalBuildCost =
        {
            BuildPoints = a.BuildPoints * int2float b
            Duranium = a.Duranium * int2float b
            Corbomite = a.Corbomite * int2float b
            Gallicite = a.Gallicite * int2float b
            Boronide = a.Boronide * int2float b
            Uridium = a.Uridium * int2float b
            Mercassium = a.Mercassium * int2float b
        }
    
