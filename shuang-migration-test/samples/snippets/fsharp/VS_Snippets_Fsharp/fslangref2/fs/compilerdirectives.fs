// CompilerDirectives.fs
// created by GHogen 3/20/09
module compilerDirectives

// <snippet7301>
#if VERSION1
let function1 x y =
   printfn "x: %d y: %d" x y
   x + 2 * y
#else
let function1 x y =
   printfn "x: %d y: %d" x y
   x - 2*y
#endif

let result = function1 10 20
// </snippet7301>

(*
// <snippet7302>
let x = #if SYMBOL1 0 #else 1 #endif
// </snippet7302>
*)

// <snippet7303>
# 25
#line 25
#line 25 "C:\\Projects\\MyProject\\MyProject\\Script1"
#line 25 @"C:\Projects\MyProject\MyProject\Script1"
# 25 @"C:\Projects\MyProject\MyProject\Script1"
// </snippet7303>
let y = 1
