module State.Builder

open Global

open State.Model

open State.Saving

type ModelBuilder(storage: Storage, initial: Model) =
    let mutable built = initial

    member __.Zero() = initial

    member __.Bind((m: Model -> Model), f) =
        let built' = m built
        built <- built'
        f ()

    member __.Bind((m: Model -> Model * 'a), f) =
        let (built', b) = m built
        built <- built'
        f b

    member __.Return(x) =
        built.PendingSaves
        |> List.iter (function
            | SetShip ship ->
                storage.SaveShip ship
            | RemoveShip ship ->
                storage.DeleteShip ship
            | SetComponent comp ->
                if not comp.BuiltIn then do
                    storage.SaveComponent comp
            | RemoveComponent comp ->
                storage.DeleteComponent comp
            | SetCurrentTechnologies techs ->
                storage.SaveCurrentTechnology techs
        )

        { built with PendingSaves = List.empty }, x

let builder initial = ModelBuilder initial
