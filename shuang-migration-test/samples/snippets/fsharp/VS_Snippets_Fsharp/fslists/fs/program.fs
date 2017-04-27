// Learn more about F# at http://fsharp.net


let list123 = [ 1; 2; 3 ]

(*let list123 = [
    1
    2
    3 ]
    *)

//let myControlList : Control list = [ new Button(); new CheckBox() ]

let list1 = [ 1 .. 10 ]

let listOfSquares = [ for i in 1 .. 10 -> i*i ]

// An empty list.
let listEmpty = []

let squaresList = [ for i in 1 .. 10 -> i * i ]

let ListOperators() =
    let list2 = 100 :: list1

    let list3 = list1 @ list2
    ()

let listProperties() =

    let list1 = [ 1; 2; 3 ]
        
    // Properties.
    printfn "list1.IsEmpty is %b" (list1.IsEmpty)
    printfn "list1.Length is %d" (list1.Length)
    printfn "list1.Head is %d" (list1.Head)
    printfn "list1.Tail.Head is %d" (list1.Tail.Head)
    printfn "list1.Tail.Tail.Head is %d" (list1.Tail.Tail.Head)
    printfn "list1.Item(1) is %d" (list1.Item(1))

let rec sum list =
    match list with
    | head :: tail -> head + sum tail
    | [] -> 0

let sum2 list =
    let rec loop list acc =
        match list with
        | head :: tail -> loop tail (acc + head)
        | [] -> acc
    loop list 0

let IsPrimeMultipleTest n x =
    x = n || x % n <> 0

let rec RemoveAllMultiples listn listx =
    match listn with
    | head :: tail -> RemoveAllMultiples tail (List.filter (IsPrimeMultipleTest head) listx)
    | [] -> listx


let GetPrimesUpTo n =
    let max = int (sqrt (float n))
    RemoveAllMultiples [ 2 .. max ] [ 2 .. n ]

printfn "Primes Up To %d:\n %A" 100 (GetPrimesUpTo 100)

//Primes Up To 100:
//[2; 3; 5; 7; 11; 13; 17; 19; 23; 29; 31; 37; 41; 43; 47; 53; 59; 61; 67; 71; 73; 79; 83; 89; 97]

// <snippet1>
// Use List.exists to determine whether there is an element of a list satisfies a given Boolean expression.
// containsNumber returns true if any of the elements of the supplied list match 
// the supplied number.
let containsNumber number list = List.exists (fun elem -> elem = number) list
let list0to3 = [0 .. 3]
printfn "For list %A, contains zero is %b" list0to3 (containsNumber 0 list0to3)
// </snippet1>
//The output is as follows:
//For list [0; 1; 2; 3], contains zero is true
//The following example demonstrates the use of List.exists2.

// <snippet2>
// Use List.exists2 to compare elements in two lists.
// isEqualElement returns true if any elements at the same position in two supplied
// lists match.
let isEqualElement list1 list2 = List.exists2 (fun elem1 elem2 -> elem1 = elem2) list1 list2
let list1to5 = [ 1 .. 5 ]
let list5to1 = [ 5 .. -1 .. 1 ]
if (isEqualElement list1to5 list5to1) then
    printfn "Lists %A and %A have at least one equal element at the same position." list1to5 list5to1
else
    printfn "Lists %A and %A do not have an equal element at the same position." list1to5 list5to1
// </snippet2>
//The output is as follows:
//Lists [1; 2; 3; 4; 5] and [5; 4; 3; 2; 1] have at least one equal element at the same position.
// <snippet3>
let isAllZeroes list = List.forall (fun elem -> elem = 0.0) list
printfn "%b" (isAllZeroes [0.0; 0.0])
printfn "%b" (isAllZeroes [0.0; 1.0])
// </snippet3>
//The output is as follows:
//true
//false
// <snippet4>
let listEqual list1 list2 = List.forall2 (fun elem1 elem2 -> elem1 = elem2) list1 list2
printfn "%b" (listEqual [0; 1; 2] [0; 1; 2])
printfn "%b" (listEqual [0; 0; 0] [0; 1; 0])
// </snippet4>
//The output is as follows:
//true
//false
// <snippet5>
let sortedList1 = List.sort [1; 4; 8; -2; 5]
printfn "%A" sortedList1
// </snippet5>
//The output is as follows:
//[-2; 1; 4; 5; 8]
//The following example demonstrates the use of List.sortBy.
// <snippet6>
let sortedList2 = List.sortBy (fun elem -> abs elem) [1; 4; 8; -2; 5]
printfn "%A" sortedList2
// </snippet6>
//The output is as follows:
//[1; -2; 4; 5; 8]
///The next example demonstrates the use of List.sortWith. In this example, the custom comparison function compareWidgets is used to first compare one field of a custom type, and then another when the values of the first field are equal.
// <snippet7>
type Widget = { ID: int; Rev: int }

let compareWidgets widget1 widget2 =
   if widget1.ID < widget2.ID then -1 else
   if widget1.ID > widget2.ID then 1 else
   if widget1.Rev < widget2.Rev then -1 else
   if widget1.Rev > widget2.Rev then 1 else
   0

let listToCompare = [
    { ID = 92; Rev = 1 }
    { ID = 110; Rev = 1 }
    { ID = 100; Rev = 5 }
    { ID = 100; Rev = 2 }
    { ID = 92; Rev = 1 }
    ]

let sortedWidgetList = List.sortWith compareWidgets listToCompare
printfn "%A" sortedWidgetList
// </snippet7>
//The output is as follows:
//  [{ID = 92;
//    Rev = 1;}; {ID = 92;
//                Rev = 1;}; {ID = 100;
//                            Rev = 2;}; {ID = 100;
//                                        Rev = 5;}; {ID = 110;
//                                                    Rev = 1;}]
//Search Operations on Lists
//Numerous search operations are supported for lists. The simplest, List.find, enables you to find the first element that matches a given condition.
//The following code example demonstrates the use of List.find to find the first number that is divisible by 5 in a list.
//[F#]
// <snippet8>
let isDivisibleBy number elem = elem % number = 0
let result = List.find (isDivisibleBy 5) [ 1 .. 100 ]
printfn "%d " result
// </snippet8>
//The result is 5.
//If the elements must be transformed first, call List.pick, which takes a function that returns an option, and looks for the first option value that is Some(x). Instead of returning the element, List.pick returns the result x. If no matching element is found, List.pick throws T:System.Collections.Generic.KeyNotFoundException. The following code shows the use of List.pick.
//[F#]
// <snippet9>
let valuesList = [ ("a", 1); ("b", 2); ("c", 3) ]

let resultPick = List.pick (fun elem ->
                    match elem with
                    | (value, 2) -> Some value
                    | _ -> None) valuesList
printfn "%A" resultPick
// </snippet9>
//The output is as follows:
//("b", 2)
//Another group of search operations, List.tryFind and related functions, return an option value. The List.tryFind function returns the first element of a list that satisfies a condition if such an element exists, but the option value None if not. The variation List.tryFindIndex returns the index of the element, if one is found, rather than the element itself. These functions are illustrated in the following code. 
//[F#]
// <snippet10>
let list1d = [1; 3; 7; 9; 11; 13; 15; 19; 22; 29; 36]
let isEven x = x % 2 = 0
match List.tryFind isEven list1d with
| Some value -> printfn "The first even value is %d." value
| None -> printfn "There is no even value in the list."

match List.tryFindIndex isEven list1d with
| Some value -> printfn "The first even value is at position %d." value
| None -> printfn "There is no even value in the list."
// </snippet10>
//The output is as follows:
//The first even value is 22.
//The first even value is at position 8.
//Arithmetic Operations on Lists
//Common arithmetic operations such as sum and average are built into the List module. To work with List.sum, the list element type must support the + operator and have a zero value. All built-in arithmetic types satisfy these conditions. To work with List.average, the element type must support division without a remainder, which excludes integral types but allows for floating point types. The List.sumBy and List.averageBy functions take a function as a parameter, and this function's results are used to calculate the values for the sum or average.
//The following code demonstrates the use of List.sum, List.sumBy, and List.average.
//[F#]
let ListArithmetic() =
    // <snippet11>
    // Compute the sum of the first 10 integers by using List.sum.
    let sum1 = List.sum [1 .. 10]

    // Compute the sum of the squares of the elements of a list by using List.sumBy.
    let sum2 = List.sumBy (fun elem -> elem*elem) [1 .. 10]

    // <snippet111>
    // Compute the average of the elements of a list by using List.average.
    let avg1 = List.average [0.0; 1.0; 1.0; 2.0]

    printfn "%f" avg1
    // </snippet111>
    // </snippet11>
//The output is 1.000000. 
//The following code shows the use of List.averageBy.
//[F#]
// <snippet12>
let avg2 = List.averageBy (fun elem -> float elem) [1 .. 10]
printfn "%f" avg2
// </snippet12>
//The output is 5.5.
//Lists and Tuples
//Lists that contain tuples can be manipulated by zip and unzip functions. These functions combine two lists of single values into one list of tuples or separate one list of tuples into two lists of single values. The simplest List.zip function takes two lists of single elements and produces a single list of tuple pairs. Another version, List.zip3, takes three lists of single elements and produces a single list of tuples that have three elements. The following code example demonstrates the use of List.zip.
//[F#]
let listZipping() =
    // <snippet13>
    let list1 = [ 1; 2; 3 ]
    let list2 = [ -1; -2; -3 ]
    let listZip = List.zip list1 list2
    printfn "%A" listZip
    // </snippet13>
//The output is as follows:
//[(1, -1); (2, -2); (3; -3)]
//The following code example demonstrates the use of List.zip3.
//[F#]
    // <snippet14>
    let list3 = [ 0; 0; 0]
    let listZip3 = List.zip3 list1 list2 list3
    printfn "%A" listZip3
    // </snippet14>
//The output is as follows:
//[(1, -1, 0); (2, -2, 0); (3, -3, 0)]
//The corresponding unzip versions, List.unzip and List.unzip3, take lists of tuples and return lists in a tuple, where the first list contains all the elements that were first in each tuple, and the second list contains the second element of each tuple, and so on.
//The following code example demonstrates the use of List.unzip.
//[F#]
// <snippet15>
let lists = List.unzip [(1,2); (3,4)]
printfn "%A" lists
printfn "%A %A" (fst lists) (snd lists)
// </snippet15>
//The output is as follows:
//([1; 3], [2; 4])
//[1; 3] [2; 4]
//The following code example demonstrates the use of List.unzip3.
//[F#]
// <snippet16>
let listsUnzip3 = List.unzip3 [(1,2,3); (4,5,6)]
printfn "%A" listsUnzip3
// </snippet16>
//The output is as follows:
//([1; 4], [2; 5], [3; 6])
//Operating on List Elements
//F# supports a variety of operations on list elements. The simplest is List.iter, which enables you to call a function on every element of a list. Variations include List.iter2, which enables you to perform an operation on elements of two lists, List.iteri, which is like List.iter except that the index of each element is passed as an argument to the function that is called for each element, and List.iteri2, which is a combination of the functionality of List.iter2 and List.iteri.
//[F#]
let ListIteration() =
    // <snippet17>
    let list1 = [1; 2; 3]
    let list2 = [4; 5; 6]
    List.iter (fun x -> printfn "List.iter: element is %d" x) list1
    List.iteri(fun i x -> printfn "List.iteri: element %d is %d" i x) list1
    List.iter2 (fun x y -> printfn "List.iter2: elements are %d %d" x y) list1 list2
    List.iteri2 (fun i x y ->
                   printfn "List.iteri2: element %d of list1 is %d element %d of list2 is %d"
                     i x i y)
                list1 list2
    // </snippet17>
//The output is as follows:
//List.iter: element is 1
//List.iter: element is 2
//List.iter: element is 3
//List.iteri: element 0 is 1
//List.iteri: element 1 is 2
//List.iteri: element 2 is 3
//List.iter2: elements are 1 4
//List.iter2: elements are 2 5
//List.iter2: elements are 3 6
//List.iteri2: element 0 of list1 is 1; element 0 of list2 is 4
//List.iteri2: element 1 of list1 is 2; element 1 of list2 is 5
//List.iteri2: element 2 of list1 is 3; element 2 of list2 is 6
//Another frequently used function that transforms list elements is List.map, which enables you to apply a function to each element of a list and put all the results into a new list. List.map2 and List.map3 are variations that take multiple lists. You can also use List.mapi and List.mapi2, if, in addition to the element, the function needs to be passed the index of each element. The only difference between List.mapi2 and List.mapi is that List.mapi2 works with two lists. The following example illustrates List.map.
//[F#]
let ListMap() =
    // <snippet18>
    let list1 = [1; 2; 3]
    let newList = List.map (fun x -> x + 1) list1
    printfn "%A" newList
    // </snippet18>
//The output is as follows:
//[2; 3; 4]
//The following example shows the use of List.map2.
//[F#]
let ListMap2() =
    // <snippet19>
    let list1 = [1; 2; 3]
    let list2 = [4; 5; 6]
    let sumList = List.map2 (fun x y -> x + y) list1 list2
    printfn "%A" sumList
    // </snippet19>
//The output is as follows:
//[5; 7; 9]
//The following example shows the use of List.map3.
//[F#]
    // <snippet20>
    let newList2 = List.map3 (fun x y z -> x + y + z) list1 list2 [2; 3; 4]
    printfn "%A" newList2
    // </snippet20>
    //The output is as follows:
    //[7; 10; 13]
    //The following example shows the use of List.mapi.
    //[F#]
    // <snippet21>
    let newListAddIndex = List.mapi (fun i x -> x + i) list1
    printfn "%A" newListAddIndex
    // </snippet21>
    //The output is as follows:
    //[1; 3; 5]
    //The following example shows the use of List.mapi2.
    //[F#]
    // <snippet22>
    let listAddTimesIndex = List.mapi2 (fun i x y -> (x + y) * i) list1 list2
    printfn "%A" listAddTimesIndex
// </snippet22>
//The output is as follows:
//[0; 7; 18]
//List.collect is like List.map, except that each element produces a list and all these lists are concatenated into a final list. In the following code, each element of the list generates three numbers. These are all collected into one list.
//[F#]
// <snippet23>
let collectList = List.collect (fun x -> [for i in 1..3 -> x * i]) list1
printfn "%A" collectList
// </snippet23>
//The output is as follows:
//[1; 2; 3; 2; 4; 6; 3; 6; 9]
//You can also use List.filter, which takes a Boolean condition and produces a new list that consists only of elements that satisfy the given condition.
//[F#]
// <snippet24>
let evenOnlyList = List.filter (fun x -> x % 2 = 0) [1; 2; 3; 4; 5; 6]
// </snippet24>
//The resulting list is [2; 4; 6].
//A combination of map and filter, List.choose enables you to transform and select elements at the same time. List.choose applies a function that returns an option to each element of a list, and returns a new list of the results for elements when the function returns the option value Some.
//The following code demonstrates the use of List.choose to select capitalized words out of a list of words.
//[F#]
// <snippet25>
let listWords = [ "and"; "Rome"; "Bob"; "apple"; "zebra" ]
let isCapitalized (string1:string) = System.Char.IsUpper string1.[0]
let results = List.choose (fun elem ->
    match elem with
    | elem when isCapitalized elem -> Some(elem + "'s")
    | _ -> None) listWords
printfn "%A" results
// </snippet25>
//The output is as follows:
//["Rome"; "Bob"]
//Operating on Multiple Lists
//Lists can be joined together. To join two lists into one, use List.append. To join more than two lists, use List.concat.
//[F#]
// <snippet26>
let list1to10 = List.append [1; 2; 3] [4; 5; 6; 7; 8; 9; 10]
let listResult = List.concat [ [1; 2; 3]; [4; 5; 6]; [7; 8; 9] ]
List.iter (fun elem -> printf "%d " elem) list1to10
printfn ""
List.iter (fun elem -> printf "%d " elem) listResult
// </snippet26>
//Fold and Scan Operations
//Some list operations involve interdependencies between all of the list elements. The fold and scan operations are like List.iter and List.map in that you invoke a function on each element, but these operations provide an additional parameter called the accumulator that carries information through the computation.
//Use List.fold to perform a calculation on a list. 
//The following code example demonstrates the use of List.fold to perform various operations.
//The list is traversed; the accumulator acc is a value that is passed along as the calculation proceeds. The first argument takes the accumulator and the list element, and returns the interim result of the calculation for that list element. The second argument is the initial value of the accumulator.
//[F#]
// The following example adds the elements of a list.
let ListFolding() =
    // <snippet27>
    let sumList list = List.fold (fun acc elem -> acc + elem) 0 list
    printfn "Sum of the elements of list %A is %d." [ 1 .. 3 ] (sumList [ 1 .. 3 ])

    // The following example computes the average of a list.
    let averageList list = (List.fold (fun acc elem -> acc + float elem) 0.0 list / float list.Length)

    // The following example computes the standard deviation of a list.
    // The standard deviation is computed by taking the square root of the
    // sum of the variances, which are the differences between each value
    // and the average.
    let stdDevList list =
        let avg = averageList list
        sqrt (List.fold (fun acc elem -> acc + (float elem - avg) ** 2.0 ) 0.0 list / float list.Length)

    let testList listTest =
        printfn "List %A average: %f stddev: %f" listTest (averageList listTest) (stdDevList listTest)

    testList [1; 1; 1]
    testList [1; 2; 1]
    testList [1; 2; 3]

    // List.fold is the same as to List.iter when the accumulator is not used.
    let printList list = List.fold (fun acc elem -> printfn "%A" elem) () list
    printList [0.0; 1.0; 2.5; 5.1 ]

    // The following example uses List.fold to reverse a list.
    // The accumulator starts out as the empty list, and the function uses the cons operator
    // to add each successive element to the head of the accumulator list, resulting in a
    // reversed form of the list.
    let reverseList list = List.fold (fun acc elem -> elem::acc) [] list
    printfn "%A" (reverseList [1 .. 10])
    // </snippet27>
//The versions of these functions that have a digit in the function name operate on more than one list. For example, List.fold2 performs computations on two lists. 
//The following example demonstrates the use of List.fold2.
//[F#]
let listFold2() =
    // <snippet28>
    // Use List.fold2 to perform computations over two lists (of equal size) at the same time.
    // Example: Sum the greater element at each list position.
    let sumGreatest list1 list2 = List.fold2 (fun acc elem1 elem2 ->
                                                  acc + max elem1 elem2) 0 list1 list2

    let sum = sumGreatest [1; 2; 3] [3; 2; 1]
    printfn "The sum of the greater of each pair of elements in the two lists is %d." sum
    // </snippet28>
//List.fold and List.scan differ in that List.fold returns the final value of the extra parameter, but List.scan returns the list of the intermediate values (along with the final value) of the extra parameter.
//Each of these functions includes a reverse variation, for example, List.foldBack, which differs in the order in which the list is traversed and the order of the arguments. Also, List.fold and List.foldBack have variations, List.fold2 and List.foldBack2, that take two lists of equal length. The function that executes on each element can use corresponding elements of both lists to perform some action. The element types of the two lists can be different, as in the following example, in which one list contains transaction amounts for a bank account, and the other list contains the type of transaction: deposit or withdrawal.
//[F#]
// <snippet29>
// Discriminated union type that encodes the transaction type.
type Transaction =
    | Deposit
    | Withdrawal

let transactionTypes = [Deposit; Deposit; Withdrawal]
let transactionAmounts = [100.00; 1000.00; 95.00 ]
let initialBalance = 200.00

// Use fold2 to perform a calculation on the list to update the account balance.
let endingBalance = List.fold2 (fun acc elem1 elem2 ->
                                match elem1 with
                                | Deposit -> acc + elem2
                                | Withdrawal -> acc - elem2)
                                initialBalance
                                transactionTypes
                                transactionAmounts
printfn "%f" endingBalance
// </snippet29>
//For a calculation like summation, List.fold and List.foldBack have the same effect because the result does not depend on the order of traversal. In the following example, List.foldBack is used to add the elements in a list.
//[F#]
// <snippet30>
let sumListBack list = List.foldBack (fun acc elem -> acc + elem) list 0
printfn "%d" (sumListBack [1; 2; 3])

// For a calculation in which the order of traversal is important, fold and foldBack have different
// results. For example, replacing fold with foldBack in the listReverse function
// produces a function that copies the list, rather than reversing it.
let copyList list = List.foldBack (fun elem acc -> elem::acc) list []
printfn "%A" (copyList [1 .. 10])
// </snippet30>
//The following example returns to the bank account example. This time a new transaction type is added: an interest calculation. The ending balance now depends on the order of transactions.
//[F#]
// <snippet34>
// <snippet31>
type Transaction2 =
    | Deposit
    | Withdrawal
    | Interest

let transactionTypes2 = [Deposit; Deposit; Withdrawal; Interest]
let transactionAmounts2 = [100.00; 1000.00; 95.00; 0.05 / 12.0 ]
let initialBalance2 = 200.00

// Because fold2 processes the lists by starting at the head element,
// the interest is calculated last, on the balance of 1205.00.
let endingBalance2 = List.fold2 (fun acc elem1 elem2 ->
                                match elem1 with
                                | Deposit -> acc + elem2
                                | Withdrawal -> acc - elem2
                                | Interest -> acc * (1.0 + elem2))
                                initialBalance2
                                transactionTypes2
                                transactionAmounts2
printfn "%f" endingBalance2
// </snippet31>
// <snippet32>
// Because foldBack2 processes the lists by starting at end of the list,
// the interest is calculated first, on the balance of only 200.00.
let endingBalance3 = List.foldBack2 (fun elem1 elem2 acc ->
                                match elem1 with
                                | Deposit -> acc + elem2
                                | Withdrawal -> acc - elem2
                                | Interest -> acc * (1.0 + elem2))
                                transactionTypes2
                                transactionAmounts2
                                initialBalance2
printfn "%f" endingBalance3
// </snippet32>
// </snippet34>
//The function List.reduce is somewhat like List.fold and List.scan, except that instead of passing around a separate accumulator, List.reduce takes a function that takes two arguments of the element type instead of just one, and one of those arguments acts as the accumulator, meaning that it stores the intermediate result of the computation. List.reduce starts by operating on the first two list elements, and then uses the result of the operation along with the next element. Because there is not a separate accumulator that has its own type, List.reduce can be used in place of List.fold only when the accumulator and the element type have the same type. The following code demonstrates the use of List.reduce. List.reduce throws an exception if the list provided has no elements.
//In the following code, the first call to the lambda expression is given the arguments 2 and 4, and returns 6, and the next call is given the arguments 6 and 10, so the result is 16.
//[F#]
// <snippet33>
let sumAList list =
    try
        List.reduce (fun acc elem -> acc + elem) list
    with
       | :? System.ArgumentException as exc -> 0

let resultSum = sumAList [2; 4; 10]
printfn "%d " resultSum
// </snippet33>
//Converting Between Lists and Other Collection Types
//The List module provides functions for converting to and from both sequences and arrays. To convert to or from a sequence, use List.toSeq or List.ofSeq. To convert to or from an array, use List.toArray or List.ofArray.
//Additional Operations
//For information about additional operations on lists, see the library reference topic Collections.List Module (F#).
//See Also
//F# Language Reference
//F# Types
//Sequences
//Arrays
//Options (F#)

let Snippet34() =
    // <snippet42>
    let list1 = [10; 20; 30]
    let collectList = List.collect (fun x -> [for i in 1..3 -> x * i]) list1
    printfn "%A" collectList
    // </snippet42>

let Snippet35() =
     // <snippet35>
    let list1 = [1; 2; 3]
    let list2 = [4; 5; 6]
    let newList = List.map3 (fun x y z -> x + y + z) list1 list2 [2; 3; 4]
    printfn "%A" newList
    // </snippet35>


let Snippet36() =
    // <snippet36>
    let list1 = [1; 2; 3]
    let newList = List.mapi (fun i x -> (i, x)) list1
    printfn "%A" newList
    // </snippet36>

let Snippet37() =
    // <snippet37>
    let list1 = [1; 2; 3]
    let list2 = [4; 5; 6]
    let listAddTimesIndex = List.mapi2 (fun i x y -> (x + y) * i) list1 list2
    printfn "%A" listAddTimesIndex
    // </snippet37>

let Snippet38() =
    // <snippet38>
    let listA, listB = List.unzip [(1,2); (3,4)]
    printfn "%A" listA
    printfn "%A" listB
    // </snippet38>

let Snippet39() =
    // <snippet39>
    let listA, listB, listC = List.unzip3 [(1,2,3); (4,5,6)]
    printfn "%A %A %A" listA listB listC
    // </snippet39>

let Snippet40() =
    // <snippet40>
    let list1 = [ 1; 2; 3 ]
    let list2 = [ -1; -2; -3 ]
    let list3 = [ 0; 0; 0]
    let listZip3 = List.zip3 list1 list2 list3
    printfn "%A" listZip3
    // </snippet40>

let Snippet41() =
    // <snippet41>
    let sumListBack list = List.foldBack (fun acc elem -> acc + elem) list 0
    printfn "%d" (sumListBack [1; 2; 3])

    // For a calculation in which the order of traversal is important, fold and foldBack have different
    // results. For example, replacing foldBack with fold in the copyList function
    // produces a function that reverses the list, rather than copying it.
    let copyList list = List.foldBack (fun elem acc -> elem::acc) list []
    printfn "%A" (copyList [1 .. 10])
    // </snippet41>

    // 42 already used

    // The next series of examples show the pipelining functionality

let Snippet43() =
    // <snippet43>
    List.init 10 (fun i -> (i, i * i))
    |> List.filter (fun (n, nsqr) -> n % 2 = 0)
    |> List.rev
    |> List.iter (fun (n, nsqr) -> printfn "(%d, %d) " n nsqr)
    // </snippet43>

module Snippet44 =
// <snippet44>
    // A generic empty list.
    let emptyList1 = List.empty
    // An empty list of a specific type.
    let emptyList2 = List.empty<int>
// </snippet44>

module Snippet45 =
// <snippet45>
    let list1 = [ 2 .. 100 ]
    let delta = 1.0e-10
    let isPerfectSquare (x:int) =
        let y = sqrt (float x)
        abs(y - round y) < delta
    let isPerfectCube (x:int) =
        let y = System.Math.Pow(float x, 1.0/3.0)
        abs(y - round y) < delta
    let element = List.find (fun elem -> isPerfectSquare elem && isPerfectCube elem) list1
    let index = List.findIndex (fun elem -> isPerfectSquare elem && isPerfectCube elem) list1
    printfn "The first element that is both a square and a cube is %d and its index is %d." element index
// </snippet45>

module Snippet46 =
// <snippet46>
    printfn "List of squares: %A" (List.init 10 (fun index -> index * index))
// </snippet46>
(*
List of squares: [0; 1; 4; 9; 16; 25; 36; 49; 64; 81]
*)

module Snippet47 =
// <snippet47>
    let printList list1 = 
        if (List.isEmpty list1) then
            printfn "There are no elements in this list."
        else
            printfn "This list contains the following elements:"
            List.iter (fun elem -> printf "%A " elem) list1
            printfn ""
    printList [ "test1"; "test2" ]
    printList [ ]
// </snippet47>
(* 
This list contains the following elements:
"test1" "test2" 
There are no elements in this list.
*)

module Snippet48 =
// <snippet48>
    List.length [ 1 .. 100 ] |> printfn "Length: %d"
    List.length [ ] |> printfn "Length: %d"
    List.length [ 1 .. 2 .. 100 ] |> printfn "Length: %d"
// </snippet48>
(*
Length: 100
Length: 0
Length: 50
*)

module Snippet49 =
// <snippet49>
    let list1 = [ -10 .. 10 ]
    List.nth list1 5
    |> printfn "The fifth element: %d"
// </snippet49>

module Snippet50 =
// <snippet50>
    let list1 = [ 1 .. 10 ]
    let listEven, listOdd = List.partition (fun elem -> elem % 2 = 0) list1
    printfn "Evens: %A\nOdds: %A" listEven listOdd
// </snippet50>
(*
Evens: [2; 4; 6; 8; 10]
Odds: [1; 3; 5; 7; 9]
*)

module Snippet51 =
// <snippet51>
    let printPermutation n list1 =
        let length = List.length list1
        if (n > 0 && n < length) then
            List.permute (fun index -> (index + n) % length) list1
        else
            list1
        |> printfn "%A"
    let list1 = [ 1 .. 5 ]
    // There are 5 valid permutations of list1, with n from 0 to 4.
    for n in 0 .. 4 do
        printPermutation n list1
// </snippet51>
(*
[1; 2; 3; 4; 5]
[5; 1; 2; 3; 4]
[4; 5; 1; 2; 3]
[3; 4; 5; 1; 2]
[2; 3; 4; 5; 1]
*)

module Snippet52 =
// <snippet52>
    let testList = List.replicate 4 "test"
    printfn "%A" testList
// </snippet52>
(*
["test"; "test"; "test"; "test"]
*)

module Snippet53 =
// <snippet53>
    let reverseList = List.rev [ 1 .. 10 ]
    printfn "%A" reverseList
// </snippet53>
(*
[10; 9; 8; 7; 6; 5; 4; 3; 2; 1]
*)

module Snippet54 =
// <snippet54>
    let initialBalance = 1122.73
    let transactions = [ -100.00; +450.34; -62.34; -127.00; -13.50; -12.92 ]
    let balances =
        List.scan (fun balance transactionAmount -> balance + transactionAmount)
                  initialBalance transactions
    printfn "Initial balance:\n $%10.2f" initialBalance
    printfn "Transaction   Balance"
    for i in 0 .. List.length transactions - 1 do
        printfn "$%10.2f $%10.2f" transactions.[i] balances.[i]
    printfn "Final balance:\n $%10.2f" balances.[ List.length balances - 1]
// </snippet54>
(*
Initial balance:
 $   1122.73
Transaction   Balance
$   -100.00 $   1122.73
$    450.34 $   1022.73
$    -62.34 $   1473.07
$   -127.00 $   1410.73
$    -13.50 $   1283.73
$    -12.92 $   1270.23
Final balance:
 $   1257.31
*)

module Snippet55 =
// <snippet55>
    [ for x in -100 .. 100 -> 4 - x * x ]
    |> List.max
    |> printfn "%A"
// </snippet55>
(* output:
4
*)

module Snippet56 =
// <snippet56>
    [ -10.0 .. 10.0 ]
    |> List.maxBy (fun x -> 1.0 - x * x)
    |> printfn "%A"
// </snippet56>
(*
0.0
*)

module Snippet57 =
// <snippet57>
    [ for x in -100 .. 100 -> x * x - 4 ]
    |> List.min
    |> printfn "%A" 
// </snippet57>
(* output:
-4
*)

module Snippet58 =
// <snippet58>
    [ -10.0 .. 10.0 ]
    |> List.minBy (fun x -> x * x - 1.0)
    |> printfn "%A"
// </snippet58>
(*
0.0
*)

module Snippet59 =
// <snippet59>
    let list1 = List.ofArray [| 1 .. 10 |]
// </snippet59>
(*
val list1 : int list = [1; 2; 3; 4; 5; 6; 7; 8; 9; 10]
*)

module Snippet60 =
// <snippet60>
    let list1 = List.ofSeq ( seq { 1 .. 10 } )
// </snippet60>
(*
val list1 : int list = [1; 2; 3; 4; 5; 6; 7; 8; 9; 10]
*)

module Snippet61 =
    // <snippet61>
    // A list of functions that transform 
    // integers. (int -> int)
    let ops1 =
     [ (fun x -> x + 1), "add 1"
       (fun x -> x + 2), "add 2"
       (fun x -> x - 5), "subtract 5" ]

    let ops2 =
     [ (fun x -> x + 1), "add 1"
       (fun x -> x * 5), "multiply by 5"
       (fun x -> x * x), "square" ]

    // Compare scan and scanBack, which apply the
    // operations in the opposite order.
    let compareOpOrder ops x0 =
        let ops, opNames = List.unzip ops
        let xs1 = List.scan (fun x op -> op x) x0 ops
        let xs2 = List.scanBack (fun op x -> op x) ops x0

        printfn "Operations:"
        opNames |> List.iter (fun opName -> printf "%s  " opName)
        printfn ""

        // Print the intermediate results.
        let xs = List.zip xs1 (List.rev xs2)
        printfn "List.scan List.scanBack"
        for (x1, x2) in xs do
            printfn "%10d %10d" x1 x2
        printfn ""

    compareOpOrder ops1 10
    compareOpOrder ops2 10
    // </snippet61>
(*
Operations:
add 1  add 2  subtract 5  
List.scan List.scanBack
        10         10
        11          5
        13          7
         8          8

Operations:
add 1  multiply by 5  square  
List.scan List.scanBack
        10         10
        11        100
        55        500
      3025        501

*)

module Snippet62 =
// <snippet62>
    open System

    let list1 = [ "<>"; "&"; "&&"; "&&&"; "<"; ">"; "|"; "||"; "|||" ]
    printfn "Before sorting: "
    list1 |> printfn "%A"
    let sortFunction (string1:string) (string2:string) =
        if (string1.Length > string2.Length) then
           1
        else if (string1.Length < string2.Length) then
           -1
        else
            String.Compare(string1, string2)
    List.sortWith sortFunction list1
    |> printfn "After sorting:\n%A"
// </snippet62>
(*
Before sorting: 
["<>"; "&"; "&&"; "&&&"; "<"; ">"; "|"; "||"; "|||"]
After sorting:
["&"; "|"; "<"; ">"; "&&"; "||"; "<>"; "&&&"; "|||"]
*)

module Snippet63 =
// <snippet63>
    let list1 = [1; 2; 3]
    let list2 = []
    // The following line prints [2; 3].
    printfn "%A" (List.tail list1)
    // The following line throws System.ArgumentException.
    printfn "%A" (List.tail list2)
// </snippet63>
(*
[2; 3]
System.ArgumentException: The input list was empty.
*)

module Snippet64 =
// <snippet64>
    let array1 = [ 1; 3; -2; 4 ]
                 |> List.toArray
    Array.set array1 3 -10
    Array.sortInPlaceWith (fun elem1 elem2 -> compare elem1 elem2) array1
    printfn "%A" array1
// </snippet64>
(*
[|-10; -2; 1; 3|]
*)

module Snippet65 =
// <snippet65>
    let findPerfectSquareAndCube list1 =
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
        match List.tryPick testElement list1 with
        | Some (n, sqrt, cuberoot) ->
            printfn "Found an element %d with square root %d and cube root %d." n sqrt cuberoot
        | None ->
            printfn "Did not find an element that is both a perfect square and a perfect cube."

    findPerfectSquareAndCube [ 1 .. 10 ]
    findPerfectSquareAndCube [ 2 .. 100 ]
    findPerfectSquareAndCube [ 100 .. 1000 ]
    findPerfectSquareAndCube [ 1000 .. 10000 ]
    findPerfectSquareAndCube [ 2 .. 50 ]
// </snippet65>
(*
Found an element 1 with square root 1 and cube root 1.
Found an element 64 with square root 8 and cube root 4.
Found an element 729 with square root 27 and cube root 9.
Found an element 4096 with square root 64 and cube root 16.
Did not find an element that is both a perfect square and a perfect cube.
*)

module Snippet66 =
// <snippet66>
    [ 1 .. 10 ]
    |> List.sum
    |> printfn "Sum: %d"
// </snippet66>
(* output:
55
*)

module Snippet67 =
// <snippet67>
    [ 1 .. 10 ]
    |> List.sumBy (fun x -> x * x)
    |> printfn "Sum: %d"
// </snippet67>
(* output:
385
*)

module Snippet69 =
// <snippet68>
    [ 1 .. 10 ]
    |> List.toSeq
    |> Seq.truncate 5
    |> Seq.iter (fun elem -> printf "%d " elem)
    printfn ""
// </snippet68>
(* output:
1 2 3 4 5
*)

