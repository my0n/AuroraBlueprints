module Tests.NameTests

open Expecto
open Model.Names

[<Tests>]
let tests =
    [
        "Tribal", "Tribal Mk II"
        "Tribal Mk I", "Tribal Mk II"
        "Tribal Mk II", "Tribal Mk III"
        "Tribal Mk III", "Tribal Mk IV"

        "Tribal MkI", "Tribal MkII"
        "Tribal MkII", "Tribal MkIII"
        "Tribal MkIII", "Tribal MkIV"

        "Tribal Mark I", "Tribal Mark II"
        "Tribal Mark II", "Tribal Mark III"
        "Tribal Mark III", "Tribal Mark IV"

        "Tribal MarkI", "Tribal MarkII"
        "Tribal MarkII", "Tribal MarkIII"
        "Tribal MarkIII", "Tribal MarkIV"

        "Tribal - MkI", "Tribal - MkII"

        "Tribal   Mark - MMCDLXI", "Tribal   Mark - MMCDLXII"
        "Mark Mark IV", "Mark Mark V"
        "Mark Mk I Mark MK2 CopyIV", "Mark Mk I Mark MK2 CopyV"

        "Mk I", "Mk II"
        "MkI", "MkII"
        "MkM", "MkMI"

        "Tribal - Copy", "Tribal - Copy 2"
        "Tribal - Copy 13", "Tribal - Copy 14"

        "Tribal Mk -1", "Tribal Mk -2"
        "Tribal Mk -I", "Tribal Mk -II"
        "Tribal Mk 0", "Tribal Mk 1"
        
        "Tribal Mk elbow", "Tribal Mk elbow Mk II"
    ]
    |> List.map (fun (actual, expectedCopy) ->
        let name = sprintf "copy of %s = %s" actual expectedCopy
        testCase name <| fun _ ->
            let actualCopy = nameOfCopy actual
            Expect.equal
                actualCopy
                expectedCopy
                "name of copy not equal"
    )
    |> testList "names"
