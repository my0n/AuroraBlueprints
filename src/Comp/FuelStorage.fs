module Comp.FuelStorage

open System
open Model.BuildCost
open Model.Measures

type FuelStorage =
    {
        Guid: Guid
        ShipGuid: Guid

        Tiny: int<comp>
        Small: int<comp>
        Standard: int<comp>
        Large: int<comp>
        VeryLarge: int<comp>
        UltraLarge: int<comp>
        
        // calculated values
        TotalSize: float<hs>
        FuelCapacity: float<kl>
        BuildCost: TotalBuildCost
    }
    static member Zero
        with get() =
            {
                Guid = Guid.NewGuid()
                ShipGuid = Guid.Empty

                Tiny = 0<comp>
                Small = 0<comp>
                Standard = 0<comp>
                Large = 0<comp>
                VeryLarge = 0<comp>
                UltraLarge = 0<comp>

                TotalSize = 0.0<hs>
                FuelCapacity = 0.0<kl>
                BuildCost = TotalBuildCost.Zero
            }
    member this.calculate =
        // cost does not scale proportionately to size and capacity
        let cost = (int2float this.Tiny * 1.0
                  + int2float this.Small * 1.5
                  + int2float this.Standard * 5.0
                  + int2float this.Large * 15.0
                  + int2float this.VeryLarge * 35.0
                  + int2float this.UltraLarge * 100.0) * 1.0</comp>
        { this with
            TotalSize = (int2float this.Tiny * 0.1
                       + int2float this.Small * 0.2
                       + int2float this.Standard * 1.0
                       + int2float this.Large * 5.0
                       + int2float this.VeryLarge * 20.0
                       + int2float this.UltraLarge * 100.0) * 1.0<hs/comp>
            FuelCapacity = (int2float this.Tiny * 1.0
                          + int2float this.Small * 2.0
                          + int2float this.Standard * 10.0
                          + int2float this.Large * 50.0
                          + int2float this.VeryLarge * 200.0
                          + int2float this.UltraLarge * 1000.0) * 5.0<kl/comp>
            BuildCost =
                { TotalBuildCost.Zero with
                    BuildPoints = cost * 2.0
                    Duranium = cost
                    Boronide = cost
                }
        }
