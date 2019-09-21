module Nerds.Common

open Nerds.Icon
open Fable.Helpers.React
open Fable.Helpers.React.Props

type NerdRenderer =
    | NoLabel
    | IconForm
    | DescriptiveForm

type INerd =
    abstract member Text: string with get
    abstract member Description: string option with get
    abstract member Tooltip: string with get
    abstract member Icon: Icon with get
    abstract member Render: bool with get

let render options (nerd: INerd) =
    match options with
    | NoLabel ->
        div [ HTMLAttr.Title nerd.Tooltip ]
            [ str nerd.Text ]
    | IconForm ->
        div [ HTMLAttr.Title nerd.Tooltip ]
            [ str nerd.Text
              i [ ClassName << Nerds.Icon.render <| nerd.Icon ]
                []
            ]
    | DescriptiveForm ->
        div [ HTMLAttr.Title nerd.Tooltip ]
            [ str << Option.defaultValue "" <| nerd.Description
            ]
