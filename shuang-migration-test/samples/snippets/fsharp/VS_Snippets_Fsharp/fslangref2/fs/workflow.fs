// Workflow.fs
// created by GHogen 3/20/09
module workflowModule



// <snippet7002>
// Module1.fs
module Module1 =

 // Functions that implement the builder methods.
 let bind value1 function1 = 
     printfn "Binding %A." value1 
     function1 value1

 let result value1 =
     printfn "Returning result: %A" value1
     fun () -> value1

 let delay function1 =
     fun () -> function1()
 
 // The builder class for the "trace" workflow.
 type TraceBuilder() =
     member x.Bind(value1, function1) = 
         bind value1 function1
     member x.Return(value1)  = result value1
     member x.Delay(function1)   = 
         printfn "Starting traced execution."
         delay function1
 
 let trace = new TraceBuilder()
// </snippet7002>

module Module2 =

 // <snippet7001>
 // Program.fs
 open Module1

 // Create the computation expression object.
 let trace1 = trace {
    // A normal let expression (does not call Bind).
    let x = 1
    // A let expression that uses the Bind method.
    let! y = 2
    let sum = x + y
    // return executes the Return method.
    return sum  
    }

 // Execute the code. Start with the Delay method.
 let result = trace1()
 // </snippet7001>

