// Learn more about F# at http://fsharp.net

let Snippet1() =
// <snippet1>
    let stringOpt1 = Some("Mirror Image")
    let stringOpt2 = None
    let reverse (string : System.String) =
        match string with
        | "" -> None
        | s -> Some(new System.String(string.ToCharArray() |> Array.rev))
        
    let result1 = Option.bind reverse stringOpt1
    printfn "%A" result1
    let result2 = Option.bind reverse stringOpt2
    printfn "%A" result2
// </snippet1>

let Snippet2() =
// <snippet2>
    let opt1 = Some("test")
    let opt2 = None
    printfn "%A %A" (Option.count opt1) (Option.count opt2)

    // Use Option.count to count the number of Some values in
    // an array of options.
    let getCount (array1 : int option array) =
        Array.fold (fun state elem -> state + Option.count elem) 0 array1
    let testArray1 = [| Some(10); None; Some(1); None; None; Some(56) |]
    let testArray2 = [| for i in 1 .. 10 do if i % 2 = 0 then yield Some(i) else yield None |]
    printfn "%d" <| getCount testArray1
    printfn "%d" <| getCount testArray2
// </snippet2>

let Snippet3() =
// <snippet3>
    let isValue opt value =
        Option.exists (fun elem -> elem = value) opt
    let testOpt1 = Some(10)
    let testOpt2 = Some(11)
    let testOpt3 = None
    printfn "%b" <| isValue testOpt1 10
    printfn "%b" <| isValue testOpt2 10
    printfn "%b" <| isValue testOpt3 10
// </snippet3>

let Snippet4() =
// <snippet4>
    let consOption list opt =
        Option.fold (fun state value -> value :: state) list opt
    printfn "%A" <| consOption [1 .. 10] None
    printfn "%A" <| consOption [1 .. 10] (Some(0))

    // Read input from the console, and if the input parses as
    // an integer, cons to the list.
    let readNumber () =
        let line = System.Console.ReadLine()
        let (success, value) = System.Int32.TryParse(line)
        if success then Some(value) else None
    let mutable list1 = []
    let mutable count = 0
    while count < 5 do
        printfn "Enter a number: "
        list1 <- consOption list1 (readNumber())
        printfn "New list: %A" <| list1
        count <- count + 1
// </snippet4>

let Snippet5() =
// <snippet5>
    let consOption list opt =
        Option.foldBack (fun value state -> value :: state) list opt
    printfn "%A" <| consOption None [ 1 .. 10 ]
    printfn "%A" <| consOption (Some(0)) [1 .. 10] 
// </snippet5>

let Snippet6() =
// <snippet6>
    let isEven opt =
        Option.forall (fun elem -> elem % 2 = 0) opt
    printfn "%b" <| isEven (Some(2))
    printfn "%b" <| isEven None
    printfn "%b" <| isEven (Some(1))

    // Use this function with an array of int options.
    let forAllOptions function1 = List.forall (fun opt -> Option.forall function1 opt)
    let list1 = [ for i in 1 .. 10 do yield Some(i) ]
    let list2 = [ for i in 1 .. 10 do yield if (i % 2) = 0 then Some(i) else None ]
    let list3 = [ for i in 1 .. 10 do yield if (i % 2) = 1 then Some(i) else None ]
    let evalList list = printfn "%b" <| forAllOptions (fun value -> value % 2 = 0) list
    let lists = [ list1; list2; list3 ]
    List.iter evalList lists
// </snippet6>

let Snippet7() =
// <snippet7>
    let printOption opt =
        if (Option.isSome opt) then
            printfn "%A" <| Option.get opt
        else ()
    printOption (Some(1))
    printOption (Some("xyz"))
    printOption (None)
    printOption (Some(1.0))
// </snippet7>

