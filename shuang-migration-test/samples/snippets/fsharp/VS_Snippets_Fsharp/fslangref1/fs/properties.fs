// Properties.fs
// created by GHogen 3/18/09
module Properties

type Dummy1() =

    let mutable myInternalValue = 10

// <snippet3201>
    // A read-only property.
    member this.MyReadOnlyProperty = myInternalValue
    // A write-only property.
    member this.MyWriteOnlyProperty with set (value) = myInternalValue <- value
    // A read-write property.
    member this.MyReadWriteProperty
        with get () = myInternalValue
        and set (value) = myInternalValue <- value
// </snippet3201>

// <snippet3202>
type MyClass(x : string) =
    let mutable myInternalValue = x
    member this.MyProperty
         with get() = myInternalValue
         and set(value) = myInternalValue <- value
// </snippet3202>

    // <snippet3203>
    member this.MyReadWriteProperty with get () = myInternalValue
    member this.MyReadWriteProperty with set (value) = myInternalValue <- value
    // </snippet3203>

type Dummy2() =
    static let mutable myStaticValue = 1
    let mutable myInternalValue = 1

    // <snippet3204>
    static member MyStaticProperty
        with get() = myStaticValue
        and set(value) = myStaticValue <- value
    // </snippet3204>

    // <snippet3205>
    // To apply a type annotation to a property that does not have an explicit 
    // get or set, apply the type annotation directly to the property.
    member this.MyProperty1 : int = myInternalValue
    // If there is a get or set, apply the type annotation to the get or set method.
    member this.MyProperty2 with get() : int = myInternalValue
    // </snippet3205>

type MyType(a) =
   let mutable backingStore = a

   member this.MyProperty
       with get() : int = backingStore
       and set(value) = backingStore <- value
// <snippet3206>

// Assume that the constructor argument sets the initial value of the
// internal backing store.
let mutable myObject = new MyType(10)
myObject.MyProperty <- 20
printfn "%d" (myObject.MyProperty)
// </snippet3206>

// <snippet3207>
// Abstract property in abstract class.
// The property is an int type that has a get and
// set method
[<AbstractClass>]
type AbstractBase() =
   abstract Property1 : int with get, set

// Implementation of the abstract property
type Derived1() =
   inherit AbstractBase()
   let mutable value = 10 
   override this.Property1 with get() = value and set(v : int) = value <- v

// A type with a "virtual" property.
 type Base1() =
   let mutable value = 10
   abstract Property1 : int with get, set
   default this.Property1 with get() = value and set(v : int) = value <- v

// A derived type that overrides the virtual property
type Derived2() =
   inherit Base1()
   let mutable value2 = 11
   override this.Property1 with get() = value2 and set(v) = value2 <- v
// </snippet3207>