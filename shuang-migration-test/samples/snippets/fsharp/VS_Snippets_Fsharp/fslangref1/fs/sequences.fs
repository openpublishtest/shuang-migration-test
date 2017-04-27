// Sequences.fs
// created GHogen 3/18/09
module Sequences

//<snippet1501>
seq { 1 .. 5 }
//</snippet1501>

// <snippet1502>
// Sequence that has an increment.
seq { 0 .. 10 .. 100 }
// </snippet1502>

// <snippet1503>
seq { for i in 1 .. 10 do yield i * i }
// </snippet1503>

// <snippet1504>
seq { for i in 1 .. 10 -> i * i }
// </snippet1504>

// <snippet1505>
let (height, width) = (10, 10)
seq { for row in 0 .. width - 1 do
         for col in 0 .. height - 1 do
           yield (row, col, row*width + col)
    }
// </snippet1505>

// <snippet1507>
// Recursive isprime function.
let isprime n =
    let rec check i =
        i > n/2 || (n % i <> 0 && check (i + 1))
    check 2

let aSequence = seq { for n in 1..100 do if isprime n then yield n }
for x in aSequence do
    printfn "%d" x
// </snippet1507>

// <snippet1506>
seq { for n in 1 .. 100 do if isprime n then yield n }
// </snippet1506>

// <snippet1508>
let multiplicationTable =
  seq { for i in 1..9 do
            for j in 1..9 do
               yield (i, j, i*j) }
// </snippet1508>

// <snippet1509>
// Yield the values of a binary tree in a sequence.
type Tree<'a> =
   | Tree of 'a * Tree<'a> * Tree<'a>
   | Leaf of 'a

// inorder : Tree<'a> -> seq<'a>   
let rec inorder tree =
    seq {
      match tree with
          | Tree(x, left, right) ->
               yield! inorder left
               yield x
               yield! inorder right
          | Leaf x -> yield x
    }   
       
let mytree = Tree(6, Tree(2, Leaf(1), Leaf(3)), Leaf(9))
let seq1 = inorder mytree
printfn "%A" seq1
// </snippet1509>