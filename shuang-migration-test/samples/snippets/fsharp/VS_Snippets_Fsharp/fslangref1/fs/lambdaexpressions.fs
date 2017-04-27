// LambdaExpressions.fs
// created by GHogen 3/18/09
module LambdaExpressions
(*
// <snippet301>
fun x -> x + 1
fun a b c -> printfn "%A %A %A" a b c
fun (a: int) (b: int) (c: int) -> a + b * c
fun x y -> let swap (a, b) = (b, a) in swap (x, y)
// </snippet301>
*)

// <snippet302>
let list = List.map (fun i -> i + 1) [1;2;3]
printfn "%A" list
// </snippet302>
