module Bulma.FC

open System
open Global
open Model.Measures
open Fable.Helpers.React
open Fable.Helpers.React.Props

[<Flags>]
type ControlOpts =
    | Empty = 0
    | IsSmall = 1
    | IsExpanded = 2

[<Flags>]
type FieldOpts =
    | Empty = 0
    | IsHorizontal = 1
    | HasAddons = 2

[<Flags>]
type LabelOpts =
    | Empty = 0
    | IsDisallowed = 1

[<Flags>]
type FieldLabelOpts =
    | Empty = 0
    | IsNormal = 1

let inline private optLbl lbl lblFn inp =
    (([]
        @+? (match lbl with
            | Some lbl -> Some <| lblFn lbl
            | None -> None
            )
        )
        @ inp
    )

let Control (opts: ControlOpts) els =
    div [ classList [ "control", true
                      "is-small", opts.HasFlag ControlOpts.IsSmall
                      "is-expanded", opts.HasFlag ControlOpts.IsExpanded
                    ]
        ] els
    
let Button lbl cb =
    a [ ClassName "button"
        OnClick (fun _ -> cb())
      ]
      [ str lbl ]
    
let Field (opts: FieldOpts) els =
    div [ classList [ "field", true
                      "is-horizontal", opts.HasFlag FieldOpts.IsHorizontal
                      "has-addons", opts.HasFlag FieldOpts.HasAddons
                    ]
        ] els
    
let FieldLabel (opts: FieldLabelOpts) lbl =
    div [ classList [ "field-label", true
                      "is-normal", opts.HasFlag FieldLabelOpts.IsNormal
                    ]
        ] [ str lbl ]
    
let FieldBody els =
    div [ ClassName "field-body" ] els

let HorizontalGroup lbl els =
    els
    |> List.map (List.wrap >> Field FieldOpts.Empty)
    |> FieldBody
    |> List.wrap
    |> optLbl lbl (FieldLabel FieldLabelOpts.IsNormal)
    |> Field FieldOpts.IsHorizontal
    
let Label (opts: LabelOpts) lbl =
    div [ classList [ "label", true
                      "pseudodisabled", opts.HasFlag LabelOpts.IsDisallowed
                    ]
        ] [ str lbl ]

type FltInpOptions<[<Measure>] 'a> =
    {
        Label: string option
        Value: float<'a>
    }
    
let FloatInput (opts: FltInpOptions<'a>) (cb: float<'a> -> unit) =
    input [ ClassName "input"
            Type "number"
            Value opts.Value
            Min 0
            OnChange (fun event ->
                        match System.Double.TryParse event.Value with
                        | true, num -> cb <| float2float<'a> num
                        | _ -> cb <| float2float<'a> 0.0
                     )
          ]
    |> List.wrap
    |> Control ControlOpts.IsSmall
    |> List.wrap
    |> optLbl opts.Label (Label LabelOpts.Empty)
    |> Control ControlOpts.Empty

type IntInpOptions<[<Measure>] 'a> =
    {
        Label: string option
        Min: int option
        Max: int option
        Value: int<'a>
        Disabled: bool
    }

let IntInput (opts: IntInpOptions<'a>) (cb: int<'a> -> unit) =
    input [ classList [ "input", true; "pseudodisabled", opts.Disabled ]
            Type "number"
            Value opts.Value
            Min opts.Min
            Max opts.Max
            OnChange (fun event ->
                        match System.Int32.TryParse event.Value with
                        | true, num -> cb <| int2int<'a> num
                        | _ -> cb <| int2int<'a> 0
                     )
          ]
    |> List.wrap
    |> Control ControlOpts.IsSmall
    |> List.wrap
    |> (fun c ->
        let lblOpts =
            match opts.Disabled with
            | true -> LabelOpts.IsDisallowed
            | false -> LabelOpts.Empty
        c
        |> optLbl opts.Label (Label lblOpts)
    )
    |> Control ControlOpts.Empty

let TextInput value cb =
    input [ ClassName "input"
            Type "text"
            Value value
            OnChange (fun event -> cb event.Value)
          ]
    |> List.wrap
    |> Control ControlOpts.IsExpanded

let WithLabel lbl els =
    els
    |> Control ControlOpts.Empty
    |> List.wrap
    |> List.append [ Label LabelOpts.Empty lbl ]
    |> Control ControlOpts.Empty

type CheckboxOptions =
    {
        Disabled: bool
        Checked: bool
        Label: string
    }

let Checkbox opts cb =
    label [ ClassName "checkbox"; Disabled opts.Disabled ]
          [ input [ Type "checkbox"
                    OnChange (fun event -> cb event.Checked)
                    Disabled opts.Disabled
                    Checked opts.Checked
                  ]
            str opts.Label
          ]

let Radio name lbl cb =
    label [ ClassName "radio" ]
          [ input [ Type "radio"
                    Name name
                    OnSelect (fun _ -> cb())
                  ]
            str lbl
          ]

let RadioGroup opts =
    opts
    |> List.map (fun (a, b) -> Radio "member" a b)
    |> Control ControlOpts.Empty

type SelectOptions =
    {
        Label: string option
        Options: {| Key: int; Text: string; Disallowed: bool |} list
        Value: int
    }

let Select opts cb =
    div [ ClassName "select is-fullwidth" ]
        [ select [ OnChange (fun event ->
                               match System.Int32.TryParse event.Value with
                               | true, num -> cb num
                               | _ -> cb 0
                            )
                 ]
                 (opts.Options
                  |> List.map (fun o ->
                    option [ classList [ "pseudodisabled", o.Disallowed ]; Value o.Key; Selected (o.Key = opts.Value) ] [ str o.Text ]
                  )
                 )
        ]
    |> List.wrap
    |> Control ControlOpts.IsExpanded
    |> List.wrap
    |> optLbl opts.Label (Label LabelOpts.Empty)
    |> Control ControlOpts.Empty

let AddonGroup els = Field FieldOpts.HasAddons els

let StaticButton lbl =
    a [ ClassName "button is-static" ] [ str lbl ]
