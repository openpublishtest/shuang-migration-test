// Loops2.fs
// created by GHogen  3/20/09
module Loops2

// <snippet5201>
// Looping over a list.
let list1 = [ 1; 5; 100; 450; 788 ]
for i in list1 do
   printfn "%d" i
// </snippet5201>

// <snippet5202>
let seq1 = seq { for i in 1 .. 10 -> (i, i*i) }
for (a, asqr) in seq1 do
  printfn "%d squared is %d" a asqr
// </snippet5202>

// <snippet5203>
let function1() =
  for i in 1 .. 10 do
    printf "%d " i
  printfn ""
function1()
// </snippet5203>

// <snippet5204>
let function2() =
  for i in 1 .. 2 .. 10 do
     printf "%d " i
  printfn ""
function2()
// </snippet5204>

// <snippet5205>
let function3() =
  for c in 'a' .. 'z' do
    printf "%c " c
  printfn ""
function3()
// </snippet5205>

// <snippet5208>
let function4() =
    for i in 10 .. -1 .. 1 do
        printf "%d " i
    printfn " ... Lift off!"
function4()
// </snippet5208>

// <snippet5206>
let beginning x y = x - 2*y
let ending x y = x + 2*y

let function5 x y =
  for i in (beginning x y) .. (ending x y) do
     printf "%d " i
  printfn ""

function5 10 4
// </snippet5206>

// <snippet5207>
let mutable count = 0
for _ in list1 do
   count <- count + 1
printfn "Number of elements in list1: %d" count
// </snippet5207>

