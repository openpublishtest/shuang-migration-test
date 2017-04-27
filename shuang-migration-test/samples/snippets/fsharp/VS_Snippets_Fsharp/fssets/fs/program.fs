// Learn more about F# at http://fsharp.net

let Snippet1() =
// <snippet1>
    let set1 = Set.ofList [ 1 .. 3 ]
    let set2 = Set.ofList [ 4 .. 6 ]

    let set3 = set1 + set2
    let set4 = set3 - set1
    let set5 = set3 - set2

    printfn "set1: %A" set1
    printfn "set2: %A" set2
    printfn "set3 = set1 + set2: %A" set3
    printfn "set3 - set1: %A" set4
    printfn "set3 - set2: %A" set5
// </snippet1>
(*
set1: set [1; 2; 3]
set2: set [4; 5; 6]
set3 = set1 + set2: set [1; 2; 3; 4; 5; 6]
set3 - set1: set [4; 5; 6]
set3 - set2: set [1; 2; 3]
*)

let Snippet2() =
// <snippet2>
    let set1 = Set.ofList [ 1 .. 3 ]
    let set2 = Set.ofList [ 2 .. 6 ]
    let setDiff = Set.difference set2 set1
    printfn "Set.difference [2 .. 6] [1 .. 3] yields %A" setDiff
// </snippet2>

let Snippet3() =
// <snippet3>
    let set1 = Set.ofList [ 1 .. 10]
               |> Set.filter (fun elem -> elem % 2 = 0)
    printfn "%A" set1
// </snippet3>
(*
set [2; 4; 6; 8; 10]
*)

let Snippet4() =
// <snippet4>
    let set1 = Set.ofList [ 1 .. 3 ]
    let set2 = Set.ofList [ 2 .. 6 ] 
    let setIntersect = Set.intersect set1 set2
    printfn "Set.intersect [1 .. 3] [2 .. 6] yields %A" setIntersect
// </snippet4>
(*
Set.intersect [1 .. 3] [2 .. 6] yields set [2; 3]
*)

let Snippet5() =
// <snippet5>
    let seqOfSets =
        seq { for i in 1 .. 9 do yield Set.ofList [ i .. i .. 10000 ] }  
    let setResult = Set.intersectMany seqOfSets
    printfn "Numbers between 1 and 10,000 that are divisible by "
    printfn "all the numbers from 1 to 9:"
    printfn "%A" setResult
// </snippet5>

let Snippet6() =
// <snippet6>
    let set1 = Set.ofList [ 1 .. 6 ]
    let set2 = Set.ofList [ 1 .. 5 ]
    let set3 = Set.ofList [ 1 .. 6 ]
    let set4 = Set.ofList [ 5 .. 10 ]
    printfn "%A is a proper subset of %A: %b" set2 set1 (set2.IsProperSubsetOf set1)
    printfn "%A is a proper subset of %A: %b" set3 set1 (set3.IsProperSubsetOf set1) 
    printfn "%A is a proper subset of %A: %b" set4 set1 (set4.IsProperSubsetOf set1) 
// </snippet6>

let Snippet7() =
// <snippet7>
    let set1 = Set.ofList [ 1 .. 6 ]
    let set2 = Set.ofList [ 1 .. 5 ]
    let set3 = Set.ofList [ 1 .. 6 ]
    let set4 = Set.ofList [ 5 .. 10 ]
    printfn "%A is a proper subset of %A: %b" set2 set1 (Set.isProperSubset set2 set1)
    printfn "%A is a proper subset of %A: %b" set3 set1 (Set.isProperSubset set3 set1) 
    printfn "%A is a proper subset of %A: %b" set4 set1 (Set.isProperSubset set4 set1) 
// </snippet7>

let Snippet8() =
// <snippet8>
    let set1 = Set.ofList [ 1 .. 6 ]
    let set2 = Set.ofList [ 1 .. 9 ]
    let set3 = Set.ofList [ 1 .. 6 ]
    let set4 = Set.ofList [ 5 .. 10 ]
    printfn "%A is a proper superset of %A: %b" set2 set1 (set2.IsProperSupersetOf set1)
    printfn "%A is a proper superset of %A: %b" set3 set1 (set3.IsProperSupersetOf set1) 
    printfn "%A is a proper superset of %A: %b" set4 set1 (set4.IsProperSupersetOf set1) 
// </snippet8>

let Snippet9() =
// <snippet9>
    let set1 = Set.ofList [ 1 .. 6 ]
    let set2 = Set.ofList [ 1 .. 9 ]
    let set3 = Set.ofList [ 1 .. 6 ]
    let set4 = Set.ofList [ 5 .. 10 ]
    printfn "%A is a proper superset of %A: %b" set2 set1 (Set.isProperSuperset set2 set1)
    printfn "%A is a proper superset of %A: %b" set3 set1 (Set.isProperSuperset set3 set1) 
    printfn "%A is a proper superset of %A: %b" set4 set1 (Set.isProperSuperset set4 set1) 
    // </snippet9>

let Snippet10() =
// <snippet10>
    let set1 = Set.ofList [ 1 .. 6 ]
    let set2 = Set.ofList [ 1 .. 5 ]
    let set3 = Set.ofList [ 1 .. 6 ]
    let set4 = Set.ofList [ 5 .. 10 ]
    printfn "%A is a subset of %A: %b" set2 set1 (set2.IsSubsetOf set1)
    printfn "%A is a subset of %A: %b" set3 set1 (set3.IsSubsetOf set1) 
    printfn "%A is a subset of %A: %b" set4 set1 (set4.IsSubsetOf set1) 
// </snippet10>

let Snippet11() =
// <snippet11>
    let set1 = Set.ofList [ 1 .. 6 ]
    let set2 = Set.ofList [ 1 .. 5 ]
    let set3 = Set.ofList [ 1 .. 6 ]
    let set4 = Set.ofList [ 5 .. 10 ]
    printfn "%A is a subset of %A: %b" set2 set1 (Set.isSubset set2 set1)
    printfn "%A is a subset of %A: %b" set3 set1 (Set.isSubset set3 set1) 
    printfn "%A is a subset of %A: %b" set4 set1 (Set.isSubset set4 set1) 
// </snippet11>

let Snippet12() =
// <snippet12>
    let set1 = Set.ofList [ 1 .. 6 ]
    let set2 = Set.ofList [ 1 .. 9 ]
    let set3 = Set.ofList [ 1 .. 6 ]
    let set4 = Set.ofList [ 5 .. 10 ]
    printfn "%A is a superset of %A: %b" set2 set1 (set2.IsSupersetOf set1)
    printfn "%A is a superset of %A: %b" set3 set1 (set3.IsSupersetOf set1) 
    printfn "%A is a superset of %A: %b" set4 set1 (set4.IsSupersetOf set1) 
// </snippet12>

let Snippet13() =
// <snippet13>
    let set1 = Set.ofList [ 1 .. 6 ]
    let set2 = Set.ofList [ 1 .. 9 ]
    let set3 = Set.ofList [ 1 .. 6 ]
    let set4 = Set.ofList [ 5 .. 10 ]
    printfn "%A is a superset of %A: %b" set2 set1 (Set.isSuperset set2 set1)
    printfn "%A is a superset of %A: %b" set3 set1 (Set.isSuperset set3 set1) 
    printfn "%A is a superset of %A: %b" set4 set1 (Set.isSuperset set4 set1) 
 // </snippet13>

let Snippet14() =
//<snippet14>
    let set1 = Set.ofList [ 2 .. 2 .. 8 ]
    let set2 = Set.ofList [ 1 .. 2 .. 9 ]
    let set3 = Set.union set1 set2
    printfn "%A union %A yields %A" set1 set2 set3
// </snippet14>

let Snippet15() =
// <snippet15>    
    let seqOfSets =
        seq { for i in 2 .. 5 do yield Set.ofList [ i .. i .. 40 ] }  
    let setResult = Set.unionMany seqOfSets
    printfn "Numbers up to 40 that are multiples of numbers from 2 to 5:"
    Set.iter (fun elem -> printf "%d " elem) setResult

// </snippet15>

let Snippet16() =
// <snippet16>
    let seq1 = seq { for i in 1..2..5 do for j in 1..2..5 do yield i*j }
    printfn "%A" seq1
    let myset = set seq1
    printfn "%A" myset
// </snippet16>

