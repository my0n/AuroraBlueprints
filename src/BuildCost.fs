module BuildCost

open Measures

type BuildCost =
    {
        BuildPoints: float</comp>
        Gallicite: float</comp>
    }
    static member empty =
        {
            BuildPoints = 0.0</comp>
            Gallicite = 0.0</comp>
        }
