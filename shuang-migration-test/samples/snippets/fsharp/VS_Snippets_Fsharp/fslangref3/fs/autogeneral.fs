// AutoGeneral.fs
// created by GHogen 11/19/09

module AutomaticGeneralization

// <snippet101>
let max a b = if a > b then a else b
// </snippet101>

// <snippet102>
let biggestFloat = max 2.0 3.0
let biggestInt = max 2 3
// </snippet102>

// <snippet103>
// Error: type mismatch.
//let biggestIntFloat = max 2.0 3
// </snippet103>

// <snippet104>
let testString = max "cab" "cat"
// </snippet104>

// <snippet105>
//let sqrList = [ for i in 1..10 -> i*i ]
// Adding a type annotation fixes the problem:
let sqrList : int list = [ for i in 1..10 -> i*i ]
// </snippet105>

// <snippet106>
//let maxhash = max hash
// The following is acceptable because the argument for maxhash is explicit:
let maxhash obj = max (hash obj)
// </snippet106>

// <snippet107>
//let emptyList10 = Array.create 10 []
// Adding an extra (unused) parameter makes it a function, which is generalizable.
let emptyList10 () = Array.create 10 []
// </snippet107>

// <snippet108>
//let emptyset = Set.empty
// Adding a type parameter and type annotation lets you write a generic value.
let emptyset<'a when 'a : comparison> : Set<'a> = Set.empty
// </snippet108>