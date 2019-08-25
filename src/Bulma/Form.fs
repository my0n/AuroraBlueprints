module Bulma.Form

open Global
open Fable.Helpers.React
open Fable.Helpers.React.Props

type FltInpOptions =
    {
        Label: string option
        Value: float
    }

type IntInpOptions =
    {
        Label: string option
        Max: int option
        Value: int
    }

type TxtInpOptions =
    {
        Label: string option
        Value: string
    }

type SelectOptions =
    {
        Label: string option
        Options: (int * string) list
        Value: int
    }

type FormElement =
    | HorGrp of string option * FormElement list
    | FltInp of FltInpOptions * (float -> unit)
    | IntInp of IntInpOptions * (int -> unit)
    | TxtInp of TxtInpOptions * (string -> unit)
    | RadGrp of (string * (unit -> unit)) list
    | Select of SelectOptions * (int -> unit)
    | StcBtn of string
    | Addons of FormElement list
    | Button of string * (unit -> unit)
    | Label of string

let inline private optLbl lbl lblFn inp =
    (([]
        @+? (match lbl with
            | Some lbl -> Some <| lblFn lbl
            | None -> None
            )
        )
        @ inp
    )

let rec render f =
    f
    |> List.map(fun x ->
        match x with
        | HorGrp (lbl, els) ->
            let contents =
                render els
                |> List.map (fun el -> div [ ClassName "field" ] [ el ])

            [ div [ ClassName "field-body" ] contents ]
            |> optLbl lbl
                      (fun lbl -> div [ ClassName "field-label is-normal" ] [ str lbl ])
            |> div [ ClassName "field is-horizontal" ]
        | FltInp (options, cb) ->
            [ div [ ClassName "control is-small" ]
                  [ input [ ClassName "input"
                            Type "number"
                            Value options.Value
                            Min 0
                            OnChange (fun event ->
                                        match System.Double.TryParse event.Value with
                                        | true, num -> cb num
                                        | _ -> cb 0.0
                                     )
                          ]
                  ]
            ]
            |> optLbl options.Label
                      (fun lbl -> label [ ClassName "label" ] [ str lbl ])
            |> div [ ClassName "control" ]
        | IntInp (options, cb) ->
            [ div [ ClassName "control is-small" ]
                  [ input [ ClassName "input"
                            Type "number"
                            Value options.Value
                            Min 0
                            Max options.Max
                            OnChange (fun event ->
                                        match System.Int32.TryParse event.Value with
                                        | true, num -> cb num
                                        | _ -> cb 0
                                     )
                          ]
                  ]
            ]
            |> optLbl options.Label
                      (fun lbl -> label [ ClassName "label" ] [ str lbl ])
            |> div [ ClassName "control" ]
        | TxtInp (options, cb) ->
            [ div [ ClassName "control" ]
                  [ input [ ClassName "input"
                            Type "text"
                            Value options.Value
                            OnChange (fun event -> cb event.Value)
                          ]
                  ]
            ]
            |> optLbl options.Label
                      (fun lbl -> label [ ClassName "label" ] [ str lbl ])
            |> div [ ClassName "control" ]
        | RadGrp options ->
            options
            |> List.map (fun (lbl, cb) ->
                label [ ClassName "radio" ]
                      [ input [ Type "radio"
                                Name "member"
                                OnSelect (fun _ -> cb())
                              ]
                        str lbl
                      ]
            )
            |> div [ ClassName "control" ]
        | Select (options, cb) ->
            [ div [ ClassName "control is-expanded" ]
                    [ div [ ClassName "select is-fullwidth" ]
                        [ select [ OnChange (fun event ->
                                               match System.Int32.TryParse event.Value with
                                               | true, num -> cb num
                                               | _ -> cb 0
                                            )
                                 ]
                                 (options.Options
                                  |> List.map (fun (k, v) ->
                                    option [ Value k; Selected (k = options.Value) ] [ str v ]
                                  )
                                 )
                        ]
                    ]
            ]
            |> optLbl options.Label
                      (fun lbl -> label [ ClassName "label" ] [ str lbl ])
            |> div [ ClassName "control" ]
        | Addons els ->
            render els
            |> div [ ClassName "field has-addons" ]
        | Label lbl ->
            div [] [ str lbl ]
        | StcBtn lbl ->
            a [ ClassName "button is-static" ] [ str lbl ]
        | Button (lbl, cb) ->
            a [ ClassName "button"
                OnClick (fun _ -> cb())
              ]
              [ str lbl ]
    )
