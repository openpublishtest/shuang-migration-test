// Functions.fs
// Created 3/18/09 by GHogen
module Functions
(*
module M1 =

 // <snippet101>
 let list1 = [ 1; 2; 3]
 // Error: duplicate definition.
 let list1 = []  
 let function1 =
    let list1 = [1; 2; 3]
    let list1 = []
    list1
 // </snippet101>
*)

module M2 =

 // <snippet102>
 let list1 = [ 1; 2; 3]
 let sumPlus x =
 // OK: inner list1 hides the outer list1.
    let list1 = [1; 5; 10]  
    x + List.sum list1
 // </snippet102>

 // <snippet103>
 let cylinderVolume radius length =
     // Define a local value pi.
     let pi = 3.14159
     length * pi * radius * radius
 // </snippet103>

module M3 =

// <snippet105>

 let cylinderVolume radius length : float =
    // Define a local value pi.
    let pi = 3.14159
    length * pi * radius * radius
 // </snippet105>

 // <snippet106>
 let smallPipeRadius = 2.0
 let bigPipeRadius = 3.0

 // These define functions that take the length as a remaining
 // argument:

 let smallPipeVolume = cylinderVolume smallPipeRadius
 let bigPipeVolume = cylinderVolume bigPipeRadius
 // </snippet106>
 
 // <snippet107>
 let length1 = 30.0
 let length2 = 40.0
 let smallPipeVol1 = smallPipeVolume length1
 let smallPipeVol2 = smallPipeVolume length2
 let bigPipeVol1 = bigPipeVolume length1
 let bigPipeVol2 = bigPipeVolume length2
 // </snippet107>

 // <snippet108> 
 let rec fib n = if n < 2 then 1 else fib (n - 1) + fib (n - 2)
 // </snippet108>


 // <snippet109>
 let apply1 (transform : int -> int ) y = transform y
 // </snippet109>

 // <snippet110>
 let increment x = x + 1

 let result1 = apply1 increment 100
 // </snippet110>

 // <snippet111>
 let apply2 ( f: int -> int -> int) x y = f x y

 let mul x y = x * y
  
 let result2 = apply2 mul 10 20
 // </snippet111>

 // <snippet112>
 let result3 = apply1 (fun x -> x + 1) 100

 let result4 = apply2 (fun x y -> x * y ) 10 20
 // </snippet112>

module M4 =

 // <snippet113>
 let function1 x = x + 1
 let function2 x = x * 2
 let h = function1 >> function2
 let result5 = h 100
 // </snippet113>

 // <snippet114>
 let result6 = 100 |> function1 |> function2
 // </snippet114>
