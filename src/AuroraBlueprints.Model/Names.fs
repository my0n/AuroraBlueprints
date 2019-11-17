module Model.Names

open Global
open System
open System.Text.RegularExpressions

let readRomanNumerals (str: string) =
    let numbers = Map.ofList [('I',1);('V',5);('X',10);('L',50);('C',100);('D',500);('M',1000)]
    Array.foldBack (fun ele (prevalue, total) ->
        let currentvalue = numbers.Item ele
        match currentvalue >= prevalue with
        | true  -> currentvalue, total + currentvalue
        | false -> currentvalue, total - currentvalue
    ) (str.ToCharArray()) (0,0)
    |> snd

let writeRomanNumerals num =
    let numerals = [(1000,"M");(900,"CM");(500,"D");(400,"CD");(100,"C");(90,"XC");(50,"L");(40,"XL");(10,"X");(9,"IX");(5,"V");(4,"IV");(1,"I")]
    let rec acc (v, r) (m, s) =
        match v < m with
        | true  -> v, r
        | false -> acc (v-m, r+s) (m, s)
    List.fold acc (num, "") numerals |> snd

let incrementTextCount (original: string) =
    match Int32.TryParse original with
    | true, parsed ->
        parsed + 1
        |> sprintf "%d"
    | _ ->
        readRomanNumerals original
        + 1
        |> writeRomanNumerals

let nameOfCopy (original: string) =
    let opts = RegexOptions.IgnoreCase
    let m = Regex.Match(original, "^(.*)([\\. -]*)(mk|mark|copy)([\\. -]*)([0-9]*|[ivxlcdm]*)$", opts)
    match m.Success with
    | false -> original + " Mk II"
    | true ->
        [m.Groups.[1];m.Groups.[2];m.Groups.[3];m.Groups.[4];m.Groups.[5]]
        |> List.map (fun a -> a.Value)
        |> function
        | [a; b; c; ""; ""] when c.ToLower() = "copy" -> a, b, c, " ", "2"
        | [a; b; c; d; ""] when c.ToLower() = "copy" -> a, b, c, d, "2"
        | [a; b; c; d; ""] -> a, b, c, d, "II"
        | [a; b; c; d; e] -> a, b, c, d, incrementTextCount (e.ToUpper())
        | _ -> original, " ", "Mk", " ", "II"
    |||||> sprintf "%s%s%s%s%s"
