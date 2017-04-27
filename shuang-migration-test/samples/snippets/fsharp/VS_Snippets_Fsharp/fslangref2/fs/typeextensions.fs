// TypeExtensions.fs
// created by GHogen 3/20/09
module TypeExtensions

// <snippet3701>
module MyModule1 =

    // Define a type.
    type MyClass() =
      member this.F() = 100

    // Define type extension.
    type MyClass with
       member this.G() = 200

module MyModule2 =
   let function1 (obj1: MyModule1.MyClass) =
      // Call an ordinary method.
      printfn "%d" (obj1.F())
      // Call the extension method.
      printfn "%d" (obj1.G())
// </snippet3701>

// <snippet3702>
// Define a new member method FromString on the type Int32.
type System.Int32 with
    member this.FromString( s : string ) =
       System.Int32.Parse(s)
     
let testFromString str =  
    let mutable i = 0
    // Use the extension method.
    i <- i.FromString(str)
    printfn "%d" i
    
testFromString "500"
// </snippet3702>