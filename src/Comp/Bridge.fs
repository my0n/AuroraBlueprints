module Comp.Bridge

open System
open Model.Measures
open Model.BuildCost

type Bridge =
    {
        Guid: Guid

        Count: int<comp>
    }
    static member Zero
        with get() =
            {
                Guid = Guid.NewGuid()
                Count = 1<comp>
            }
    member this.Size = 1<hs/comp>
    member this.TotalSize = hs2tonint <| this.Size * this.Count
    member this.Crew = 5<people/comp>
    member this.BuildCost =
        { BuildCost.Zero with
            BuildPoints = 10.0</comp>
            Duranium = 5.0</comp>
            Corbomite = 5.0</comp>
        }
