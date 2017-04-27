// Modules7.fs
// created by GHogen 3/20/09
module Modules7

module M6607 =

 // <snippet6607>
 module Y =
     let x = 1 
 
     module Z =
         let z = 5
// </snippet6607>

module M6608 =

 // <snippet6608>
 module Y =
     let x = 1 

 module Z =
     let z = 5
 // </snippet6608>

module M6609 =

// <snippet6609>
 module Y =
         let x = 1

     module Z =
         let z = 5
// </snippet6609>

module M6610 =

 // <snippet6610>
 // This code produces a warning, but treats Z as a inner module.
 module Y =
 module Z =
     let z = 5
 // </snippet6610>

module M6611 =

// <snippet6611>
 module Y =
     module Z =
         let z = 5
// </snippet6611>




