// TypeInference.fs
// created by GHogen 4/20/10
module TypeInference

// <snippet301>
let f a b = a + b + 100
// </snippet301>

let Snippet302() =
// <snippet302>
    // Type annotations on a parameter.
    let addu1 (x : uint32) y =
        x + y

    // Type annotations on an expression.
    let addu2 x y =
        (x : uint32) + y
// </snippet302>
    ()

let Snippet303() =
    // <snippet303>
    let addu1 x y : uint32 =
       x + y
    // </snippet303>
    ()

// <snippet304>
let replace(str: string) =
    str.Replace("A", "a")
// </snippet304>

// <snippet305>
let makeTuple a b = (a, b)
// </snippet305>