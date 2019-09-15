module Comp.Sensors

open System
open Model.BuildCost
open Model.MaintenanceClass
open Model.Measures

type Sensors =
    {
        Guid: Guid
        
        StandardGeo: int<comp>
        ImprovedGeo: int<comp>
        AdvancedGeo: int<comp>
        PhasedGeo: int<comp>
        
        StandardGrav: int<comp>
        ImprovedGrav: int<comp>
        AdvancedGrav: int<comp>
        PhasedGrav: int<comp>

        // calculated values
        Size: int<hs>
        Crew: int<people>
        BuildCost: TotalBuildCost
        GeoSensorRating: int
        GravSensorRating: int
        MaintenenceClass: MaintenanceClass
    }
    static member Zero
        with get() =
            {
                Guid = Guid.NewGuid()
                StandardGeo = 0<comp>
                ImprovedGeo = 0<comp>
                AdvancedGeo = 0<comp>
                PhasedGeo = 0<comp>
                StandardGrav = 0<comp>
                ImprovedGrav = 0<comp>
                AdvancedGrav = 0<comp>
                PhasedGrav = 0<comp>

                Size = 0<hs>
                Crew = 0<people>
                BuildCost = TotalBuildCost.Zero
                GeoSensorRating = 0
                GravSensorRating = 0
                MaintenenceClass = Commercial
            }
    member this.calculate =
        let total = this.StandardGeo + this.StandardGrav
                  + this.ImprovedGeo + this.ImprovedGrav
                  + this.AdvancedGeo + this.AdvancedGrav
                  + this.PhasedGeo + this.PhasedGrav
        let cost = ((this.StandardGeo + this.StandardGrav) * 100
                  + (this.ImprovedGeo + this.ImprovedGrav) * 150
                  + (this.AdvancedGeo + this.AdvancedGrav) * 200
                  + (this.PhasedGeo + this.PhasedGrav) * 300
                   ) * 1</comp>
        let maint = match [ this.StandardGrav; this.ImprovedGrav; this.AdvancedGrav; this.PhasedGrav ]
                          |> List.exists (fun a -> a > 0<comp>) with
                    | true -> Military
                    | false -> Commercial
        { this with
            Size = total * 5<hs/comp>
            Crew = total * 10<people/comp>
            GeoSensorRating = (this.StandardGeo * 1
                             + this.ImprovedGeo * 2
                             + this.AdvancedGeo * 3
                             + this.PhasedGeo * 5
                              ) * 1</comp>
            GravSensorRating = (this.StandardGrav * 1
                              + this.ImprovedGrav * 2
                              + this.AdvancedGrav * 3
                              + this.PhasedGrav * 5
                               ) * 1</comp>
            BuildCost =
                { TotalBuildCost.Zero with
                    BuildPoints = float cost
                    Uridium = float cost
                }
            MaintenenceClass = maint
        }
