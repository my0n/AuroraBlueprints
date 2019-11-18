module Bulma.Tabs

open Global
open Fable.React
open Fable.React.Props

let bar tabs active cb =
    tabs
    |> List.map (fun tab ->
        li [ classList ["is-active", tab = active] ]
            [
                a [OnClick (fun _ -> cb tab)] [str tab]
            ]
    )
    |> ul []
    |> List.wrap
    |> div [ ClassName "tabs" ]

type TabContainerProps =
    {
        Contents: (string * (unit -> ReactElement)) list
    }
    
type TabContainerState =
    {
        ActiveTab: string
    }

type private TabContainer(props) as this =
    inherit Fable.React.Component<TabContainerProps, TabContainerState>(props)
    do this.setInitState({ ActiveTab = fst props.Contents.[0] })

    let activateTab tab = this.ActivateTab tab

    member this.ActivateTab tab =
        this.setState(fun s p ->
            { s with
                ActiveTab = tab
            }
        )

    override this.render () =
        let tabs = List.map fst this.props.Contents
        let page =
            this.props.Contents
            |> List.tryFind (fst >> (=) this.state.ActiveTab)
            |> Option.map snd
            |> Option.defaultValue (fun () -> fragment [] [])
            <| ()
        div []
            [
                bar tabs this.state.ActiveTab activateTab
                page
            ]

let container contents = ofType<TabContainer,_,_> { Contents = contents } []
