module Comp.Bridge

open Global
open Model.Measures
open Model.BuildCost

type Bridge =
    {
        Id: GameObjectId
        Locked: bool
        BuiltIn: bool
    }
    static member Zero
        with get() =
            {
                Id = GameObjectId.generate()
                Locked = false
                BuiltIn = false
            }
    member this.Size = 1<hs/comp>
    member this.Crew = 5<people/comp>
    member this.BuildCost =
        { BuildCost.Zero with
            BuildPoints = 10.0</comp>
            Duranium = 5.0</comp>
            Corbomite = 5.0</comp>
        }
