// Constructors.fs
// created by GHogen 3/19/09
module Constructors

module M3501 =

 // <snippet3501>
 // This class has a primary constructor that takes three arguments
 // and an additional constructor that calls the primary constructor.
 type MyClass(x0, y0, z0) =
     let mutable x = x0
     let mutable y = y0
     let mutable z = z0
     do
         printfn "Initialized object that has coordinates (%d, %d, %d)" x y z
     member this.X with get() = x and set(value) = x <- value
     member this.Y with get() = y and set(value) = y <- value
     member this.Z with get() = z and set(value) = z <- value
     new() = MyClass(0, 0, 0)

 // Create by using the new keyword.
 let myObject1 = new MyClass(1, 2, 3)
 // Create without using the new keyword.
 let myObject2 = MyClass(4, 5, 6)
 // Create by using named arguments.
 let myObject3 = MyClass(x0 = 7, y0 = 8, z0 = 9)
 // Create by using the additional constructor.
 let myObject4 = MyClass()
 // </snippet3501>

 // <snippet3502>
 type MyStruct =
     struct
        val X : int
        val Y : int
        val Z : int
        new(x, y, z) = { X = x; Y = y; Z = z }
     end

 let myStructure1 = new MyStruct(1, 2, 3) 
 // </snippet3502>
 
 // <snippet3503>
  // Executing side effects in the primary constructor and
 // additional constructors.
 type Person(nameIn : string, idIn : int) =
     let mutable name = nameIn
     let mutable id = idIn
     do printfn "Created a person object."
     member this.Name with get() = name and set(v) = name <- v
     member this.ID with get() = id and set(v) = id <- v
     new() = 
         Person("Invalid Name", -1)
         then
             printfn "Created an invalid person object."

 let person1 = new Person("Humberto Acevedo", 123458734)
 let person2 = new Person()
 // </snippet3503>

 // <snippet3504>
 type MyClass1(x) as this =
     // This use of the self identifier produces a warning - avoid.
     let x1 = this.X
     // This use of the self identifier is acceptable.
     do printfn "Initializing object with X =%d" this.X
     member this.X = x
 // </snippet3504>

 // <snippet3505>
 type MyClass2(x : int) =
     member this.X = x
     new() as this = MyClass2(0) then printfn "Initializing with X = %d" this.X
 // </snippet3505> 

 // <snippet3506>
  type Account() =
     let mutable balance = 0.0
     let mutable number = 0
     let mutable firstName = ""
     let mutable lastName = ""
     member this.AccountNumber
        with get() = number
        and set(value) = number <- value
     member this.FirstName
        with get() = firstName
        and set(value) = firstName <- value
     member this.LastName
        with get() = lastName
        and set(value) = lastName <- value
     member this.Balance
        with get() = balance
        and set(value) = balance <- value
     member this.Deposit(amount: float) = this.Balance <- this.Balance + amount
     member this.Withdraw(amount: float) = this.Balance <- this.Balance - amount
    
   
 let account1 = new Account(AccountNumber=8782108, 
                            FirstName="Darren", LastName="Parker",
                            Balance=1543.33)
// </snippet3506>

module M3507 =

 // <snippet3507>
  type Account(accountNumber : int, ?first: string, ?last: string, ?bal : float) =
     let mutable balance = defaultArg bal 0.0
     let mutable number = accountNumber
     let mutable firstName = defaultArg first ""
     let mutable lastName = defaultArg last ""
     member this.AccountNumber
        with get() = number
        and set(value) = number <- value
     member this.FirstName
        with get() = firstName
        and set(value) = firstName <- value
     member this.LastName
        with get() = lastName
        and set(value) = lastName <- value
     member this.Balance
        with get() = balance
        and set(value) = balance <- value
     member this.Deposit(amount: float) = this.Balance <- this.Balance + amount
     member this.Withdraw(amount: float) = this.Balance <- this.Balance - amount
   
   
  let account1 = new Account(8782108, bal = 543.33,
                            FirstName="Raman", LastName="Iyer")
// </snippet3507>

