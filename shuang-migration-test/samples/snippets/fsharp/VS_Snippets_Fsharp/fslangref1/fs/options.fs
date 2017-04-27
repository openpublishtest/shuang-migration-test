// Options.fs
// created GHogen 3/18/09
module Options

// <snippet1404>
let keepIfPositive (a : int) = if a > 0 then Some(a) else None
// </snippet1404>

// <snippet1401>
let exists (x : int option) = 
    match x with
    | Some(x) -> true
    | None -> false
// </snippet1401>

// <snippet1402>
open System.IO
let openFile filename =
    try
        let file = File.Open (filename, FileMode.Create)
        Some(file)
    with
        | ex -> eprintf "An exception occurred with message %s" ex.Message
                None    
// </snippet1402>

// <snippet1403>
let rec tryFindMatch pred list =
    match list with
    | head :: tail -> if pred(head)
                        then Some(head)
                        else tryFindMatch pred tail
    | [] -> None

// result1 is Some 100 and its type is int option.
let result1 = tryFindMatch (fun elem -> elem = 100) [ 200; 100; 50; 25 ] 

// result2 is None and its type is int option.
let result2 = tryFindMatch (fun elem -> elem = 26) [ 200; 100; 50; 25 ]
// </snippet1403>

