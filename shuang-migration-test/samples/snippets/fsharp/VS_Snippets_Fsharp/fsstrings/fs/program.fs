// Learn more about F# at http://fsharp.net

let Snippet1() =
// <snippet1>
    let spaceOut inputString =
         String.collect (fun c -> sprintf "%c " c) inputString
    printfn "%s" (spaceOut "Hello World!")
// </snippet1>

let Snippet2() =
// <snippet2>
    let strings = [ "tomatoes"; "bananas"; "apples" ]
    let fullString = String.concat ", " strings
    printfn "%s" fullString
// </snippet2>

let Snippet3() =
// <snippet3>
    let containsUppercase string1 =
        if (String.exists (fun c -> System.Char.IsUpper(c)) string1) then
            printfn "The string \"%s\" contains uppercase characters." string1
        else
            printfn "The string \"%s\" does not contain uppercase characters." string1
    containsUppercase "Hello World!"
    containsUppercase "no"
// </snippet3>

let Snippet4() =
// <snippet4>
    let isWholeNumber string1 =
        if (String.forall (fun c -> System.Char.IsDigit(c)) string1) then
            printfn "The string \"%s\" is a whole number." string1
        else
            printfn "The string \"%s\" is not a valid whole number." string1
    isWholeNumber "8005"
    isWholeNumber "512"
    isWholeNumber "0x20"
    isWholeNumber "1.0E-5"
    isWholeNumber "-20"
// </snippet4>

let Snippet5() =
// <snippet5>
    let string1 = String.init 10 (fun i -> i.ToString())
    printfn "%s" string1
    let string2 = String.init 26 (fun i ->
        sprintf "%c" (char (i + int 'A')))
    printfn "%s" string2
// </snippet5>

(*
let Snippet6() =
// <snippet6>
    printfn "%ld" 100
    printfn "%lu" 100
    printfn "%Ld" 100
    printfn "%n" 100
    printfn "%U" 100
// </snippet6>

*)

let Snippet7() =
//<snippet7> 
    let rot13 c =
        let upperZero = int 'A' - 1
        let lowerZero = int 'a' - 1
        if System.Char.IsLetter(c) then
            if System.Char.IsUpper(c) then
                char (((int c + 13 - upperZero) % 26) + upperZero)
            else
                char (((int c + 13 - lowerZero) % 26) + lowerZero)
        else c
    let test = "The quick sly fox jumped over the lazy brown dog."
    printfn "%s" test
    printfn "%s" <| (String.map rot13 test)
//</snippet7>

let Snippet8() =
    let basePath = @"C:\\Users\\ghogen\\Downloads\\audio_captcha\\audio"
// <snippet8>
    let sayLetter inputChar =
        if (not (System.Char.IsLetterOrDigit(inputChar))) then () else
        let simpleSound = new System.Media.SoundPlayer(sprintf "%s\\%c.wav" basePath (System.Char.ToLower(inputChar)))
        simpleSound.Play()
        System.Threading.Thread.Sleep(1000)
    while true do
        let input = System.Console.ReadLine()
        String.iter (fun c -> sayLetter(c)) input
// </snippet8>
Snippet8()

let Snippet9() =
// <snippet9>
    let enumerateCharacters inputString = 
        String.iteri (fun i c -> printfn "%d %c" i c) inputString
    enumerateCharacters "TIME"
    enumerateCharacters "SPACE"
// </snippet9>

let Snippet10() =
// <snippet10>
    let replaceNth n newChar inputString =
        let result = String.mapi (fun i c -> if i = n then newChar else c) inputString
        printfn "%s" result
        result
    printfn "MASK"
    "MASK" |> replaceNth 0 'B'
    |> replaceNth 3 'H'
    |> replaceNth 2 'T'
    |> replaceNth 1 'O'
    |> replaceNth 0 'M'
    |> replaceNth 1 'A'
    |> replaceNth 2 'S'
    |> replaceNth 3 'K'
// </snippet10>

let Snippet11() =
// <snippet11>
    printfn "%s" <| String.replicate 10 "XO"
// </snippet11>

