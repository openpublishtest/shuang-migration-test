// Events.fs
// created by GHogen 3/20/09
module Events

// <snippet3601>
open System.Windows.Forms

let form = new Form(Text="F# Windows Form",
                    Visible = true,
                    TopMost = true)

form.Click.Add(fun evArgs -> System.Console.Beep())
Application.Run(form)
// </snippet3601>

module Snippet3602 =
    // <snippet3602>
    open System.Windows.Forms

    let Beep evArgs =
        System.Console.Beep( )  
        

    let form = new Form(Text = "F# Windows Form",
                        Visible = true,
                        TopMost = true)
                            
    let MouseMoveEventHandler (evArgs : System.Windows.Forms.MouseEventArgs) =
        form.Text <- System.String.Format("{0},{1}", evArgs.X, evArgs.Y)

    form.Click.Add(Beep)
    form.MouseMove.Add(MouseMoveEventHandler)
    Application.Run(form)
    // </snippet3602>

// <snippet3605>
open System.Collections.Generic

type MyClassWithCLIEvent() =

    let event1 = new Event<_>()

    [<CLIEvent>]
    member this.Event1 = event1.Publish

    member this.TestEvent(arg) =
        event1.Trigger(this, arg)

let classWithEvent = new MyClassWithCLIEvent()
classWithEvent.Event1.Add(fun (sender, arg) -> 
        printfn "Event1 occurred! Object data: %s" arg)

classWithEvent.TestEvent("Hello World!")

System.Console.ReadLine() |> ignore
// </snippet3605>

// <snippet3603>
type MyType() =
    let myEvent = new Event<_>()

    member this.AddHandlers() =
       Event.add (fun string1 -> printfn "%s" string1) myEvent.Publish
       Event.add (fun string1 -> printfn "Given a value: %s" string1) myEvent.Publish

    member this.Trigger(message) =
       myEvent.Trigger(message)

let myMyType = MyType()
myMyType.AddHandlers()
myMyType.Trigger("Event occurred.")
// </snippet3603>

module Snippet3604 =
    // <snippet3604>
    let form = new Form(Text = "F# Windows Form",
                        Visible = true,
                        TopMost = true)
    form.MouseMove
        |> Event.filter ( fun evArgs -> evArgs.X > 100 && evArgs.Y > 100)
        |> Event.add ( fun evArgs ->
            form.BackColor <- System.Drawing.Color.FromArgb(
                evArgs.X, evArgs.Y, evArgs.X ^^^ evArgs.Y) )
    // </snippet3604>