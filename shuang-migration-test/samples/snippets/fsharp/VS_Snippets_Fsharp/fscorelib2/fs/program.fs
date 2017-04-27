// Learn more about F# at http://fsharp.net

open System
open System.Collections.Generic

module Snippet1 =
// <snippet1>
    open System
    open System.Collections.Generic

    let seq1 = seq { for i in 1..10 -> i, i*i }
    let dictionary1 = dict seq1
    if dictionary1.IsReadOnly then
        Console.WriteLine("The dictionary is read only.")
    // The type is a read only IDictionary.
    // If you try to add or remove elements,
    // NotSupportedException is generated, as in the following line:
    //dictionary1.Add(new KeyValuePair<int, int>(0, 0))
    // You can use read-only methods as in the following lines.
    if dictionary1.ContainsKey(5) then
        Console.WriteLine("Value for key 5: {0}", dictionary1.Item(5))
    for elem in dictionary1 do
       Console.WriteLine("Key: {0} Value: {1}", elem.Key, elem.Value) 
// </snippet1>
(*
The dictionary is read only.
Value for key 5: 25
Key: 1 Value: 1
Key: 2 Value: 4
Key: 3 Value: 9
Key: 4 Value: 16
Key: 5 Value: 25
Key: 6 Value: 36
Key: 7 Value: 49
Key: 8 Value: 64
Key: 9 Value: 81
Key: 10 Value: 100
*)

let Snippet2() =
// <snippet2>
    let maxValue = 10
    let function1 x =
       if (x > maxValue) then
           eprintf "Error: the input %d exceeds the maximum value, %d." x maxValue
       else
           printfn "Success!"
    function1 1
    function1 11
    // Issue a newline to stderr to trigger printing.
    stderr.WriteLine()
// </snippet2>
(*
Success!
Error: the input 11 exceeds the maximum value 10.
*)

let Snippet3() =
// <snippet3>
    let maxValue = 10
    let function1 x =
       if (x > maxValue) then
           eprintfn "Error: the input %d exceeds the maximum value, %d." x maxValue
       else
           printfn "Success!"
    function1 1
    function1 11
// </snippet3>
(*
Success!
Error: the input 11 exceeds the maximum value 10.
*)

let Snippet4() =
// <snippet4>
    let reportError componentName code =
        failwithf "Component %s reported a failure. Error code: 0x%x" componentName code
    reportError "Filesystem monitor" 0x80000005
// </snippet4>

let Snippet5() =
// <snippet5>
    let fileName = "directoryListing.txt"
    let printDirectoryInfo (dirName:string) (fileName:string) =
        use file = System.IO.File.CreateText(fileName)
        System.IO.Directory.EnumerateFileSystemEntries(dirName)
        |> Seq.iter (fun elem -> fprintfn file "%s" elem )
    printDirectoryInfo @"C:\" fileName
    printfn "%s" (System.IO.File.OpenText(fileName).ReadToEnd())
// </snippet5>

let Snippet6() =
// <snippet6>
    let fileName = "directoryListingXY.txt"
    let printDirectoryInfo (dirName:string) (fileName:string) =
        use file = System.IO.File.CreateText(fileName)
        System.IO.Directory.EnumerateDirectories(dirName)
        |> Seq.map (fun elem -> new System.IO.DirectoryInfo(elem))
        |> Seq.iter (fun elem -> fprintfn file "%50s %A" elem.FullName elem.LastAccessTime )
        System.IO.Directory.EnumerateFiles(dirName)
        |> Seq.map (fun elem -> new System.IO.FileInfo(elem))
        |> Seq.iter (fun elem -> fprintfn file "%50s %A" elem.FullName elem.LastAccessTime )
    printDirectoryInfo @"C:\" fileName
    printfn "%s" (System.IO.File.OpenText(fileName).ReadToEnd())
// </snippet6>

let Snippet7() =
// <snippet7>
   let printNumbersToFile fileName n =
      // "use" will cause the file to be closed when it
      // goes out of scope.
      use file = System.IO.File.CreateText(fileName)
      [ 1 .. n ]
      |> List.iter (fun elem -> fprintf file "%d " elem)
      fprintfn file ""
   printNumbersToFile "1to100.txt" 100
// </snippet7>

let Snippet8() =
// <snippet8>
    let rec factorial n = match n with 0 | 1 -> 1 | n -> n * (factorial (n-1))
    let lazyValue = lazy ( factorial (10) )
    // No computation occurs until the match expression executes.
    match lazyValue with
    | Lazy value -> printfn "10 factorial is %d" value
// </snippet8>
(*
10 factorial is 3628800
*)

let Snippet9() =
// <snippet9>
    let isDone = false
    printfn "Printing Boolean values: %b %b" isDone (not isDone)
    let s1,s2 = "test1", @"C:\test2"
    printfn "Printing strings (note literal printing of string with special character): %s%s" s1 s2
    let i1, i2 = -123, 1891
    printfn "Printing an integer in decimal form, with and without a width: %d %10d" i1 i2
    printfn "Printing an integer in lowercase hexadecimal: %x or 0x%x" i1 i2
    printfn "Printing as an unsigned integer: %u %u" i1 i2
    printfn "Printing an integer as uppercase hexadecimal: %X or 0x%X" i1 i2
    printfn "Printing as an octal integer: %o %o" i1 i2
    printfn "Printing in columns."
    for i in 115 .. 59 .. 1000 do
        printfn "%10d%10d%10d%10d%10d" (10100015-i) (i-100) (115+i) (99992/i) (i-8388229)
    let x1, x2 = 3.141592654, 6.022E23
    printfn "Printing floating point numbers %e %e" x1 x2
    printfn "Printing floating point numbers %E %E" x1 x2
    printfn "Printing floating point numbers %f %f" x1 x2
    printfn "Printing floating point numbers %F %F" x1 x2
    printfn "Printing floating point numbers %g %G" x1 x2
    printfn "Using the width and precision modifiers: %10.5e %10.3e" x1 x2

    printfn "Using the flags:\nZero Pad:|%010d| Plus:|%+10d |LeftJustify:|%-10d| SpacePad:|% d|" 1001 1001 1001 1001 
    printfn "zero pad   | |+- both   | |- and ' ' | |' ' and 0 | | normal "
    for i in -115 .. 17 .. 100 do
        printfn "|%010d| |%+-10d| |%- 10d| |% 010d| |%10d|" (80-i) (i+85) (100+i) (99992/i) (i-80)

    let d = 0.124M
    printfn "Decimal: %M" d

    printfn "Print as object: %O %O %O %O" 12 1.1 "test" (fun x -> x + 1)

    printfn "%A" [| 1; 2; 3 |]

    printfn "Printing from a function (no args): %t" (fun writer -> writer.WriteLine("X"))

    printfn "Printing from a function with arg: %a" (fun writer (value:int) -> writer.WriteLine("Printing {0}.", value)) 10
// </snippet9>

let Snippet10() =
// <snippet10>
    let printToString value =
        sprintf "Formatted string with value %d..." value
    printfn "%s" (printToString 109)
// </snippet10>

let Snippet11() =
// <snippet11>
    let lazyValue n = Lazy.Create (fun () ->
        let rec factorial n =
            match n with
            | 0 | 1 -> 1
            | n -> n * factorial (n - 1)
        factorial n)
    let lazyVal = lazyValue 10
    printfn "%d" (lazyVal.Force())
// </snippet11>

let Snippet12() =
// <snippet12>

    let cacheMap = new System.Collections.Generic.Dictionary<_, _>()
    cacheMap.Add(0, 1I)
    cacheMap.Add(1, 1I)

    let lazyFactorial n =
        let rec factorial n =
            if cacheMap.ContainsKey(n) then cacheMap.[n] else
            let result = new System.Numerics.BigInteger(n) * factorial (n - 1)
            cacheMap.Add(n, result)
            result
        if cacheMap.ContainsKey(n) then
            printfn "Reading factorial for %d from cache." n
            Lazy.CreateFromValue(cacheMap.[n])
        else
            printfn "Creating lazy factorial for %d." n
            Lazy.Create (fun () ->
                printfn "Evaluating lazy factorial for %d." n
                let result = factorial n
                result)

    printfn "%A" ((lazyFactorial 12).Force())
    printfn "%A" ((lazyFactorial 10).Force())
    printfn "%A" ((lazyFactorial 11).Force())
    printfn "%A" ((lazyFactorial 30).Force())
// </snippet12>

// <snippet13>
    let lazyFactorial n = Lazy.Create (fun () ->
        let rec factorial n =
            match n with
            | 0 | 1 -> 1
            | n -> n * factorial (n - 1)
        factorial n)
    let printLazy (lazyVal:Lazy<int>) =
        if (lazyVal.IsValueCreated) then
            printfn "Retrieving stored value: %d" (lazyVal.Value)
        else
            printfn "Computing value: %d" (lazyVal.Force())
    let lazyVal1 = lazyFactorial 12
    let lazyVal2 = lazyFactorial 10
    lazyVal1.Force() |> ignore
    printLazy lazyVal1
    printLazy lazyVal2
// </snippet13>

let Snippet14() =
// BigInteger.op_Explicit
// <snippet14>
    let bigint1 = bigint 10
    let bigint2 = bigint 10.0
    let bigint3 = bigint 100L
    printfn "%s %s %s" (bigint1.ToString()) (bigint2.ToString()) (bigint3.ToString())
// </snippet14>

let Snippet15() =
// BigInteger.Parse
// <snippet15>
    let bigint1 = bigint.Parse("20029374923749273947298347928374927349872")

    try
       let bigint2 = bigint.Parse("x")
       printfn "%s" (bigint2.ToString())
    with
       | :? System.FormatException as e -> printfn "Error: %s" e.Message
// </snippet15>


