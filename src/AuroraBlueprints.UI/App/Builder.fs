module App.Model.Builder

open Elmish

open Global

open App.Model
open App.Msg

open Saving.LocalStorage

type ModelBuilder(initial: Model) =
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

    member __.Return(x: Cmd<Msg>) =
        built.PendingSaves
        |> List.iter (function
            | PendingSave.SetShip ship ->
                ship
                |> Saving.Ships.serialize
                ||||> save
            | PendingSave.RemoveShip ship ->
                ship
                |> Saving.Ships.serialize
                ||||> delete
            | PendingSave.SetComponent comp ->
                if not comp.BuiltIn then do
                    comp
                    |> Saving.Components.serialize
                    ||||> save
            | PendingSave.RemoveComponent comp ->
                comp
                |> Saving.Components.serialize
                ||||> delete
            | PendingSave.SetCurrentTechnologies techs ->
                techs
                |> Saving.Technology.serialize
                ||||> save
        )

        { built with PendingSaves = List.empty }, x

let builder initial = ModelBuilder initial
