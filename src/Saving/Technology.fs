module Saving.Technology

open Global
open LocalStorage
open Thoth.Json

let serialize (ct: GameObjectId list) = "ct", "currenttech", "1", ct

let deserialize version key str =
    Fable.Core.JS.console.log str
    match version with
    | "1" ->
        match Decode.Auto.fromString<GameObjectId list> str with
        | Ok s -> Success s
        | Error s -> Failure s
    | s -> Failure <| sprintf "Invalid version %s" s
