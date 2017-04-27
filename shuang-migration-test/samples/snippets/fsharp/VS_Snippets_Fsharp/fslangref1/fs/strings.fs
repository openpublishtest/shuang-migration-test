// Strings.fs
// created GHogen 3/18/09

module Strings

// <snippet1001>
let str1 = "abc
     def"
let str2 = "abc\
     def"
// </snippet1001>

// <snippet1002>
printfn "%c" str1.[1]
// </snippet1002>

// <snippet1003>
printfn "%s" (str1.[0..2])
printfn "%s" (str2.[3..5])
// </snippet1003>

let Snippet1004() =
// <snippet1004>
    // "abc" interpreted as a Unicode string.
    let str1 : string = "abc"
    // "abc" interpreted as an ASCII byte array. 
    let bytearray : byte[] = "abc"B 
// </snippet1004>
    ()

// <snippet1005>
let printChar (str : string) (index : int) =
    printfn "First character: %c" (str.Chars(index))
// </snippet1005>

// <snippet1006>
let string1 = "Hello, " + "world"
// </snippet1006>