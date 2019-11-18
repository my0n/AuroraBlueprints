module App.View

open Elmish
open Elmish.Navigation
open Elmish.UrlParser
open Fable.Core.JsInterop
open App.State
open App.Model

importAll "./sass/main.sass"

open Fable.React
open Fable.React.Props

let root model dispatch =

  let pageHtml page =
    match page with
    | Ships -> ShipView.Page.root model dispatch
    | Tech -> TechView.root model dispatch

  div
    []
    [ Navbar.View.root model
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
|> Program.withReactBatched "elmish-app"
|> Program.run
