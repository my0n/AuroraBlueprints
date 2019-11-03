module Bulma.Card

open Global
open Fable.React
open Fable.React.Props
open Fable.Core
open Fable.Import
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

type CardProps =
    {
        key: string
        HeaderItems: CardHeaderElement list
        Contents: ReactElement list
        Actions: (string * ColorStatus * (unit -> unit)) list
        HasExpanderToggle: bool
    }
    
type CardState =
    {
        IsBodyVisible: bool
    }

type private Card(props) as this =
    inherit Fable.React.Component<CardProps, CardState>(props)
    do this.setInitState({ IsBodyVisible = true })

    let toggleState _ = this.ToggleState

    member this.ToggleState =
        this.setState(fun s p ->
            { s with
                IsBodyVisible = not s.IsBodyVisible
            }
        )

    override this.render () =
        let h =
            let mutable headers = this.props.HeaderItems
            if this.props.HasExpanderToggle then do
                headers <- headers @ [ Button (AngleDown, toggleState) ]
            
            match headers
                  |> List.map (fun hi ->
                      match hi with
                      | Title t ->
                          div [ ClassName "card-header-title" ] [ str t ]
                          |> Some
                      | Info (el) ->
                          div [ ClassName "card-header-subtitle" ] [ el ]
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
                                   [ i [ ClassName (Nerds.Icon.render ic) ] [] ]
                            ]
                          |> Some
                      | NoRender -> None
                  )
                  |> List.choose id with
            | [] ->
                None
            | li ->
                header
                    [
                        ClassName "card-header"
                        OnClick (fun event ->
                            event.stopPropagation() |> ignore
                            event.preventDefault() |> ignore
                            toggleState event
                        )
                    ]
                    li
                |> Some
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
