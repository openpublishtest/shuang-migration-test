// CastingAndConversions.fs
// created by GHogen 3/20/09
module CastingAndConversions

// <snippet4401>
let x : int = 5

let b : byte = byte x
// </snippet4401>

// <snippet4402>
type Color =
    | Red = 1
    | Green = 2
    | Blue = 3

// The target type of the conversion is determined by type inference.
let col1 = enum 1
// The target type is supplied by a type annotation.
let col2 : Color = enum 2 
do
    if (col1 = Color.Red) then
       printfn "Red"
// </snippet4402>

// <snippet4403>
type Base1() =
    abstract member F : unit -> unit
    default u.F() =
     printfn "F Base1"
  
type Derived1() =
    inherit Base1()
    override u.F() =
      printfn "F Derived1"
      
      
let d1 : Derived1 = Derived1()

// Upcast to Base1.
let base1 = d1 :> Base1

// This might throw an exception, unless
// you are sure that base1 is really a Derived1 object, as
// is the case here.
let derived1 = base1 :?> Derived1

// If you cannot be sure that b1 is a Derived1 object,
// use a type test, as follows:
let downcastBase1 (b1 : Base1) =
   match b1 with
   | :? Derived1 as derived1 -> derived1.F()
   | _ -> ()

downcastBase1 base1
// </snippet4403>
