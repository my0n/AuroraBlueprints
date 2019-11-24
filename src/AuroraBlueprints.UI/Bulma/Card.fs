module Bulma.Card

open Global
open Fable.React
open Fable.React.Props
open Nerds.Icon

type CardHeaderElement =
    | Title of string
    | Info of ReactElement
    | Button of Icon * (unit -> unit)
    | NoRender

type ColorStatus =
    | NormalColor
    | InfoColor
    | SuccessColor
    | WarningColor
    | DangerColor

type ExpanderProps =
    {
        IsExpanded: bool
        OnExpanderToggled: bool -> unit
    }

type CardProps
    (
        contents: ReactElement list,
        ?headerItems: CardHeaderElement list,
        ?actions: (string * ColorStatus * (unit -> unit)) list,
        ?name: string,
        ?expander: ExpanderProps
    ) =
    member __.Name = defaultArg name ""
    member __.HeaderItems = defaultArg headerItems []
    member __.Expander = expander
    member __.Contents = contents
    member __.Actions = defaultArg actions []

let render (props: CardProps) =
    let expanded =
        props.Expander
        |> Option.map (fun expander -> expander.IsExpanded)
        |> Option.defaultValue true

    let headerItems =
        let renderTitle title =
            div [ ClassName "card-header-title" ] [ str title ]

        let renderInfo el =
            div [ ClassName "card-header-subtitle" ] [ el ]

        let renderButton icon cb =
            a
                [
                    ClassName "card-header-icon"
                    OnClick (fun event ->
                        event.stopPropagation() |> ignore
                        event.preventDefault() |> ignore
                        cb()
                    )
                ]
                [
                    span
                        [ ClassName "icon" ]
                        [ i [ ClassName (Nerds.Icon.render icon) ] [] ]
                ]
    
        let expanderCB =
            props.Expander
            |> Option.map (fun expander ->
                (fun _ ->
                    not expander.IsExpanded
                    |> expander.OnExpanderToggled
                )
            )

        props.HeaderItems
        @
        match expanderCB with
        | Some cb -> [ Button (AngleDown, cb) ]
        | None -> []
        |> List.map (function
            | Title t -> Some <| renderTitle t
            | Info el -> Some <| renderInfo el
            | Button (ic, cb) -> Some <| renderButton ic cb
            | NoRender -> None
        )
        |> List.choose id
        |> function
        | [] -> None
        | li ->
            header
                [
                    ClassName "card-header"
                    OnClick (fun event ->
                        match expanderCB with
                        | Some cb ->
                            event.stopPropagation() |> ignore
                            event.preventDefault() |> ignore
                            cb()
                        | None -> ()
                    )
                ]
                li
            |> Some

    let contents =
        div [
                classList
                    [
                        "card-content", true
                        "is-hidden", not expanded
                    ]
            ]
            props.Contents

    let actionBar =
        match props.Actions with
        | [] -> None
        | li ->
            li
            |> List.map (fun (lbl, st, cb) ->
                a [ classList [ "card-footer-item", true
                                "has-text-info", st = InfoColor
                                "has-text-success", st = SuccessColor
                                "has-text-warning", st = WarningColor
                                "has-text-danger", st = DangerColor
                              ]
                    OnClick (fun event ->
                                event.stopPropagation() |> ignore
                                cb()
                            )
                  ]
                  [ str lbl ]
            )
            |> footer [ classList [ "card-footer", true; "is-hidden", not expanded ] ]
            |> Some

    [
        headerItems
        Some contents
        actionBar
    ]
    |> List.choose id
    |> div [ ClassName "card" ]
