module Navbar.View

open Fable.React
open Fable.React.Props

open State.Model

let navButton label page currentPage =
    p [ ClassName "control" ]
      [ a [ classList [ "is-active", page = currentPage
                        "button", true ]
            Href (toHash page) ]
          [ str label ]
      ]

let navButtons currentPage =
    span [ ClassName "navbar-item" ]
         [ div
            [ ClassName "field is-grouped" ]
            [
                navButton "Ships" Ships currentPage
                navButton "Tech" Tech currentPage
            ]
         ]

let private pageTitle model =
    match model.FullyInitialized with
    | true -> "Aurora4x Blueprints"
    | false -> "Aurora4x Blueprints (loading)"

let root model =
    nav [ ClassName "navbar is-dark" ]
        [ div [ ClassName "container" ]
              [ div [ ClassName "navbar-brand" ]
                    [ h1 [ ClassName "navbar-item title is-4" ]
                         [ str <| pageTitle model ] ]
                div [ ClassName "navbar-end" ]
                    [ navButtons model.CurrentPage ]
              ]
        ]
