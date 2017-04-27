// Generics.fs
// created GHogen 3/18/09
module Generics

//<snippet1700>
let makeList a b =
    [a; b]
//</snippet1700>

//<snippet1701>
let function1 (x: 'a) (y: 'a) =
    printfn "%A %A" x y
//</snippet1701>
// <snippet1703>
let function2<'T> x y =
    printfn "%A, %A" x y
//</snippet1703>
//<snippet1702>
// In this case, the type argument is inferred to be int.
function1 10 20
// In this case, the type argument is float.
function1 10.0 20.0
// Type arguments can be specified, but should only be specified
// if the type parameters are declared explicitly. If specified,
// they have an effect on type inference, so in this example,
// a and b are inferred to have type int. 
let function3 a b =
    // The compiler reports a warning:
    function1<int> a b
    // No warning.
    function2<int> a b
//</snippet1702>



// <snippet1704>
let printSequence (sequence1: Collections.seq<_>) =
   Seq.iter (fun elem -> printf "%s " (elem.ToString())) sequence1
//</snippet1704>

module Snippet1705 =
    //<snippet1705>
    // A generic function.
    // In this example, the generic type parameter 'a makes function3 generic.
    let function3 (x : 'a) (y : 'a) =
        printf "%A %A" x y

    // A generic record, with the type parameter in angle brackets.
    type GR<'a> = 
        {
            Field1: 'a;
            Field2: 'a;
        }

    // A generic class.
    type C<'a>(a : 'a, b : 'a) =
        let z = a
        let y = b
        member this.GenericMethod(x : 'a) =
            printfn "%A %A %A" x y z

    // A generic discriminated union.
    type U<'a> =
        | Choice1 of 'a
        | Choice2 of 'a * 'a

    type Test() =
        // A generic member
        member this.Function1<'a>(x, y) =
            printfn "%A, %A" x y

        // A generic abstract method.
        abstract abstractMethod<'a, 'b> : 'a * 'b -> unit
        override this.abstractMethod<'a, 'b>(x:'a, y:'b) =
             printfn "%A, %A" x y
    //</snippet1705>
