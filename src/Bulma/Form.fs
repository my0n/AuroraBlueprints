module Bulma.Form

open Global
open Fable.Helpers.React
open Fable.Helpers.React.Props

type IntInpOptions =
    {
        Label: string option
        Value: int
    }

type TxtInpOptions =
    {
        Label: string option
        Value: string
    }

type SelectOption =
    {
        Key: string
        Label: string
    }

type SelectOptions =
    {
        Label: string option
        Options: SelectOption list
        Value: string
    }

type FormElement =
    | HorGrp of string option * FormElement list
    | IntInp of IntInpOptions * (int -> unit)
    | TxtInp of TxtInpOptions * (string -> unit)
    | RadGrp of (string * (unit -> unit)) list
    | Select of SelectOptions * (string -> unit)
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
        | IntInp (options, cb) ->
            [ div [ ClassName "control is-small" ]
                  [ input [ ClassName "input"
                            Type "number"
                            Value options.Value
                            Min 0
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
            [ div [ ClassName "control" ]
                    [ div [ ClassName "select" ]
                        [ select [ OnChange (fun event -> cb event.Value) ]
                                 (options.Options
                                  |> List.map (fun opt ->
                                    option [ Value opt.Key; Selected (opt.Key = options.Value) ] [ str opt.Label ]
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
