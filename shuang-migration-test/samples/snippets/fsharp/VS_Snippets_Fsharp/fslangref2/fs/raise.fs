// Raise.fs
// created by GHogen 3/20/09
module RaiseModule

// <snippet5801>
exception InnerError of string
exception OuterError of string

let function1 x y =
   try
     try
        if x = y then raise (InnerError("inner"))
        else raise (OuterError("outer"))
     with
      | InnerError(str) -> printfn "Error1 %s" str
   finally
      printfn "Always print this."
      
      
let function2 x y =
  try
     function1 x y
  with
     | OuterError(str) -> printfn "Error2 %s" str
     
function2 100 100
function2 100 10
// </snippet5801>

// <snippet5802>
let divide x y =
  if (y = 0) then raise (System.ArgumentException("Divisor cannot be zero!"))
  else
     x / y
// </snippet5802>