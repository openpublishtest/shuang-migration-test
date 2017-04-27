// Delegates.fs
// created by GHogen 3/20/09
module Delegates

// <snippet4201>
type Test1() =
  static member add(a : int, b : int) =
     a + b
  static member add2 (a : int) (b : int) =
     a + b

  member x.Add(a : int, b : int) =
     a + b
  member x.Add2 (a : int) (b : int) =
     a + b


// Delegate1 works with tuple arguments.
type Delegate1 = delegate of (int * int) -> int
// Delegate2 works with curried arguments.
type Delegate2 = delegate of int * int -> int

let InvokeDelegate1 (dlg : Delegate1) (a : int) (b: int) =
   dlg.Invoke(a, b)
let InvokeDelegate2 (dlg : Delegate2) (a : int) (b: int) =
   dlg.Invoke(a, b)

// For static methods, use the class name, the dot operator, and the
// name of the static method.
let del1 : Delegate1 = new Delegate1( Test1.add )
let del2 : Delegate2 = new Delegate2( Test1.add2 )

let testObject = Test1()

// For instance methods, use the instance value name, the dot operator, and the instance method name.
let del3 : Delegate1 = new Delegate1( testObject.Add )
let del4 : Delegate2 = new Delegate2( testObject.Add2 )

for (a, b) in [ (100, 200); (10, 20) ] do
  printfn "%d + %d = %d" a b (InvokeDelegate1 del1 a b)
  printfn "%d + %d = %d" a b (InvokeDelegate2 del2 a b)
  printfn "%d + %d = %d" a b (InvokeDelegate1 del3 a b)
  printfn "%d + %d = %d" a b (InvokeDelegate2 del4 a b)
// </snippet4201>

module Snippet4202 =
    // <snippet4202>
    type Delegate1 = delegate of int * char -> string

    let replicate n c = String.replicate n (string c)

    // An F# function value constructed from an unapplied let-bound function 
    let function1 = replicate

    // A delegate object constructed from an F# function value
    let delObject = new Delegate1(function1)

    // An F# function value constructed from an unapplied .NET member
    let functionValue = delObject.Invoke

    List.map (fun c -> functionValue(5,c)) ['a'; 'b'; 'c']
    |> List.iter (printfn "%s")

    // Or if you want to get back the same curried signature
    let replicate' n c =  delObject.Invoke(n,c)

    // You can pass a lambda expression as an argument to a function expecting a compatible delegate type
    // System.Array.ConvertAll takes an array and a converter delegate that transforms an element from
    // one type to another according to a specified function.
    let stringArray = System.Array.ConvertAll([|'a';'b'|], fun c -> replicate' 3 c)
    printfn "%A" stringArray
    // </snippet4202>