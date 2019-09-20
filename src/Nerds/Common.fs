module Nerds.Common

open Nerds.Icon
open Fable.Helpers.React
open Fable.Helpers.React.Props

type INerd =
    abstract member Text: string with get
    abstract member Tooltip: string with get
    abstract member Icon: Icon with get
    abstract member Render: bool with get

let render (nerd: INerd) =
    div [ HTMLAttr.Title nerd.Tooltip ]
        [ str nerd.Text
          i [ ClassName << Nerds.Icon.render <| nerd.Icon ]
            []
        ]
