// Lazy.fs
// created by GHogen 4/21/10
module Lazy

// <snippet73011>
let x = 10
let result = lazy (x + 10)
printfn "%d" (result.Force())
// </snippet73011>