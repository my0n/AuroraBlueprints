module Tests.StateComponentTests

open Expecto
open State.Model
open State.Components

module Expect =
    let canRemoveComponent model comp =
        Expect.isNonEmpty model.AllComponents "model was empty after adding the component"
        let model', removed = Model.removeComponent comp model
        Expect.isTrue removed "success bool was false"
        Expect.isFalse (model'.AllComponents.ContainsKey comp.Id) "model contained the component after attempting to remove it"

    let cannotRemoveComponent model comp =
        Expect.isNonEmpty model.AllComponents "model was empty after adding the component"
        let model', removed = Model.removeComponent comp model
        Expect.isFalse removed "success bool was true"
        Expect.isTrue (model'.AllComponents.ContainsKey comp.Id) "model did not contain the component after attempting to remove it"

[<Tests>]
let tests =
    testList "components" [
        testList "remove" [
            testList "not built-in" [
                Generators.components
                |> Seq.filter (fun comp -> not comp.BuiltIn)
                |> Seq.map (fun comp ->
                    let name = sprintf "component '%s' can be removed from a model which only has that component" comp.Id
                    testCase name <| fun _ ->
                        let comps =
                            Map.ofList [comp.Id, comp]
                            
                        Expect.canRemoveComponent
                            { Model.empty with AllComponents = comps }
                            comp
                )
                |> Seq.toList
                |> testList "lean list"

                Generators.components
                |> Seq.filter (fun comp -> not comp.BuiltIn)
                |> Seq.map (fun comp ->
                    let name = sprintf "component '%s' can be removed from a model that has all the components" comp.Id
                    testCase name <| fun _ ->
                        let comps =
                            Generators.components
                            |> Seq.map (fun c -> c.Id, c)
                            |> Map.ofSeq
                            
                        Expect.canRemoveComponent
                            { Model.empty with AllComponents = comps }
                            comp
                )
                |> Seq.toList
                |> testList "busy list"
            ]
            
            testList "built-in" [
                Generators.components
                |> Seq.filter (fun comp -> comp.BuiltIn)
                |> Seq.map (fun comp ->
                    let name = sprintf "component '%s' cannot be removed from a model which only has that component" comp.Id
                    testCase name <| fun _ ->
                        let comps =
                            Map.ofList [comp.Id, comp]
                            
                        Expect.cannotRemoveComponent
                            { Model.empty with AllComponents = comps }
                            comp
                )
                |> Seq.toList
                |> testList "lean list"

                Generators.components
                |> Seq.filter (fun comp -> comp.BuiltIn)
                |> Seq.map (fun comp ->
                    let name = sprintf "component '%s' cannot be removed from a model that has all the components" comp.Id
                    testCase name <| fun _ ->
                        let comps =
                            Generators.components
                            |> Seq.map (fun c -> c.Id, c)
                            |> Map.ofSeq

                        Expect.cannotRemoveComponent
                            { Model.empty with AllComponents = comps }
                            comp
                )
                |> Seq.toList
                |> testList "busy list"
            ]
        ]
    ]
