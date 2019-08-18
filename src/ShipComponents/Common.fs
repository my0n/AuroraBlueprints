module ShipComponents.Common

open Fable.Helpers.React
open Fable.Helpers.React.Props

let shipComponentCard header weight contents =
    div []
        [
            div [ ClassName "is-4" ] [str header]
        ]
