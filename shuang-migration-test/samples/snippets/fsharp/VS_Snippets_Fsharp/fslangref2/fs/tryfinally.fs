// TryFinally.fs
// created by GHogen 3/20/09
module TryFinally

// <snippet5701>
let divide x y =
   let stream : System.IO.FileStream = System.IO.File.Create("test.txt")
   let writer : System.IO.StreamWriter = new System.IO.StreamWriter(stream)
   try
      writer.WriteLine("test1");
      Some( x / y )
   finally
      writer.Flush()
      printfn "Closing stream"
      stream.Close()
      
let result =
  try
     divide 100 0
  with
     | :? System.DivideByZeroException -> printfn "Exception handled."; None
// </snippet5701>

// <snippet5702>
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
// </snippet5702>
