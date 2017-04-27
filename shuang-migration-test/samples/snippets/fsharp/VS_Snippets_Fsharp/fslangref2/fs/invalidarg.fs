// InvalidArg.fs
// created by GHogen 3/20/09
module InvalidArgModule

// <snippet6101>
let months = [| "January"; "February"; "March"; "April";
                "May"; "June"; "July"; "August"; "September";
                "October"; "November"; "December" |]

let lookupMonth month =
   if (month > 12 || month < 1)
     then invalidArg "month" (sprintf "Value passed in was %d." month)
   months.[month - 1]

printfn "%s" (lookupMonth 12)
printfn "%s" (lookupMonth 1)
printfn "%s" (lookupMonth 13)
// </snippet6101>