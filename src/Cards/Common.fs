module Cards.Common

open System

open Global

open Fable.Helpers.React
open Fable.Helpers.React.Props
open Bulma.Card
open Nerds.Common

open Technology

type ShipComponentCardHeaderItem =
    | Name of string
    | Nerd of INerd

let renderNerd (nerd: INerd) =
    div [ HTMLAttr.Title nerd.Tooltip ]
        [ str nerd.Text
          i [ ClassName << Nerds.Icon.render <| nerd.Icon ]
            []
        ]

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
                |> Info
    )

let inline boundNameField ship dispatch getName getGeneratedName setName =
    Bulma.FC.WithLabel
        "Name"
        [
            Bulma.FC.AddonGroup
                [
                    Bulma.FC.TextInput
                        getName
                        (fun n -> App.Msg.ReplaceShipComponent (ship, setName n) |> dispatch)
                    Bulma.FC.Button
                        "Generate"
                        (fun _ -> App.Msg.ReplaceShipComponent (ship, setName getGeneratedName) |> dispatch)
                ]
        ]

let inline boundStringField ship dispatch lbl getter setter =
    Bulma.FC.WithLabel
        lbl
        [
            Bulma.FC.TextInput
                getter
                (fun n -> App.Msg.ReplaceShipComponent (ship, setter n) |> dispatch)
        ]

let inline boundShipIntField dispatch lbl (min, max) getter setter =
    Bulma.FC.IntInput
        {
            Label = Some lbl
            Value = getter
            Min = min
            Max = max
            Disabled = false
        }
        (fun n -> App.Msg.ReplaceShip (setter n) |> dispatch)

let inline boundIntField ship dispatch lbl (min, max) getter setter =
    Bulma.FC.IntInput
        {
            Label = Some lbl
            Value = getter
            Min = min
            Max = max
            Disabled = false
        }
        (fun n -> App.Msg.ReplaceShipComponent (ship, setter n) |> dispatch)

let inline boundFloatChoiceField (availableOptions: float list) ship dispatch lbl (options: float list) (getter: float) (nameFn: float -> string) (setter: float -> Comp.ShipComponent.ShipComponent) = 
    Bulma.FC.Select
        {
            Label = Some lbl
            Options =
                options
                |> List.map (fun v ->
                    {|
                        Key = v.ToString()
                        Text = nameFn v
                        Disallowed = not <| (List.exists (fun t -> t = getter) availableOptions)
                    |}
                )
            Value = getter.ToString()
        }
        (fun n -> App.Msg.ReplaceShipComponent (ship, setter options.[n]) |> dispatch)

let inline boundTechField<'a when 'a :> TechBase> (currentTech: Guid list) ship dispatch lbl (options: 'a list) (getter: 'a) (setter: 'a -> Comp.ShipComponent.ShipComponent) = 
    Bulma.FC.Select
        {
            Label = Some lbl
            Options =
                options
                |> List.map (fun v ->
                    {|
                        Key = v.Guid.ToString()
                        Text = v.Name
                        Disallowed = not <| (List.exists (fun t -> t = getter.Guid) currentTech)
                    |}
                )
            Value = getter.Guid.ToString()
        }
        (fun n -> App.Msg.ReplaceShipComponent (ship, setter options.[n]) |> dispatch)

let inline boundShipTechField<'a when 'a :> TechBase> (currentTech: Guid list) dispatch lbl (options: 'a list) (getter: 'a) (setter: 'a -> Comp.Ship.Ship) = 
    Bulma.FC.Select
        {
            Label = Some lbl
            Options =
                options
                |> List.map (fun v ->
                    {|
                        Key = v.Guid.ToString()
                        Text = v.Name
                        Disallowed = not <| (List.exists (fun t -> t = getter.Guid) currentTech)
                    |}
                )
            Value = getter.Guid.ToString()
        }
        (fun n -> App.Msg.ReplaceShip (setter options.[n]) |> dispatch)

let shipComponentCard key header contents actions =
    Bulma.Card.render {
        key = key
        HeaderItems = renderHeader header
        Contents = contents
        Actions = actions
        HasExpanderToggle = true
    }
