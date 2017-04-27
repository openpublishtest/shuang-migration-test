// AutomaticGeneralization.fs
// created by GHogen 3/18/09
module AutomaticGeneralization

// <snippet1801>
let max a b = if a > b then a else b
// </snippet1801>

// <snippet1802>
let biggest_float = max 2.0 3.0

let biggest_int = max 2 3
// </snippet1802>

// <snippet1803>
let test_string = max "cab" "cat"
// </snippet1803>

(*
// <snippet1804>
// Case 1: Too complex an expression.
// In this example, the list sqrList is intended to be a list of integers,
// but it is not defined as a simple immutable value.
let sqrList1 = [ for i in 1..10 -> i*i ]
// Adding a type annotation fixes the problem:
let sqrList2 : int list = [ for i in 1..10 -> i*i ]

// Case 2: Using a nongeneralizable construct to define a generic function.
// In this example, the construct is nongeneralizable because
// it involves partial application of function arguments.
let maxhash1 = max hash
// The following is acceptable because the argument for maxhash is explicit:
let maxhash2 obj = max hash obj

// Case 3: Adding an extra, unused parameter.
// Because this expression is not simple enough for generalization, the compiler
// issues the value restriction error.
let zeroList1 = Array.create 10 []
// Adding an extra (unused) parameter makes it a function, which is generalizable.
let zeroList2 () = Array.create 10 []

// Case 4: Adding type parameters.
let emptyset1 = Set.Empty
// Adding a type parameter and type annotation lets you write a generic value.
let emptyset2<'a> : Set<'a> = Set.Empty
// </snippet1804>
*)