// Created by GHogen 3/10/10

// <snippet1>
let array1 = [| 1; 2; 3 |]
// </snippet1>
let ArrayInit() =
    // <snippet2>
    let array1 = 
        [|
            1
            2
            3
         |]
    // </snippet2>
    ()

// <snippet3>
let array3 = [| for i in 1 .. 10 -> i * i |]
// </snippet3>
// <snippet4>
let arrayOfTenZeroes : int array = Array.zeroCreate 10
// </snippet4>
let ArraySyntax () =
    // <snippet5>
    array1.[0]
    // </snippet5>
    // <snippet51>
    // Accesses elements from 0 to 2.
    // <snippet6>
    array1.[0..2]  
    // </snippet6>
    // Accesses elements from the beginning of the array to 2.
    // <snippet7>
    array1.[..2] 
    // </snippet7>
    // Accesses elements from 2 to the end of the array.
    // <snippet8>
    array1.[2..] 
    // </snippet8>
    // </snippet51>
let ArrayCreateSetGet () =
    // <snippet9>
    let array1 = Array.create 10 ""
    for i in 0 .. array1.Length - 1 do
        Array.set array1 i (i.ToString())
    for i in 0 .. array1.Length - 1 do
        printf "%s " (Array.get array1 i)
    // </snippet9>
//The output is as follows.
//0 1 2 3 4 5 6 7 8 9
// <snippet91>
// <snippet10>
let myEmptyArray = Array.empty
printfn "Length of empty array: %d" myEmptyArray.Length
// </snippet10>

// <snippet100>
printfn "Array of floats set to 5.0: %A" (Array.create 10 5.0)
// </snippet100>
// <snippet101>
printfn "Array of squares: %A" (Array.init 10 (fun index -> index * index))
// </snippet101>
let (myZeroArray : float array) = Array.zeroCreate 10
// </snippet91>
//The output is as follows.
//Length of empty array: 0
//Area of floats set to 5.0: [|5.0; 5.0; 5.0; 5.0; 5.0; 5.0; 5.0; 5.0; 5.0; 5.0|]
//Array of squares: [|0; 1; 4; 9; 16; 25; 36; 49; 64; 81|]

// <snippet11>
open System.Text

let firstArray : StringBuilder array = Array.init 3 (fun index -> new StringBuilder(""))
let secondArray = Array.copy firstArray
// Reset an element of the first array to a new value.
firstArray.[0] <- new StringBuilder("Test1")
// Change an element of the first array.
firstArray.[1].Insert(0, "Test2") |> ignore
printfn "%A" firstArray
printfn "%A" secondArray
// </snippet11>
//The output of the preceding code is as follows:
//[|Test1; Test2; |]
//[|; Test2; |]

// <snippet12>
let a1 = [| 0 .. 99 |]
let a2 = Array.sub a1 5 10
printfn "%A" a2
// </snippet12>
//The output shows that the subarray starts at element 5 and contains 10 elements.
//[|5; 6; 7; 8; 9; 10; 11; 12; 13; 14|]

// <snippet13>
printfn "%A" (Array.append [| 1; 2; 3|] [| 4; 5; 6|])
// </snippet13>
//The output of the preceding code is as follows.
//[|1; 2; 3; 4; 5; 6|]

// <snippet14>
printfn "%A" (Array.choose (fun elem -> if elem % 2 = 0 then
                                            Some(float (elem*elem - 1))
                                        else
                                            None) [| 1 .. 10 |])
// </snippet14>
//The output of the preceding code is as follows.
//[|3.0; 15.0; 35.0; 63.0; 99.0|]

// <snippet15>
printfn "%A" (Array.collect (fun elem -> [| 0 .. elem |]) [| 1; 5; 10|])
// </snippet15>
//The output of the preceding code is as follows.
//[|0; 1; 0; 1; 2; 3; 4; 5; 0; 1; 2; 3; 4; 5; 6; 7; 8; 9; 10|]

// <snippet16>
let multiplicationTable max = seq { for i in 1 .. max -> [| for j in 1 .. max -> (i, j, i*j) |] }
printfn "%A" (Array.concat (multiplicationTable 3))
// </snippet16>
//The output of the preceding code is as follows.
//[|(1, 1, 1); (1, 2, 2); (1, 3, 3); (2, 1, 2); (2, 2, 4); (2, 3, 6); (3, 1, 3);
//  (3, 2, 6); (3, 3, 9)|]

// <snippet17>
printfn "%A" (Array.filter (fun elem -> elem % 2 = 0) [| 1 .. 10|])
// </snippet17>
//The output of the preceding code is as follows.
//[|2; 4; 6; 8; 10|]

// <snippet18>
let stringReverse (s: string) =
    System.String(Array.rev (s.ToCharArray()))

printfn "%A" (stringReverse("!dlrow olleH"))
// </snippet18>
//The output of the preceding code is as follows.
//"Hello world!"

// <snippet19>
[| 1 .. 10 |]
|> Array.filter (fun elem -> elem % 2 = 0)
|> Array.choose (fun elem -> if (elem <> 8) then Some(elem*elem) else None)
|> Array.rev
|> printfn "%A"
// </snippet19>
//The output is
//[|100; 36; 16; 4|]

// <snippet20>
let my2DArray = array2D [ [ 1; 0]; [0; 1] ]
// </snippet20>

// <snippet21>
let arrayOfArrays = [| [| 1.0; 0.0 |]; [|0.0; 1.0 |] |]
let twoDimensionalArray = Array2D.init 2 2 (fun i j -> arrayOfArrays.[i].[j]) 
// </snippet21>

// <snippet22>
twoDimensionalArray.[0, 1] <- 1.0
// </snippet22>

// <snippet23>
// <snippet231>
let allNegative = Array.exists (fun elem -> abs (elem) = elem) >> not
printfn "%A" (allNegative [| -1; -2; -3 |])
printfn "%A" (allNegative [| -10; -1; 5 |])
printfn "%A" (allNegative [| 0 |])
// </snippet231>
// <snippet232>
let haveEqualElement = Array.exists2 (fun elem1 elem2 -> elem1 = elem2)
printfn "%A" (haveEqualElement [| 1; 2; 3 |] [| 3; 2; 1|])
// </snippet232>
// </snippet23>
//The output of the preceding code is as follows.
//true
//false
//false
//true
// <snippet24>
// <snippet241>
let allPositive = Array.forall (fun elem -> elem > 0)
printfn "%A" (allPositive [| 0; 1; 2; 3 |])
printfn "%A" (allPositive [| 1; 2; 3 |])
// </snippet241>
// <snippet242>
let allEqual = Array.forall2 (fun elem1 elem2 -> elem1 = elem2)
printfn "%A" (allEqual [| 1; 2 |] [| 1; 2 |])
printfn "%A" (allEqual [| 1; 2 |] [| 2; 1 |])
// </snippet242>
// </snippet24>
//The output for these examples is as follows.
//false
//true
//true
//false

// <snippet25>
let arrayA = [| 2 .. 100 |]
let delta = 1.0e-10
let isPerfectSquare (x:int) =
    let y = sqrt (float x)
    abs(y - round y) < delta
let isPerfectCube (x:int) =
    let y = System.Math.Pow(float x, 1.0/3.0)
    abs(y - round y) < delta
let element = Array.find (fun elem -> isPerfectSquare elem && isPerfectCube elem) arrayA
let index = Array.findIndex (fun elem -> isPerfectSquare elem && isPerfectCube elem) arrayA
printfn "The first element that is both a square and a cube is %d and its index is %d." element index
// </snippet25>
//The output is as follows.
//The first element that is both a square and a cube is 64 and its index is 62.
let Snippet26() =
    // <snippet26>
    let delta = 1.0e-10
    let isPerfectSquare (x:int) =
        let y = sqrt (float x)
        abs(y - round y) < delta
    let isPerfectCube (x:int) =
        let y = System.Math.Pow(float x, 1.0/3.0)
        abs(y - round y) < delta
    let lookForCubeAndSquare array1 =
        let result = Array.tryFind (fun elem -> isPerfectSquare elem && isPerfectCube elem) array1
        match result with
        | Some x -> printfn "Found an element: %d" x
        | None -> printfn "Failed to find a matching element."

    lookForCubeAndSquare [| 1 .. 10 |]
    lookForCubeAndSquare [| 100 .. 1000 |]
    lookForCubeAndSquare [| 2 .. 50 |]
    // </snippet26>
Snippet26()

//The output is as follows.
//Found an element: 1
//Found an element: 729

let Snippet27() =
    // <snippet27>
    let findPerfectSquareAndCube array1 =
        let delta = 1.0e-10
        let isPerfectSquare (x:int) =
            let y = sqrt (float x)
            abs(y - round y) < delta
        let isPerfectCube (x:int) =
            let y = System.Math.Pow(float x, 1.0/3.0)
            abs(y - round y) < delta
        // intFunction : (float -> float) -> int -> int
        // Allows the use of a floating point function with integers.
        let intFunction function1 number = int (round (function1 (float number)))
        let cubeRoot x = System.Math.Pow(x, 1.0/3.0)
        // testElement: int -> (int * int * int) option
        // Test an element to see whether it is a perfect square and a perfect
        // cube, and, if so, return the element, square root, and cube root
        // as an option value. Otherwise, return None.
        let testElement elem = 
            if isPerfectSquare elem && isPerfectCube elem then
                Some(elem, intFunction sqrt elem, intFunction cubeRoot elem)
            else None
        match Array.tryPick testElement array1 with
        | Some (n, sqrt, cuberoot) -> printfn "Found an element %d with square root %d and cube root %d." n sqrt cuberoot
        | None -> printfn "Did not find an element that is both a perfect square and a perfect cube."

    findPerfectSquareAndCube [| 1 .. 10 |]
    findPerfectSquareAndCube [| 2 .. 100 |]
    findPerfectSquareAndCube [| 100 .. 1000 |]
    findPerfectSquareAndCube [| 1000 .. 10000 |]
    findPerfectSquareAndCube [| 2 .. 50 |]
    // </snippet27>
Snippet27()
//The output is as follows.
//Found an element 1 with square root 1 and cube root 1.
//Found an element 64 with square root 8 and cube root 4.
//Found an element 729 with square root 27 and cube root 9.
//Found an element 4096 with square root 64 and cube root 16.
//Did not find an element that is both a perfect square and a perfect cube.

// <snippet28>
let arrayFill1 = [| 1 .. 25 |]
Array.fill arrayFill1 2 20 0
printfn "%A" arrayFill1
// </snippet28>
//The output is as follows.
//[|1; 2; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 0; 23; 24; 25|]

// Now filling in gaps in the snippets.
let Snipept29() =
    // <snippet29>
    let avg2 = Array.averageBy (fun elem -> float elem) [|1 .. 10|]
    printfn "%f" avg2
    // </snippet29>

let Snippet30() =
// Array.blit
// <snippet30>
    let array1 = [| 1 .. 10 |]
    let array2 = Array.zeroCreate 20
    // Copy 4 elements from index 3 of array1 to index 5 of array2.
    Array.blit array1 3 array2 5 4
    printfn "%A" array2
// </snippet30>

let Snippet31() =
// Array.copy
// <snippet31>
    let array1 = [| 1 .. 10 |]
    let array2 = Array.copy array1
    printfn "%A\n%A" array1 array2
// </snippet31>

let Snippet32() =
// Array.fold
    // <snippet32>
    let sumArray array = Array.fold (fun acc elem -> acc + elem) 0 array
    printfn "Sum of the elements of array %A is %d." [ 1 .. 3 ] (sumArray [| 1 .. 3 |])

    // The following example computes the average of a array.
    let averageArray array = (Array.fold (fun acc elem -> acc + float elem) 0.0 array / float array.Length)

    // The following example computes the standard deviation of a array.
    // The standard deviation is computed by taking the square root of the
    // sum of the variances, which are the differences between each value
    // and the average.
    let stdDevArray array =
        let avg = averageArray array
        sqrt (Array.fold (fun acc elem -> acc + (float elem - avg) ** 2.0 ) 0.0 array / float array.Length)

    let testArray arrayTest =
        printfn "Array %A average: %f stddev: %f" arrayTest (averageArray arrayTest) (stdDevArray arrayTest)

    testArray [|1; 1; 1|]
    testArray [|1; 2; 1|]
    testArray [|1; 2; 3|]

    // Array.fold is the same as to Array.iter when the accumulator is not used.
    let printArray array = Array.fold (fun acc elem -> printfn "%A" elem) () array
    printArray [|0.0; 1.0; 2.5; 5.1 |]
    // </snippet32>

let Snippet33() =
 // <snippet33>
    let removeOutliers array1 min max =
        Array.partition (fun elem -> elem > min && elem < max) array1
        |> fst
    removeOutliers [| 1 .. 100 |] 50 60
    |> printf "%A"
 // </snippet33>

let Snippet34() =
// Array.permute
// <snippet34>
    let printPermutation n array1 =
        let length = Array.length array1
        if (n > 0 && n < length) then
            Array.permute (fun index -> (index + n) % length) array1
        else
            array1
        |> printfn "%A"
    let array1 = [| 1 .. 5 |]
    // There are 5 valid permutations of array1, with n from 0 to 4.
    for n in 0 .. 4 do
        printPermutation n array1 
// </snippet34>

let Snippet35() =
// Array.scan
// <snippet35>
    let initialBalance = 1122.73
    let transactions = [| -100.00; +450.34; -62.34; -127.00; -13.50; -12.92 |]
    let balances =
        Array.scan (fun balance transactionAmount -> balance + transactionAmount) initialBalance transactions
    printfn "Initial balance:\n $%10.2f" initialBalance
    printfn "Transaction   Balance"
    for i in 0 .. Array.length transactions - 1 do
        printfn "$%10.2f $%10.2f" transactions.[i] balances.[i]
    printfn "Final balance:\n $%10.2f" balances.[ Array.length balances - 1]
// </snippet35>

let Snippet36() =
// Array.scanBack
// <snippet36>
    // An array of functions that transform 
    // integers. (int -> int)
    let ops1 =
     [| fun x -> x + 1
        fun x -> x + 2
        fun x -> x - 5 |]

    let ops2 =
     [| fun x -> x + 1
        fun x -> x * 5
        fun x -> x * x |]

    // Compare scan and scanBack, which apply the
    // operations in the opposite order.
    let compareOpOrder ops x0 =
        let xs1 = Array.scan (fun x op -> op x) x0 ops
        let xs2 = Array.scanBack (fun op x -> op x) ops x0

        // Print the intermediate results
        let xs = Array.zip xs1 (Array.rev xs2)
        for (x1, x2) in xs do
            printfn "%10d %10d" x1 x2
        printfn ""

    compareOpOrder ops1 10
    compareOpOrder ops2 10
// </snippet36>

let Snippet37() =
    // <snippet37>
    let sortedArray1 = Array.sort [|1; 4; 8; -2; 5|]
    printfn "%A" sortedArray1
    // </snippet37>

let Snippet38() =
    // <snippet38>
    let sortedArray2 = Array.sortBy (fun elem -> abs elem) [|1; 4; 8; -2; 5|]
    printfn "%A" sortedArray2
    // </snippet38>

module Snippet39 =
    // <snippet39>
    type Widget = { ID: int; Rev: int }

    let compareWidgets widget1 widget2 =
       if widget1.ID < widget2.ID then -1 else
       if widget1.ID > widget2.ID then 1 else
       if widget1.Rev < widget2.Rev then -1 else
       if widget1.Rev > widget2.Rev then 1 else
       0

    let arrayToSort =
     [|
        { ID = 92; Rev = 1 }
        { ID = 110; Rev = 1 }
        { ID = 100; Rev = 5 }
        { ID = 100; Rev = 2 }
        { ID = 92; Rev = 1 }
     |]

    let sortedWidgetArray = Array.sortWith compareWidgets arrayToSort
    printfn "%A" sortedWidgetArray
    // </snippet39>

let Snippet40() =
    // <snippet40>
    let array1 = [|1; 4; 8; -2; 5|]
    Array.sortInPlace array1
    printfn "%A" array1
    // </snippet40>

let Snippet41() =
    // <snippet41>
    let array1 = [|1; 4; 8; -2; 5|]
    Array.sortInPlaceBy (fun elem -> abs elem) array1
    printfn "%A" array1
    // </snippet41>

module Snippet42 =
    // <snippet42>
    type Widget = { ID: int; Rev: int }

    let compareWidgets widget1 widget2 =
       if widget1.ID < widget2.ID then -1 else
       if widget1.ID > widget2.ID then 1 else
       if widget1.Rev < widget2.Rev then -1 else
       if widget1.Rev > widget2.Rev then 1 else
       0

    let array1 =
     [|
        { ID = 92; Rev = 1 }
        { ID = 110; Rev = 1 }
        { ID = 100; Rev = 5 }
        { ID = 100; Rev = 2 }
        { ID = 92; Rev = 1 }
     |]

    Array.sortInPlaceWith compareWidgets array1
    printfn "%A" array1
    // </snippet42>

module Snippet43 =
//<snippet43>
    let average1 = Array.average [| 1.0 .. 10.0 |]
    printfn "Average: %f" average1
    // To get the average of an array of integers, 
    // use Array.averageBy to convert to float.
    let average2 = Array.averageBy (fun elem -> float elem) [|1 .. 10 |]
    printfn "Average: %f" average2
//</snippet43>
(* OUTPUT:
Average: 5.500000
Average: 5.500000
*)

module Snippet44 =
// <snippet44>
    // Specify the type by using a type argument.
    let array1 = Array.empty<int>
    // Specify the type by using a type annotation.
    let array2 : int array = Array.empty

    // Even though array3 has a generic type,
    // you can still use methods such as Length on it.
    let array3 = Array.empty
    printfn "Length of empty array: %d" array3.Length
// </snippet44>
(* output:
Length of empty array: 0
*)

module Snippet45 =
// <snippet45>
    // Use Array.fold2 to perform computations over two arrays (of equal size)
    // at the same time.
    // Example: Add the greater element at each array position.
    let sumGreatest array1 array2 =
        Array.fold2 (fun acc elem1 elem2 ->
            acc + max elem1 elem2) 0 array1 array2

    let sum = sumGreatest [| 1; 2; 3 |] [| 3; 2; 1 |]
    printfn "The sum of the greater of each pair of elements in the two arrays is %d." sum
// </snippet45>
(* output
The sum of the greater of each pair of elements in the two arrays is 8.
*)

module Snippet46 =
// <snippet46>
    // This computes 3 - 2 - 1, which evalates to -6.
    let subtractArray array1 = Array.fold (fun acc elem -> acc - elem) 0 array1
    printfn "%d" (subtractArray [| 1; 2; 3 |])

    // This computes 3 - (2 - (0 - 1)), which evaluates to 2.
    let subtractArrayBack array1 = Array.foldBack (fun elem acc -> elem - acc) array1 0
    printfn "%d" (subtractArrayBack [| 1; 2; 3 |])
// </snippet46>
(* output:
-6
2
*)

module Snippet47 =
// <snippet47>
    type Transaction =
        | Deposit
        | Withdrawal

    let transactionTypes = [| Deposit; Deposit; Withdrawal |]
    let transactionAmounts = [| 100.00; 1000.00; 95.00 |]
    let initialBalance = 200.00

    let endingBalance = Array.foldBack2 (fun elem1 elem2 acc ->
                            match elem1 with
                            | Deposit -> acc + elem2
                            | Withdrawal -> acc - elem2)
                            transactionTypes
                            transactionAmounts
                            initialBalance
    printfn "Ending balance: $%.2f" endingBalance
// </snippet47>
(*
Ending balance: $1205.00
*)

module Snippet48 =
// <snippet48>
    let printArray array1 = 
        if (Array.isEmpty array1) then
            printfn "There are no elements in this array."
        else
            printfn "This array contains the following elements:"
            Array.iter (fun elem -> printf "%A " elem) array1
            printfn ""
    printArray [| "test1"; "test2" |]
    printArray [| |]
// </snippet48>
(*
This array contains the following elements:
"test1" "test2" 
There are no elements in this array.
*)

module Snippet49 =
// <snippet49>
    let array1 = [| 1; 2; 3 |]
    let array2 = [| 4; 5; 6 |]
    Array.iter (fun x -> printfn "Array.iter: element is %d" x) array1
    Array.iteri(fun i x -> printfn "Array.iteri: element %d is %d" i x) array1
    Array.iter2 (fun x y -> printfn "Array.iter2: elements are %d %d" x y) array1 array2
    Array.iteri2 (fun i x y ->
                   printfn "Array.iteri2: element %d of array1 is %d element %d of array2 is %d"
                     i x i y)
                array1 array2
// </snippet49>
(*
Array.iter: element is 1
Array.iter: element is 2
Array.iter: element is 3
Array.iteri: element 0 is 1
Array.iteri: element 1 is 2
Array.iteri: element 2 is 3
Array.iter2: elements are 1 4
Array.iter2: elements are 2 5
Array.iter2: elements are 3 6
Array.iteri2: element 0 of array1 is 1 element 0 of array2 is 4
Array.iteri2: element 1 of array1 is 2 element 1 of array2 is 5
Array.iteri2: element 2 of array1 is 3 element 2 of array2 is 6
*)

module Snippet50 =
// <snippet50>
    Array.length [| 1 .. 100 |] |> printfn "Length: %d"
    Array.length [| |] |> printfn "Length: %d"
    Array.length [| 1 .. 2 .. 100 |] |> printfn "Length: %d"
// </snippet50>
(*
Length: 100
Length: 0
Length: 50
 *)

module Snippet510 =
// <snippet510>
    let data = [| 1; 2; 3; 4 |]
    let r1 = data |> Array.map (fun x -> x + 1)
    printfn "Adding '1' using map = %A" r1
    let r2 = data |> Array.map string
    printfn "Converting to strings by using map = %A" r2
    let r3 = data |> Array.map (fun x -> (x, x))
    printfn "Converting to tupels by using map = %A" r3
// </snippet510>
(* output:
Adding '1' using map = [|2; 3; 4; 5|]
Converting to strings by using map = [|"1"; "2"; "3"; "4"|]
Converting to tuples by using map = [|(1, 1); (2, 2); (3, 3); (4, 4)|]
*)

module Snippet52 =
// <snippet52>
    let array1 = [| 1; 2; 3 |]
    let array2 = [| 4; 5; 6 |]
    let arrayOfSums = Array.map2 (fun x y -> x + y) array1 array2
    printfn "%A" arrayOfSums
// </snippet52>
(*
[|5; 7; 9|]
*)

module Snippet53 =
// <snippet53>
    let array1 = [| 1; 2; 3 |]
    let newArray = Array.mapi (fun i x -> (i, x)) array1
    printfn "%A" newArray
// </snippet53>
(* output:
[|(0, 1); (1, 2); (2, 3)|]
*)

module Snippet54 =
// <snippet54>
    let array1 = [| 1; 2; 3 |]
    let array2 = [| 4; 5; 6 |]
    let arrayAddTimesIndex = Array.mapi2 (fun i x y -> (x + y) * i) array1 array2
    printfn "%A" arrayAddTimesIndex
// </snippet54>
(*
[|0; 7; 18|]
*)

module Snippet55 =
// <snippet55>
    [| for x in -100 .. 100 -> 4 - x * x |]
    |> Array.max
    |> printfn "%A"
// </snippet55>
(* output:
4
*)

module Snippet56 =
// <snippet56>
    [| -10.0 .. 10.0 |]
    |> Array.maxBy (fun x -> 1.0 - x * x)
    |> printfn "%A"
// </snippet56>
(*
0.0
*)

module Snippet57 =
// <snippet57>
    [| for x in -100 .. 100 -> x * x - 4 |]
    |> Array.min
    |> printfn "%A" 
// </snippet57>
(* output:
-4
*)

module Snippet58 =
// <snippet58>
    [| -10.0 .. 10.0 |]
    |> Array.minBy (fun x -> x * x - 1.0)
    |> printfn "%A"
// </snippet58>
(*
0.0
*)

module Snippet59 =
// <snippet59>
    let array1 = Array.ofList [ 1 .. 10]
// </snippet59>
(*
val array1 : int [] = [|1; 2; 3; 4; 5; 6; 7; 8; 9; 10|]
*)

module Snippet60 =
// <snippet60>
    let array1 = Array.ofSeq ( seq { 1 .. 10 } )
// </snippet60>
(*
val array1 : int [] = [|1; 2; 3; 4; 5; 6; 7; 8; 9; 10|]
*)

module Snippet61 =
// <snippet61>
    let array1 = Array.ofSeq ( seq { 1 .. 10 } )
// </snippet61>

module Snippet62 =
// <snippet62>
    let values = [| ("a", 1); ("b", 2); ("c", 3) |]

    let resultPick = Array.pick (fun elem ->
                        match elem with
                        | (value, 2) -> Some value
                        | _ -> None) values
    printfn "%A" resultPick
// </snippet62>

module Snippet63 =
// <snippet63>
    // Computes ((1 - 2) - 3) - 4 = -8
    Array.reduce (fun elem acc -> elem - acc) [| 1; 2; 3; 4 |]
    |> printfn "%A"
    // Computes 1 - (2 - (3 - 4)) = -2
    Array.reduceBack (fun elem acc -> elem - acc) [| 1; 2; 3; 4 |]
    |> printfn "%A"
// </snippet63>
(*
-8
-2
*)


// Array.sortInPlaceWith
module Snippet64 =
// <snippet64>
    open System

    let array1 = [| "<>"; "&"; "&&"; "&&&"; "<"; ">"; "|"; "||"; "|||" |]
    printfn "Before sorting: "
    array1 |> printfn "%A"
    let sortFunction (string1:string) (string2:string) =
        if (string1.Length > string2.Length) then
           1
        else if (string1.Length < string2.Length) then
           -1
        else
            String.Compare(string1, string2)
    Array.sortInPlaceWith sortFunction array1
    printfn "After sorting: "
    array1 |> printfn "%A"
// </snippet64>
(*
Before sorting: 
[|"<>"; "&"; "&&"; "&&&"; "<"; ">"; "|"; "||"; "|||"|]
After sorting: 
[|"&"; "|"; "<"; ">"; "&&"; "||"; "<>"; "&&&"; "|||"|]
*)

module Snippet65 =
// <snippet65>
    open System

    let array1 = [| "<>"; "&"; "&&"; "&&&"; "<"; ">"; "|"; "||"; "|||" |]
    printfn "Before sorting: "
    array1 |> printfn "%A"
    let sortFunction (string1:string) (string2:string) =
        if (string1.Length > string2.Length) then
           1
        else if (string1.Length < string2.Length) then
           -1
        else
            String.Compare(string1, string2)

    Array.sortWith sortFunction array1
    |> printfn "After sorting: \n%A"
// </snippet65>
(*
Before sorting: 
[|"<>"; "&"; "&&"; "&&&"; "<"; ">"; "|"; "||"; "|||"|]
After sorting: 
[|"&"; "|"; "<"; ">"; "&&"; "||"; "<>"; "&&&"; "|||"|]
*)

module Snippet66 =
// <snippet66>
    [| 1 .. 10 |]
    |> Array.sum
    |> printfn "Sum: %d"
// </snippet66>
(* output:
55
*)

module Snippet67 =
// <snippet67>
    [| 1 .. 10 |]
    |> Array.sumBy (fun x -> x * x)
    |> printfn "Sum: %d"
// </snippet67>
(* output:
385
*)

module Snippet68 =
// <snippet68>
    [| 1 .. 10 |]
    |> Array.toList
    |> List.rev
    |> List.iter (fun elem -> printf "%d " elem)
    printfn ""
// </snippet68>
(* output;:
10 9 8 7 6 5 4 3 2 1
*)

module Snippet69 =
// <snippet69>
    [| 1 .. 10 |]
    |> Array.toSeq
    |> Seq.truncate 5
    |> Seq.iter (fun elem -> printf "%d " elem)
    printfn ""
// </snippet69>
(* output:
1 2 3 4 5
*)

module Snippet70 =
// <snippet70>
    let array1, array2 = Array.unzip [| (1, 2); (3, 4) |]
    printfn "%A" array1
    printfn "%A" array2
// </snippet70>
(* output:
[|1; 3|]
[|2; 4|]
*)

module Snippet71 =
// <snippet71>
    let array1, array2, array3 = Array.unzip3 [| (1, 2,3 ); (3, 4, 5) |]
    printfn "%A" array1
    printfn "%A" array2
    printfn "%A" array3
// </snippet71>
(* output:
[|1; 3|]
[|2; 4|]
[|3; 5|]
*)

module Snippet72 =
// <snippet72>
    let array1 = [| 1; 2; 3 |]
    let array2 = [| -1; -2; -3 |]
    let arrayZip = Array.zip array1 array2
    printfn "%A" arrayZip
// </snippet72>
(* output:
[|(1, -1); (2, -2); (3, -3)|]
*)

module Snippet73 =
// <snippet73>
    let array1 = [| 1; 2; 3 |]
    let array2 = [| -1; -2; -3 |]
    let array3 = [| "horse"; "dog"; "elephant" |]
    let arrayZip3 = Array.zip3 array1 array2 array3
    printfn "%A" arrayZip3
// </snippet73>
(*
[|(1, -1, "horse"); (2, -2, "dog"); (3, -3, "elephant")|]
*)

