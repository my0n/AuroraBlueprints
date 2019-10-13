module Cards.Common

open Global

open Fable.Helpers.React
open Fable.Helpers.React.Props
open Bulma.Card
open Nerds.Common

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

let inline boundTechField (tech: Set<Technology.Tech>) ship dispatch lbl options (getter: ^a) setter = 
    Bulma.FC.Select
        {
            Label = Some lbl
            Options =
                options
                |> Map.mapKvp (fun k v ->
                    {|
                        Key = k
                        Text = (^a : (member Name : string) getter)
                        Disallowed = not <| tech.Contains (^a : (member Tech : Technology.Tech) v)
                    |}
                )
            Value = (^a : (member Level : int) getter)
        }
        (fun n -> App.Msg.ReplaceShipComponent (ship, setter options.[n]) |> dispatch)

let shipComponentCard key header contents actions =
    Bulma.Card.render {
        key = key
        HeaderItems = renderHeader header
        Contents = contents
        Actions = actions
        HasExpanderToggle = true
    }
