// Tuples.fs
// created GHogen 3/18/09
module Tuples

let a = 1
let b = 2

// <snippet1201>
// Tuple of two integers.
( 1, 2 ) 

// Triple of strings.
( "one", "two", "three" ) 

// Tuple of unknown types.
( a, b ) 

// Tuple that has mixed types.
( "one", 1, 2.0 ) 

// Tuple of integer expressions.
( a + 1, b + 1) 
// </snippet1201>

// <snippet1209>
let c = fst (1, 2)
let d = snd (1, 2)
// </snippet1209>

// <snippet1202>
let third (_, _, c) = c
// </snippet1202>

// <snippet1203>
let function1 (a, b) = a + b
// </snippet1203>

// <snippet1204>
let print tuple1 =
   match tuple1 with
    | (a, b) -> printfn "Pair %A %A" a b
// </snippet1204>

// <snippet1205>
let divRem a b = 
   let x = a / b
   let y = a % b
   (x, y)
// </snippet1205>

// <snippet1206>
let sumNoCurry (a, b) = a + b
// </snippet1206>

// <snippet1207>
let sum a b = a + b
// </snippet1207>

// <snippet1208>
let addTen = sum 10
let result = addTen 95  
// Result is 105.
// </snippet1208>