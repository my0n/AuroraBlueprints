module GameInfo

open Global

open Fetch
open Fable.SimpleJson

type Preset =
    {
        Name: string
        Technologies: GameObjectId list
    }

type GameInfo =
    {
        DefaultPreset: string
        Presets: Preset list
    }

let gameInfo =
    fetch "data/gameinfo.json" []
    |> Promise.bind (fun res -> res.text ())
    |> Promise.map Json.parseAs<GameInfo>