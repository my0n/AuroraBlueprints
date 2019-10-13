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
    member private this._ArmorCalculation =
        lazy (
            let (armorSize, buildCost) =
                let capacityCost =
                    let costFactor =
                        this.Size * 5</hs>
                        |> int2float
                    { BuildCost.Zero with
                        BuildPoints = costFactor
                        Duranium = costFactor * 0.75
                        Tritanium = costFactor * 0.25
                    }

                match this.HTK with
                | htk when htk <= 1 ->
                    (0.0<hs>, capacityCost)
                | htk ->
                    let area =
                        this.Size
                        |> (*) 1<comp>
                        |> int2float
                        |> hs2sa
                        |> (*) 1.0</comp>
                    let strReq =
                        area * 0.1<armorStrength/hsSA>
                        |> rounduom 0.01<armorStrength/comp>
                    let armorCost =
                        let costFactor =
                            strReq * (int2float (htk - 1))
                            |> float
                            |> (*) 1.0</comp>
                        { BuildCost.Zero with
                            BuildPoints = costFactor
                            Duranium = costFactor * this.Armor.DuraniumRatio
                            Neutronium = costFactor * this.Armor.NeutroniumRatio
                        }
                    (armorSize, armorCost + capacityCost)

            {|
                ArmorSize = armorSize
                BuildCost = buildCost
            |}

            //   if (!technology) throw 'technology is not defined before calculating armor'
            //   let strPerHS = technology.strengthPerHS;

            //   if (isNaN(size)) throw 'size is NaN before calculating magazine armor'
            //   if (isNaN(htk)) throw 'htk is NaN before calculating magazine armor'

            //   let armorSize = 0;
            //   let area = getArea(size / 50);
            //   let materials = getMaterials(0, technology);
            //   if (htk > 1) {
            //     let strReq = Math.round(area * 10) / 100; // constants intentional
            //     armorSize = (htk - 1) * strReq / strPerHS;
            //     let armorCost = cost + strReq * (htk - 1);
            //     materials = getMaterials(armorCost, technology);
            //   }

            //   armorSize = Math.round(armorSize * 100) / 2; 
            //   let capacitySize = Math.max(0, size - armorSize * 50); // size of the actual contents of the magazine

            //   return { capacitySize, armorSize, materials };
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
            |> max 1<people/comp>
        )
    member private this._GeneratedName =
        lazy (
            String.Format("Capacity {0} Magazine: Exp {1}%  HTK{2}", this.Capacity, int (this.Ejection.EjectionChance * 100.0), this.HTK)
        )
    //#endregion

    //#region Accessors
    member this.BuildCost with get() = this._ArmorCalculation.Value.BuildCost
    member this.Capacity with get() = this._Capacity.Value
    member this.Crew with get() = this._Crew.Value
    member this.GeneratedName with get() = this._GeneratedName.Value
    //#endregion

