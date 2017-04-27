// AsynchronousWorkflows.fs
// created by GHogen 3/20/09
module AsynchronousWorkflows

module Snippet8001 =
    // <snippet8001>
    let task1 = async {
            let x = 10
            let y = 15
            for i in x ..y do
               let logi = log (float i)
               printf "%d %f" i logi
            return x + y
       }

    let task2 = async {
            let x = 10
            let y = 20
            for i in x ..y do
               printf "[%s] " (i.ToString())
            return x + y
       }

    // Execute the code, mixing the results.
    let results = Async.RunSynchronously (Async.Parallel [ task1; task2 ])
    // </snippet8001>



module Snippet8003 =
    // <snippet8003>
    open System.Net
    open Microsoft.FSharp.Control.WebExtensions

    let urlList = [ "Microsoft.com", "http://www.microsoft.com/"
                    "MSDN", "http://msdn.microsoft.com/"
                    "Bing", "http://www.bing.com"
                  ]

    let fetchAsync(name, url:string) =
        async { 
            try
                let uri = new System.Uri(url)
                let webClient = new WebClient()
                let! html = webClient.AsyncDownloadString(uri)
                printfn "Read %d characters for %s" html.Length name
            with
                | ex -> printfn "%s" (ex.Message);
        }

    let runAll() =
        urlList
        |> Seq.map fetchAsync
        |> Async.Parallel 
        |> Async.RunSynchronously
        |> ignore

    runAll()
    // </snippet8003>