module InputComponents

open Fable.Helpers.React
open Fable.Helpers.React.Props

open Types

type TextInputOptions =
    {
        Label: string
        OnChange: string -> Msg
    }

let textInput options dispatch value =
    div [ ClassName "field" ]
        [ div [ ClassName "control" ]
              [ label [ ClassName "label" ]
                      [ str options.Label ]
                input [ ClassName "input"
                        Type "text"
                        Value value
                        OnChange (fun event -> options.OnChange event.Value |> dispatch )
                      ]
              ]
        ]
        
type IntegerInputOptions =
    {
        Label: string
        OnChange: int -> Msg
    }

let integerInput options dispatch value =
    div [ ClassName "field" ]
        [ div [ ClassName "control" ]
              [ label [ ClassName "label" ]
                      [ str options.Label ]
                input [ ClassName "input"
                        Type "text"
                        Value value
                        OnChange (fun event ->
                            match System.Int32.TryParse event.Value with
                            | true, num -> options.OnChange num |> dispatch
                            | _ -> options.OnChange 0 |> dispatch
                        )
                      ]
              ]
        ]
