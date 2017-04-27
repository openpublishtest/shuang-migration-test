// Attributes.fs
// created by GHogen 3/20/09
module AttributesModule

open System


//<snippet6202>
open System.Runtime.InteropServices

[<DllImport("kernel32", SetLastError=true)>]
extern bool CloseHandle(nativeint handle)
// </snippet6202>

type OwnerAttribute(ownerString0 : string) =
    inherit Attribute()
    let mutable ownerString = ownerString0
    member this.Owner
        with get() = ownerString
        and set(value) = ownerString <- value

type CompanyAttribute(companyString0 : string) =
    inherit Attribute()
    let mutable companyString = companyString0
    member this.Company
        with get() = companyString
        and set(value) = companyString <- value

// <snippet6603>
[<Owner("Jason Carlson")>]
[<Company("Microsoft")>]
type SomeType1 =
// </snippet6603>
   member this.F() = 1

// <snippet6604>
[<Owner("Darren Parker"); Company("Microsoft")>]
type SomeType2 =
// </snippet6604>
    member this.F() = 1

// <snippet6605>
open System

[<Obsolete("Do not use. Use newFunction instead.")>]
let obsoleteFunction x y =
  x + y
  
let newFunction x y =
  x + 2 * y

// The use of the obsolete function produces a warning.
let result1 = obsoleteFunction 10 100
let result2 = newFunction 10 100
// </snippet6605>

// <snippet6606>
open System.Reflection
[<assembly:AssemblyVersionAttribute("1.0.0.0")>]
do
   printfn "Executing..."
// </snippet6606>




