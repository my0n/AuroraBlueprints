module Ships.Types

type ShipComponent =
    | FuelStorage

type Ship =
    {
        Name: string
        Weight: double
        Components: ShipComponent list
    }

type Model =
    {
        CurrentShip: Ship option
        Ships: Ship list
    }

type Msg =
    | Abcdefg
