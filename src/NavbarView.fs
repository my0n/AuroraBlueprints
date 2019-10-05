module Navbar.View

open Fable.Helpers.React
open Fable.Helpers.React.Props

open App.Model

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

let root currentPage =
    nav [ ClassName "navbar is-dark" ]
        [ div [ ClassName "container" ]
              [ div [ ClassName "navbar-brand" ]
                    [ h1 [ ClassName "navbar-item title is-4" ]
                         [ str "Aurora4x Blueprints" ] ]
                div [ ClassName "navbar-end" ]
                    [ navButtons currentPage ]
              ]
        ]
