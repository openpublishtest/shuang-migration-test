// FsSequences
// edited by GHogen 12/12/11

// <snippet1>
// Sequence that has an increment.
seq { 0 .. 10 .. 100 }
// </snippet1>
// <snippet2>
seq { for i in 1 .. 10 do yield i * i }
// </snippet2>
// <snippet3>
seq { for i in 1 .. 10 -> i * i }
// </snippet3>
// <snippet4>
let (height, width) = (10, 10)
let coordinates = seq {
     for row in 0 .. width - 1 do
        for col in 0 .. height - 1 do
            yield (row, col, row*width + col) }
// </snippet4>

// <snippet6>
// Recursive isprime function.
let isprime n =
    let rec check i =
        i > n/2 || (n % i <> 0 && check (i + 1))
    check 2

let aSequence = seq { for n in 1..100 do if isprime n then yield n }
for x in aSequence do
    printfn "%d" x
// </snippet6>
// <snippet5>
seq { for n in 1 .. 100 do if isprime n then yield n }
// </snippet5>
// <snippet7>
let multiplicationTable =
    seq { for i in 1..9 do
            for j in 1..9 do
              yield (i, j, i*j) }
// </snippet7>
// <snippet8>
// Yield the values of a binary tree in a sequence.
type Tree<'a> =
   | Tree of 'a * Tree<'a> * Tree<'a>
   | Leaf of 'a

// inorder : Tree<'a> -> seq<'a>   
let rec inorder tree =
    seq {
      match tree with
          | Tree(x, left, right) ->
               yield! inorder left
               yield x
               yield! inorder right
          | Leaf x -> yield x
    }   
       
let mytree = Tree(6, Tree(2, Leaf(1), Leaf(3)), Leaf(9))
let seq1 = inorder mytree
printfn "%A" seq1
// </snippet8>
// <snippet9>
let seqEmpty = Seq.empty
let seqOne = Seq.singleton 10
// </snippet9>
// <snippet10>
let seqFirst5MultiplesOf10 = Seq.init 5 (fun n -> n * 10)
Seq.iter (fun elem -> printf "%d " elem) seqFirst5MultiplesOf10
// </snippet10>

// <snippet11>
// Convert an array to a sequence by using a cast.
let seqFromArray1 = [| 1 .. 10 |] :> seq<int>
// Convert an array to a sequence by using Seq.ofArray.
let seqFromArray2 = [| 1 .. 10 |] |> Seq.ofArray
// </snippet11>
// <snippet12>
open System
let mutable arrayList1 = new System.Collections.ArrayList(10)
for i in 1 .. 10 do arrayList1.Add(10) |> ignore
let seqCast : seq<int> = Seq.cast arrayList1
// </snippet12>
// <snippet13>
let seqInfinite = Seq.initInfinite (fun index ->
    let n = float( index + 1 )
    1.0 / (n * n * (if ((index + 1) % 2 = 0) then 1.0 else -1.0)))
printfn "%A" seqInfinite
// </snippet13>
let Snippet14() =
    // <snippet14>
    let seq1 = Seq.unfold (fun state -> if (state > 20) then None else Some(state, state + 1)) 0
    printfn "The sequence seq1 contains numbers from 0 to 20."
    for x in seq1 do printf "%d " x
    let fib = Seq.unfold (fun state ->
        if (snd state > 1000) then None
        else Some(fst state + snd state, (snd state, fst state + snd state))) (1,1)
    printfn "\nThe sequence fib contains Fibonacci numbers."
    for x in fib do printf "%d " x
    // </snippet14>
// <snippet15>
// infiniteSequences.fs
// generateInfiniteSequence generates sequences of floating point
// numbers. The sequences generated are computed from the fDenominator
// function, which has the type (int -> float) and computes the
// denominator of each term in the sequence from the index of that
// term. The isAlternating parameter is true if the sequence has
// alternating signs.
let generateInfiniteSequence fDenominator isAlternating =
    if (isAlternating) then
        Seq.initInfinite (fun index -> 1.0 /(fDenominator index) * (if (index % 2 = 0) then -1.0 else 1.0))
    else
        Seq.initInfinite (fun index -> 1.0 /(fDenominator index))

// The harmonic series is the series of reciprocals of whole numbers.
let harmonicSeries = generateInfiniteSequence (fun index -> float index) false
// The harmonic alternating series is like the harmonic series
// except that it has alternating signs.
let harmonicAlternatingSeries = generateInfiniteSequence (fun index -> float index) true
// This is the series of reciprocals of the odd numbers.
let oddNumberSeries = generateInfiniteSequence (fun index -> float (2 * index - 1)) true
// This is the series of recipocals of the squares.
let squaresSeries = generateInfiniteSequence (fun index -> float (index * index)) false

// This function sums a sequence, up to the specified number of terms.
let sumSeq length sequence =
    Seq.unfold (fun state ->
        let subtotal = snd state + Seq.nth (fst state + 1) sequence
        if (fst state >= length) then None
        else Some(subtotal,(fst state + 1, subtotal))) (0, 0.0)

// This function sums an infinite sequence up to a given value
// for the difference (epsilon) between subsequent terms,
// up to a maximum number of terms, whichever is reached first.
let infiniteSum infiniteSeq epsilon maxIteration =
    infiniteSeq
    |> sumSeq maxIteration
    |> Seq.pairwise
    |> Seq.takeWhile (fun elem -> abs (snd elem - fst elem) > epsilon)
    |> List.ofSeq
    |> List.rev
    |> List.head
    |> snd

// Compute the sums for three sequences that converge, and compare
// the sums to the expected theoretical values.
let result1 = infiniteSum harmonicAlternatingSeries 0.00001 100000
printfn "Result: %f  ln2: %f" result1 (log 2.0)

let pi = Math.PI
let result2 = infiniteSum oddNumberSeries 0.00001 10000
printfn "Result: %f pi/4: %f" result2 (pi/4.0)

// Because this is not an alternating series, a much smaller epsilon
// value and more terms are needed to obtain an accurate result.
let result3 = infiniteSum squaresSeries 0.0000001 1000000
printfn "Result: %f pi*pi/6: %f" result3 (pi*pi/6.0)
// </snippet15>
// <snippet16>
let mySeq = seq { for i in 1 .. 10 -> i*i }
let truncatedSeq = Seq.truncate 5 mySeq
let takenSeq = Seq.take 5 mySeq

let truncatedSeq2 = Seq.truncate 20 mySeq
let takenSeq2 = Seq.take 20 mySeq

let printSeq seq1 = Seq.iter (printf "%A ") seq1; printfn ""

// Up to this point, the sequences are not evaluated.
// The following code causes the sequences to be evaluated.
truncatedSeq |> printSeq
truncatedSeq2 |> printSeq
takenSeq |> printSeq
// The following line produces a run-time error (in printSeq):
takenSeq2 |> printSeq
// </snippet16>
//The output, before the error occurs, is as follows.
//1 4 9 16 25 
//1 4 9 16 25 36 49 64 81 100 
//1 4 9 16 25 
//1 4 9 16 25 36 49 64 81 100

let Snippet17() =
// <snippet17>
    // takeWhile
    let mySeqLessThan10 = Seq.takeWhile (fun elem -> elem < 10) mySeq
    mySeqLessThan10 |> printSeq

    // skip
    let mySeqSkipFirst5 = Seq.skip 5 mySeq
    mySeqSkipFirst5 |> printSeq

    // skipWhile
    let mySeqSkipWhileLessThan10 = Seq.skipWhile (fun elem -> elem < 10) mySeq
    mySeqSkipWhileLessThan10 |> printSeq
// </snippet17>

let Snippet170() =
    // <snippet170>
    let mySeq = seq { for i in 1 .. 10 -> i*i }
    let printSeq seq1 = Seq.iter (printf "%A ") seq1; printfn ""
    let mySeqLessThan10 = Seq.takeWhile (fun elem -> elem < 10) mySeq
    mySeqLessThan10 |> printSeq
    // </snippet170>

// skip
let Snippet171() =
    // <snippet171>
    let mySeq = seq { for i in 1 .. 10 -> i*i }
    let printSeq seq1 = Seq.iter (printf "%A ") seq1; printfn ""
    let mySeqSkipFirst5 = Seq.skip 5 mySeq
    mySeqSkipFirst5 |> printSeq
    // </snippet171>
// skipWhile
let Snippet172() =
    // <snippet172>
    let mySeq = seq { for i in 1 .. 10 -> i*i }
    let printSeq seq1 = Seq.iter (printf "%A ") seq1; printfn ""
    let mySeqSkipWhileLessThan10 = Seq.skipWhile (fun elem -> elem < 10) mySeq
    mySeqSkipWhileLessThan10 |> printSeq
// </snippet172>
//The output is as follows.
//1 4 9 
//36 49 64 81 100 
//16 25 36 49 64 81 100 
let Snippet18() =
// <snippet18>
    let printSeq seq1 = Seq.iter (printf "%A ") seq1; printfn ""
    let seqPairwise = Seq.pairwise (seq { for i in 1 .. 10 -> i*i })
    printSeq seqPairwise

    printfn ""
    let seqDelta = Seq.map (fun elem -> snd elem - fst elem) seqPairwise
    printSeq seqDelta
// </snippet18>

let printList list1 = List.iter (printf "%A ") list1

let Snippet180() =
    // <snippet180>
    let seqNumbers = [ 1.0; 1.5; 2.0; 1.5; 1.0; 1.5 ] :> seq<float>
    let seqWindows = Seq.windowed 3 seqNumbers
    let seqMovingAverage = Seq.map Array.average seqWindows
    printfn "Initial sequence: "
    printSeq seqNumbers
    printfn "\nWindows of length 3: "
    printSeq seqWindows
    printfn "\nMoving average: "
    printSeq seqMovingAverage
    // </snippet180>

 
let Snippet19() =
    // <snippet19>
    let sequence1 = seq { 1 .. 10 }
    let sequence2 = seq { 10 .. -1 .. 1 }

    // Compare two sequences element by element.
    let compareSequences = Seq.compareWith (fun elem1 elem2 ->
        if elem1 > elem2 then 1
        elif elem1 < elem2 then -1
        else 0) 

    let compareResult1 = compareSequences sequence1 sequence2
    match compareResult1 with
    | 1 -> printfn "Sequence1 is greater than sequence2."
    | -1 -> printfn "Sequence1 is less than sequence2."
    | 0 -> printfn "Sequence1 is equal to sequence2."
    | _ -> failwith("Invalid comparison result.")
    // </snippet19>

let Snippet201() =
// <snippet201>
    let mySeq1 = seq { 1.. 100 }
    let printSeq seq1 = Seq.iter (printf "%A ") seq1; printfn ""
    let seqResult = Seq.countBy (fun elem -> if elem % 3 = 0 then 0
                                             elif elem % 3 = 1 then 1
                                             else 2) mySeq1

    printSeq seqResult
// </snippet201>

let Snippet202() =
// <snippet202>
    let sequence = seq { 1 .. 100 }
    let printSeq seq1 = Seq.iter (printf "%A ") seq1; printfn ""
    let sequences3 = Seq.groupBy (fun index ->
                                    if (index % 3 = 0) then 0
                                      elif (index % 3 = 1) then 1
                                      else 2) sequence
    sequences3 |> printSeq
// </snippet202>

let Snippet20() =
    // <snippet20>
    let mySeq1 = seq { 1.. 100 }
    let printSeq seq1 = Seq.iter (printf "%A ") seq1; printfn ""
    let seqResult = Seq.countBy (fun elem ->
        if (elem % 2 = 0) then 0 else 1) mySeq1

    printSeq seqResult
    // </snippet20>

let Snippet21() =
// <snippet21>
    let sequence = seq { 1 .. 100 }
    let printSeq seq1 = Seq.iter (printf "%A ") seq1; printfn ""
    let sequences3 = Seq.groupBy (fun index ->
        if (index % 2 = 0) then 0 else 1) sequence
    sequences3 |> printSeq
// </snippet21>
 
// <snippet22>
let binary n =
    let rec generateBinary n =
        if (n / 2 = 0) then [n]
        else (n % 2) :: generateBinary (n / 2)
    generateBinary n |> List.rev |> Seq.ofList

printfn "%A" (binary 1024)

let resultSequence = Seq.distinct (binary 1024)
printfn "%A" resultSequence
// </snippet22>
let Snippet23() =
    // <snippet23>
    let inputSequence = { -5 .. 10 }
    let printSeq seq1 = Seq.iter (printf "%A ") seq1; printfn ""
    printfn "Original sequence: "
    printSeq inputSequence
    printfn "\nSequence with distinct absolute values: "
    let seqDistinctAbsoluteValue = Seq.distinctBy (fun elem -> abs elem) inputSequence
    seqDistinctAbsoluteValue |> printSeq
    // </snippet23>
// <snippet24>
type ArrayContainer(start, finish) =
    let internalArray = [| start .. finish |]
    member this.RangeSeq = Seq.readonly internalArray
    member this.RangeArray = internalArray

let newArray = new ArrayContainer(1, 10)
let rangeSeq = newArray.RangeSeq
let rangeArray = newArray.RangeArray
// These lines produce an error: 
//let myArray = rangeSeq :> int array
//myArray.[0] <- 0
// The following line does not produce an error. 
// It does not preserve encapsulation.
rangeArray.[0] <- 0
// </snippet24>

module Snippet25 =
// <snippet25>
    printfn "%A" (Seq.append [| 1; 2; 3|] [ 4; 5; 6])
// </snippet25>
(*
seq [1; 2; 3; 4; ...]
*)

module Snippet26 =
// <snippet26>
    // You can use Seq.average to average elements of a list, array, or sequence.
    let average1 = Seq.average [ 1.0 .. 10.0 ]
    printfn "Average: %f" average1
    // To average a sequence of integers, use Seq.averageBy to convert to float.
    let average2 = Seq.averageBy (fun elem -> float elem) (seq { 1 .. 10 })
    printfn "Average: %f" average2
// </snippet26>
(*
Average: 5.500000
Average: 5.500000
*)

module Snippet27 =
// <snippet27>
   // Recursive isprime function.
   let isPrime n =
       let rec check i =
           i > n/2 || (n % i <> 0 && check (i + 1))
       check 2

   let seqPrimes = seq { for n in 2 .. 10000 do if isPrime n then yield n }
   // Cache the sequence to avoid recomputing the sequence elements.
   let cachedSeq = Seq.cache seqPrimes
   for index in 1..5 do
       printfn "%d is prime." (Seq.nth (Seq.length cachedSeq - index) cachedSeq)
// </snippet27>
(*
9973 is prime.
9967 is prime.
9949 is prime.
9941 is prime.
9931 is prime.
*)

// Seq.collect.
module Snippet28 =
// <snippet28>
    let addNegations seq1 =
       Seq.collect (fun x -> seq { yield x; yield -x }) seq1
       |> Seq.sort
    addNegations [ 1 .. 4 ] |> Seq.iter (fun elem -> printf "%d " elem)
    printfn ""
    addNegations [| 0; -4; 2; -12 |] |> Seq.iter (fun elem -> printf "%d " elem)
// </snippet28>
(*
-4 -3 -2 -1 1 2 3 4 
-12 -4 -2 0 0 2 4 12
*)

module Snippet29 =
// <snippet29>
    // Using Seq.append to append an array to a list.
    let seq1to10 = Seq.append [1; 2; 3] [| 4; 5; 6; 7; 8; 9; 10 |]
    // Using Seq.concat to concatenate a list of arrays.
    let seqResult = Seq.concat [ [| 1; 2; 3 |]; [| 4; 5; 6 |]; [|7; 8; 9|] ]
    Seq.iter (fun elem -> printf "%d " elem) seq1to10
    printfn ""
    Seq.iter (fun elem -> printf "%d " elem) seqResult
// </snippet29>
(*
1 2 3 4 5 6 7 8 9 10 
1 2 3 4 5 6 7 8 9 
*)

module Snippet30 =
// <snippet30>
    // Normally sequences are evaluated lazily.  In this case,
    // the sequence is created from a list, which is not evaluated
    // lazily. Therefore, without Seq.delay, the elements would be
    // evaluated at the time of the call to makeSequence.
    let makeSequence function1 maxNumber = Seq.delay (fun () ->
        let rec loop n acc =
            printfn "Evaluating %d." n
            match n with
            | 0 -> acc
            | n -> (function1 n) :: loop (n - 1) acc
        loop maxNumber []
        |> Seq.ofList)
    printfn "Calling makeSequence."
    let seqSquares = makeSequence (fun x -> x * x) 4          
    let seqCubes = makeSequence (fun x -> x * x * x) 4
    printfn "Printing sequences."
    printfn "Squares:"
    seqSquares |> Seq.iter (fun x -> printf "%d " x)
    printfn "\nCubes:"
    seqCubes |> Seq.iter (fun x -> printf "%d " x)                       
// </snippet30>
(*
Calling makeSequence.
Printing sequences.
Squares:
Evaluating 4.
Evaluating 3.
Evaluating 2.
Evaluating 1.
Evaluating 0.
16 9 4 1 
Cubes:
Evaluating 4.
Evaluating 3.
Evaluating 2.
Evaluating 1.
Evaluating 0.
64 27 8 1
*)
module Snippet31 =
// <snippet31>
    // Compare the output of this example with that of the previous.
    // Notice that Seq.delay delays the
    // execution of the loop until the sequence is used.
    let makeSequence function1 maxNumber =
        let rec loop n acc =
            printfn "Evaluating %d." n
            match n with
            | 0 -> acc
            | n -> (function1 n) :: loop (n - 1) acc
        loop maxNumber []
        |> Seq.ofList
    printfn "Calling makeSequence."
    let seqSquares = makeSequence (fun x -> x * x) 4          
    let seqCubes = makeSequence (fun x -> x * x * x) 4
    printfn "Printing sequences."
    printfn "Squares:"
    seqSquares |> Seq.iter (fun x -> printf "%d " x)
    printfn "\nCubes:"
    seqCubes |> Seq.iter (fun x -> printf "%d " x)
// </snippet31>
(*
Calling makeSequence.
Evaluating 4.
Evaluating 3.
Evaluating 2.
Evaluating 1.
Evaluating 0.
Evaluating 4.
Evaluating 3.
Evaluating 2.
Evaluating 1.
Evaluating 0.
Printing sequences.
Squares:
16 9 4 1 
Cubes:
64 27 8 1 
*)

module Snippet32 =
// <snippet32>
    // A generic empty sequence.
    let emptySeq1 = Seq.empty
    // A typed generic sequence.
    let emptySeq2 = Seq.empty<string>
// </snippet32>

module Snippet33 =
// <snippet33>
    // Use Seq.exists to determine whether there is an element of a sequence
    // that satisfies a given Boolean expression.
    // containsNumber returns true if any of the elements of the supplied sequence match 
    // the supplied number.
    let containsNumber number seq1 = Seq.exists (fun elem -> elem = number) seq1
    let seq0to3 = seq {0 .. 3}
    printfn "For sequence %A, contains zero is %b" seq0to3 (containsNumber 0 seq0to3)
// </snippet33>
(*
For sequence seq [0; 1; 2; 3], contains zero is true
*)


module Snippet34 =
// <snippet34>
    // Use Seq.exists2 to compare elements in two sequences.
    // isEqualElement returns true if any elements at the same position in two supplied
    // sequences match.
    let isEqualElement seq1 seq2 = Seq.exists2 (fun elem1 elem2 -> elem1 = elem2) seq1 seq2
    let seq1to5 = seq { 1 .. 5 }
    let seq5to1 = seq { 5 .. -1 .. 1 }
    if (isEqualElement seq1to5 seq5to1) then
        printfn "Sequences %A and %A have at least one equal element at the same position." seq1to5 seq5to1
    else
        printfn "Sequences %A and %A do not have any equal elements that are at the same position." seq1to5 seq5to1
// </snippet34>

module Snippet35 =
// <snippet35>
    let random = new System.Random()
    Seq.initInfinite (fun _ -> random.Next())
    |> Seq.filter (fun x -> x % 2 = 0)
    |> Seq.take 5
    |> Seq.iter (fun elem -> printf "%d " elem)
    printfn ""
// </snippet35>
(* Sample Output:
2140052690 963487404 467169526 1800517368 1225141818
*)
module Snippet36 =
// <snippet36>
    let isDivisibleBy number elem = elem % number = 0
    let result = Seq.find (isDivisibleBy 5) [ 1 .. 100 ]
    printfn "%d " result
// </snippet36>

module Snippet37 =
// <snippet37>
    let seqA = [| 2 .. 100 |]
    let delta = 1.0e-10
    let isPerfectSquare (x:int) =
        let y = sqrt (float x)
        abs(y - round y) < delta
    let isPerfectCube (x:int) =
        let y = System.Math.Pow(float x, 1.0/3.0)
        abs(y - round y) < delta
    let element = Seq.find (fun elem -> isPerfectSquare elem && isPerfectCube elem) seqA
    let index = Seq.findIndex (fun elem -> isPerfectSquare elem && isPerfectCube elem) seqA
    printfn "The first element that is both a square and a cube is %d and its index is %d." element index
// </snippet37>
(*
The first element that is both a square and a cube is 64 and its index is 62.
*)

module Snippet38 =
// <snippet38>
    let sumSeq sequence1 = Seq.fold (fun acc elem -> acc + elem) 0 sequence1
    Seq.init 10 (fun index -> index * index)
    |> sumSeq
    |> printfn "The sum of the elements is %d."
// </snippet38>
(*
The sum of the elements is 285.
*)

module Snippet39 =
// <snippet39>
    // This function can be used on any sequence, so the same function
    // works with both lists and arrays.
    let allPositive coll = Seq.forall (fun elem -> elem > 0) coll
    printfn "%A" (allPositive [| 0; 1; 2; 3 |])
    printfn "%A" (allPositive [ 1; 2; 3 ])
// </snippet39>

module Snippet40 =
// <snippet40>
    // This function can be used on any sequence, so the same function
    // works with both lists and arrays.
    let allEqual coll = Seq.forall2 (fun elem1 elem2 -> elem1 = elem2) coll
    printfn "%A" (allEqual [| 1; 2 |] [| 1; 2 |])
    printfn "%A" (allEqual [ 1; 2 ] [ 2; 1 ])
// </snippet40>
(*
true
false
*)
module Snippet41 =
// <snippet41>
    let headItem = Seq.head [| 1 .. 10 |]
    printfn "%d" headItem
// </snippet41>

module Snippet42 =
// <snippet42>
    let emptySeq = Seq.empty
    let nonEmptySeq = seq { 1 .. 10 }
    Seq.isEmpty emptySeq |> printfn "%b"
    Seq.isEmpty nonEmptySeq |> printfn "%b"
// </snippet42>
(*
true
false
*)

module Snippet43 =
// <snippet43>
    let seq1 = [1; 2; 3]
    let seq2 = [4; 5; 6]
    Seq.iter (fun x -> printfn "Seq.iter: element is %d" x) seq1
    Seq.iteri(fun i x -> printfn "Seq.iteri: element %d is %d" i x) seq1
    Seq.iter2 (fun x y -> printfn "Seq.iter2: elements are %d %d" x y) seq1 seq2
// </snippet43>

module Snippet44 =
// <snippet44>
    let table1 = seq { for i in 1 ..10 do
                          for j in 1 .. 10 do
                              yield (i, j, i*j)
                     }
    Seq.length table1 |> printfn "Length: %d"
// </snippet44>
(*
Length: 100
*)
module Snippet45 =
// <snippet45>
    let data = [(1,1,2001); (2,2,2004); (6,17,2009)]
    let seq1 =
        data |> Seq.map (fun (a,b,c) -> 
            let date = new System.DateTime(c, a, b)
            date.ToString("F"))

    for i in seq1 do printfn "%A" i
// </snippet45>
(*
"Monday, January 01, 2001 12:00:00 AM"
"Monday, February 02, 2004 12:00:00 AM"
"Wednesday, June 17, 2009 12:00:00 AM"
*)

module Snippet46 =
// <snippet46>
    let seq1 = [1; 2; 3]
    let seq2 = [4; 5; 6]
    let sumSeq = Seq.map2 (fun x y -> x + y) seq1 seq2
    printfn "%A" sumSeq
// </snippet46>

module Snippet47 =
// <snippet47>
    let seq1 = [1; 2; 3]
    let newSeq = Seq.mapi (fun i x -> (i, x)) seq1
    printfn "%A" newSeq
// </snippet47>

module Snippet48 =
// <snippet48>
    [| for x in -100 .. 100 -> 4 - x * x |]
    |> Seq.max
    |> printfn "%A"
// </snippet48>

module Snippet56 =
// <snippet56>
    [| -10.0 .. 10.0 |]
    |> Seq.maxBy (fun x -> 1.0 - x * x)
    |> printfn "%A"
// </snippet56>
(*
0.0
*)

module Snippet57 =
// <snippet57>
    [| for x in -100 .. 100 -> x * x - 4 |]
    |> Seq.min
    |> printfn "%A" 
// </snippet57>
(* output:
-4
*)

module Snippet58 =
// <snippet58>
    [| -10.0 .. 10.0 |]
    |> Seq.minBy (fun x -> x * x - 1.0)
    |> printfn "%A"
// </snippet58>
(*
0.0
*)

module Snippet59 =
// <snippet59>
    let seq1 = [ -10 .. 10 ]
    Seq.nth 5 seq1
    |> printfn "The fifth element: %d"
// </snippet59>

module Snippet60 =
// <snippet60>
    let seq1 = Array.init 10 (fun index -> index.ToString()) 
               |> Seq.ofArray
// </snippet60>
(*
val seq1 : seq<string>
*)

module Snippet61 =
// <snippet61>
    let seq1 = List.init 10 (fun index -> index.ToString())
               |> Seq.ofList
// </snippet61>

module Snippet62 =
// <snippet62>
    let valuesSeq = [ ("a", 1); ("b", 2); ("c", 3) ]

    let resultPick = Seq.pick (fun elem ->
                        match elem with
                        | (value, 2) -> Some value
                        | _ -> None) valuesSeq
    printfn "%A" resultPick
// </snippet62>

module Snippet63 =
// <snippet63>
    let names = [| "A"; "man"; "landed"; "on"; "the"; "moon" |]
    let sentence = names |> Seq.reduce (fun acc item -> acc + " " + item)
    printfn "sentence = %s" sentence
// </snippet63>

module Snippet64 =
// <snippet64>
    let initialBalance = 1122.73
    let transactions = [| -100.00; +450.34; -62.34; -127.00; -13.50; -12.92 |]
    let balances =
        Seq.scan (fun balance transactionAmount -> balance + transactionAmount)
                 initialBalance transactions
        |> Array.ofSeq
    printfn "Initial balance:\n $%10.2f" initialBalance
    printfn "Transaction   Balance"
    for i in 0 .. Seq.length transactions - 1 do
        printfn "$%10.2f $%10.2f" transactions.[i] balances.[i]
    printfn "Final balance:\n $%10.2f" balances.[ Array.length balances - 1]
// </snippet64>

module Snippet65 =
// <snippet65>
    let seq1 = Seq.singleton "zero"
// </snippet65>

