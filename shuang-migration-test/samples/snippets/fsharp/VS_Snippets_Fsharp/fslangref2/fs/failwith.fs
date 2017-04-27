// FailWith.fs
// created by GHogen 3/20/09
module FailWith

// <snippet6001>
let divideFailwith x y =
  if (y = 0) then failwith "Divisor cannot be zero."
  else
    x / y
    
let testDivideFailwith x y =
  try
     divideFailwith x y
  with
     | Failure(msg) -> printfn "%s" msg; 0
  
let result1 = testDivideFailwith 100 0
// </snippet6001>
