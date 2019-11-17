module Cards.FuelStorage

open Global

open App.Msg
open Bulma.Card
open Cards.Common
open Model.Measures
open Comp.FuelStorage
open Comp.ShipComponent
open Comp.Ship

open Nerds.PriceTotalNerd
open Nerds.SizeNerd
open Nerds.FuelCapacityNerd

open Model.Technology

let render (allTechs: AllTechnologies) (currentTech: GameObjectId list) (ship: Ship) (comp: FuelStorage) dispatch =
    let header =
        [
            Name "Fuel Storage"
            Nerd { TotalBuildCost = comp.BuildCost * 1<comp> }
            Nerd { RenderMode = HS; Count = 1<comp>; Size = comp.Size }
            Nerd { FuelCapacity = comp.FuelCapacity * 1.0<comp> }
        ]
    let form =
        [
            Bulma.FC.HorizontalGroup
                None
                (
                    allTechs.FuelStorages
                    |> List.map (fun tech ->
                        Bulma.FC.IntInput
                            {
                                Label = Some tech.Name
                                Value =
                                    comp.FuelStorages
                                    |> Map.tryFind tech
                                    |> Option.defaultValue 0<comp>
                                Min = Some 0
                                Max = None
                                Disabled = not <| List.contains tech.Id currentTech
                            }
                            (fun n ->
                                Msg.UpdateComponent
                                    (
                                        FuelStorage
                                            { comp with
                                                FuelStorages = comp.FuelStorages.Add (tech, n)
                                            }
                                    )
                                |> dispatch
                            )
                    )
                )
        ]
    let actions =
        [
            "Remove", DangerColor, (fun _ -> Msg.RemoveComponentFromShip (ship, FuelStorage comp) |> dispatch)
        ]
    shipComponentCard (comp.Id.ToString ()) header form actions
