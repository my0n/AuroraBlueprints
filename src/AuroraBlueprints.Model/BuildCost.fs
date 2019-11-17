module Model.BuildCost

open Model.Measures

type TotalBuildCost =
    {
        BuildPoints: float
        Boronide: float
        Corbomite: float
        Corundium: float
        Duranium: float
        Gallicite: float
        Mercassium: float
        Neutronium: float
        Sorium: float
        Tritanium: float
        Uridium: float
        Vendarite: float
    }
    static member (+) (a: TotalBuildCost, b: TotalBuildCost) =
        {
            BuildPoints = a.BuildPoints + b.BuildPoints
            Boronide = a.Boronide + b.Boronide
            Corbomite = a.Corbomite + b.Corbomite
            Corundium = a.Corundium + b.Corundium
            Duranium = a.Duranium + b.Duranium
            Gallicite = a.Gallicite + b.Gallicite
            Mercassium = a.Mercassium + b.Mercassium
            Neutronium = a.Neutronium + b.Neutronium
            Sorium = a.Sorium + b.Sorium
            Tritanium = a.Tritanium + b.Tritanium
            Uridium = a.Uridium + b.Uridium
            Vendarite = a.Vendarite + b.Vendarite
        }
    static member (*) (a: TotalBuildCost, b: float) =
        {
            BuildPoints = a.BuildPoints * b
            Boronide = a.Boronide * b
            Corbomite = a.Corbomite * b
            Corundium = a.Corundium * b
            Duranium = a.Duranium * b
            Gallicite = a.Gallicite * b
            Mercassium = a.Mercassium * b
            Neutronium = a.Neutronium * b
            Sorium = a.Sorium * b
            Tritanium = a.Tritanium * b
            Uridium = a.Uridium * b
            Vendarite = a.Vendarite * b
        }
    static member Zero
        with get() =
            {
                BuildPoints = 0.0
                Boronide = 0.0
                Corbomite = 0.0
                Corundium = 0.0
                Duranium = 0.0
                Gallicite = 0.0
                Mercassium = 0.0
                Neutronium = 0.0
                Sorium = 0.0
                Tritanium = 0.0
                Uridium = 0.0
                Vendarite = 0.0
            }

type BuildCost =
    {
        BuildPoints: float</comp>
        Boronide: float</comp>
        Corbomite: float</comp>
        Corundium: float</comp>
        Duranium: float</comp>
        Gallicite: float</comp>
        Mercassium: float</comp>
        Neutronium: float</comp>
        Sorium: float</comp>
        Tritanium: float</comp>
        Uridium: float</comp>
        Vendarite: float</comp>
    }
    static member Zero
        with get() =
            {
                BuildPoints = 0.0</comp>
                Boronide = 0.0</comp>
                Corbomite = 0.0</comp>
                Corundium = 0.0</comp>
                Duranium = 0.0</comp>
                Gallicite = 0.0</comp>
                Mercassium = 0.0</comp>
                Neutronium = 0.0</comp>
                Sorium = 0.0</comp>
                Tritanium = 0.0</comp>
                Uridium = 0.0</comp>
                Vendarite = 0.0</comp>
            }
    static member (+) (a: BuildCost, b: BuildCost): BuildCost =
        {
            BuildPoints = a.BuildPoints + b.BuildPoints
            Boronide = a.Boronide + b.Boronide
            Corbomite = a.Corbomite + b.Corbomite
            Corundium = a.Corundium + b.Corundium
            Duranium = a.Duranium + b.Duranium
            Gallicite = a.Gallicite + b.Gallicite
            Mercassium = a.Mercassium + b.Mercassium
            Neutronium = a.Neutronium + b.Neutronium
            Sorium = a.Sorium + b.Sorium
            Tritanium = a.Tritanium + b.Tritanium
            Uridium = a.Uridium + b.Uridium
            Vendarite = a.Vendarite + b.Vendarite
        }
    static member (*) (a: BuildCost, b: int<comp>): TotalBuildCost =
        {
            BuildPoints = a.BuildPoints * int2float b
            Boronide = a.Boronide * int2float b
            Corbomite = a.Corbomite * int2float b
            Corundium = a.Corundium * int2float b
            Duranium = a.Duranium * int2float b
            Gallicite = a.Gallicite * int2float b
            Mercassium = a.Mercassium * int2float b
            Neutronium = a.Neutronium * int2float b
            Sorium = a.Sorium * int2float b
            Tritanium = a.Tritanium * int2float b
            Uridium = a.Uridium * int2float b
            Vendarite = a.Vendarite * int2float b
        }
    
