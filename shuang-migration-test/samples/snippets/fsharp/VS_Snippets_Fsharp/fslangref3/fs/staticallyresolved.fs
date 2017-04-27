//StaticallyResolved.fs
// created by GHogen 4/21/10
module StaticallyResolved

// <snippet401>
let inline (+@) x y = x + x * y
// Call that uses int.
printfn "%d" (1 +@ 1)
// Call that uses float.
printfn "%f" (1.0 +@ 0.5)
// </snippet401>