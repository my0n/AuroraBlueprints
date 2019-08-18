module InputComponents

open Fable.Helpers.React
open Fable.Helpers.React.Props

open Global
open TableCommon
open Types

type TextInputOptions =
    {
        OnChange: string -> Msg
    }

let textInput options dispatch value =
    div [ ClassName "field" ]
        [ div [ ClassName "control" ]
              [ input [
                        ClassName "input"
                        Type "text"
                        Value value
                        OnChange (fun event -> options.OnChange event.Value |> dispatch )
                      ]
              ]
        ]
