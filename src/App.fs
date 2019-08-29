module App.View

open Elmish
open Elmish.Browser.Navigation
open Elmish.Browser.UrlParser
open Fable.Core
open Fable.Core.JsInterop
open Fable.Import
open Fable.Import.Browser
open App.State
open AppModel.Model
open Global

importAll "../sass/main.sass"

open Fable.Helpers.React
open Fable.Helpers.React.Props

let root model dispatch =

  let pageHtml page =
    match page with
    | Ships -> Ships.View.root model dispatch

  div
    []
    [ Navbar.View.root model.CurrentPage
      div
        [ ClassName "section" ]
        [ div
            [ ClassName "column" ]
            [ pageHtml model.CurrentPage ] ] ]

open Elmish.React
open Elmish.Debug
open Elmish.HMR

// App
Program.mkProgram init update root
|> Program.toNavigable (parseHash PageState.pageParser) PageState.urlUpdate
#if DEBUG
|> Program.withDebugger
#endif
|> Program.withReact "elmish-app"
|> Program.run
