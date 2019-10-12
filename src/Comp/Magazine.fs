module Comp.Magazine

open System
open Model.BuildCost
open Model.Measures

type Magazine =
    {
        Guid: Guid
        Name: string
        Manufacturer: string

        Count: int<comp>
        HTK: int
        Size: int<hs/comp>
        Armor: Technology.ArmorTech
        FeedSystem: Technology.MagazineFeedEfficiencyTech
        Ejection: Technology.MagazineEjectionChanceTech
    }
    static member Zero
        with get() =
            let zero =
                {
                    Guid = Guid.NewGuid()
                    Name = ""
                    Manufacturer = "Aurora Industries"

                    Count = 0<comp>
                    Size = 1<hs/comp>
                    HTK = 1
                    Armor = Technology.armor.[0]
                    FeedSystem = Technology.feedEfficiency.[0]
                    Ejection = Technology.ejectionChance.[0]
                }
            { zero with
                Name = zero.GeneratedName
            }

    //#region Calculated Values
    member private this._BuildCost =
        lazy (
            { BuildCost.Zero with
                BuildPoints = 1.0</comp>
            }
        )
    member private this._Capacity =
        lazy (
            int2float this.Size
            * this.FeedSystem.AmmoDensity
            |> ceiluom
        )
    member private this._Crew =
        lazy (
            int2float this.Size
            * 0.5<people/hs>
            |> ceiluom
        )
    member private this._GeneratedName =
        lazy (
            String.Format("Capacity {0} Magazine: Exp {1}%  HTK{2}", this.Capacity, int (this.Ejection.EjectionChance * 100.0), this.HTK)
        )
    //#endregion

    //#region Accessors
    member this.BuildCost with get() = this._BuildCost.Value
    member this.Capacity with get() = this._Capacity.Value
    member this.Crew with get() = this._Crew.Value
    member this.GeneratedName with get() = this._GeneratedName.Value
    //#endregion

