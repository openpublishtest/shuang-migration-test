// Arrays.fs
// created by GHogen 3/18/09
module Arrays

//<snippet1601>
let array1 = [| 1; 2; 3 |]
//</snippet1601>

//<snippet1602>
let array3 = [| for i in 1 .. 10 -> i * i |]
//</snippet1602>

// <snippet1603>
array1.[1]
// </snippet1603>

// <snippet1604>
array1.[0..2]  // elements from 0 to 2
array1.[..2] // elements the beginning of the array to 2
array1.[2..] // elements from 2 to the end
// </snippet1604>

// <snippet1605>
let arrayOfTenZeroes : int array = Array.zeroCreate 10
// </snippet1605>

