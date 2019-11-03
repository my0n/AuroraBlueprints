module Comp.Magazine

open System
open Global
open Model.BuildCost
open Model.Measures
open Technology

type Magazine =
    {
        Id: GameObjectId
        Name: string
        Manufacturer: string

        Count: int<comp>
        HTK: int
        Size: int<hs/comp>
        Armor: ArmorTech
        FeedSystem: MagazineEfficiencyTech
        Ejection: MagazineEjectionTech
    }

    //#region Calculated Values
    member private this._ArmorCalculation =
        lazy (
            let (armorSize, capacitySize, buildCost) =
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
                    (0.0<hs/comp>, int2float this.Size, capacityCost)
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
                            (int2float (htk - 1))
                            * strReq
                            |> float
                            |> (*) 1.0</comp>
                        { BuildCost.Zero with
                            BuildPoints = costFactor
                            Duranium = costFactor * this.Armor.DuraniumRatio
                            Neutronium = costFactor * this.Armor.NeutroniumRatio
                        }
                    let armorSize =
                        (int2float (htk - 1))
                        * strReq
                        / this.Armor.Strength
                        |> rounduom 0.01<hs/comp>
                    (armorSize, int2float this.Size - armorSize, armorCost + capacityCost)

            {|
                ArmorSize = armorSize
                CapacitySize = capacitySize
                BuildCost = buildCost
            |}
        )
    member private this._Capacity =
        lazy (
            this._ArmorCalculation.Value.CapacitySize
            * this.FeedSystem.AmmoDensity
            |> flooruom
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
            String.Format("Capacity {0} Magazine: Exp {1}%  HTK{2}", this.Capacity, int (100.0 - this.Ejection.EjectionChance * 100.0), this.HTK)
        )
    member private this._TotalSize =
        lazy (
            this.Size
            * this.Count
            |> hs2tonint
        )
    //#endregion

    //#region Accessors
    member this.BuildCost with get() = this._ArmorCalculation.Value.BuildCost
    member this.Capacity with get() = this._Capacity.Value
    member this.Crew with get() = this._Crew.Value
    member this.GeneratedName with get() = this._GeneratedName.Value
    member this.TotalSize with get() = this._TotalSize.Value
    //#endregion

let magazine (allTechs: AllTechnologies) =
    let zero =
        {
            Id = GameObjectId.generate()
            Name = ""
            Manufacturer = "Aurora Industries"

            Count = 0<comp>
            Size = 1<hs/comp>
            HTK = 1
            Armor = allTechs.DefaultArmor
            FeedSystem = allTechs.DefaultFeedEfficiency
            Ejection = allTechs.DefaultEjectionChance
        }
    { zero with
        Name = zero.GeneratedName
    }
