// RecursiveFunctions.fs
// Created by GHogen 3/18/09
module RecursiveFunctions

// <snippet4001>
let rec fib n =
   if n <= 2 then 1
   else fib (n - 1) + fib (n - 2)
// </snippet4001>

// <snippet4002>
let rec Even x =
   if x = 0 then true
   else Odd (x - 1)
and Odd x =
   if x = 1 then true
   else Even (x - 1)
// </snippet4002>