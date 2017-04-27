// InlineFunctions.fs
// Created by GHogen 4/20/10

module InlineFunctions

// <snippet201>
let inline increment x = x + 1
type WrapInt32() =
    member inline this.incrementByOne(x) = x + 1
    static member inline Increment(x) = x + 1
// </snippet201>

// <snippet202>
let inline printAsFloatingPoint number =
    printfn "%f" (float number)
// </snippet202>