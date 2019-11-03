module Cards.CargoHold

open Global
open System

open App.Msg
open Bulma.Card
open Cards.Common
open Comp.CargoHold
open Comp.ShipComponent
open Comp.Ship

open Model.Measures

open Nerds.CargoCapacityNerd
open Nerds.PriceTotalNerd
open Nerds.SizeNerd
open Nerds.TractorStrengthNerd

open Technology

let render (allTechs: AllTechnologies) (tech: GameObjectId list) (ship: Ship) (comp: CargoHold) dispatch =
    let header =
        [
            Name "Cargo Hold"
            Nerd { TotalBuildCost = comp.BuildCost }
            Nerd { RenderMode = HS; Count = 1<comp>; Size = comp.TotalSize*1</comp> }
            Nerd { CargoCapacity = comp.CargoCapacity }
            Nerd { TractorStrength = comp.TractorStrength; LoadTime = ship.LoadTime }
        ]
    let form =
        [
            Bulma.FC.HorizontalGroup
                None
                [
                    Bulma.FC.IntInput
                        {
                            Label = Some "Tiny Cargo Hold"
                            Value = comp.Tiny
                            Min = Some 0
                            Max = None
                            Disabled = false
                        }
                        (fun n -> Msg.ReplaceShipComponent (ship, CargoHold { comp with Tiny = n }) |> dispatch)
                    Bulma.FC.IntInput
                        {
                            Label = Some "Small Cargo Hold"
                            Value = comp.Small
                            Min = Some 0
                            Max = None
                            Disabled = false
                        }
                        (fun n -> Msg.ReplaceShipComponent (ship, CargoHold { comp with Small = n }) |> dispatch)
                    Bulma.FC.IntInput
                        {
                            Label = Some "Standard Cargo Hold"
                            Value = comp.Standard
                            Min = Some 0
                            Max = None
                            Disabled = false
                        }
                        (fun n -> Msg.ReplaceShipComponent (ship, CargoHold { comp with Standard = n }) |> dispatch)
                ]
            Bulma.FC.HorizontalGroup
                None
                (
                    allTechs.CargoHandling
                    |> List.map (fun tech ->
                        Bulma.FC.IntInput
                            {
                                Label = Some tech.Name
                                Value =
                                    comp.CargoHandlingSystems
                                    |> Map.tryFind tech
                                    |> Option.defaultValue 0<comp>
                                Min = Some 0
                                Max = None
                                Disabled = false
                            }
                            (fun n ->
                                Msg.ReplaceShipComponent
                                    (
                                        ship,
                                        CargoHold
                                            { comp with
                                                CargoHandlingSystems = comp.CargoHandlingSystems.Add (tech, n)
                                            }
                                    )
                                |> dispatch
                            )
                        )
                )
        ]
    let actions =
        [
            "Remove", DangerColor, (fun _ -> Msg.RemoveComponentFromShip (ship, CargoHold comp) |> dispatch)
        ]
    shipComponentCard (comp.Id.ToString ()) header form actions
