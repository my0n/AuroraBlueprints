module Bulma.Button

open Fable.Helpers.React
open Fable.Helpers.React.Props

let render name callback =
    p [ ClassName "control" ]
      [
          div [ ClassName "button"
                OnClick (fun event ->
                    event.stopPropagation() |> ignore
                    callback()
                )
              ]
              [ str name ]
      ]

