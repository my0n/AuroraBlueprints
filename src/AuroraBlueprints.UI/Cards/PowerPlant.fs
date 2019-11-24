module Cards.PowerPlant

open Model.Measures
open State.Msg
open State.Model
open State.UI
open Bulma.Card
open Cards.Common
open Comp.PowerPlant
open Comp.ShipComponent
open Comp.Ship
open System
open Global

open Nerds.PriceNerd
open Nerds.SizeNerd
open Nerds.PowerProductionNerd

let render (comp: PowerPlant) (count: int<comp>) (model: State.Model.Model) (ship: Ship) dispatch =
    let currentTech = model.CurrentTechnology
    let allTechs = model.AllTechnologies
    let key = ship.Id.ToString() + comp.Id.ToString()
    let expanded = model |> Model.isExpanded key

    let header =
        [
            Name comp.Name
            Nerd { Count = count; BuildCost = comp.BuildCost }
            Nerd { Count = count; RenderMode = HS; Size = float2int <| comp.Size * 50.0<ton/hs> }
            Nerd { Count = count; PowerOutput = comp.Power }
        ]

    let sizeOptions =
        [ 0.1 .. 0.1 .. 0.9 ] @ [1.0 .. 1.0 .. 30.0]
        |> List.map (fun o -> o * 1.0<hs/comp>)

    let form =
        [
            Bulma.FC.HorizontalGroup
                None
                [
                    boundCountField ship (PowerPlant comp) dispatch
                        "Count"
                        count
                    Bulma.FC.WithLabel
                        "Name"
                        [
                            Bulma.FC.AddonGroup
                                [
                                    Bulma.FC.TextInput
                                        comp.Name
                                        (fun n -> Msg.UpdateComponent (PowerPlant { comp with Name = n }) |> dispatch)
                                    Bulma.FC.Button
                                        "Generate"
                                        Bulma.FC.ButtonOpts.Empty
                                        (fun _ -> Msg.UpdateComponent (PowerPlant { comp with Name = comp.GeneratedName }) |> dispatch)
                                ]
                        ]
                    Bulma.FC.WithLabel
                        "Manufacturer"
                        [
                            Bulma.FC.TextInput
                                comp.Manufacturer
                                (fun n -> Msg.UpdateComponent (PowerPlant { comp with Manufacturer = n }) |> dispatch)
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
                                        Key = i.ToString()
                                        Text = sprintf "%.1f" o
                                        Disallowed = false
                                    |}
                                )
                            Value =
                                (sizeOptions
                                 |> List.tryFindIndex (fun o -> o = comp.Size)
                                 |> Option.defaultValue 1
                                ).ToString()
                        }
                        (fun n -> Msg.UpdateComponent (PowerPlant { comp with Size = sizeOptions.[Int32.Parse(n)]}) |> dispatch)
                    boundTechField currentTech
                        "Power Plant Technology"
                        allTechs.Reactors
                        (fun t -> String.Format("{0} power - {1}", t.PowerOutput, t.Name))
                        comp.Technology
                        (fun n -> State.Msg.UpdateComponent (PowerPlant { comp with Technology = n }) |> dispatch)
                    boundTechField currentTech
                        "Power Boost"
                        allTechs.ReactorsPowerBoost
                        (fun t -> t.Name)
                        comp.PowerBoost
                        (fun n -> State.Msg.UpdateComponent (PowerPlant { comp with PowerBoost = n }) |> dispatch)
                ]
        ]
    let actions =
        [
            "Remove", DangerColor, (fun _ -> Msg.RemoveComponentFromShip (ship, PowerPlant comp) |> dispatch)
        ]
    shipComponentCard key header form actions expanded dispatch
