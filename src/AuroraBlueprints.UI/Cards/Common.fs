module Cards.Common

open System

open Global

open Fable.React
open Fable.React.Props

open State.Msg
open Bulma.Card
open Nerds.Common

open Comp.Ship
open Comp.ShipComponent
open Model.Technology
open Model.Measures

type ShipComponentCardHeaderItem =
    | Name of string
    | Nerd of INerd

let renderNerd (nerd: INerd) =
    div [ HTMLAttr.Title nerd.Tooltip ]
        [ str nerd.Text
          i [ ClassName << Nerds.Icon.render <| nerd.Icon ] [] ]

let inline private renderHeader header: CardHeaderElement list =
    header
    |> List.map (fun h ->
        match h with
        | Name name -> Title name
        | Nerd nerd ->
            match nerd.Render with
            | false -> NoRender
            | true ->
                nerd
                |> renderNerd
                |> Info)

type BoundNameFieldsProps =
    { Name: string
      Manufacturer: string
      GeneratedName: string
      OnNameChange: string -> ShipComponent
      OnManufacturerChange: string -> ShipComponent }

let inline nameFields (props: BoundNameFieldsProps) dispatch =
    fragment []
        [ Bulma.FC.WithLabel "Name"
              [ Bulma.FC.AddonGroup
                  [ Bulma.FC.TextInput props.Name
                        (props.OnNameChange
                         >> Msg.UpdateComponent
                         >> dispatch)
                    Bulma.FC.Button "Generate" Bulma.FC.ButtonOpts.Empty (fun _ ->
                        props.GeneratedName
                        |> props.OnNameChange
                        |> Msg.UpdateComponent
                        |> dispatch) ] ]
          Bulma.FC.WithLabel "Manufacturer"
              [ Bulma.FC.TextInput props.Manufacturer
                    (props.OnManufacturerChange
                     >> Msg.UpdateComponent
                     >> dispatch) ] ]

let inline boundStringField ship dispatch lbl getter setter =
    Bulma.FC.WithLabel lbl
        [ Bulma.FC.TextInput getter
              (setter
               >> State.Msg.UpdateComponent
               >> dispatch) ]

type BoundFloatFieldProps<'a, [<Measure>] 'b> =
    { Label: string
      Value: float<'b>
      OnChange: float<'b> -> 'a }

let inline compFloatField (props: BoundFloatFieldProps<ShipComponent, 'b>) dispatch =
    Bulma.FC.FloatInput
        { Label = Some props.Label
          Value = props.Value }
        (props.OnChange
         >> Msg.UpdateComponent
         >> dispatch)

let inline shipFloatField (props: BoundFloatFieldProps<Ship, 'b>) dispatch =
    Bulma.FC.FloatInput
        { Label = Some props.Label
          Value = props.Value }
        (props.OnChange
         >> Msg.ReplaceShip
         >> dispatch)

type BoundIntFieldProps<'a, [<Measure>] 'b> =
    { Label: string
      Value: int<'b>
      Min: int option
      Max: int option
      Disabled: bool
      OnChange: int<'b> -> 'a }

let inline compIntField (props: BoundIntFieldProps<ShipComponent, 'b>) dispatch =
    Bulma.FC.IntInput
        { Label = Some props.Label
          Value = props.Value
          Min = props.Min
          Max = props.Max
          Disabled = props.Disabled }
        (props.OnChange
         >> Msg.UpdateComponent
         >> dispatch)

let inline shipIntField (props: BoundIntFieldProps<Ship, 'b>) dispatch =
    Bulma.FC.IntInput
        { Label = Some props.Label
          Value = props.Value
          Min = props.Min
          Max = props.Max
          Disabled = false }
        (props.OnChange
         >> Msg.ReplaceShip
         >> dispatch)

type BountCountFieldProps<'a> =
    { Value: int<comp>
      Ship: Ship
      Component: ShipComponent }

let inline countField (props: BountCountFieldProps<Ship>) dispatch =
    Bulma.FC.IntInput
        { Label = Some "Count"
          Value = props.Value
          Min = Some 0
          Max = None
          Disabled = false } (fun n -> Msg.SetComponentCount(props.Ship, props.Component, n) |> dispatch)

type BoundFloatChoiceProps<[<Measure>] 'a> =
    { Label: string
      Value: float<'a>
      Options: float<'a> list
      AvailableOptions: float<'a> list
      GetName: float<'a> -> string
      OnChange: float<'a> -> ShipComponent }

let inline floatChoiceField (props: BoundFloatChoiceProps<'a>) dispatch =
    Bulma.FC.Select
        { Label = Some props.Label
          Options =
              props.Options |> List.map (fun v ->
                                   {| Key = v.ToString()
                                      Text = props.GetName v
                                      Disallowed = not <| (List.contains v props.AvailableOptions) |})
          Value = props.Value.ToString() }
        (Double.Parse
         >> float2float
         >> props.OnChange
         >> Msg.UpdateComponent
         >> dispatch)

type BoundTechFieldProps<'a, 'b when 'a :> TechBase> =
    { CurrentTech: GameObjectId list
      Label: string
      Value: 'a
      Options: 'a list
      GetName: 'a -> string
      OnChange: 'a -> 'b }

let inline compTechField (props: BoundTechFieldProps<'a, ShipComponent>) dispatch =
    Bulma.FC.Select
        { Label = Some props.Label
          Options =
              props.Options |> List.map (fun v ->
                                   {| Key = v.Id.ToString()
                                      Text = props.GetName v
                                      Disallowed = not <| (List.contains v.Id props.CurrentTech) |})
          Value = props.Value.Id.ToString() } (fun n ->
        props.OnChange(List.find (fun t -> t.Id = n) props.Options)
        |> Msg.UpdateComponent
        |> dispatch)

let inline shipTechField (props: BoundTechFieldProps<'a, Ship>) dispatch =
    Bulma.FC.Select
        { Label = Some props.Label
          Options =
              props.Options |> List.map (fun v ->
                                   {| Key = v.Id.ToString()
                                      Text = props.GetName v
                                      Disallowed = not <| (List.contains v.Id props.CurrentTech) |})
          Value = props.Value.Id.ToString() } (fun n ->
        props.OnChange(List.find (fun t -> t.Id = n) props.Options)
        |> Msg.ReplaceShip
        |> dispatch)

type BountTechCountFieldProps<'a when 'a :> TechBase and 'a: comparison> =
    { Values: Map<'a, int<comp>>
      CurrentTech: GameObjectId list
      AllTechs: 'a list
      GetName: 'a -> string
      OnChange: 'a -> int<comp> -> ShipComponent }

let inline techCountFields (props: BountTechCountFieldProps<'a>) dispatch =
    props.AllTechs
    |> List.map (fun tech ->
        compIntField
            { Label = tech.Name
              Value =
                  props.Values
                  |> Map.tryFind tech
                  |> Option.defaultValue 0<comp>
              Min = Some 0
              Max = None
              Disabled = not <| List.contains tech.Id props.CurrentTech
              OnChange = fun n -> props.OnChange tech n } dispatch)

let shipComponentCard key header contents actions expanded dispatch =
    Bulma.Card.render
    <| Bulma.Card.CardProps
        (name = key, headerItems = renderHeader header, contents = contents, actions = actions,
         expander =
             { IsExpanded = expanded
               OnExpanderToggled = (fun expanded -> State.Msg.SetSectionExpanded(key, expanded) |> dispatch) })
