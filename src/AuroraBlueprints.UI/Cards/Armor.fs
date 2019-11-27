module Cards.Armor

open State.UI
open State.Model

open Cards.Common
open Comp.Ship

open Model.Measures

open Nerds.ArmorSizeNerd
open Nerds.ArmorStrengthNerd
open Nerds.PriceTotalNerd
open Nerds.SizeNerd

let render (model: State.Model.Model) (ship: Ship) dispatch =
    let currentTech = model.CurrentTechnology
    let allTechs = model.AllTechnologies
    let key = ship.Id.ToString() + "armor"
    let expanded = model |> Model.isExpanded key

    let header =
        [ Name ship.ArmorTechnology.Name
          Nerd { TotalBuildCost = ship.ArmorBuildCost }
          Nerd
              { RenderMode = HS
                Count = 1<comp>
                Size = ship.ArmorSize * 1<1/comp> }
          Nerd { Strength = ship.ArmorStrength }
          Nerd
              { Width = ship.ArmorWidth
                Depth = ship.ArmorDepth } ]
    let form =
        Bulma.FC.HorizontalGroup None
            [ shipIntField
                { Label = "Armor Depth"
                  Value = ship.ArmorDepth
                  Min = Some 1
                  Max = None
                  Disabled = false
                  OnChange = fun n -> { ship with ArmorDepth = n } } dispatch
              shipTechField
                  { CurrentTech = currentTech
                    Label = "Armor Technology"
                    Value = ship.ArmorTechnology
                    Options = allTechs.Armor
                    GetName = fun t -> t.Name
                    OnChange = fun n -> { ship with ArmorTechnology = n } } dispatch ]

    let actions = []
    shipComponentCard key header [ form ] actions expanded dispatch
