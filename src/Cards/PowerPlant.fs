module Cards.PowerPlant

open Model.Measures
open App.Msg
open Bulma.Card
open Cards.Common
open Model.Measures
open Comp.PowerPlant
open Comp.ShipComponent
open Comp.Ship
open System
open Global

open Nerds.PriceNerd
open Nerds.SizeNerd
open Nerds.PowerProductionNerd

let render (tech: Technology.TechBase list) (ship: Ship) (comp: PowerPlant) dispatch =
    let header =
        [
            Name comp.Name
            Nerd { Count = comp.Count; BuildCost = comp.BuildCost }
            Nerd { RenderMode = HS; Count = comp.Count; Size = float2int <| comp.Size * 50.0<ton/hs> }
            Nerd { Count = comp.Count; PowerOutput = comp.Power }
        ]

    let sizeOptions =
        [ 0.1 .. 0.1 .. 0.9 ] @ [1.0 .. 1.0 .. 30.0]
        |> List.map (fun o -> o * 1.0<hs/comp>)

    let form =
        [
            Bulma.FC.HorizontalGroup
                None
                [
                    Bulma.FC.IntInput
                        {
                            Label = Some "Count"
                            Value = comp.Count
                            Min = Some 0
                            Max = None
                            Disabled = false
                        }
                        (fun n -> Msg.ReplaceShipComponent (ship, PowerPlant { comp with Count = n }) |> dispatch)
                    Bulma.FC.WithLabel
                        "Name"
                        [
                            Bulma.FC.AddonGroup
                                [
                                    Bulma.FC.TextInput
                                        comp.Name
                                        (fun n -> Msg.ReplaceShipComponent (ship, PowerPlant { comp with Name = n }) |> dispatch)
                                    Bulma.FC.Button
                                        "Generate"
                                        (fun _ -> Msg.ReplaceShipComponent (ship, PowerPlant { comp with Name = comp.GeneratedName }) |> dispatch)
                                ]
                        ]
                    Bulma.FC.WithLabel
                        "Manufacturer"
                        [
                            Bulma.FC.TextInput
                                comp.Manufacturer
                                (fun n -> Msg.ReplaceShipComponent (ship, PowerPlant { comp with Manufacturer = n }) |> dispatch)
                        ]
                ]
            Bulma.FC.HorizontalGroup
                None
                [
                    Bulma.FC.Select
                        {
                            Label = Some "Size"
                            Options =
                                sizeOptions
                                |> List.mapi (fun i o ->
                                    {|
                                        Key = i
                                        Text = sprintf "%.1f" o
                                        Disallowed = false
                                    |}
                                )
                            Value =
                                sizeOptions
                                |> List.tryFindIndex (fun o -> o = comp.Size)
                                |> Option.defaultValue 1
                        }
                        (fun n -> Msg.ReplaceShipComponent (ship, PowerPlant { comp with Size = sizeOptions.[n]}) |> dispatch)
                    boundTechField tech ship dispatch
                        "Power Plant Technology"
                        Technology.reactors
                        comp.Technology
                        (fun n -> PowerPlant { comp with Technology = n })
                    boundTechField tech ship dispatch
                        "Power Boost"
                        Technology.powerBoost
                        comp.PowerBoost
                        (fun n -> PowerPlant { comp with PowerBoost = n })
                ]
        ]
    let actions =
        [
            "Remove", DangerColor, (fun _ -> Msg.RemoveComponentFromShip (ship, PowerPlant comp) |> dispatch)
        ]
    shipComponentCard (comp.Guid.ToString ()) header form actions
