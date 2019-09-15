module Bulma.Card

open Global
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.Core
open Fable.Import

type CardHeaderIcon =
    | Close
    | Weight
    | GasPump
    | Tachometer
    | AngleDoubleRight
    | Dollar
    | GlobeAmericas
    | Shield
    | AngleDown

type CardHeaderElement =
    | Title of string
    | Info of string * string * CardHeaderIcon
    | Button of CardHeaderIcon * (unit -> unit)
    | NoRender

type ColorStatus =
    | NormalColor
    | InfoColor
    | SuccessColor
    | WarningColor
    | DangerColor

let inline private renderIcon ic =
    match ic with
    | Close -> "fas fa-times"
    | Weight -> "fas fa-weight-hanging"
    | GasPump -> "fas fa-gas-pump"
    | Tachometer -> "fas fa-tachometer-alt"
    | AngleDoubleRight -> "fas fa-angle-double-right"
    | Dollar -> "fas fa-dollar-sign"
    | GlobeAmericas -> "fas fa-globe-americas"
    | Shield -> "fas fa-shield-alt"
    | AngleDown -> "fas fa-angle-down"

type CardProps =
    {
        HeaderItems: CardHeaderElement list
        Contents: React.ReactElement list
        Actions: (string * ColorStatus * (unit -> unit)) list
        HasExpanderToggle: bool
    }
    
type CardState =
    {
        IsBodyVisible: bool
    }

type private Card(props) =
    inherit React.Component<CardProps, CardState>(props)
    do base.setInitState({ IsBodyVisible = true })

    member this.toggleState _ =
        this.setState(fun s p ->
            { s with
                IsBodyVisible = not s.IsBodyVisible
            }
        )

    override this.render () =
        let h =
            let mutable headers = this.props.HeaderItems
            if this.props.HasExpanderToggle then do
                headers <- headers @ [ Button (AngleDown, this.toggleState) ]

            match headers
                  |> List.map (fun hi ->
                      match hi with
                      | Title t ->
                          div [ ClassName "card-header-title" ] [ str t ]
                          |> Some
                      | Info (t, hover, ic) ->
                          div [ ClassName "card-header-subtitle"; HTMLAttr.Title hover ] [ str t; i [ ClassName (renderIcon ic) ] [] ]
                          |> Some
                      | Button (ic, cb) ->
                          a [ ClassName "card-header-icon"
                              OnClick (fun event ->
                                event.stopPropagation() |> ignore
                                event.preventDefault() |> ignore
                                cb()
                              )
                            ]
                            [ span [ ClassName "icon" ]
                                   [ i [ ClassName (renderIcon ic) ] [] ]
                            ]
                          |> Some
                      | NoRender -> None
                  )
                  |> List.choose id with
            | [] -> None
            | li -> Some <| header [ ClassName "card-header"
                                     OnClick (fun event ->
                                       event.stopPropagation() |> ignore
                                       event.preventDefault() |> ignore
                                       this.toggleState event
                                     )
                                   ]
                                   li
        let c = div [ classList [ "card-content", true; "is-hidden", not this.state.IsBodyVisible ] ] this.props.Contents
        let f =
            match this.props.Actions with
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
                |> footer [ classList [ "card-footer", true; "is-hidden", not this.state.IsBodyVisible ] ]
                |> Some
        [ h
          Some c
          f
        ]
        |> List.choose id
        |> div [ ClassName "card" ]

let render props = ofType<Card,_,_> props []
