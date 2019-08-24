module Bulma.Card

open Global
open Fable.Helpers.React
open Fable.Helpers.React.Props

type CardHeaderIcon =
    | Close
    | Weight
    | GasPump
    | Tachometer
    | AngleDoubleRight
    | Dollar

type CardHeaderElement =
    | Title of string
    | Info of string * string * CardHeaderIcon
    | Button of CardHeaderIcon * (unit -> unit)

let inline private renderIcon ic =
    match ic with
    | Close -> "fas fa-times"
    | Weight -> "fas fa-weight-hanging"
    | GasPump -> "fas fa-gas-pump"
    | Tachometer -> "fas fa-tachometer-alt"
    | AngleDoubleRight -> "fas fa-angle-double-right"
    | Dollar -> "fas fa-dollar-sign"

let inline render headerItems contents =
    let h =
        headerItems
        |> Option.bind (
            List.map (fun hi ->
                match hi with
                | Title t ->
                    div [ ClassName "card-header-title" ] [ str t ]
                | Info (t, hover, ic) ->
                    div [ ClassName "card-header-subtitle"; HTMLAttr.Title hover ] [ str t; i [ ClassName (renderIcon ic) ] [] ]
                | Button (ic, cb) ->
                    a [ ClassName "card-header-icon"
                        OnClick (fun event -> cb())
                      ]
                      [ span [ ClassName "icon" ]
                             [ i [ ClassName (renderIcon ic) ] [] ]
                      ]
            )
            >> header [ ClassName "card-header" ]
            >> Some
        )
    let c = div [ ClassName "card-content" ] contents

    div [ ClassName "card" ]
        ([ h; Some c ] |> List.choose id)
