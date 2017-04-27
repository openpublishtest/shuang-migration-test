// Interfaces.fs
// created by GHogen 3/18/09
module Interfaces

// <snippet2801>
type IPrintable =
   abstract member Print : unit -> unit

type SomeClass1(x: int, y: float) =
   interface IPrintable with
      member this.Print() = printfn "%d %f" x y
// </snippet2801>

//  <snippet2802>
let x1 = new SomeClass1(1, 2.0)
(x1 :> IPrintable).Print()
//  </snippet2802>

// <snippet2803>
type SomeClass2(x: int, y: float) =
   member this.Print() = (this :> IPrintable).Print()
   interface IPrintable with
      member this.Print() = printfn "%d %f" x y

let x2 = new SomeClass2(1, 2.0)
x2.Print()
// </snippet2803>

// <snippet2804>
let makePrintable(x: int, y: float) =
    { new IPrintable with
              member this.Print() = printfn "%d %f" x y }
let x3 = makePrintable(1, 2.0) 
x3.Print()
// </snippet2804>

// <snippet2805>
type Interface1 =
    abstract member Method1 : int -> int

type Interface2 =
    abstract member Method2 : int -> int

type Interface3 =
    inherit Interface1
    inherit Interface2
    abstract member Method3 : int -> int

type MyClass() =
    interface Interface3 with
        member this.Method1(n) = 2 * n
        member this.Method2(n) = n + 100
        member this.Method3(n) = n / 10
// </snippet2805>