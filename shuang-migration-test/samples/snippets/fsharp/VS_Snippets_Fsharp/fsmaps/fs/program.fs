// Learn more about F# at http://fsharp.net

let Snippet1() =
// <snippet1>
    Map.ofList [ (1, "one"); (2, "two"); (3, "three") ]
    |> Map.add(0) "zero"
    |> Map.iter (fun key value -> printfn "key: %d value: %s" key value)
// </snippet1>

let Snippet2() =
// <snippet2>
    let map1 = Map.ofList [ (1, "one"); (2, "two"); (3, "three") ]
    let map2 = map1.Add(0, "zero")
    map2 |> Map.iter (fun key value -> printfn "key: %d value: %s" key value)
// </snippet2>

let Snippet3() =
// <snippet3>
    let map1 = Map.ofList [ (1, "one"); (2, "two"); (3, "three") ]
    let findKeyAndPrint key map =
        if (Map.containsKey key map) then
            printfn "The specified map contains the key %d." key
        else
            printfn "The specified map does not contain the key %d." key
    findKeyAndPrint 1 map1
    findKeyAndPrint 0 map1
// </snippet3>

let Snippet4() =
// <snippet4>
    let map1 = Map.ofList [ (1, "one"); (2, "two"); (3, "three") ]
    let findKeyAndPrint key (map : Map<int,string>) =
        if (map.ContainsKey key) then
            printfn "The specified map contains the key %d." key
        else
            printfn "The specified map does not contain the key %d." key
    findKeyAndPrint 1 map1
    findKeyAndPrint 0 map1
// </snippet4>

let Snippet5() =
// <snippet5>
    printfn "Even numbers and their squares."
    let map1 = Map.ofList [for i in 1 .. 10 -> (i, i*i)]
               |> Map.filter (fun key _ -> key % 2 = 0)
               |> Map.iter (fun key value -> printf "(%d, %d) " key value)
    printfn ""
// </snippet5>

let Snippet6() =
// <snippet6>
    let findAndPrint key map =
        printfn "With key %d, found value %A." key (Map.find key map)
    let map1 = Map.ofList [ (1, "one"); (2, "two"); (3, "three") ]
    let map2 = Map.ofList [ for i in 1 .. 10 -> (i, i*i) ]
    try
        findAndPrint 1 map1
        findAndPrint 2 map1
        findAndPrint 3 map2
        findAndPrint 5 map2
        // The key is not in the map, so this throws an exception.
        findAndPrint 0 map2
    with
         :? System.Collections.Generic.KeyNotFoundException as e -> printfn "%s" e.Message
// </snippet6>

let Snippet7() =
// <snippet7>
    let findKeyFromValue findValue map =
        printfn "With value %A, found key %A." findValue (Map.findKey (fun key value -> value = findValue) map)
    let map1 = Map.ofList [ (1, "one"); (2, "two"); (3, "three") ]
    let map2 = Map.ofList [ for i in 1 .. 10 -> (i, i*i) ]
    try
        findKeyFromValue "one" map1
        findKeyFromValue "two" map1
        findKeyFromValue 9 map2
        findKeyFromValue 25 map2
        // The key is not in the map, so the following line throws an exception.
        findKeyFromValue 0 map2
    with
         :? System.Collections.Generic.KeyNotFoundException as e -> printfn "%s" e.Message
// </snippet7>

let Snippet8() =
// <snippet8>
    let map1 = Map.ofList [ (1, "one"); (2, "two"); (3, "three") ]
    // Sum the keys.
    let result1 = Map.fold (fun state key value -> state + key) 0 map1
    printfn "Result: %d" result1
    // Concatenate the values.
    let result2 = Map.fold (fun state key value -> state + value + " ") "" map1
    printfn "Result: %s" result2
// </snippet8>

let Snippet9() =
// <snippet9>
    let map1 = Map.ofList [ (1, "one"); (2, "two"); (3, "three") ]
    // Sum the keys.
    let result1 = Map.foldBack (fun key value state -> state + key) map1 0
    printfn "Result: %d" result1
    // Concatenate the values.
    let result2 = Map.foldBack (fun key value state -> state + value + " ") map1 ""
    printfn "Result: %s" result2 
// </snippet9>

// This one is too complicated; do not use
let Snippet10() =
// <snippet10>
    let makeMap (f : float -> float) min max step =
        seq { for x in min .. step .. max -> (x, f x) } |> Map.ofSeq
    let functions = [ sin; cos; tan ]
    let functionData = functions |> List.map (fun f -> makeMap f 0.0 (3.14159/2.0) 0.1)
    let map1 = List.zip functions functionData
    let positiveInRangeResults = List.map (fun (key, valueMap) -> Map.forall (fun key value -> value > 0.0) valueMap) map1
    printfn "%A" positiveInRangeResults
// </snippet10>

let Snippet11() =
// <snippet11>
    let map1 = Map.ofList [ (1, "one"); (2, "two"); (3, "three") ]
    let map2 = Map.ofList [ (-1, "negative one"); (2, "two"); (3, "three") ]
    let allPositive = Map.forall (fun key value -> key > 0)
    printfn "%b %b" (allPositive map1) (allPositive map2)
// </snippet11>

let Snippet12() =
// <snippet12>
    let map1 = Map.ofList [ (1, "One"); (2, "Two"); (3, "Three") ]
    let map2 = map1 |> Map.map (fun key value -> value.ToUpper())
    let map3 = map1 |> Map.map (fun key value -> value.ToLower())
    printfn "%A" map1
    printfn "%A" map2
    printfn "%A" map3
// </snippet12>

let Snippet13() =
// <snippet13>
    let map1 = [ for i in 1..10 -> (i, i*i)] |> Map.ofList
    let (mapEven, mapOdd) = Map.partition (fun key value -> key % 2 = 0) map1
    printfn "Evens: %A" mapEven
    printfn "Odds: %A" mapOdd
// </snippet13>

let Snippet14() =
// <snippet14>
    let map1 = [ for i in 1 .. 100 -> (i, 100 - i) ] |> Map.ofList
    let result = Map.pick (fun key value -> if key = value then Some(key) else None) map1
    printfn "Result where key and value are the same: %d" result
// </snippet14>

let Snippet15() =
// <snippet15>
    let map1 = [ for i in 1 .. 100 -> (i, i*i) ] |> Map.ofList
    let result = Map.tryFind 50 map1
    match result with
    | Some x -> printfn "Found %d." x
    | None -> printfn "Did not find the specified value."
// </snippet15>

let Snippet16() =
// <snippet16>
    let map1 = [ for i in 1 .. 100 -> (i, i*i) ] |> Map.ofList
    let result = map1.TryFind 50
    match result with
    | Some x -> printfn "Found %d." x
    | None -> printfn "Did not find the specified value."
// </snippet16>

let Snippet17() =
// <snippet17>
    let map1 = [ for i in 1 .. 100 -> (i, i*i) ] |> Map.ofList
    let result = Map.tryFindKey (fun key value -> key = value) map1
    match result with
    | Some key -> printfn "Found element with key %d." key
    | None -> printfn "Did not find any element that matches the condition."
// </snippet17>

let Snippet18() =
// <snippet18>
    let map1 = [ for i in 1 .. 100 -> (i, 100 - i) ] |> Map.ofList
    let result = Map.tryPick (fun key value -> if key = value then Some(key) else None) map1
    match result with
    | Some x -> printfn "Result where key and value are the same: %d" x
    | None -> printfn "No result satisifies the condition."
// </snippet18>

