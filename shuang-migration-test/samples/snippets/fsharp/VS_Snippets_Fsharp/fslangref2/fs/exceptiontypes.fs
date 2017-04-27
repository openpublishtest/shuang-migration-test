// ExceptionTypes.fs
// created by GHogen 3/20/09
module ExceptionTypes

// <snippet5501>
exception MyError of string
// </snippet5501>

// <snippet5502>
raise (MyError("Error message"))
// </snippet5502>

// <snippet5503>
exception Error1 of string
// Using a tuple type as the argument type.
exception Error2 of string * int

let function1 x y =
   try
      if x = y then raise (Error1("x"))
      else raise (Error2("x", 10))
   with
      | Error1(str) -> printfn "Error1 %s" str
      | Error2(str, i) -> printfn "Error2 %s %d" str i
 
function1 10 10
function1 9 2
// </snippet5503>

