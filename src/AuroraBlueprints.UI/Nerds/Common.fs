module Nerds.Common

open Nerds.Icon

type INerd =
    abstract member Text: string with get
    abstract member Description: string option with get
    abstract member Tooltip: string with get
    abstract member Icon: Icon with get
    abstract member Render: bool with get
