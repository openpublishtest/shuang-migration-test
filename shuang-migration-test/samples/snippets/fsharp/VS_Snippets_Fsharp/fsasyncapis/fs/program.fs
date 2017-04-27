// Learn more about F# at http://fsharp.net
module Program

// Code examples for Async APIs.

// Async.Parallel
// Async.RunSynchronously

// Write 1000 permutations of a buffer out to 1000 files asynchronously.

let Snippet1() =
// <snippet1>
    let bufferData (number:int) =
        [| for count in 1 .. 1000 -> byte (count % 256) |]
        |> Array.permute (fun index -> index)

    let writeFile fileName bufferData =
        async {
          use outputFile = System.IO.File.Create(fileName)
          do! outputFile.AsyncWrite(bufferData) 
        }

    Seq.init 1000 (fun num -> bufferData num)
    |> Seq.mapi (fun num value -> writeFile ("file" + num.ToString() + ".dat") value)
    |> Async.Parallel
    |> Async.RunSynchronously
    |> ignore
// </snippet1>
    
// This example shows how to ensure that operations are performed together, such
// as a long operation and an operation that records the completion of the
// long operation.

let Snippet2() =
// <snippet2>
    let bufferData (number:int) =
        [| for i in 1 .. 1000 -> byte (i % 256) |]
        |> Array.permute (fun index -> index)

    // Create a counter as a reference cell that can be modified in parallel.
    let counter = ref 0

    // writeFileInner writes the data to an open stream
    // that represents the file. It also updates the counter.

    // The counter is locked because it will be accessed by
    // multiple asynchronous computations.

    // The counter must be updated as soon as the
    // AsyncWrite completes, in the same synchronous
    // program flow. There must not be a let! or do! between
    // the AsyncWrite call and the counter update.
    let writeFileInner (stream:System.IO.Stream) data =
        let result = stream.AsyncWrite(data)
        lock counter (fun () -> counter := !counter + 1)
        result

    // writeFile encapsulates the asynchronous write operation.
    // The do! includes both the file I/O operation and the
    // counter update in order to keep those operations
    // together.
    let writeFile fileName bufferData =
        async {
          use outputFile = System.IO.File.Create(fileName)
          do! writeFileInner outputFile bufferData
          // Updating the counter here would not be effective.
        }

    let async1 = Seq.init 1000 (fun num -> bufferData num)
                 |> Seq.mapi (fun num value ->
                     writeFile ("file" + num.ToString() + ".dat") value)
                 |> Async.Parallel
    try
        Async.RunSynchronously(async1, 100) |> ignore
    with
       | exc -> printfn "%s" exc.Message
                printfn "%d write operations completed successfully." !counter
// </snippet2>

// TODO: These examples don't really seem to demonstrate the differences between
// Async.Start and Async.StartImmediate.

// This example demonstrates Async.Start.

module Snippet31 =
// <snippet31>
    open System.Windows.Forms

    let bufferData = Array.zeroCreate<byte> 100000000

    let async1 =
         async {
           use outputFile = System.IO.File.Create("longoutput.dat")
           do! outputFile.AsyncWrite(bufferData) 
         }
      

    let form = new Form(Text = "Test Form")
    let button = new Button(Text = "Start")
    form.Controls.Add(button)
    button.Click.Add(fun args -> Async.Start(async1))
    Application.Run(form)
// </snippet31>

// This example demonstrates Async.StartImmediate.

module Snippet320 =
// <snippet320>

    open System.Windows.Forms

    let bufferData = Array.zeroCreate<byte> 100000000

    let async1 (button : Button) =
         async {
           button.Text <- "Busy"
           button.Enabled <- false
           let context = System.Threading.SynchronizationContext.Current
           do! Async.SwitchToThreadPool()
           use outputFile = System.IO.File.Create("longoutput.dat")
           do! outputFile.AsyncWrite(bufferData)
           do! Async.SwitchToContext(context)
           button.Text <- "Start"
           button.Enabled <- true
         }


    let form = new Form(Text = "Test Form")
    let button = new Button(Text = "Start")
    form.Controls.Add(button)
    button.Click.Add(fun args -> Async.StartImmediate(async1 button))
    Application.Run(form)
// </snippet320>

// This example demonstrates Async.StartAsTask.

module Snippet330 =
// <snippet330>
    open System.Windows.Forms

    let bufferData = Array.zeroCreate<byte> 100000000

    let async1 =
         async {
           use outputFile = System.IO.File.Create("longoutput.dat")
           do! outputFile.AsyncWrite(bufferData) 
         }
      

    let form = new Form(Text = "Test Form")
    let button = new Button(Text = "Start")
    form.Controls.Add(button)
    button.Click.Add(fun args -> let task = Async.StartAsTask(async1)
                                 printfn "Do some other work..."
                                 task.Wait()
                                 printfn "done")
    Application.Run(form)
// </snippet330>

// Using Async.StartChild to perform asynchronous computations
// where each calculation is a child job.

// <snippet4>
open System.Windows.Forms

let bufferData = Array.zeroCreate<byte> 100000000

let asyncChild filename =
        async {
            printfn "Child job start: %s" filename
            use outputFile = System.IO.File.Create(filename)
            do! outputFile.AsyncWrite(bufferData)
            printfn "Child job end: %s " filename
        }

let asyncParent =
        async {
            printfn "Parent job start."
            let! childAsync1 = Async.StartChild(asyncChild "longoutput1.dat")
            let! childAsync2 = Async.StartChild(asyncChild "longoutput2.dat")
            let! result1 = childAsync1
            let! result2 = childAsync2
            printfn "Parent job end."
        }
      

let form = new Form(Text = "Test Form")
let button = new Button(Text = "Start")
form.Controls.Add(button)
button.Click.Add(fun args -> Async.Start(asyncParent)
                             printfn "Completed execution." )
Application.Run(form)
// </snippet4>

// Async.StartWithContinuations. This one involves three continuations: completion, error and canceled.
// We have a form with a Start button and a Cancel button.
// Update the UI on completion, error and on cancellation.

module Snippet5 =
// <snippet5>
    open System.Windows.Forms

    let bufferData = Array.zeroCreate<byte> 100000000

    let async1 (label:System.Windows.Forms.Label) filename =
         Async.StartWithContinuations(
             async {
                label.Text <- "Operation started."
                use outputFile = System.IO.File.Create(filename)
                do! outputFile.AsyncWrite(bufferData)
                },
             (fun _ -> label.Text <- "Operation completed."),
             (fun _ -> label.Text <- "Operation failed."),
             (fun _ -> label.Text <- "Operation canceled."))
        
      

    let form = new Form(Text = "Test Form")
    let button1 = new Button(Text = "Start")
    let button2 = new Button(Text = "Start Invalid", Top = button1.Height + 10)
    let button3 = new Button(Text = "Cancel", Top = 2 * button1.Height + 20)
    let label1 = new Label(Text = "", Width = 200, Top = 3 * button1.Height + 30)
    form.Controls.AddRange [| button1; button2; button3; label1 |]
    button1.Click.Add(fun args -> async1 label1 "longoutput.dat")
    // Try an invalid filename to test the error case.
    button2.Click.Add(fun args -> async1 label1 "|invalid.dat")
    button3.Click.Add(fun args -> Async.CancelDefaultToken())
    Application.Run(form)
// </snippet5>

// Use of Async.Sleep to create a series of simulated asynchronous jobs that
// complete at specified timing.
// <snippet6>
let simulatedJob id time =
    let timestamp() = System.DateTime.Now.Ticks
    async {
       printfn "Job %d start" id
       let timestamp1 = timestamp()
       do! Async.Sleep(time * 1000)
       let timestamp2 = timestamp()
       let timespan = System.TimeSpan(timestamp2 - timestamp1)
       printfn "Job %d end %s" id (timespan.ToString("G"))
    }

[ 1 .. 10]
|> List.mapi (fun index time -> simulatedJob index time)
|> Async.Parallel
|> Async.RunSynchronously
|> ignore
// </snippet6>

// Now let's try having multiple jobs that we might want to cancel by using
// the cancellation tokens.

module Snippet7 =
// <snippet7>
    // This is a simulated cancellable computation. It checks the token source
    // to see if the cancel signal was received.
    let computation (tokenSource:System.Threading.CancellationTokenSource) =
        async {
            while (not tokenSource.IsCancellationRequested) do
                do! Async.Sleep(100)
        }

    let tokenSource1 = new System.Threading.CancellationTokenSource()
    let tokenSource2 = new System.Threading.CancellationTokenSource()
    Async.StartWithContinuations(computation tokenSource1, 
                                 (fun _ -> printfn "Computation 1 completed." ),
                                 (fun _ -> printfn "Computation 1 exception." ),
                                 (fun _ -> printfn "Computation 1 canceled." ),
                                 tokenSource1.Token)
    Async.StartWithContinuations(computation tokenSource2,
                                 (fun _ -> printfn "Computation 2 completed." ),
                                 (fun _ -> printfn "Computation 2 exception." ),
                                 (fun _ -> printfn "Computation 2 canceled." ),
                                 tokenSource2.Token)
    printfn "Started computations."
    System.Threading.Thread.Sleep(1000)
    printfn "Sending cancellation signal."
    tokenSource1.Cancel()
    tokenSource2.Cancel()

    // Wait for user input to prevent application termination.
    System.Console.ReadLine() |> ignore
    // </snippet7>

module Snippet8 =
    // <snippet8>
    // This is a simulated cancellable computation. It checks the token source
    // to see whether the cancel signal was received.
    let computation (tokenSource:System.Threading.CancellationTokenSource) =
        async {
            use! cancelHandler = Async.OnCancel(fun () -> printfn "Canceling operation.")
            // Async.Sleep checks for cancellation at the end of the sleep interval,
            // so loop over many short sleep intervals instead of sleeping
            // for a long time.
            while true do
                do! Async.Sleep(100)
        }

    let tokenSource1 = new System.Threading.CancellationTokenSource()
    let tokenSource2 = new System.Threading.CancellationTokenSource()

    Async.Start(computation tokenSource1, tokenSource1.Token)
    Async.Start(computation tokenSource2, tokenSource2.Token)
    printfn "Started computations."
    System.Threading.Thread.Sleep(1000)
    printfn "Sending cancellation signal."
    tokenSource1.Cancel()
    tokenSource2.Cancel()

    // Wait for user input to prevent application termination.
    System.Console.ReadLine() |> ignore
// </snippet8>

module Snippet9 =
// <snippet9>
    // This is a mock cancellable computation. It checks the token source
    // to see if the cancel signal was received.
    let computation (tokenSource:System.Threading.CancellationTokenSource) =
        async {
            use! cancelHandler = Async.OnCancel(fun () -> printfn "Cancelling operation.")
            // Async.Sleep checks for cancellation at the end of the sleep,
            // so loop over many short sleeps instead of sleeping for a long time.
            while true do
                do! Async.Sleep(100)
        }

    let tokenSource1 = new System.Threading.CancellationTokenSource()
    let tokenSource2 = new System.Threading.CancellationTokenSource()

    Async.StartWithContinuations(computation tokenSource1, 
                                 (fun _ -> printfn "Computation 1 completed." ),
                                 (fun _ -> printfn "Computation 1 exception." ),
                                 (fun _ -> printfn "Computation 1 canceled." ),
                                 tokenSource1.Token)
    Async.StartWithContinuations(computation tokenSource2,
                                 (fun _ -> printfn "Computation 2 completed." ),
                                 (fun _ -> printfn "Computation 2 exception." ),
                                 (fun _ -> printfn "Computation 2 canceled." ),
                                 tokenSource2.Token)
    printfn "Started computations."
    System.Threading.Thread.Sleep(1000)
    printfn "Sending cancellation signal."
    tokenSource1.Cancel()
    tokenSource2.Cancel()

    // Wait for user input to prevent application termination.
    System.Console.ReadLine() |> ignore
// </snippet9>


// Pattern 1: Use polling to check on completion
module Snippet10 =
// <snippet10>
    let computation duration =
            printfn "Sleep duration set: %d ms" duration
            let result = Async.Sleep(duration)
            printfn "Created async work item"
            result

    let invokeAndPoll computation =
        printfn "Create begin/end functions"
        let (beginInvoke, endInvoke, cancelInvoke) = Async.AsBeginEnd(computation)
        printfn "BeginInvoke"
        let result = beginInvoke(1000, null, null)
        printf "Waiting"
        while (not result.IsCompleted) do
            System.Threading.Thread.Sleep(100)
            printf "."
        printfn ""
        printfn "The operation completed"
        endInvoke(result)
        ()

    invokeAndPoll computation
// </snippet10>


// Pattern 2: Use a wait handle to block waiting for completion
module Snippet11 =
// <snippet11>
    let computation duration =
            printfn "Sleep duration set: %d ms" duration
            let result = Async.Sleep(duration)
            printfn "Created async work item"
            result

    let invokeAndWait computation duration (timeout:int) =
        printfn "Create begin/end functions"
        let (beginInvoke, endInvoke, cancelInvoke) = Async.AsBeginEnd(computation)
        printfn "BeginInvoke"
        let result = beginInvoke(duration, null, null)
        printfn "Waiting"
        match result.AsyncWaitHandle.WaitOne(timeout) with
        | true -> printfn "The operation completed."
        | false -> printfn "The operation failed to complete."
        endInvoke(result)
        ()

    // Duration of the async workflow, and timeout: in ms
    invokeAndWait computation 1000 5000
    invokeAndWait computation 10000 5000
// </snippet11>

// Pattern 3: Use a callback to wait for completion
let Snippet12 =
// <snippet12>
    let computation duration =
            printfn "Sleep duration set: %d ms" duration
            let result = Async.Sleep(duration)
            printfn "Created async work item"
            result

    let invokeAndContinue computation =
        printfn "Create begin/end functions"
        let (beginInvoke, endInvoke, cancelInvoke) = Async.AsBeginEnd(computation)
        let callBack (result: System.IAsyncResult) =
            printfn "callBack executing"
            if (result.IsCompleted) then endInvoke(result)
            printfn "callBack executed"
        printfn "BeginInvoke"
        let result = beginInvoke(10000, new System.AsyncCallback(callBack), new System.Object())
        printfn "Waiting..."
        ()

    invokeAndContinue computation
    printfn "Press any key to stop waiting."
    System.Console.ReadLine() |> ignore
// </snippet12>

// Pattern 4. Check a status object.
module Snippet13 =
// <snippet13>
    type status = {
        mutable percentDone : int
        mutable isDone : bool
        }

    let computation (statusObj: status) duration =
            // Round off the duration to a multiple of 100.
            let duration = (duration / 100) * 100
            printfn "Sleep duration set: %d ms" duration
            let result = async {
                for i in 1 .. 100 do
                    do! Async.Sleep(duration / 100)
                    statusObj.percentDone <- i
                statusObj.isDone <- true
                }
            printfn "Created async work item"
            result

    let invokeAndMonitor computation =
        printfn "Create begin/end functions"
        let statusObj = { percentDone = 0; isDone = false }
        let (beginInvoke, endInvoke, cancelInvoke) = Async.AsBeginEnd(computation statusObj)
        printfn "BeginInvoke"
        let result = beginInvoke(10000, null, statusObj)
        printfn "Waiting..."
        let mutable isDone = false
        while (not isDone) do
            System.Threading.Thread.Sleep(1000)
            let statusObj = result.AsyncState :?> status
            isDone <- statusObj.isDone
            printf "%d%% " statusObj.percentDone
        printfn ""
        printfn "The operation completed"
        endInvoke(result)
        ()

    invokeAndMonitor computation
// </snippet13>

// Scenario: use Async.AwaitEvent to wait for a signal to proceed. 
// In this case, wait for a write to complete before reading a file
module Snippet14 =
// <snippet14>
    open System.Windows.Forms
    open System.IO

    let filename = "longoutput.dat"
    if File.Exists(filename) then
        File.Delete(filename)
    let watcher = new FileSystemWatcher(Path = Directory.GetCurrentDirectory(),
                                        NotifyFilter = NotifyFilters.LastWrite,
                                        Filter = filename)
    watcher.Changed.Add(fun args -> printfn "The file %s is changed." args.Name)
    watcher.EnableRaisingEvents <- true

    let testFile = File.CreateText("Test.txt")
    testFile.WriteLine("Testing...")
    testFile.Close()

    let form = new Form(Text = "Test Form")
    let buttonSpacing = 5
    let button1 = new Button(Text = "Start")
    let button2 = new Button(Text = "Start Invalid", Top = button1.Height + buttonSpacing)
    let button3 = new Button(Text = "Cancel", Top = 2 * (button1.Height + buttonSpacing))
    let label1 = new Label(Text = "", Width = 200, Top = 3 * (button1.Height + buttonSpacing))
    let label2 = new Label(Text = "", Width = 200, Top = 4 * (button1.Height + buttonSpacing))
    form.Controls.AddRange [| button1; button2; button3; label1 |]
    form.Controls.Add(button1)

    let bufferData = Array.zeroCreate<byte> 100000000

    let async1 filename =
         async {
           printfn "Creating file %s." filename
           use outputFile = File.Create(filename)
           printfn "Attempting to write to file %s." filename
           do! outputFile.AsyncWrite(bufferData) 
         }

    let async2 filename =
         async {
           printfn "Waiting for file system watcher notification."
           // If you omit the call to AwaitEvent, an exception is thrown that indicates that the
           // file is locked.
           let! args = Async.AwaitEvent(watcher.Changed)
           printfn "Attempting to open and read file %s." filename
           use inputFile = File.OpenRead(filename)
           let! buffer = inputFile.AsyncRead(100000000)
           printfn "Successfully read file %s." filename
           return buffer
         }   
    
    button1.Click.Add(fun _ ->
                      // Start these as tasks simultaneously.
                      Async.StartAsTask(async1 filename) |> ignore
                      Async.StartAsTask(async2 filename) |> ignore
                      ())
    button2.Click.Add(fun _ ->
                      Async.StartAsTask(async1 filename) |> ignore
                      Async.StartAsTask(async2 "longoutputX.dat")  |> ignore
                      ())
    Application.Run(form)
// </snippet14>

// Next example: Async.AwaitIAsyncResult

module Snippet15 =
// <snippet15>
    open System.IO

    let streamWriter1 = File.CreateText("test1.txt")
    let count = 10000000
    let buffer = Array.init count (fun index -> byte (index % 256)) 

    printfn "Writing to file test1.txt."
    let asyncResult = streamWriter1.BaseStream.BeginWrite(buffer, 0, count, null, null)

    // Read a file, but use AwaitIAsyncResult to wait for the write operation
    // to be completed before reading.
    let readFile filename asyncResult count = 
        async {
            let! returnValue = Async.AwaitIAsyncResult(asyncResult)
            printfn "Reading from file test1.txt."
            // Close the file.
            streamWriter1.Close()
            // Now open the same file for reading.
            let streamReader1 = File.OpenText(filename)
            let! newBuffer = streamReader1.BaseStream.AsyncRead(count)
            return newBuffer
        }

    let bufferResult = readFile "test1.txt" asyncResult count
                       |> Async.RunSynchronously
// </snippet15>
    
// TODO: C# interop example with Asyncs from a C# BeginInvoke call?

// Next up: Async.AwaitTask.  With AwaitTask, you have the ability to get the
// returned data from the task you are waiting for. In this case, a file is
// created by one task, and its name is returned for the other task to open
// and read the same file.

module Snippet16 =
// <snippet16>
    open System
    open System.IO
    open System.Threading.Tasks
    open Microsoft.FSharp.Control

    let baseFilename = "file"

    let task1 = Task.Factory.StartNew(fun () ->
            let count = 100000000
            let random = new Random()
            let number = random.Next(10)
            let filename = baseFilename + number.ToString() + ".dat"
            use file = File.Create(filename)
            printfn "Writing to file %s." filename
            file.Write(Array.zeroCreate<byte> count, 0, count)
            printfn "Completed write operation on file %s." filename
            filename)
    
    let readFromFileCreatedByTask task =
        async {
            let! filename = Async.AwaitTask(task)
            use file = File.OpenRead(filename)
            printfn "Reading from file %s." filename
            let! buffer = file.AsyncRead(100000000)
            printfn "Read %d bytes from file %s." (buffer.Length) filename
        }

    Async.RunSynchronously(readFromFileCreatedByTask task1)

    printfn "Completed."
// </snippet16>

// Async.AwaitWaitHandle

module Snippet17 =
// <snippet17>
    open System.IO

    let streamWriter1 = File.CreateText("test1.txt")
    let count = 10000000
    let buffer = Array.init count (fun index -> byte (index % 256)) 

    printfn "Writing to file test1.txt."
    let asyncResult = streamWriter1.BaseStream.BeginWrite(buffer, 0, count, null, null)

    // Read a file, but use the waitHandle to wait for the write operation
    // to be completed before reading.
    let readFile filename waitHandle count = 
        async {
            let! returnValue = Async.AwaitWaitHandle(waitHandle)
            printfn "Reading from file test1.txt."
            // Close the file.
            streamWriter1.Close()
            // Now open the same file for reading.
            let streamReader1 = File.OpenText(filename)
            let! newBuffer = streamReader1.BaseStream.AsyncRead(count)
            return newBuffer
        }

    let bufferResult = readFile "test1.txt" asyncResult.AsyncWaitHandle count
                       |> Async.RunSynchronously
// </snippet17>

// Async.Catch

module Snippet18 =
// <snippet18>
    open System
    open System.IO

    let writeToFile filename numBytes = 
        async {
            use file = File.Create(filename)
            printfn "Writing to file %s." filename
            do! file.AsyncWrite(Array.zeroCreate<byte> numBytes)
        }

    let readFile filename numBytes =
        async {
            use file = File.OpenRead(filename)
            printfn "Reading from file %s." filename
            do! file.AsyncRead(numBytes) |> Async.Ignore
        }
        
    let filename = "BigFile.dat"
    let numBytes = 100000000

    let result1 = writeToFile filename numBytes
                 |> Async.Catch
                 |> Async.RunSynchronously
    match result1 with
    | Choice1Of2 _ -> printfn "Successfully wrote to file."; ()
    | Choice2Of2 exn -> 
          printfn "Exception occurred writing to file %s: %s" filename exn.Message
 
    // Start these next two operations asynchronously, forcing an exception due
    // to trying to access the file twice simultaneously.
    Async.Start(readFile filename numBytes)
    let result2 = writeToFile filename numBytes
                 |> Async.Catch
                 |> Async.RunSynchronously
    match result2 with
    | Choice1Of2 buffer -> printfn "Successfully read from file."
    | Choice2Of2 exn ->
        printfn "Exception occurred reading from file %s: %s" filename (exn.Message)
// </snippet18>

// Proof of concept.
module Snippet19 =
// <snippet19>
    let file = System.IO.File.Create("BigFile.dat")
    let buffer = Array.zeroCreate<byte> 100000000
    let asyncWrite = Async.FromBeginEnd(buffer, 0, 100000000, file.BeginWrite, file.EndWrite)
    let asyncRead = Async.FromBeginEnd(buffer, 0, 100000000, file.BeginRead, file.EndRead)

    Async.RunSynchronously(asyncWrite)
    printfn "Wrote file."
    Async.RunSynchronously(asyncRead) |> ignore
    printfn "Read file."
// </snippet19>

// Scenario: use Async.FromBeginEnd to wrap a .NET Begin/End pair as an F# async and then use that
// from within an async operation.

// This example uses the .NET socket API.
// <snippet20>
module SocketClient =

    open System.Net
    open System.Net.Sockets
    open System.Collections.Generic

    let toIList<'T> (data : 'T array) =
        let segment = new System.ArraySegment<'T>(data)
        let data = new List<System.ArraySegment<'T>>() :> IList<System.ArraySegment<'T>>
        data.Add(segment)
        data

    type Socket with
        member this.MyAcceptAsync(receiveSize) =
            Async.FromBeginEnd(receiveSize,
                               (fun (receiveSize, callback, state) ->
                                   this.BeginAccept(receiveSize, callback, state)),
                               this.EndConnect)
        member this.MyConnectAsync(ipAddress : IPAddress, port : int) =
            Async.FromBeginEnd(ipAddress, port,
                               (fun (ipAddress:IPAddress, port, callback, state) ->
                                   this.BeginConnect(ipAddress, port, callback, state)),
                               this.EndConnect)
        member this.MySendAsync(data, flags : SocketFlags) =
            Async.FromBeginEnd(toIList data, flags, 
                               (fun (data : IList<System.ArraySegment<byte>>,
                                     flags : SocketFlags, callback, state) ->
                                         this.BeginSend(data, flags, callback, state)),
                               this.EndSend)
        member this.MyReceiveAsync(data, flags : SocketFlags) =
            Async.FromBeginEnd(toIList data, flags, 
                               (fun (data : IList<System.ArraySegment<byte>>,
                                     flags : SocketFlags, callback, state) ->
                                         this.BeginReceive(data, flags, callback, state)),
                               this.EndReceive)

    let port = 11000

    let socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
    let ipHostEntry = Dns.Resolve("hostname.contoso.com")
    printfn "Server address: %s" (ipHostEntry.AddressList.[0].ToString())
    
    let connectSendReceive (socket : Socket) =
        async {
            do! socket.MyConnectAsync(ipHostEntry.AddressList.[0], 11000)
            printfn "Connected to remote host."
            let buffer1 = [| 0uy .. 255uy |]
            let buffer2 = Array.zeroCreate<byte> 255
            let flags = new SocketFlags()
            printfn "Sending data..."
            let! flag = socket.MySendAsync(buffer1, flags)
            printfn "Receiving data..."
            let! result = socket.MyReceiveAsync(buffer2, flags)
            printfn "Received data from remote host."
            return buffer2
        }

    let acceptReceiveSend (socket : Socket) =
        async {
            socket.Listen(1)
            do! socket.MyAcceptAsync(256)
            let buffer1 = Array.zeroCreate<byte> 255
            let flags = new SocketFlags()
            let! flag = socket.MyReceiveAsync(buffer1, flags)
            let buffer2 = Array.rev buffer1
            let! flag = socket.MySendAsync(buffer2, flags)
            return buffer2
        }

    let taskClient = Async.StartAsTask(connectSendReceive(socket))
    
    taskClient.Wait()
    taskClient.Result |> Array.iter (fun elem -> printf "%d " elem)
// </snippet20>
    
// Scenario: use Async.FromBeginEnd to wrap a .NET Begin/End pair as an F# async and then use that
// from within an async operation.
// <snippet200>
module SocketServer =

    open System.Net
    open System.Net.Sockets
    open System.Collections.Generic

    let toIList<'T> (data : 'T array) =
        let segment = new System.ArraySegment<'T>(data)
        let data = new List<System.ArraySegment<'T>>() :> IList<System.ArraySegment<'T>>
        data.Add(segment)
        data

    type Socket with
        member this.MyAcceptAsync() =
            Async.FromBeginEnd((fun (callback, state) -> this.BeginAccept(callback, state)),
                               this.EndAccept)
        member this.MyConnectAsync(ipAddress : IPAddress, port : int) =
            Async.FromBeginEnd(ipAddress, port,
                               (fun (ipAddress:IPAddress, port, callback, state) ->
                                   this.BeginConnect(ipAddress, port, callback, state)),
                               this.EndConnect)
        member this.MySendAsync(data : byte array, flags : SocketFlags) =
            Async.FromBeginEnd(toIList data, flags, 
                               (fun (data : IList<System.ArraySegment<byte>>,
                                     flags : SocketFlags, callback, state) ->
                                         this.BeginSend(data, flags, callback, state)),
                               this.EndSend)
        member this.MyReceiveAsync(data : byte array, flags : SocketFlags) =
            Async.FromBeginEnd(toIList data, flags, 
                               (fun (data : IList<System.ArraySegment<byte>>,
                                     flags : SocketFlags, callback, state) ->
                                         this.BeginReceive(data, flags, callback, state)),
                               this.EndReceive)

    let port = 11000

    let socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
    let ipHostInfo = Dns.Resolve(Dns.GetHostName())
    let localIPAddress = ipHostInfo.AddressList.[0]
    let localEndPoint = new IPEndPoint(localIPAddress, port)
    socket.Bind(localEndPoint)


    let connectSendReceive (socket : Socket) =
        async {
            do! socket.MyConnectAsync(ipHostInfo.AddressList.[0], 11000)
            let buffer1 = [| 0uy .. 255uy |]
            let buffer2 = Array.zeroCreate<byte> 255
            let flags = new SocketFlags()
            let! flag = socket.MySendAsync(buffer1, flags)
            let! result = socket.MyReceiveAsync(buffer2, flags)
            return buffer2
        }

    let acceptReceiveSend (socket : Socket) =
        async {
            printfn "Listening..."
            socket.Listen(10)
            printfn "Accepting..."
            let! socket = socket.MyAcceptAsync()
            
            let buffer1 = Array.zeroCreate<byte> 256
            let flags = new SocketFlags()
            printfn "Receiving..."
            let! nBytes = socket.MyReceiveAsync(buffer1, flags)
            printfn "Received %d bytes from client computer." nBytes
            let buffer2 = Array.rev buffer1
            printfn "Sending..."
            let! flag = socket.MySendAsync(buffer2, flags)
            printfn "Completed."
            return buffer2
        }

    let taskServer = Async.StartAsTask(acceptReceiveSend(socket))    
    taskServer.Wait()
    socket.Close()
// </snippet200>

// Now, Async.FromContinuations
module FromContinuations1 =
// <snippet21>
    open System
    open System.Threading

    let mutable cancelFlag = false

    let callback operation (successContinuation : unit -> unit,
                            exceptionContinuation : exn -> unit,
                            cancelContinuation : OperationCanceledException -> unit) =
        printfn "Operation running."
        try
            Async.Start(async {
                operation()
            })
            if (cancelFlag) then
                cancelContinuation(new OperationCanceledException())
            else
                successContinuation()
        with
        | exn -> exceptionContinuation(exn)
        

    let operation = 
        async {
            printfn "Operation started."
            do! Async.FromContinuations(callback (fun () -> printfn "Operation executing."
                                                            Thread.Sleep(10000)))
        }

    Async.StartWithContinuations(operation,
                                 (fun _ -> printfn "Success!"),
                                 (fun exn -> printfn "Exception: %s" (exn.Message)),
                                 (fun _ -> printfn "Operation cancelled."))
    
    Console.WriteLine("Press enter to cancel.")
    Console.ReadLine() |> ignore
    cancelFlag <- true
    printfn "Done."
    
    
    Console.WriteLine("Press enter to exit.")
    Console.ReadLine() |> ignore
// </snippet21>


module FromContinuations2 =
// <snippet22>
    open System
    open System.IO

    type Status<'T> =
    |   Success of 'T
    |   Failure of exn
    |   Cancel

    let Continue value =
        Async.FromContinuations(fun (cont, econt, ccont) ->
            match value with
            | Success result -> cont(result)
            | Failure exn -> econt(exn)
            | Cancel -> ccont(new OperationCanceledException()))
    
    let mutable cancelFlag = false

    let writeLongFileInChunks filename (data : byte array) =
        async {
            do! Async.SwitchToNewThread()
            let result =
                try
                    printfn "Opening file %s" filename
                    use file = System.IO.File.Create(filename)
                    let mutable count = 0
                    let blockSize = 20
                    while (cancelFlag = false && count < data.Length / blockSize) do
                        let slice = data.[count * blockSize .. (count + 1) * blockSize - 1]
                        Async.RunSynchronously(file.AsyncWrite slice)
                        count <- count + 1
                    if (cancelFlag = false) then
                        // Write the last data, if any.
                        if (data.Length % blockSize <> 0) then
                           let slice = data.[(count + 1) * blockSize .. data.Length - 1]
                           Async.RunSynchronously(file.AsyncWrite slice)
                        Continue (Success(()))
                    else
                        Continue Cancel
                        
                with
                | exn -> Continue (Failure(exn))
                         
            return! result
        }


    let data = Array.zeroCreate<byte> 10000000

    Async.StartWithContinuations(writeLongFileInChunks "x.data" data,
                                 (fun _ -> printfn "Success!"),
                                 (fun exn -> printfn "Exception: %s" (exn.Message)),
                                 (fun _ -> printfn "Operation cancelled."))

    Console.WriteLine("Press enter to cancel writing to file x.dat.")
    Console.ReadLine() |> ignore
    cancelFlag <- true
// </snippet22>

// Wrap event-based async as an Async<'T> using Async.FromContinuations
// <snippet23>
    open System
    open System.ComponentModel
    open System.Windows.Forms

    type BackgroundWorker with
            member this.AsyncRunWorker (computation, argument : 'T, progressChangedHandler) : Async<'U> =
                let workerAsync =
                    Async.FromContinuations (fun (cont, econt, ccont) ->
                                let handler = new RunWorkerCompletedEventHandler (fun sender args ->          
                                    if args.Cancelled then
                                        ccont (new OperationCanceledException()) 
                                    elif args.Error <> null then
                                        econt args.Error
                                    else
                                        cont (args.Result :?> 'U))
                                this.WorkerSupportsCancellation <- true;
                                this.WorkerReportsProgress <- true
                                this.DoWork.AddHandler(new DoWorkEventHandler(fun sender args ->
                                    args.Result <- computation(argument, this, args)))
                                this.ProgressChanged.AddHandler(progressChangedHandler)
                                this.RunWorkerCompleted.AddHandler(handler)
                                this.RunWorkerAsync(argument)
                            )

                async { 
                    use! holder = Async.OnCancel(fun _ -> this.CancelAsync())
                    return! workerAsync
                 }

    let factorial number =
        let rec fact number =
            match number with
            | value when value < 0I ->
                raise (InvalidOperationException(sprintf "Cannot compute the factorial of a negative number: %s." (value.ToString())))
            | value when value > 2000I ->
                raise (InvalidOperationException(sprintf "Input too large: %s" (value.ToString())))
            | value when value = 0I -> 1I
            | value when value = 1I -> 1I
            | number -> number * fact (number - 1I)
        fact number

    // Recursive isprime function.
    let isprime number =
        let rec check count =
            count > number/2 || (number % count <> 0 && check (count + 1))
        check 2

    let isprimeBigInt number =
        let rec check count =
            count > number/2I || (number % count <> 0I && check (count + 1I))
        check 2I

    let computeNthPrime (number, worker: BackgroundWorker, eventArgs: DoWorkEventArgs) =
         if (number < 1) then
             invalidOp <| sprintf "Invalid input for nth prime: %s." (number.ToString())
         let mutable count = 0
         let mutable num = 1I
         let isDone = false
         while (count < number && not eventArgs.Cancel ) do
             if (worker.CancellationPending) then
                 eventArgs.Cancel <- true
             else
                 let percentComplete = int ((float count) / (float number) * 100.0)
                 worker.ReportProgress(percentComplete, num.ToString())
             num <- num + 1I
             if (num < bigint System.Int32.MaxValue) then
                 while (not (isprime (int num))) do
                     num <- num + 1I
             else
                 while (not (isprimeBigInt num)) do
                     num <- num + 1I
             count <- count + 1
         num
             
    let async1 (progressBar:ProgressBar) (label:Label) value =
         let worker = new BackgroundWorker()
         label.Text <- "Computing..."
         let computation value = worker.AsyncRunWorker(computeNthPrime, value,
                                                       (fun sender eventArgs ->
                                                           label.Text <- "Scanning ... " + eventArgs.UserState.ToString()
                                                           progressBar.Value <- eventArgs.ProgressPercentage ))
         Async.StartWithContinuations(
             computation value,
             (fun result -> label.Text <- sprintf "Result: %s" (result.ToString())),
             (fun exn -> label.Text <- "Operation failed with error:" + exn.Message),
             (fun _ -> label.Text <- "Operation canceled."))

    let form = new Form(Text = "Test Form", Width = 400, Height = 400)
    let panel1 = new Panel(Dock = DockStyle.Fill)
    panel1.DockPadding.All <- 10
    let spacing = 5
    let button1 = new Button(Text = "Start")
    let button2 = new Button(Text = "Start Invalid", Top = button1.Height + spacing)
    let button3 = new Button(Text = "Cancel", Top = 2 * (button1.Height + spacing))
    let updown1 = new System.Windows.Forms.NumericUpDown(Top = 3 * (button1.Height + spacing), 
                                                         Value = 20m,
                                                         Minimum = 0m,
                                                         Maximum = 1000000m)
    let label1 = new Label (Text = "", Top = 4 * (button1.Height + spacing),
                            Width = 300, Height = 2 * button1.Height)
    let progressBar = new ProgressBar(Top = 6 * (button1.Height + spacing),
                                      Width = 300)
    panel1.Controls.AddRange [| button1; button2; button3; updown1; label1; progressBar; |]
    form.Controls.Add(panel1)
    button1.Click.Add(fun args -> async1 progressBar label1 (int updown1.Value))
    button2.Click.Add(fun args -> async1 progressBar label1 (int (-updown1.Value)))
    button3.Click.Add(fun args -> Async.CancelDefaultToken())
    Application.Run(form)
// </snippet23>

module AsyncIgnore =
// <snippet24>
    let bufferData number =
        [| for count in 1 .. 1000 -> byte (count % 256) |]
        |> Array.permute (fun num -> num)

    let writeFile fileName bufferData =
        async {
            use outputFile = System.IO.File.Create(fileName)
            do! outputFile.AsyncWrite(bufferData) 
        }

    Seq.init 1000 (fun num -> bufferData num)
    |> Seq.mapi (fun num value -> writeFile ("file" + num.ToString() + ".dat") value)
    |> Async.Parallel
    |> Async.Ignore
    |> Async.RunSynchronously
// </snippet24>

// Demonstrate UI thread best practices. Switch to the UI thread to update the UI.

module Snippet25 =
// <snippet25>
    open System.Windows.Forms

    let form = new Form(Text = "Test Form", Width = 400, Height = 400)
    let syncContext = System.Threading.SynchronizationContext()
    let button1 = new Button(Text = "Start")
    let label1 = new Label(Text = "", Height = 200, Width = 200,
                           Top = button1.Height + 10)
    form.Controls.AddRange([| button1; label1 |] )

    let async1(syncContext, form : System.Windows.Forms.Form) =
        async {
            let label1 = form.Controls.[1]
            // Do something.
            do! Async.Sleep(10000)
            let threadName = System.Threading.Thread.CurrentThread.Name
            let threadNumber = System.Threading.Thread.CurrentThread.ManagedThreadId
            label1.Text <- label1.Text + sprintf "Something [%s] [%d]" threadName threadNumber
        
            // Switch to the UI thread and update the UI.
            do! Async.SwitchToContext(syncContext)
            let threadName = System.Threading.Thread.CurrentThread.Name
            let threadNumber = System.Threading.Thread.CurrentThread.ManagedThreadId
            label1.Text <- label1.Text + sprintf "Here [%s] [%d]" threadName threadNumber

            // Switch back to the thread pool.
            do! Async.SwitchToThreadPool()
            // Do something.
            do! Async.Sleep(10000)
            let threadName = System.Threading.Thread.CurrentThread.Name
            let threadNumber = System.Threading.Thread.CurrentThread.ManagedThreadId
            label1.Text <- label1.Text +
                           sprintf "Switched to thread pool [%s] [%d]" threadName threadNumber
        }

    let buttonClick(sender:obj, args) =
        let button = sender :?> Button
        Async.Start(async1(syncContext, button.Parent :?> Form))
        let threadName = System.Threading.Thread.CurrentThread.Name
        let threadNumber = System.Threading.Thread.CurrentThread.ManagedThreadId
        button.Parent.Text <- sprintf "Started asynchronous workflow [%s] [%d]" threadName threadNumber
        ()

    button1.Click.AddHandler(fun sender args -> buttonClick(sender, args))
    Application.Run(form)
// </snippet25>


// Using Async.FromBeginEnd with any .NET method!!
// <snippet26>
open System

let GetDirectoriesAsync path =  
    let fn = new Func<_, _>(System.IO.Directory.GetDirectories) 
    Async.FromBeginEnd(path, fn.BeginInvoke, fn.EndInvoke)
// </snippet26>

// Experiment with SwitchToNewThread wrt Start vs StartImmediate.
module Snippet27 =
// <snippet27>
    open System

    let async1 = async {
        do! Async.SwitchToNewThread()
        for i in 1 .. 10 do
           printfn "%d" i
           do! Async.Sleep(1000)
        }

    Async.StartImmediate(async1)
    printfn "Got to here"

    Console.WriteLine("Press a key")
    Console.ReadLine() |> ignore
// </snippet27>


// Scenario:
// Build an async primitive from a regular .NET method.

module AsyncPrimitive =
// <snippet28>
    open System
    open System.IO

    let asyncMethod f = 
        async {  
            do! Async.SwitchToNewThread() 
            let result = f() 
            do! Async.SwitchToThreadPool() 
            return result
        } 

    let GetFilesAsync(path) = asyncMethod (fun () -> Directory.GetFiles(path))

    let listFiles path =
        async {
            let! files = GetFilesAsync(path)
            Array.iter (fun elem -> printfn "%s" elem) files
        }

    printfn "Here we go..."
    // The output is interleaved, which shows that these are both 
    // running simultaneously.
    Async.Start(listFiles ".")
    Async.Start(listFiles ".")
    Console.WriteLine("Press a key to continue...")
    Console.ReadLine() |> ignore
// </snippet28>

module DrawingDemo =
// <snippet29>
    open System.Windows.Forms
    open System.Drawing

    let form = new Form(Text = "Test Form", Width = 400, Height = 400)
    let syncContext = System.Threading.SynchronizationContext()

    let pen = Pens.Black
    let graphics = form.CreateGraphics()   

    let mutable x0 = 0
    let mutable y0 = 0
    let mutable isDrawing = false

    form.MouseMove.AddHandler(fun _ mouseEventArgs ->
        if (isDrawing) then
            graphics.DrawLine(pen, x0, y0, mouseEventArgs.X, mouseEventArgs.Y)
        x0 <- mouseEventArgs.X
        y0 <- mouseEventArgs.Y)

    form.MouseDown.AddHandler(fun _ _ -> isDrawing <- true)

    form.MouseUp.AddHandler(fun _ _ -> isDrawing <- false)

    Application.Run(form)
// </snippet29>

module AsyncTryCancelExample =
// <snippet30>
    open System
    open System.Windows.Forms

    let form = new Form(Text = "Test Form", Width = 400, Height = 400)
    let panel1 = new Panel(Dock = DockStyle.Fill)
    panel1.DockPadding.All <- 10
    let spacing = 5
    let startAsyncButton = new Button(Text = "Start", Enabled = true)
    let controlHeight = startAsyncButton.Height
    let button2 = new Button(Text = "Start Invalid", Top = controlHeight + spacing)
    let cancelAsyncButton = new Button(Text = "Cancel",
                                       Top = 2 * (controlHeight + spacing),
                                       Enabled = false)
    let updown1 = new System.Windows.Forms.NumericUpDown(Top = 3 * (controlHeight + spacing), 
                                                         Value = 20m, Minimum = 0m,
                                                         Maximum = 1000000m)
    let label1 = new Label (Text = "", Top = 4 * (controlHeight + spacing),
                            Width = 300, Height = 2 * controlHeight)
    let progressBar = new ProgressBar(Top = 6 * (controlHeight + spacing),
                                      Width = 300)
    panel1.Controls.AddRange [| startAsyncButton; button2; cancelAsyncButton;
                                updown1; label1; progressBar; |]
    form.Controls.Add(panel1)

    // Recursive isprime function.
    let isprime number =
        let rec check count =
            count > number/2 || (number % count <> 0 && check (count + 1))
        check 2

    let isprimeBigInt number =
        let rec check count =
            count > number/2I || (number % count <> 0I && check (count + 1I))
        check 2I

    let computeNthPrime (number) =
         if (number < 1) then
             invalidOp <| sprintf "Invalid input for nth prime: %s." (number.ToString())
         let mutable count = 0
         let mutable num = 1I
         let isDone = false
         while (count < number) do
             num <- num + 1I
             if (num < bigint System.Int32.MaxValue) then
                 while (not (isprime (int num))) do
                     num <- num + 1I
             else
                 while (not (isprimeBigInt num)) do
                     num <- num + 1I
             count <- count + 1
         num

    let async1 context value =
        let asyncTryWith =
            async {
                        try
                            let nthPrime = ref 0I
                            for count in 1 .. value - 1 do
                                // The cancellation check is implicit and
                                // cooperative at for!, do!, and so on.
                                nthPrime := computeNthPrime(count)
                                // Report progress as a percentage of the total task.
                                let percentComplete = (int)((float)count /
                                                            (float)value * 100.0)
                                do! Async.SwitchToContext(context)
                                progressBar.Value <- percentComplete
                                do! Async.SwitchToThreadPool()
                            // Handle the case in which the operation succeeds.
                            do! Async.SwitchToContext(context)
                            label1.Text <- sprintf "%s" ((!nthPrime).ToString())
                        with 
                            | e -> 
                                // Handle the case in which an exception is thrown.
                                do! Async.SwitchToContext(context)
                                MessageBox.Show(e.Message) |> ignore
                    }
        async {
            try
                do! Async.TryCancelled(asyncTryWith,
                                       (fun oce -> 
                                          // Handle the case in which the user cancels the operation.
                                          context.Post((fun _ ->
                                              label1.Text <- "Canceled"), null)))
            finally 
                context.Post((fun _ ->
                    updown1.Enabled <- true
                    startAsyncButton.Enabled <- true
                    cancelAsyncButton.Enabled <- false),
                    null)
        }

    startAsyncButton.Click.Add(fun args -> 
        cancelAsyncButton.Enabled <- true
        let context = System.Threading.SynchronizationContext.Current
        Async.Start(async1 context (int updown1.Value)))
    button2.Click.Add(fun args ->
       let context = System.Threading.SynchronizationContext.Current
       Async.Start(async1 context (int (-updown1.Value))))
    cancelAsyncButton.Click.Add(fun args -> Async.CancelDefaultToken())
    Application.Run(form)
// </snippet30>

let Snippet1X() =
// <snippet32>
    let bufferData (number:int) =
        [| for count in 1 .. 1000 -> byte (count % 256) |]
        |> Array.permute (fun index -> index)

    let writeFiles bufferData =
        Seq.init 1000 (fun num -> bufferData num)
        |> Seq.mapi (fun num value ->
            async {
                let fileName = "file" + num.ToString() + ".dat"
                use outputFile = System.IO.File.Create(fileName)
                do! outputFile.AsyncWrite(value)
            })
        |> Async.Parallel
        |> Async.Ignore

    writeFiles bufferData
    |> Async.Start
// </snippet32>

module Snippet33 =
    // <snippet33>
    let generateInfiniteSequence fDenominator isAlternating =
        if (isAlternating) then
            Seq.initInfinite (fun index -> 1.0 /(fDenominator index) * (if (index % 2 = 0) then -1.0 else 1.0))
        else
            Seq.initInfinite (fun index -> 1.0 /(fDenominator index))

    // This is the series of recipocals of the squares.
    let squaresSeries = generateInfiniteSequence (fun index -> float (index * index)) false

    // This function sums a sequence, up to the specified number of terms.
    let sumSeq length sequence =
        Seq.unfold (fun state ->
            let subtotal = snd state + Seq.nth (fst state + 1) sequence
            if (fst state >= length) then None
            else Some(subtotal, (fst state + 1, subtotal))) (0, 0.0)

    // This function sums an infinite sequence up to a given value
    // for the difference (epsilon) between subsequent terms,
    // up to a maximum number of terms, whichever is reached first.
    let infiniteSum infiniteSeq epsilon maxIteration =
        infiniteSeq
        |> sumSeq maxIteration
        |> Seq.pairwise
        |> Seq.takeWhile (fun elem -> abs (snd elem - fst elem) > epsilon)
        |> List.ofSeq
        |> List.rev
        |> List.head
        |> snd

    let pi = System.Math.PI

    // Because this is not an alternating series, a much smaller epsilon
    // value and more terms are needed to obtain an accurate result.
    let computeSum epsilon =
        let maxTerms =  10000000
        async {
            let result = infiniteSum squaresSeries epsilon maxTerms
            printfn "Result: %f pi*pi/6: %f" result (pi*pi/6.0) 
        }

    // Start the computation on a new thread.
    Async.Start(computeSum 1.0e-8)

    // Because the computation is running on a separate thread,
    // this message is printed to the console without any delay.
    printfn "Computing. Press any key to stop waiting."
    System.Console.ReadLine() |> ignore
    // </snippet33>


module SnippetX =
// <snippet34>
    open System
    open System.IO

    let writeToFile filename numBytes = 
        async {
            use file = File.Create(filename)
            printfn "Writing to file %s." filename
            do! file.AsyncWrite(Array.zeroCreate<byte> numBytes)
        }

    let readFile filename numBytes =
        async {
            use file = File.OpenRead(filename)
            printfn "Reading from file %s." filename
            // Throw away the data being read.
            do! file.AsyncRead(numBytes) |> Async.Ignore
        }
        
    let filename = "BigFile.dat"
    let numBytes = 100000000

    writeToFile filename numBytes
    |> Async.RunSynchronously

    readFile filename numBytes
    |> Async.RunSynchronously
// </snippet34>

