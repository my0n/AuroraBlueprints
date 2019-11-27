module Cards.Bridge

open Model.Measures

open State.Msg
open State.UI
open Bulma.Card
open Cards.Common
open Comp.Bridge
open Comp.ShipComponent
open Comp.Ship

open Nerds.PriceNerd
open Nerds.SizeNerd

let render (comp: Bridge) (count: int<comp>) (model: State.Model.Model) (ship: Ship) dispatch =
    let key = ship.Id.ToString() + comp.Id.ToString()
    let expanded = model |> Model.isExpanded key

    let header =
        [ Name "Bridge"
          Nerd
              { Count = count
                BuildCost = comp.BuildCost }
          Nerd
              { RenderMode = HS
                Count = count
                Size = comp.Size * 50<ton/hs> } ]

    let form =
        countField
            { Value = count
              Ship = ship
              Component = Bridge comp } dispatch

    let actions = [ "Remove", DangerColor, (fun _ -> Msg.RemoveComponentFromShip(ship, Bridge comp) |> dispatch) ]
    shipComponentCard key header [ form ] actions expanded dispatch
