module Nerds.Icon

type Icon =
    | NoIcon
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
    | LifeRing
    | FireAlt
    | BroadcastTower
    | Boxes
    | CalendarAlt
    | Calendar

let inline render ic =
    match ic with
    | NoIcon -> ""
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
    | LifeRing -> "fas fa-life-ring"
    | FireAlt -> "fas fa-fire-alt"
    | BroadcastTower -> "fas fa-broadcast-tower"
    | Boxes -> "fas fa-boxes"
    | CalendarAlt -> "fas fa-calendar-alt"
    | Calendar -> "fas fa-calendar"
