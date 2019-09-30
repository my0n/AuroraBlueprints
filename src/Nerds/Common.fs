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
