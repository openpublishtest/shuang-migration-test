// Learn more about F# at http://fsharp.net


open System.Windows.Forms


let event1 = new Microsoft.FSharp.Control.Event<int>()

//event1.Publish
//event1.Trigger

//Event.add
module Snippet1 =
// <snippet1>
    let form = new Form(Text = "F# Windows Form",
                        Visible = true,
                        TopMost = true)
    form.MouseMove
        |> Event.filter ( fun evArgs -> evArgs.X > 100 && evArgs.Y > 100)
        |> Event.add ( fun evArgs ->
            form.BackColor <- System.Drawing.Color.FromArgb(
                evArgs.X, evArgs.Y, evArgs.X ^^^ evArgs.Y) )
// </snippet1>

//Event.choose
module Snippet2 =
// <snippet2>
    // When the mouse button is down, the form changes color
    // as the mouse pointer is moved.

    let form = new Form(Text = "F# Windows Form",
                        Visible = true,
                        TopMost = true)
    form.MouseMove
        |> Event.choose(fun evArgs ->
            if (evArgs.Button <> MouseButtons.None) then
                Some( evArgs.X, evArgs.Y)
            else None)

        |> Event.add ( fun (x, y) ->
            form.BackColor <- System.Drawing.Color.FromArgb(
                x, y, x ^^^ y) )
// </snippet2>

//Event.filter
module Snippet3 =
// <snippet3>
    let form = new Form(Text = "F# Windows Form",
                        Visible = true,
                        TopMost = true)
    form.MouseMove
        |> Event.filter ( fun evArgs -> evArgs.X > 100 && evArgs.Y > 100)
        |> Event.add ( fun evArgs ->
            form.BackColor <- System.Drawing.Color.FromArgb(
                evArgs.X, evArgs.Y, evArgs.X ^^^ evArgs.Y) )
// </snippet3>

//Event.map
module Snippet4 =
// <snippet4>
    let form = new Form(Text = "F# Windows Form",
                        Visible = true,
                        TopMost = true)
    form.MouseMove
        |> Event.map (fun evArgs -> (evArgs.X, evArgs.Y))
        |> Event.add ( fun (x, y) ->
            form.BackColor <- System.Drawing.Color.FromArgb(
                x, y, x ^^^ y) )
// </snippet4>
//Event.merge
module Snippet5 =
// <snippet5>
    let form = new Form(Text = "F# Windows Form",
                        Visible = true,
                        TopMost = true)
    form.MouseClick
        |> Event.merge(form.MouseDoubleClick)
        |> Event.add ( fun evArgs ->
            form.BackColor <- System.Drawing.Color.FromArgb(
                evArgs.X, evArgs.Y, evArgs.X ^^^ evArgs.Y) )
// </snippet5>

//Event.pairwise
module Snippet6 =
// <snippet6>
    open System.Windows.Forms
    open System.Drawing

    let form = new Form(Text = "F# Windows Form",
                        Visible = true,
                        TopMost = true)

    let graphics = BufferedGraphicsManager.Current.Allocate(form.CreateGraphics(), 
                                    new Rectangle( 0, 0, form.Width, form.Height ))
    let whitePen = new Pen(Color.White)

    form.MouseClick
        |> Event.pairwise
        |> Event.add ( fun (evArgs1, evArgs2) ->
            graphics.Graphics.DrawLine(whitePen, evArgs1.X, evArgs1.Y, evArgs2.X, evArgs2.Y)
            form.Refresh())

    form.Paint
        |> Event.add(fun evArgs -> graphics.Render(evArgs.Graphics))
// </snippet6>
    

//Event.partition
module Snippet7 =
// <snippet7>
    open System.Windows.Forms
    open System.Drawing

    let form = new Form(Text = "F# Windows Form",
                        Visible = true,
                        TopMost = true)

    let (event1, event2) =
        form.MouseMove 
        |> Event.partition (fun evArgs -> evArgs.X > 100)

    event1 |> Event.add ( fun evArgs ->
        form.BackColor <- System.Drawing.Color.FromArgb(
            evArgs.X, evArgs.Y, evArgs.X ^^^ evArgs.Y))
    event2 |> Event.add ( fun evArgs ->
        form.BackColor <- System.Drawing.Color.FromArgb(
            evArgs.Y, evArgs.X, evArgs.Y ^^^ evArgs.X))
// </snippet7>
//Event.scan

module Snippet8 =
// <snippet8>
    // This code implements a simple click counter. Every time
    // the user clicks the form, the state increments by 1
    // and the form's text is changed to display the new state.

    open System.Windows.Forms
    open System.Drawing
    open Microsoft.FSharp.Core

    let form = new Form(Text = "F# Windows Form",
                        Visible = true,
                        TopMost = true)

    let initialState = 0
               
    form.Click
    |> Event.scan (fun state _ -> state + 1) initialState
    |> Event.add (fun state -> form.Text <- state.ToString() )
// </snippet8>

//Event.split
module Snippet9 =
// <snippet9>
    open System.Windows.Forms
    open System.Drawing
    open Microsoft.FSharp.Core

    let form = new Form(Text = "F# Windows Form",
                        Visible = true,
                        TopMost = true)

    let button = new Button(Text = "Button",
                            Visible = true,
                            Left = 100,
                            Width = 50,
                            Top = 100,
                            Height = 20)

    form.Controls.Add(button)
    let originalColor = button.BackColor
    let mutable xOff, yOff = (0, 0)

    let (|Down|Up|) (evArgs:MouseEventArgs) =
        match evArgs.Button with
        | MouseButtons.Left 
        | MouseButtons.Right 
        | MouseButtons.Middle -> Down(evArgs)
        | _ -> Up

    button.MouseDown 
    |> Event.add(fun evArgs ->
        xOff <- evArgs.X
        yOff <- evArgs.Y)

    form.MouseMove
    |> Event.map (fun evArgs -> (evArgs.X, evArgs.Y))
    |> Event.add (fun (x, y) -> form.Text <- sprintf "(%d, %d)" x y)

    let (dragButton, noDragButton) = Event.split (|Down|Up|) button.MouseMove

    // Move the button, and change its color if the user uses the
    // right or middle mouse button.
    dragButton |> Event.add ( fun evArgs ->
        match evArgs.Button with
        | MouseButtons.Left ->
            button.BackColor <- originalColor
        | MouseButtons.Right ->
            button.BackColor <- Color.Red
        | MouseButtons.Middle ->
            button.BackColor <- Color.Blue
        | _ -> ()
        button.Left <- button.Left + evArgs.X - xOff
        button.Top <- button.Top + evArgs.Y - yOff
        button.Refresh())

    // Restore the button's original color when the mouse is moved after
    // the release of the button.
    noDragButton |> Event.add ( fun () -> 
        button.BackColor <- originalColor)
// </snippet9>
