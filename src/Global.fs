module Global

module List =
    let wrap element = [ element ]

type Page =
    | Ships
    | Home
    | Counter
    | About

let toHash page =
    match page with
    | Ships -> "#ships"
    | About -> "#about"
    | Counter -> "#counter"
    | Home -> "#home"
