// Lists.fs
// created GHogen 3/18/09
module Lists

// <snippet1301>
let list123 = [ 1; 2; 3 ]
// </snippet1301>

let Snippet13011 () =
// <snippet13011>
    let list123 = [
        1
        2
        3 ]
// </snippet13011>
    ()

open System.Windows.Forms
// <snippet13012>
let myControlList : Control list = [ new Button(); new CheckBox() ]
// </snippet13012>

// <snippet1302>
let list1 = [ 1 .. 10 ]
// </snippet1302>

// <snippet1303>
let listOfSquares = [ for i in 1 .. 10 -> i*i ]
// </snippet1303>

// <snippet1304>
// An empty list.
let listEmpty = []
// </snippet1304>

// <snippet1305>
let list2 = 100 :: list1
// </snippet1305>

// <snippet1306>
let list3 = list1 @ list2
// </snippet1306>

module M1307 =

 // <snippet1307>
 let list1 = [ 1; 2; 3 ]

 // Properties
 printfn "list1.IsEmpty is %b" (list1.IsEmpty)
 printfn "list1.Length is %d" (list1.Length)
 printfn "list1.Head is %d" (list1.Head)
 printfn "list1.Tail.Head is %d" (list1.Tail.Head)
 printfn "list1.Tail.Tail.Head is %d" (list1.Tail.Tail.Head)
 printfn "list1.Item(1) is %d" (list1.Item(1))
 // </snippet1307>

 // <snippet13071>
 let rec sum list =
    match list with
    | head :: tail -> head + sum tail
    | [] -> 0
 // </snippet13071>

module Snippet13072 =
 // <snippet13072>
 let sum list =
    let rec loop list acc =
        match list with
        | head :: tail -> loop tail (acc + head)
        | [] -> acc
    loop list 0
 // </snippet13072>

 // <snippet1308>
 let IsPrimeMultipleTest n x =
    x = n || x % n <> 0

 let rec RemoveAllMultiples listn listx =
    match listn with
    | head :: tail -> RemoveAllMultiples tail (List.filter (IsPrimeMultipleTest head) listx)
    | [] -> listx


 let GetPrimesUpTo n =
     let max = int (sqrt (float n))
     RemoveAllMultiples [ 2 .. max ] [ 1 .. n ]

 printfn "Primes Up To %d:\n %A" 100 (GetPrimesUpTo 100)
// </snippet1308>

module M1309 =

 // <snippet1309>
 let list1 = [1;2;3]
 let list2 = [4;5;6]
 List.iter (fun x -> printfn "List.iter: element is %d" x) list1
 List.iteri(fun i x -> printfn "List.iteri: element %d is %d" i x) list1
 List.iter2 (fun x y -> printfn "List.iter2: elements are %d %d" x y) list1 list2
 List.iteri2 (fun i x y ->
                printfn "List.iteri2: element %d of list_1 is %d element %d of list2 is %d"
                  i x i y)
             list1 list2
 // </snippet1309> 

 // <snippet1310>
 let newList = List.map (fun x -> x + 1) list1
 printfn "%A" newList
 // </snippet1310>

 // <snippet1311>
 let sumList = List.map2 (fun x y -> x + y) list1 list2
 printfn "%A" sumList

 let newList2 = List.map3 (fun x y z -> x + y + z) list1 list2 newList
 printfn "%A" newList2

 let collectList = List.collect (fun x -> [for i in 1..3 -> x * i]) list1
 printfn "%A" collectList

 let newListAddIndex = List.mapi (fun i x -> x + i) list1
 printfn "%A" newListAddIndex

 let listAddTimesIndex = List.mapi2 (fun i x y -> (x + y) * i) list1 list2
 printfn "%A" listAddTimesIndex
 // </snippet1311>

 // <snippet1312>
 let evenOnlyList = List.filter (fun x -> x % 2 = 0) [1; 2; 3; 4; 5; 6]
 // </snippet1312>








