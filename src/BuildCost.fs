module BuildCost


type BuildCost =
    {
        BuildPoints: float
        Gallicite: float
    }
    static member empty =
        {
            BuildPoints = 0.0
            Gallicite = 0.0
        }
