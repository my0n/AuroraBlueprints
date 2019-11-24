module App.Model.Technology

module Model =

    /// <returns>
    /// * The updated model
    /// * The new list of current technologies
    /// </returns>
    let setPreset preset model =
        let preset' =
            model.Presets
            |> List.tryFind (fun p -> p.Name = preset)
        match preset' with
        | None ->
            model,
            model.CurrentTechnology
        | Some preset' ->
            { model with
                CurrentTechnology = preset'.Technologies
                PendingSaves = model.PendingSaves @ [ SetCurrentTechnologies preset'.Technologies ]
            },
            preset'.Technologies

    /// <returns>
    /// * The updated model
    /// * The new list of current technologies
    /// </returns>
    let addCurrentTechnology tech model =
        let currentTech' =
            match List.contains tech model.CurrentTechnology with
            | false -> model.CurrentTechnology @ [tech]
            | true ->  model.CurrentTechnology

        { model with
            CurrentTechnology = currentTech'
            PendingSaves = model.PendingSaves @ [ SetCurrentTechnologies currentTech' ]
        },
        currentTech'

    /// <returns>
    /// * The updated model
    /// * The new list of current technologies
    /// </returns>
    let removeCurrentTechnology tech model =
        let currentTech' =
            model.CurrentTechnology
            |> List.except ((model.AllTechnologies.GetAllChildren model.CurrentTechnology tech) @ [tech])

        { model with
            CurrentTechnology = currentTech'
            PendingSaves = model.PendingSaves @ [ SetCurrentTechnologies currentTech' ]
        },
        currentTech'
