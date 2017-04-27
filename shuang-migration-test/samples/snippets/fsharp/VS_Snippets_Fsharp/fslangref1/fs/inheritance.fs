// Inheritance.fs
// created by GHogen 3/18/09
module Inheritance

// <snippet2601>
type MyClassBase1() =
   let mutable z = 0
   abstract member function1 : int -> int
   default u.function1(a : int) = z <- z + a; z

type MyClassDerived1() =
   inherit MyClassBase1()
   override u.function1(a: int) = a + 1
// </snippet2601>

// <snippet2602>
type MyClassBase2(x: int) =
   let mutable z = x * x
   do for i in 1..z do printf "%d " i
   

type MyClassDerived2(y: int) =
   inherit MyClassBase2(y * 2)
   do for i in 1..y do printf "%d " i
// </snippet2602>

// <snippet2603>
open System

let object1 = { new Object() with
      override this.ToString() = "This overrides object.ToString()"
      }

printfn "%s" (object1.ToString())
// </snippet2603>