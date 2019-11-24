module Cards.CargoHold

open Global

open App.Msg
open App.Model.UI
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

let render (comp: CargoHold) (model: App.Model.Model) (ship: Ship) dispatch =
    let currentTech = model.CurrentTechnology
    let allTechs = model.AllTechnologies
    let key = ship.Id.ToString() + comp.Id.ToString()
    let expanded = model |> Model.isExpanded key

    let header =
        [
            Name "Cargo Hold"
            Nerd { TotalBuildCost = comp.BuildCost * 1<comp> }
            Nerd { RenderMode = HS; Count = 1<comp>; Size = comp.Size }
            Nerd { CargoCapacity = comp.CargoCapacity * 1<comp> }
            Nerd { TractorStrength = comp.TractorStrength * 1<comp>; LoadTime = ship.LoadTime }
        ]
    let form =
        [
            Bulma.FC.HorizontalGroup
                None
                (
                    allTechs.CargoHolds
                    |> List.map (fun tech ->
                        Bulma.FC.IntInput
                            {
                                Label = Some tech.Name
                                Value =
                                    comp.CargoHolds
                                    |> Map.tryFind tech
                                    |> Option.defaultValue 0<comp>
                                Min = Some 0
                                Max = None
                                Disabled = not <| List.contains tech.Id currentTech
                            }
                            (fun n ->
                                CargoHold
                                    { comp with
                                        CargoHolds = comp.CargoHolds.Add (tech, n)
                                    }
                                |> Msg.UpdateComponent
                                |> dispatch
                            )
                    )
                )
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
                                Disabled = not <| List.contains tech.Id currentTech
                            }
                            (fun n ->
                                CargoHold
                                    { comp with
                                        CargoHandlingSystems = comp.CargoHandlingSystems.Add (tech, n)
                                    }
                                |> Msg.UpdateComponent
                                |> dispatch
                            )
                    )
                )
        ]
    let actions =
        [
            "Remove", DangerColor, (fun _ -> Msg.RemoveComponentFromShip (ship, CargoHold comp) |> dispatch)
        ]
    shipComponentCard key header form actions expanded dispatch
