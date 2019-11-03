module Bulma.Button

open Fable.React
open Fable.React.Props

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

