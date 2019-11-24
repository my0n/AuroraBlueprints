module App.Model.UI

module Model =

    let isExpanded key model =
        not <| List.contains key model.CollapsedSections

    /// <returns>The updated model</returns>
    let setSectionExpanded section expanded model =
        match expanded with
        | true ->
            { model with
                CollapsedSections = List.except [section] model.CollapsedSections
            }
        | false ->
            { model with
                CollapsedSections = model.CollapsedSections @ [section]
            }
