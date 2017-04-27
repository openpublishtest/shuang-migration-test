
// ParametersAndArguments.fs
// created by GHogen 3/20/09


// <snippet3801>
let makeList _ = [ for i in 1 .. 100 -> i * i ]
// The arguments 100 and 200 are ignored.
let list1 = makeList 100
let list2 = makeList 200
// </snippet3801>

// <snippet3803>
type Slice = Slice of int * int * string

let GetSubstring1 (Slice(p0, p1, text)) = 
    printfn "Data begins at %d and ends at %d in string %s" p0 p1 text
    text.[p0..p1]

let substring = GetSubstring1 (Slice(0, 4, "Et tu, Brute?"))
printfn "Substring: %s" substring
// </snippet3803>

// <snippet3804>
let isNil = function [] -> true | _::_ -> false
// </snippet3804>

// <snippet3805>
let GetSubstring2 (Slice(p0, p1, text) as s) = s
// </snippet3805>

// <snippet3806>
let sum [a; b; c;] = a + b + c
// </snippet3806>

// <snippet3807>
type SpeedingTicket() =
    member this.GetMPHOver(speed: int, limit: int) = speed - limit
   
let CalculateFine (ticket : SpeedingTicket) =
    let delta = ticket.GetMPHOver(limit = 55, speed = 70)
    if delta < 20 then 50.0 else 100.0

let ticket1 : SpeedingTicket = SpeedingTicket()
printfn "%f" (CalculateFine ticket1)
// </snippet3807>

// <snippet3808>
type DuplexType =
    | Full
    | Half

type Connection(?rate0 : int, ?duplex0 : DuplexType, ?parity0 : bool) =
    let duplex = defaultArg duplex0 Full
    let parity = defaultArg parity0 false
    let mutable rate = match rate0 with
                        | Some rate1 -> rate1
                        | None -> match duplex with
                                  | Full -> 9600
                                  | Half -> 4800
    do printfn "Baud Rate: %d Duplex: %A Parity: %b" rate duplex parity
   
let conn1 = Connection(duplex0 = Full)
let conn2 = Connection(duplex0 = Half)
let conn3 = Connection(300, Half, true)
// </snippet3808>

// <snippet3809>
type Incrementor(z) =
    member this.Increment(i : int byref) =
       i <- i + z

let incrementor = new Incrementor(1)
let mutable x = 10
// Not recommended: Does not actually increment the variable.
incrementor.Increment(ref x)
// Prints 10.
printfn "%d" x  

let mutable y = 10
incrementor.Increment(&y)
// Prints 11.
printfn "%d" y 

let refInt = ref 10
incrementor.Increment(refInt)
// Prints 11.
printfn "%d" !refInt  
// </snippet3809>

// <snippet3810>
// TryParse has a second parameter that is an out parameter
// of type System.DateTime.
let (b, dt) = System.DateTime.TryParse("12-20-04 12:21:00")

printfn "%b %A" b dt

// The same call, using an address of operator.
let mutable dt2 = System.DateTime.Now
let b2 = System.DateTime.TryParse("12-20-04 12:21:00", &dt2)

printfn "%b %A" b2 dt2
// </snippet3810>

// <snippet3802>
[<EntryPoint>]
let main _ =
    printfn "Entry point!"
    0
// </snippet3802>

