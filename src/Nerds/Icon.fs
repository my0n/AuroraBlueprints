module Nerds.Icon

type Icon =
    | Close
    | Weight
    | GasPump
    | Tachometer
    | AngleDoubleRight
    | Dollar
    | GlobeAmericas
    | Shield
    | AngleDown
    | ThLarge
    | FighterJet
    | Bolt

let inline render ic =
    match ic with
    | Close -> "fas fa-times"
    | Weight -> "fas fa-weight-hanging"
    | GasPump -> "fas fa-gas-pump"
    | Tachometer -> "fas fa-tachometer-alt"
    | AngleDoubleRight -> "fas fa-angle-double-right"
    | Dollar -> "fas fa-dollar-sign"
    | GlobeAmericas -> "fas fa-globe-americas"
    | Shield -> "fas fa-shield-alt"
    | AngleDown -> "fas fa-angle-down"
    | ThLarge -> "fas fa-th-large"
    | FighterJet -> "fas fa-fighter-jet"
    | Bolt -> "fas fa-bolt"
