// TypeInference.fs
// created GHogen 3/18/09
module TypeInference

module M801 =

 // <snippet801>
 let f a b = a + b + 100
 // </snippet801>

module M8002 =

 // <snippet802>
 // Type annotations on a parameter.
 let addu1 (x : uint32) y =
     x + y

 // Type annotations on an expression.
 let addu2 x y =
     (x : uint32) + y
 // </snippet802>

module M803 =

 // <snippet803>
 let addu1 x y : uint32 =
     x + y
 // </snippet803>

 // <snippet804>
 let makeTuple a b = (a, b)
 // </snippet804>
