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

let horizontalGroup label fields =
    div [ ClassName "field is-horizontal" ]
        ((
          [ (match label with None -> None | Some label -> Some (div [ ClassName "field-label" ] [ str label ])) ]
          @ [ Some (div [ ClassName "field-body" ] fields) ]
        )
        |> List.choose id)

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
