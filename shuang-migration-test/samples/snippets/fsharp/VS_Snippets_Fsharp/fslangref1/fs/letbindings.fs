// letBindings.fs
// created by GHogen 3/18/09
module letBindings

module M1 =

 // <snippet1101>
 let i = 1
 // </snippet1101>

 // <snippet1102>
 let someVeryLongIdentifier =
     // Note indentation below.
     3 * 4 + 5 * 6
 // </snippet1102>

module M2 =


 // <snippet1104>
 let result =
     // <snippet1103>
     let i, j, k = (1, 2, 3)
     // </snippet1103>
     // Body expression: 
     i + 2*j + 3*k
 // </snippet1104>

(*
// <snippet1105>
// Error:
printfn "%d" x  
let x = 100
// OK: 
printfn "%d" x
// </snippet1105>
*)

 // <snippet1106>
 let function1 a =
     a + 1
 // </snippet1106>

 // <snippet1107>
 let function2 (a, b) = a + b
 // </snippet1107>

module M3 =

 // <snippet1108>
 let function1 (a: int) : int = a + 1
 // </snippet1108>

 // <snippet1109>
 let result =
     let function3 (a, b) = a + b
     100 * function3 (1, 2)
 // </snippet1109>

 // <snippet1110>
 type MyClass(a) =
     let field1 = a
     let field2 = "text"
     do printfn "%d %s" field1 field2
     member this.F input =
         printfn "Field1 %d Field2 %s Input %A" field1 field2 input
 // </snippet1110>

 open System

 module Snippet1111 =
 // <snippet1111>
     [<Obsolete>]
     let function1 x y = x + y
 // </snippet1111>
(*
 // <snippet1112>
 let  a2, [<Obsolete>] b2 = (1, 2)
 // </snippet1112>
*)