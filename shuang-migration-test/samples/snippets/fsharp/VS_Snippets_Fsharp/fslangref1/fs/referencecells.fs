// ReferenceCells.fs
// created by GHogen 3/18/09
module ReferenceCells

// <snippet2201>
// Declare a reference.
let refVar = ref 6

// Change the value referred to by the reference.
refVar := 50

// Dereference by using the ! operator.
printfn "%d" !refVar
// </snippet2201>

// <snippet2203>
let xRef : int ref = ref 10

printfn "%d" (xRef.Value)
printfn "%d" (xRef.contents)

xRef.Value <- 11
printfn "%d" (xRef.Value)
xRef.contents <- 12
printfn "%d" (xRef.contents)
// </snippet2203>

// <snippet2204>
type Incrementor(delta) =
    member this.Increment(i : int byref) =
        i <- i + delta

let incrementor = new Incrementor(1)
let mutable myDelta1 = 10
incrementor.Increment(ref myDelta1)
// Prints 10:
printfn "%d" myDelta1  

let mutable myDelta2 = 10
incrementor.Increment(&myDelta2)
// Prints 11:
printfn "%d" myDelta2 

let refInt = ref 10
incrementor.Increment(refInt)
// Prints 11:
printfn "%d" !refInt
// </snippet2204>
(*
// <snippet2205>
// Print all the lines read in from the console.
let PrintLines1() =
    let mutable finished = false
    while not finished do
        match System.Console.ReadLine() with
        | null -> finished <- true
        | s -> printfn "line is: %s" s


// Attempt to wrap the printing loop into a 
// sequence expression to delay the computation.
let PrintLines2() =
    seq {
        let mutable finished = false
        // Compiler error:
        while not finished do  
            match System.Console.ReadLine() with
            | null -> finished <- true
            | s -> yield s
    }

// You must use a reference cell instead.
let PrintLines3() =
    seq {
        let finished = ref false
        while not !finished do
            match System.Console.ReadLine() with
            | null -> finished := true
            | s -> yield s
    }
// </snippet2205>
*)

// <snippet2207>
// The following code demonstrates the use of reference
// cells to enable partially applied arguments to be changed
// by later code.

let increment1 delta number = number + delta

let mutable myMutableIncrement = 10

// Closures created by partial application and literals.
let incrementBy1 = increment1 1
let incrementBy2 = increment1 2

// Partial application of one argument from a mutable variable.
let incrementMutable = increment1 myMutableIncrement

myMutableIncrement <- 12

// This line prints 110.
printfn "%d" (incrementMutable 100)

let myRefIncrement = ref 10

// Partial application of one argument, dereferenced
// from a reference cell.
let incrementRef = increment1 !myRefIncrement

myRefIncrement := 12

// This line also prints 110.
printfn "%d" (incrementRef 100)

// Reset the value of the reference cell.
myRefIncrement := 10

// New increment function takes a reference cell.
let increment2 delta number = number + !delta

// Partial application of one argument, passing a reference cell
// without dereferencing first.
let incrementRef2 = increment2 myRefIncrement

myRefIncrement := 12

// This line prints 112.
printfn "%d" (incrementRef2 100)
// </snippet2207>