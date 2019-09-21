module Bulma.FC

open Global
open Model.Measures
open Fable.Helpers.React
open Fable.Helpers.React.Props

type ControlSize =
    | IsSmall
    | IsExpanded

type FieldOpts =
    | IsHorizontal
    | HasAddons

type FieldLabelOpts =
    | IsNormal

let inline private optLbl lbl lblFn inp =
    (([]
        @+? (match lbl with
            | Some lbl -> Some <| lblFn lbl
            | None -> None
            )
        )
        @ inp
    )

let Control size els =
    let isSmall = size |> Option.map (fun o -> match o with IsSmall -> true | _ -> false) |> Option.defaultValue false
    let isExpanded = size |> Option.map (fun o -> match o with IsExpanded -> true | _ -> false) |> Option.defaultValue false
    div [ classList [ "control", true; "is-small", isSmall; "is-expanded", isExpanded ] ] els
    
let Button lbl cb =
    a [ ClassName "button"
        OnClick (fun _ -> cb())
      ]
      [ str lbl ]
    
let Field opts els =
    let isHorizontal = opts |> Option.map (fun o -> match o with IsHorizontal -> true | _ -> false) |> Option.defaultValue false
    let hasAddons = opts |> Option.map (fun o -> match o with HasAddons -> true | _ -> false) |> Option.defaultValue false
    div [ classList [ "field", true; "is-horizontal", isHorizontal; "has-addons", hasAddons ] ] els
    
let FieldLabel opts lbl =
    let isNormal = opts |> Option.map (fun o -> match o with IsNormal -> true | _ -> false) |> Option.defaultValue false
    div [ classList [ "field-label", true; "is-normal", isNormal ] ] [ str lbl ]
    
let FieldBody els =
    div [ ClassName "field-body" ] els

let HorizontalGroup lbl els =
    els
    |> List.map (List.wrap >> Field None)
    |> FieldBody
    |> List.wrap
    |> optLbl lbl (FieldLabel (Some IsNormal))
    |> Field (Some IsHorizontal)
    
let Label lbl =
    label [ ClassName "label" ] [ str lbl ]

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
    |> Control (Some IsSmall)
    |> List.wrap
    |> optLbl opts.Label Label
    |> Control None

type IntInpOptions<[<Measure>] 'a> =
    {
        Label: string option
        Min: int option
        Max: int option
        Value: int<'a>
    }

let IntInput (opts: IntInpOptions<'a>) (cb: int<'a> -> unit) =
    input [ ClassName "input"
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
    |> Control (Some IsSmall)
    |> List.wrap
    |> optLbl opts.Label Label
    |> Control None

let TextInput value cb =
    input [ ClassName "input"
            Type "text"
            Value value
            OnChange (fun event -> cb event.Value)
          ]
    |> List.wrap
    |> Control (Some IsExpanded)

let WithLabel lbl els =
    els
    |> Control None
    |> List.wrap
    |> List.append [ Label lbl ]
    |> Control None

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
    |> Control None

type SelectOptions =
    {
        Label: string option
        Options: (int * string) list
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
                  |> List.map (fun (k, v) ->
                    option [ Value k; Selected (k = opts.Value) ] [ str v ]
                  )
                 )
        ]
    |> List.wrap
    |> Control (Some IsExpanded)
    |> List.wrap
    |> optLbl opts.Label Label
    |> Control None

let AddonGroup els = Field (Some HasAddons) els

let StaticButton lbl =
    a [ ClassName "button is-static" ] [ str lbl ]
