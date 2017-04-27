// Learn more about F# at http://fsharp.net
module OperatorSnippets

let Snippet1() =
// <snippet1>
    let append1 string1 = string1 + ".append1"
    let append2 string1 = string1 + ".append2"

    let result1 = "abc" |> append1
    printfn "\"abc\" |> append1 gives %A" result1

    let result2 = "abc" 
                  |> append1
                  |> append2
    printfn "result2: %A" result2

    [1; 2; 3]
    |> List.map (fun elem -> elem * 100)
    |> List.rev
    |> List.iter (fun elem -> printf "%d " elem)
    printfn ""
// </snippet1>
(* Output:
"abc" |> append1 gives "abc.append1"
result2: "abc.append1.append2"
300 200 100
*)

let Snippet2() =
// <snippet2>
    let append string1 string2 = string1 + "." + string2

    let result1 = ("abc", "def") ||> append
    printfn "(\"abc\", \"def\") ||> append gives %A" result1

    let toUpper (string1:string) (string2:string) = string1.ToUpper(), string2.ToUpper()

    let result2 = ("abc", "def")
                   ||> toUpper
                   ||> append

    printfn "result2: %A" result2
// </snippet2>
(* output:
("abc", "def") ||> append gives "abc.def"
result2: "ABC.DEF"
*)

let Snippet3() =
    // <snippet3>
    let append4 string1 string2 string3 = string1 + "." + string2 + "." + string3

    // The |||> operator
    let result3 = ("abc", "def", "ghi") |||> append4
    printfn "(\"abc\", \"def\", \"ghi\") |||> append4 gives  %A" result3
    // </snippet3>
    (* Output:
    ("abc", "def", "ghi") |||> append4 gives  "abc.def.ghi"
    *)

let Snippet4() =
    // <snippet4>
    let append1 string1 = string1 + ".append1"
    let append2 string1 = string1 + ".append2"

    let result1 = append1 <| "abc"
    printfn "append1 <| \"abc\" gives %A" result1

    // Reverse pipelines require parentheses.
    let result2 :string = append2 <| (append1 <| "abc")
    printfn "result2: %A" result2

    // Reverse pipelines can be used to eliminate the need for
    // parentheses in some expressions.
    raise <| new System.Exception("A failure occurred.")
    // </snippet4>
    (* Output:
    append1 <| "abc" gives "abc.append1"
    result2: "abc.append1.append2"
    *)

let Snippet5() =
    // <snippet5>
    let append string1 string2 = string1 + "." + string2

    let result1 = append <|| ("abc", "def")
    printfn "append <|| (\"abc\", \"def\") gives %A" result1

    let toUpper (string1:string) (string2:string) = string1.ToUpper(), string2.ToUpper()

    // Reverse pipelines require parentheses.
    let result2 = append <|| (toUpper <|| ("abc", "def"))

    printfn "result2: %A" result2
    // </snippet5>
    (* Output:
    append <|| ("abc", "def") gives "abc.def"
    result2: "ABC.DEF"
    *)

let Snippet6() =
     // <snippet6>
     let append4 string1 string2 string3 = string1 + "." + string2 + "." + string3

     // The <||| operator
     let result3 = append4 <||| ("abc", "def", "ghi")
     printfn "append4 <||| (\"abc\", \"def\", \"ghi\") gives  %A" result3
     // </snippet6>
     (* Output:
     append4 <||| ("abc", "def", "ghi") gives  "abc.def.ghi"
     *)

let Snippet7() =
// <snippet7>
    let append1 string1 = string1 + ".append1"
    let append2 string1 = string1 + ".append2"

    // Composition of two functions.
    let appendBoth = append1 >> append2
    printfn "%s" (appendBoth "abc")

    // Composition of three functions.
    let append3 string1 = string1 + ".append3"
    printfn "%s" ((append1 >> append2 >> append3) "abc")

    // Composition of functions with more than one parameter.
    let appendString (string1:string) (string2:string) = string1 + string2

    let appendFileExtension extension =
        appendString "." >> appendString extension
    printfn "%s" (appendFileExtension "myfile" "txt")
// </snippet7>
(* output:
abc.append1.append2
abc.append1.append2.append3
myfile.txt
*)

let Snippet8() =
// <snippet8>
    let append1 string1 = string1 + ".append1"
    let append2 string1 = string1 + ".append2"

    // Reverse composition of two functions.
    let appendBothReverse = append1 << append2
    printfn "%s" (appendBothReverse "abc")

    // Reverse composition of three functions.
    let append3 string1 = string1 + ".append3"
    printfn "%s" ((append1 << append2 << append3) "abc")

    // Reverse composition of functions with more than one parameter.
    let appendString (string1:string) (string2:string) = string1 + string2

    let appendFileExtension extension =
        appendString extension << appendString "." 
    printfn "%s" (appendFileExtension "myfile" "txt")
// </snippet8>
(* output:
    abc.append2.append1
    abc.append3.append2.append1
    myfile.txt
*)
