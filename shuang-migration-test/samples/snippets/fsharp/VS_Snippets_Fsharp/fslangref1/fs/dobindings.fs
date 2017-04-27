// doBindings.fs
// created 3/28/09 by GHogen
module doBindings

//<snippet201>
open System
open System.Windows.Forms

let form1 = new Form()
form1.Text <- "XYZ"

[<STAThread>]
do
   Application.Run(form1)
//</snippet201>