// CodeFormatting.fs
// created by GHogen 5/1/09
module CodeFormatting

// <snippet1>
let printList list1 =
    for elem in list1 do
        if elem > 0 then
            printf "%d" elem
        elif elem = 0 then
            printf "Zero"
        else
            printf "(Negative number)"
        printf " "
    printfn "Done!"
printfn "Top-level context."
printList [-1;0;1;2;3]
// </snippet1>

// <snippet2>
let myFunction1 a b = a + b
let myFunction2(a, b) = a + b
let someFunction param1 param2 =
    let result = myFunction1 param1
                     param2
    result * 100
let someOtherFunction param1 param2 =
    let result = myFunction2(param1,
                     param2)
    result * 100
// </snippet2>

// snippet3 and snippet4 are in Program1.fs and Program2.fs respectively

// <snippet5>
let abs1 x =
    if (x >= 0)
    then
        x
    else
        -x
// </snippet5>

// <snippet6>
// The following code does not produce a warning.
let abs2 x =
    if (x >= 0)
        then
        x
        else
        -x
// </snippet6>

// <snippet7>
// The following code is not indented properly and produces a warning.
let abs3 x =
    if (x >= 0)
    then
    x
    else
    -x
// </snippet7>

// <snippet8>
let abs4 x =
    if (x >= 0) then x else
    -x
// </snippet8>

// <snippet9>
let function1 arg1 arg2 arg3 arg4 =
    arg1 + arg2
  + arg3 + arg4
// </snippet9>

// <snippet10>
let printListWithOffset a list1 =
    List.iter (fun elem ->
        printfn "%d" (a + elem)) list1
// </snippet10>

let x = 1

// <snippet11>
if (x = 0) then (
    printfn "x is zero."
) else (
    printfn "x is nonzero."
)
// </snippet11>

// <snippet12>
if (x = 1) then begin
    printfn "x is 1"
end else begin
    printfn "x is not 1"
end
// </snippet12>

// <snippet13>
type IMyInterface = interface
   abstract Function1: int -> int
end
// </snippet13>