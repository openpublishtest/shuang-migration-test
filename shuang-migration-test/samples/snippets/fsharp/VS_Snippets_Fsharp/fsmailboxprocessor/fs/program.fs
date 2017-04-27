// Learn more about F# at http://fsharp.net
module Module1 =
// <snippet1>
    open System
    open Microsoft.FSharp.Control

    type Message =
        {
            id : int
            contents : string
        }
        static member count = 0
        static member CreateMessage(content) =
            { id = Message.count; contents = content }

    let mailbox = new MailboxProcessor<Message>(fun mailbox ->
        printfn "Message received."
        Async.Sleep(10000))

    mailbox.Start()
    mailbox.Post(Message.CreateMessage("123"))
    Async.Start(async { do! mailbox.PostAndAsyncReply(fun _ ->
                                printfn "Reply received via PostAndAsyncReply."
                                Message.CreateMessage("ABC")) })

    Async.Start(async { do! mailbox.PostAndReply(fun _ ->
        printfn "Reply received via PostAndReply."
        Message.CreateMessage("DEF")) })

    Async.Start(async { 
        let! resultOption =  mailbox.PostAndTryAsyncReply(fun _ ->
                       printfn "Reply received via PostAndTryAsyncReply."
                       Message.CreateMessage("XYZ"))
        match resultOption with
        | Some value -> ()
        | None -> printfn "Operation timed out."
        } )

    Async.Start(
            async {
                while (mailbox.CurrentQueueLength > 0) do
                    let! message = mailbox.Receive()
                    // process/respond to message.
                    printfn "Message received: ID = %d Message = %s" message.id message.contents
            }
    )
// </snippet1>

module Snippet2 =

//Example: basic use of MailboxProcessor 
// <snippet2>
    open System
    open Microsoft.FSharp.Control

    type Message(id, contents) =
        static let mutable count = 0
        member this.ID = id
        member this.Contents = contents
        static member CreateMessage(contents) =
            count <- count + 1
            Message(count, contents)

    let mailbox = new MailboxProcessor<Message>(fun inbox ->
        let rec loop count =
            async { printfn "Message count = %d. Waiting for next message." count
                    let! msg = inbox.Receive()
                    printfn "Message received. ID: %d Contents: %s" msg.ID msg.Contents
                    return! loop( count + 1) }
        loop 0)

    mailbox.Start()

    mailbox.Post(Message.CreateMessage("ABC"))
    mailbox.Post(Message.CreateMessage("XYZ"))


    Console.WriteLine("Press any key...")
    Console.ReadLine() |> ignore
// </snippet2>

// other scenarios with MailboxProcessor.

// Market Maker.
// Represents a server agent.
// Sets bid and ask prices for an asset.
// Accepts incoming buy/sell requests.
// Accepts requests for bid/ask prices.
// Notifies clients that sales are completed.
// Clients buy or sell x numbers of shares, query bid/ask prices.
module Snippet3 =
// <snippet3>
    open System

    type AssetCode = string

    type Asset(code, bid, ask, initialQuantity) =
        let mutable quantity = initialQuantity
        member this.AssetCode = code
        member this.Bid = bid
        member this.Ask = ask
        member this.Quantity with get() = quantity and set(value) = quantity <- value


    type OrderType =
        | Buy of AssetCode * int
        | Sell of AssetCode * int

    type Message =
        | Query of AssetCode * AsyncReplyChannel<Reply>
        | Order of OrderType * AsyncReplyChannel<Reply>
    and Reply =
        | Failure of string
        | Info of Asset
        | Notify of OrderType

    let assets = [| new Asset("AAA", 10.0, 10.05, 1000000);
                    new Asset("BBB", 20.0, 20.10, 1000000);
                    new Asset("CCC", 30.0, 30.15, 1000000) |]

    let codeAssetMap = assets
                       |> Array.map (fun asset -> (asset.AssetCode, asset))
                       |> Map.ofArray

    let mutable totalCash = 00.00
    let minCash = -1000000000.0
    let maxTransaction = 1000000.0

    let marketMaker = new MailboxProcessor<Message>(fun inbox ->
        let rec Loop() =
            async {
                let! message = inbox.Receive()
                match message with
                | Query(assetCode, replyChannel) ->
                    match (Map.tryFind assetCode codeAssetMap) with
                    | Some asset ->
                        printfn "Replying with Info for %s" (asset.AssetCode)
                        replyChannel.Reply(Info(asset))
                    | None -> replyChannel.Reply(Failure("Asset code not found."))
                | Order(order, replyChannel) ->
                    match order with
                    | Buy(assetCode, quantity) ->
                        match (Map.tryFind assetCode codeAssetMap) with
                        | Some asset ->
                            if (quantity < asset.Quantity) then
                                asset.Quantity <- asset.Quantity - quantity
                                totalCash <- totalCash + float quantity * asset.Ask
                                printfn "Replying with Notification:\nBought %d units of %s at price $%f. Total purchase $%f."
                                        quantity asset.AssetCode asset.Ask (asset.Ask * float quantity)
                                printfn "Marketmaker balance: $%10.2f" totalCash
                                replyChannel.Reply(Notify(Buy(asset.AssetCode, quantity)))
                            else
                                printfn "Insufficient shares to fulfill order for %d units of %s."
                                        quantity asset.AssetCode
                                replyChannel.Reply(Failure("Insufficient shares to fulfill order."))
                        | None -> replyChannel.Reply(Failure("Asset code not found."))
                    | Sell(assetCode, quantity) ->
                        match (Map.tryFind assetCode codeAssetMap) with
                        | Some asset ->
                            if (float quantity * asset.Bid <= maxTransaction && totalCash - float quantity * asset.Bid > minCash) then
                                asset.Quantity <- asset.Quantity + quantity
                                totalCash <- totalCash - float quantity * asset.Bid
                                printfn "Replying with Notification:\nSold %d units of %s at price $%f. Total sale $%f."
                                        quantity asset.AssetCode asset.Bid (asset.Bid * float quantity)
                                printfn "Marketmaker balance: $%10.2f" totalCash
                                replyChannel.Reply(Notify(Sell(asset.AssetCode, quantity)))
                            else
                                printfn "Insufficient cash to fulfill order for %d units of %s."
                                        quantity asset.AssetCode
                                replyChannel.Reply(Failure("Insufficient cash to cover order."))
                        | None -> replyChannel.Reply(Failure("Asset code not found."))
                do! Loop()
            }
        Loop())

    marketMaker.Start()

    // Query price.
    let reply1 = marketMaker.PostAndReply(fun replyChannel -> 
        printfn "Posting message for AAA"
        Query("AAA", replyChannel))
        
    // Test Buy Order.
    let reply2 = marketMaker.PostAndReply(fun replyChannel -> 
        printfn "Posting message for BBB"
        Order(Buy("BBB", 100), replyChannel))

    // Test Sell Order.
    let reply3 = marketMaker.PostAndReply(fun replyChannel -> 
        printfn "Posting message for CCC"
        Order(Sell("CCC", 100), replyChannel))

    // Test incorrect code.
    let reply4 = marketMaker.PostAndReply(fun replyChannel -> 
        printfn "Posting message for WrongCode"
        Order(Buy("WrongCode", 100), replyChannel))

    // Test too large a number of shares.

    let reply5 = marketMaker.PostAndReply(fun replyChannel ->
        printfn "Posting message with large number of shares of AAA."
        Order(Buy("AAA", 1000000000), replyChannel))

    // Too large an amount of money for one transaction.

    let reply6 = marketMaker.PostAndReply(fun replyChannel ->
        printfn "Posting message with too large of a monetary amount."
        Order(Sell("AAA", 100000000), replyChannel))

    let random = new Random()
    let nextTransaction() =
        let buyOrSell = random.Next(2)
        let asset = assets.[random.Next(3)]
        let quantity = Array.init 3 (fun _ -> random.Next(1000)) |> Array.sum
        match buyOrSell with
        | n when n % 2 = 0 -> Buy(asset.AssetCode, quantity)
        | _ -> Sell(asset.AssetCode, quantity)

    let simulateOne() =
       async {
           let! reply = marketMaker.PostAndAsyncReply(fun replyChannel ->
               let transaction = nextTransaction()
               match transaction with
               | Buy(assetCode, quantity) -> printfn "Posting BUY %s %d." assetCode quantity
               | Sell(assetCode, quantity) -> printfn "Posting SELL %s %d." assetCode quantity
               Order(transaction, replyChannel))
           printfn "%s" (reply.ToString())
        }

    let simulate =
        async {
            while (true) do
                do! simulateOne()
                // Insert a delay so that you can see the results more easily.
                do! Async.Sleep(1000)
        }

    Async.Start(simulate)

    Console.WriteLine("Press any key...")
    Console.ReadLine() |> ignore
// </snippet3>


// this one doesnt work as expected, the exception is thrown but I don't think
// the exception handler ever executes.
module Example1 =
// <snippet4>
    open System
    type Message = string

    let agent = MailboxProcessor<Message>.Start(fun inbox ->
        let rec loop n =
            async {
                let! message = inbox.Receive();
                if (message = "Stop") then
                    raise (new Exception("Stop command received."))
                    ()
                else
                   printfn "Message number %d. Message contents: %s" n message
                   do! loop (n+1)
            }
        loop 0)

    printfn "Mailbox Processor Test"
    printfn "Type some text and press Enter to submit a message."
    printfn "Type 'Stop' to close the program."

    try
        while true do
            System.Console.ReadLine() |> agent.Post
    with
        | exn -> printfn "%s" exn.Message
//</snippet4>
        
        
module Example2a =
//<snippet5>
    open System

    type Message = string

    let agent = MailboxProcessor<Message>.Start(fun inbox ->
        let rec loop n =
            async {
                try
                    let! message = inbox.Receive(10000);
                    printfn "Message number %d. Message contents: %s" n message
                    do! loop (n+1)
                with
                    | :? TimeoutException -> printfn "Mailbox processor timeout exceeded."

            }
        loop 0)

    printfn "Mailbox Processor Test"
    printfn "Type some text and press Enter to submit a message."
    printfn "Type 'Stop' to close the program."


    // This one never breaks out of the loop.
    while true do
       Console.ReadLine() |> agent.Post
// </snippet5>
       
 
// This one doesn't work well since the timeout message is never printed.
module Example2b =
// <snippet6>
    open System

    type Message = string

    let agent = MailboxProcessor<Message>.Start(fun inbox ->
        let rec loop n =
            async {
                    let! message = inbox.Receive(1000);
                    printfn "Message number %d. Message contents: %s" n message
                    do! loop (n+1)
            }
        loop 0)

    printfn "Mailbox Processor Test"
    printfn "Type some text and press Enter to submit a message."
    printfn "Type 'Stop' to close the program."

    try
       while true do
           Console.ReadLine() |> agent.Post
    with
       | :? TimeoutException -> printfn "Mailbox processor timeout exceeded."
// </snippet6>
       
module MailboxProcessorTest1 =
//<snippet7>
    open System

    type Message = string * AsyncReplyChannel<string>

    let formatString = "Message number {0} was received. Message contents: {1}"

    let printThreadId note =

        // Append the thread ID.
        printfn "%d : %s" System.Threading.Thread.CurrentThread.ManagedThreadId note


    let agent = MailboxProcessor<Message>.Start(fun inbox ->
        let rec loop n =
            async {
                    let! (message, replyChannel) = inbox.Receive();
                    printThreadId "MailboxProcessor"
                    if (message = "Stop") then
                        replyChannel.Reply("Stopping.")
                    else
                        replyChannel.Reply(String.Format(formatString, n, message))
                    do! loop (n + 1)
            }
        loop 0)

    printfn "Mailbox Processor Test"
    printfn "Type some text and press Enter to submit a message."
    printfn "Type 'Stop' to close the program."

    let rec loop() =
        printf "> "
        let input = Console.ReadLine()
        printThreadId("Console loop")
        let reply = agent.PostAndReply(fun replyChannel -> input, replyChannel)
        if (reply <> "Stopping.") then
            printfn "Reply: %s" reply
            loop()
        else
            ()
    loop()

    printfn "Press Enter to continue."
    Console.ReadLine() |> ignore
// </snippet7>
    
module MailboxProcessorTest2 =
// <snippet8>
    open System

    type Message = string * AsyncReplyChannel<string>

    let formatString = "Message number {0} was received. Message contents: {1}"

    let agent = MailboxProcessor<Message>.Start(fun inbox ->
        let rec loop n =
            async {
                    let! (message, replyChannel) = inbox.Receive();
                    if (message = "Stop") then
                        replyChannel.Reply("Stopping.")
                    else   
                        replyChannel.Reply(String.Format(formatString, n, message))
                    do! loop (n + 1)
            }
        loop 0)

    printfn "Mailbox Processor Test"
    printfn "Type some text and press Enter to submit a message."
    printfn "Type 'Stop' to close the program."


    let rec loop() =
        printf "> "
        let input = Console.ReadLine()
        let reply = agent.PostAndReply(fun replyChannel -> input, replyChannel)
        if (reply <> "Stopping.") then
            printfn "Reply: %s" reply
            loop()
        else
            ()
    loop()
               
    
    printfn "Press Enter to continue."
    Console.ReadLine() |> ignore
// </snippet8>    
    

 //  Timeout works but doesn't exit completely.
module Example5 =
// <snippet9>
    open System

    type Message = string * AsyncReplyChannel<string>

    let formatString = "Message number {0} was received. Message contents: {1}"

    let agent = MailboxProcessor<Message>.Start(fun inbox ->
        let rec loop n =
            async {            
                try
                    let! (message, replyChannel) = inbox.Receive(10000);
                    
                    if (message = "Stop") then
                        replyChannel.Reply("Stop")
                    else
                        replyChannel.Reply(String.Format(formatString, n, message))
                    do! loop (n + 1)
                
                with
                | :? TimeoutException -> 
                    printfn "The mailbox processor timed out."
            }
        loop (0))

    printfn "Mailbox Processor Test"
    printfn "Type some text and press Enter to submit a message."
    printfn "Type 'Stop' to close the program."


    let rec loop() =
        printf "> "
        let input = Console.ReadLine()
        let reply = agent.PostAndReply(fun replyChannel -> input, replyChannel)
        if (reply <> "Stop") then
            printfn "Reply: %s" reply
            loop()
        else
            ()
    loop()
               


    printfn "Press Enter to continue."
    Console.ReadLine() |> ignore
// </snippet9>


module Example6 =
// <snippet10>
    open System

    type Message = string * AsyncReplyChannel<string>

    let formatString = "Message number {0} was received. Message contents: {1}"
    
    let agent = MailboxProcessor<Message>.Start(fun inbox ->
        let rec loop n =
            async {            
                try
                    let! (message, replyChannel) = inbox.Receive(10000);
                    
                    if (message = "Stop") then
                        replyChannel.Reply("Stop")
                    else
                        replyChannel.Reply(String.Format(formatString, n, message))
                    do! loop (n + 1)
                
                with
                | :? TimeoutException -> 
                    printfn "The mailbox processor timed out."
                    do! loop (n + 1)
            }
        loop (0))

    printfn "Mailbox Processor Test"
    printfn "Type some text and press Enter to submit a message."
    printfn "Type 'Stop' to close the program."

    
    let rec loop() =
        printf "> "
        let input = Console.ReadLine()
        let reply = agent.PostAndReply(fun replyChannel -> input, replyChannel)
        if (reply <> "Stop") then
            printfn "Reply: %s" reply
            loop()
        else
            ()
    loop()

    printfn "Press Enter to continue."
    Console.ReadLine() |> ignore
// </snippet10>


// PostAndAsyncReply with no timeout
module Example8 =
// <snippet11>
    open System

    type Message = string * AsyncReplyChannel<string>

    let formatString = "Message number {0} was received. Message contents: {1}"

    let agent = MailboxProcessor<Message>.Start(fun inbox ->
        let rec loop n =
            async {                 
                    let! (message, replyChannel) = inbox.Receive();
                    
                    if (message = "Stop") then
                        replyChannel.Reply("Stop")
                    else
                        replyChannel.Reply(String.Format(formatString, n, message))
                    do! loop (n + 1)
            }
        loop (0))
    
    printfn "Mailbox Processor Test"
    printfn "Type some text and press Enter to submit a message."
    printfn "Type 'Stop' to close the program."


    let rec loop() =
        printf "> "
        let input = Console.ReadLine()
        let replyAsync = agent.PostAndAsyncReply(fun replyChannel -> input, replyChannel)
        // Wait for reply.
        let reply = Async.RunSynchronously(replyAsync);
        if (reply <> "Stop") then
            printfn "Reply: %s" reply
            loop()
        else
            ()
    loop()
         
    printfn "Press Enter to continue."
    Console.ReadLine() |> ignore
// </snippet11>
// PostAndAsyncReply with timeout
module Example9 =
// <snippet12>
    open System

    type Message = string * AsyncReplyChannel<string>

    let formatString = "Message number {0} was received. Message contents: {1}"

    let agent = MailboxProcessor<Message>.Start(fun inbox ->
        let rec loop n =
            async {            
                try
                    let! (message, replyChannel) = inbox.Receive(10000);
                    
                    if (message = "Stop") then
                        replyChannel.Reply("Stop")
                    else
                        replyChannel.Reply(String.Format(formatString, n, message))
                    do! loop (n + 1)
                
                with
                | :? TimeoutException -> 
                    printfn "The mailbox processor timed out."
            }
        loop (0))

    printfn "Mailbox Processor Test"
    printfn "Type some text and press Enter to submit a message."
    printfn "Type 'Stop' to close the program."

    let rec loop() =
        printf "> "
        let input = Console.ReadLine()
        let replyAsync = agent.PostAndAsyncReply(fun replyChannel -> input, replyChannel)
        // Wait for reply.
        let reply = Async.RunSynchronously(replyAsync);
        if (reply <> "Stop") then
            printfn "Reply: %s" reply
            loop()
        else
            ()
    loop()

    printfn "Press Enter to continue."
    Console.ReadLine() |> ignore
// </snippet12>

// PostAndAsyncReply with task

module Example10 =
// <snippet13>
    open System

    type Message = string * AsyncReplyChannel<string>

    let formatString = "Message number {0} was received. Message contents: {1}"

    let agent = MailboxProcessor<Message>.Start(fun inbox ->
        let rec loop n =
            async {            
                    let! (message, replyChannel) = inbox.Receive();
                    
                    if (message = "Stop") then
                        replyChannel.Reply("Stop")
                    else
                        replyChannel.Reply(String.Format(formatString, n, message))
                    do! loop (n + 1)
            }
        loop (0))

    printfn "Mailbox Processor Test"
    printfn "Type some text and press Enter to submit a message."
    printfn "Type 'Stop' to close the program."
    
    let isCompleted = false
    let mutable tasks = []
    while (not isCompleted) do
        printf "> "
        let input = Console.ReadLine()
        let replyAsync = agent.PostAndAsyncReply(fun replyChannel -> input, replyChannel)
        // Wait for reply.
        let task = Async.StartAsTask(replyAsync);
        // Add to task collection.
        tasks <- task :: tasks
        // Check tasks for completion
        // and display the results of completed tasks.
        let mutable newTaskList = []
        for task in tasks do
            if (task.IsCompleted) then
                let result = task.Result
                printfn "Task result: %s" result
            else
                newTaskList <- task :: newTaskList
        tasks <- newTaskList

    printfn "Press Enter to continue."
    Console.ReadLine() |> ignore
// </snippet13>

// Print the reply as a continuation
module Example11 =
// <snippet14>
    open System

    type Message = string * AsyncReplyChannel<string>

    let formatString = "Message number {0} was received. Message contents: {1}"

    let agent = MailboxProcessor<Message>.Start(fun inbox ->
        let rec loop n =
            async {            
                    let! (message, replyChannel) = inbox.Receive();
                    // Delay so that the responses come in a different order.
                    do! Async.Sleep( 5000 - 1000 * n);
                    replyChannel.Reply(String.Format(formatString, n, message))
                    do! loop (n + 1)
            }
        loop (0))

    printfn "Mailbox Processor Test"
    printfn "Type some text and press Enter to submit a message."
        
    let isCompleted = false
    while (not isCompleted) do
        printf "> "
        let input = Console.ReadLine()
        let messageAsync = agent.PostAndAsyncReply(fun replyChannel -> input, replyChannel)

        // Set up a continuation function (the first argument below) that prints the reply.
        // The second argument is the exception continuation (not used).
        // The third argument is the cancellation continuation (not used).
        Async.StartWithContinuations(messageAsync, 
             (fun reply -> printfn "%s" reply),
             (fun _ -> ()),
             (fun _ -> ()))

    printfn "Press Enter to continue."
    Console.ReadLine() |> ignore
// </snippet14>    
    (*
    Mailbox Processor Test
Type some text and press Enter to submit a message.
> hello
> hello?
> testing
> testing Message number 0 was received. Message contents: hello
1
> testing2
> Message number 1 was received. Message contents: hello?
testing3
> Message number 2 was received. Message contents: testing
Message number 3 was received. Message contents: testing 1
MeMessage number 5 was received. Message contents: testing3
ssage number 4 was received. Message contents: testing2
*)


// Print the reply as a continuation
// Implement timeout.
module Example12 =
// <snippet15>
    open System

    type Message = string * AsyncReplyChannel<string>

    let formatString = "Message number {0} was received. Message contents: {1}"

    let agent = MailboxProcessor<Message>.Start(fun inbox ->
        let rec loop n =
            async {          
                let! (message, replyChannel) = inbox.Receive(10000);
                if (message = "Stop") then
                    replyChannel.Reply("Stop")
                else
                    replyChannel.Reply(String.Format(formatString, n, message))
                do! loop (n + 1)
            }
        loop (0))

    printfn "Mailbox Processor Test"
    printfn "Type some text and press Enter to submit a message."
    printfn "Type 'Stop' to close the program."

    let mutable isCompleted = false
    while (not isCompleted) do
        printf "> "
        let input = Console.ReadLine()
        let messageAsync = agent.PostAndAsyncReply(fun replyChannel -> input, replyChannel)

        // Set up a continuation function (the first argument below) that prints the reply.
        // The second argument is the exception continuation.
        // The third argument is the cancellation continuation (not used here).
        Async.StartWithContinuations(messageAsync, 
             (fun reply -> if (reply = "Stop") then
                               isCompleted <- true
                           else printfn "%s" reply),
             (fun exn ->
                printfn "Exception occurred: %s" exn.Message),
             (fun _ -> ()))

    printfn "Press Enter to continue."
    Console.ReadLine() |> ignore
// </snippet15>
    
// This one is designed to timeout on the reply.
module Example13 =
// <snippet16>
    open System

    type Message = string * AsyncReplyChannel<string>

    let formatString = "Message number {0} was received. Message contents: {1}"

    let agent = MailboxProcessor<Message>.Start(fun inbox ->
        let rec loop n =
            async {
                    let! (message, replyChannel) = inbox.Receive();
                    // The delay gets longer with each message, and eventually will trigger a timeout.
                    do! Async.Sleep(200 * n );
                    if (message = "Stop") then
                        replyChannel.Reply("Stop")
                    else
                        replyChannel.Reply(String.Format(formatString, n, message))
                    do! loop (n + 1)
            }
        loop 0)

    let asyncReadInput =
       async {
           printf "> "
           let input = Console.ReadLine();
           return input
       }

    printfn "Mailbox Processor Test"
    printfn "Type some text and press Enter to submit a message."
    printfn "Type 'Stop' to close the program."

    let rec loop() =
        Async.StartWithContinuations(asyncReadInput, (fun input ->
            let reply = agent.TryPostAndReply((fun replyChannel -> input, replyChannel), 1000)
            match reply with
            | None -> printfn "Timeout exceeded."
            | Some(reply) ->
                if (reply <> "Stop") then
                    printfn "Reply: %s" reply
                    loop()
                else
                    ()),
            (fun exn -> ()),
            (fun _ -> ()))
    loop()

    printfn "Press Enter to continue."
    Console.ReadLine() |> ignore
// </snippet16>

// Times out on the reply using TryPostAndReply


module Example14a =
// <snippet17>
    open System

    type Message = string * AsyncReplyChannel<string>

    let formatString = "Message number {0} was received. Message contents: {1}"

    let agent = MailboxProcessor<Message>.Start(fun inbox ->
        let rec loop n =
            async {
                    let! (message, replyChannel) = inbox.Receive();
                    // The delay gets longer with each message, eventually will trigger a timeout.
                    do! Async.Sleep(200 * n );
                    if (message = "Stop") then
                        replyChannel.Reply("Stop")
                    else
                        replyChannel.Reply(String.Format(formatString, n, message))
                    do! loop (n + 1)
            }
        loop 0)

    printfn "Mailbox Processor Test"
    printfn "Type some text and press Enter to submit a message."
    printfn "Type 'Stop' to close the program."

    let mutable isCompleted = false
    while (not isCompleted) do
        printf "> "
        let input = Console.ReadLine()
        let reply = agent.TryPostAndReply((fun replyChannel -> input, replyChannel), 1000)
        match reply with
        | None -> printfn "Timeout exceeded."
        | Some(reply) ->
            if (reply <> "Stop") then
                printfn "Reply: %s" reply
            else
                isCompleted <- true

    printfn "Press Enter to continue."
    Console.ReadLine() |> ignore
// </snippet17>

// This one times out on the receive. You will notice that
// if your input time exceeds the timeout, your message number
// increases by more than 1.
module Example14b =
// <snippet18>
    open System

    type Message = string * AsyncReplyChannel<string>

    let formatString = "Message number {0} was received. Message contents: {1}"

    let agent = MailboxProcessor<Message>.Start(fun inbox ->
        let rec loop n =
            async {
                    let! opt = inbox.TryReceive(10000);
                    match opt with
                    | None -> do! loop(n + 1)
                    | Some (message, replyChannel) ->
                        // The delay gets longer with each message, and eventually triggers a timeout.
                        if (message = "Stop") then
                            replyChannel.Reply("Stop")
                        else
                            replyChannel.Reply(String.Format(formatString, n, message))
                        do! loop (n + 1)
            }
        loop 0)

    printfn "Mailbox Processor Test"
    printfn "Type some text and press Enter to submit a message."
    printfn "Type 'Stop' to close the program."

    let mutable isCompleted = false
    while (not isCompleted) do
        printf "> "
        let input = Console.ReadLine()
        let reply = agent.PostAndReply(fun replyChannel -> input, replyChannel)
        if (reply <> "Stop") then
            printfn "Reply: %s" reply
        else
            isCompleted <- true

    printfn "Press Enter to continue."
    Console.ReadLine() |> ignore
// </snippet18>

module Example15 =
// <snippet19>
    open System

    type Message = string * AsyncReplyChannel<string>

    let formatString = "Message number {0} was received. Message contents: {1}"

    let agent = MailboxProcessor<Message>.Start(fun inbox ->
        let rec loop n =
            async {          
                let! (message, replyChannel) = inbox.Receive();
                // The delay gets longer with each message, and eventually triggers a timeout.
                do! Async.Sleep(200 * n );
                if (message = "Stop") then
                    replyChannel.Reply("Stop")
                else
                    replyChannel.Reply(String.Format(formatString, n, message))
                do! loop (n + 1)
            }
        loop (0))

    printfn "Mailbox Processor Test"
    printfn "Type some text and press Enter to submit a message."
    printfn "Type 'Stop' to close the program."
    
    let mutable isCompleted = false
    while (not isCompleted) do
        printf "> "
        let input = Console.ReadLine()
        let messageAsync = agent.PostAndTryAsyncReply((fun replyChannel -> input, replyChannel), 1000)
        // Set up a continuation function (the first argument below) that prints the reply.
        // The second argument is the exception continuation.
        // The third argument is the cancellation continuation (not used).
        Async.StartWithContinuations(messageAsync, 
             (fun reply -> 
                 match reply with
                 | None -> printfn "Reply timeout exceeded."
                 | Some reply -> if (reply = "Stop") then
                                     isCompleted <- true
                                 else printfn "%s" reply),
             (fun exn ->
                printfn "Exception occurred: %s" exn.Message),
             (fun _ -> ()))

    printfn "Press Enter to continue."
    Console.ReadLine() |> ignore
// </snippet19>

module Example16 =
// <snippet20>
    open System

    let random = System.Random()

    // Generates mock jobs using Async.Sleep
    let createJob(id:int) =
        let job = async {
            // Let the time be a random number between 1 and 10000
            // And the mock computed result is a floating point value
            let time = random.Next(10000)
            let result = random.NextDouble()
            do! Async.Sleep(time)
            return result
            }
        id, job

    type Result = double
    // a Job consists of a job ID and a computation that produces a single result.
    type Job = int * Async<Result>

    type Message = int * Result 

    let context = System.Threading.SynchronizationContext.Current

    // This agent processes when jobs are completed.
    let completeAgent = MailboxProcessor<Message>.Start(fun inbox ->
        let rec loop n =
            async {
                let! (id, result) = inbox.Receive()
                printfn "The result of job #%d is %f" id result
                do! loop (n + 1)
            }
        loop (0))

    // This agent starts each job in the order it is received.
    let runAgent = MailboxProcessor<Job>.Start(fun inbox ->
        let rec loop n =
            async {          
                let! (id, job) = inbox.Receive()
                printfn "Starting job #%d" id
                // Start each job, and specify a continuation that
                // posts to the completeAgent's queue.
                Async.StartWithContinuations(job,
                   (fun result -> completeAgent.Post(id, result)),
                   (fun _ -> ()),
                   (fun _ -> ()))
                do! loop (n + 1)
            }
        loop (0))


    for id in 1 .. 10 do
        runAgent.Post(createJob(id))

    printfn "Press enter to stop at any time."
    System.Console.ReadLine() |> ignore
// </snippet20>



// Scan examples should use a different example.
// imagine a way to submit multiple background jobs which take a certain time to finish.
// use scan to look at these jobs and operate on certain ones

module Example17 =
// <snippet21>
    open System
    open System.Threading

    let random = System.Random()


    // Generates mock jobs by using Async.Sleep.
    let createJob(id:int, source:CancellationTokenSource) =
        let job = async {
            // Let the time be a random number between 1 and 10000.
            // The mock computed result is a floating point value.
            let time = random.Next(10000)
            let result = random.NextDouble()
            let count = ref 1
            while (!count <= 100 && not source.IsCancellationRequested) do
                do! Async.Sleep(time / 100)
                count := !count + 1
            return result
            }
        id, job, source

    type Result = double

    // A Job consists of a job ID and a computation that produces a single result.
    type Job = int * Async<Result> * CancellationTokenSource

    type Message = int * Result

    let context = System.Threading.SynchronizationContext.Current

    // This agent processes when jobs are completed.
    let completeAgent = MailboxProcessor<Message>.Start(fun inbox ->
        let rec loop n =
            async {
                let! (id, result) = inbox.Receive()
                printfn "The result of job #%d is %f" id result
                do! loop (n + 1)
            }
        loop (0))

    // inprogressAgent maintains a queue of in-progress jobs that can be
    // scanned to remove canceled jobs. It never runs its processor function,
    // so we set it to do nothing.
    let inprogressAgent = new MailboxProcessor<Job>(fun _ -> async { () })

    // This agent starts each job in the order in which it is received.
    let runAgent = MailboxProcessor<Job>.Start(fun inbox ->
        let rec loop n =
            async {          
                let! (id, job, source) = inbox.Receive()
                printfn "Starting job #%d" id
                // Post to the in-progress queue.
                inprogressAgent.Post(id, job, source)
                // Start the job.
                Async.StartWithContinuations(job,
                    (fun result -> completeAgent.Post(id, result)),
                    (fun _ -> ()),
                    (fun cancelException -> printfn "Canceled job #%d" id),
                    source.Token)
                do! loop (n + 1)
                }
        loop (0))

    for id in 1 .. 10 do
        let source = new CancellationTokenSource()
        runAgent.Post(createJob(id, source))

    let cancelJob(cancelId) =
        Async.RunSynchronously(
            inprogressAgent.Scan(fun (jobId, result, source) ->
                let action =
                    async {
                        printfn "Canceling job #%d" cancelId
                        source.Cancel()
                    }
                // Return Some(async) if the job ID matches.
                if (jobId = cancelId) then
                    Some(action)
                else
                    None))

    printfn "Specify a job by number to cancel it, then press Enter."

    let mutable finished = false
    while not finished do
        let input = System.Console.ReadLine()
        let a = ref 0
        if (Int32.TryParse(input, a) = true) then
            cancelJob(!a)
        else
            printfn "Terminating."
            finished <- true
// </snippet21>
            

// TryScan
module Example18 =
// <snippet22>
    open System
    open System.Threading

    let random = System.Random()


    // Generates mock jobs by using Async.Sleep.
    let createJob(id:int, source:CancellationTokenSource) =
        let job = async {
            // Let the time be a random number between 1 and 10000.
            // The mock computed result is a floating point value.
            let time = random.Next(10000)
            let result = random.NextDouble()
            let count = ref 1
            while (!count <= 100 && not source.IsCancellationRequested) do
                do! Async.Sleep(time / 100)
                count := !count + 1
            return result
            }
        id, job, source

    type Result = double

    // A Job consists of a job ID, a computation that produces a single result,
    // and a cancellation token source object that can be used to cancel the job.
    type Job = int * Async<Result> * CancellationTokenSource

    type Message = int * Result

    let context = System.Threading.SynchronizationContext.Current

    // This agent processes when jobs are completed.
    let completeAgent = MailboxProcessor<Message>.Start(fun inbox ->
        let rec loop n =
            async {
                let! (id, result) = inbox.Receive()
                printfn "The result of job #%d is %f" id result
                do! loop (n + 1)
            }
        loop (0))

    // inprogressAgent maintains a queue of in-progress jobs that can be
    // scanned to remove canceled jobs. It never runs its processor function,
    // so we set it to do nothing.
    let inprogressAgent = new MailboxProcessor<Job>(fun _ -> async { () })

    // This agent starts each job in the order in which it is received.
    let runAgent = MailboxProcessor<Job>.Start(fun inbox ->
        let rec loop n =
            async {          
                let! (id, job, source) = inbox.Receive()
                printfn "Starting job #%d" id
                // Post to the in-progress queue.
                inprogressAgent.Post(id, job, source)
                // Start the job.
                Async.StartWithContinuations(job,
                    (fun result -> completeAgent.Post(id, result)),
                    (fun _ -> ()),
                    (fun cancelException -> printfn "Canceled job #%d" id),
                    source.Token)
                do! loop (n + 1)
                }
        loop (0))

    for id in 1 .. 10 do
        let source = new CancellationTokenSource()
        runAgent.Post(createJob(id, source))

    let cancelJob(cancelId) =
        Async.RunSynchronously(
             inprogressAgent.TryScan((fun (jobId, result, source) ->
                    let action =
                        async {
                            printfn "Canceling job #%d" cancelId
                            source.Cancel()
                            return cancelId
                        }
                    // Return Some(async) if the job ID matches.
                    if (jobId = cancelId) then
                        Some(action)
                    else
                        None), 1000))
        

    printfn "Specify a job by number to cancel it, then press Enter."

    let mutable finished = false
    while not finished do
        let input = System.Console.ReadLine()
        let a = ref 0
        if (Int32.TryParse(input, a) = true) then
            match cancelJob(!a) with
            | Some id -> printfn "A job was canceled: job #%d" id
            | None -> printfn "Job not found."
        else
            printfn "Terminating."
            finished <- true
// </snippet22>

module MailboxProcessorWithError =
// <snippet23>
    open System

    type Message = string

    let agent = MailboxProcessor<Message>.Start(fun inbox ->
        let rec loop n =
            async {
                    let! message = inbox.Receive(10000);
                    printfn "Message number %d. Message contents: %s" n message
                    do! loop (n + 1)
            }
        loop 0)

    agent.Error.Add(fun exn ->
        match exn with
        | :? System.TimeoutException as exn -> printfn "The agent timed out."
                                               printfn "Press Enter to close the program."
                                               Console.ReadLine() |> ignore
                                               exit(1)
        | _ -> printfn "Unknown exception.")

    printfn "Mailbox Processor Test"
    printfn "Type some text and press Enter to submit a message."
      
    while true do
        Console.ReadLine() |> agent.Post
// </snippet23>