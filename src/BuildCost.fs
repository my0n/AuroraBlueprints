module BuildCost

open Measures

type BuildCost =
    {
        BuildPoints: float</comp>
        Duranium: float</comp>
        Corbomite: float</comp>
        Gallicite: float</comp>
        Boronide: float</comp>
        Uridium: float</comp>
    }
    static member empty =
        {
            BuildPoints = 0.0</comp>
            Duranium = 0.0</comp>
            Corbomite = 0.0</comp>
            Gallicite = 0.0</comp>
            Boronide = 0.0</comp>
            Uridium = 0.0</comp>
        }
