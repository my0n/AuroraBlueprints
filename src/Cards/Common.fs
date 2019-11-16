module Cards.Common

open System

open Global
open Model.Measures

open Fable.React
open Fable.React.Props
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
                        (setName >> App.Msg.UpdateComponent >> dispatch)
                    Bulma.FC.Button
                        "Generate"
                        Bulma.FC.ButtonOpts.Empty
                        (fun _ -> App.Msg.UpdateComponent (setName getGeneratedName) |> dispatch)
                ]
        ]

let inline boundStringField ship dispatch lbl getter setter =
    Bulma.FC.WithLabel
        lbl
        [
            Bulma.FC.TextInput
                getter
                (setter >> App.Msg.UpdateComponent >> dispatch)
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
        (setter >> App.Msg.ReplaceShip >> dispatch)

let inline boundIntField ship dispatch lbl (min, max) getter setter =
    Bulma.FC.IntInput
        {
            Label = Some lbl
            Value = getter
            Min = min
            Max = max
            Disabled = false
        }
        (setter >> App.Msg.UpdateComponent >> dispatch)

let inline boundCountField ship comp dispatch lbl count =
    Bulma.FC.IntInput
        {
            Label = Some lbl
            Value = count
            Min = Some 0
            Max = None
            Disabled = false
        }
        (fun n -> App.Msg.SetComponentCount (ship, comp, n) |> dispatch)

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
                        Disallowed = not <| (List.contains v availableOptions)
                    |}
                )
            Value = getter.ToString()
        }
        (Double.Parse >> setter >> App.Msg.UpdateComponent >> dispatch)

let inline boundTechField<'a when 'a :> TechBase> (currentTech: GameObjectId list) lbl (options: 'a list) (nameFn: 'a -> string) (getter: 'a) (cb: 'a -> unit) = 
    Bulma.FC.Select
        {
            Label = Some lbl
            Options =
                options
                |> List.map (fun v ->
                    {|
                        Key = v.Id.ToString()
                        Text = nameFn v
                        Disallowed = not <| (List.contains v.Id currentTech)
                    |}
                )
            Value = getter.Id.ToString()
        }
        (fun n -> cb (List.find (fun t -> t.Id = n) options))

let shipComponentCard key header contents actions =
    Bulma.Card.render {
        key = key
        HeaderItems = renderHeader header
        Contents = contents
        Actions = actions
        HasExpanderToggle = true
    }
