// Classes.fs
// created by GHogen 3/18/09
module Classes

// <snippet2401>
type MyClass1(x: int, y: int) =
   do printfn "%d %d" x y
   new() = MyClass1(0, 0)
// </snippet2401>

// <snippet2402>
type MyClass2(dataIn) as self =
   let data = dataIn
   do
       self.PrintMessage()
   member this.PrintMessage() =
       printf "Creating MyClass2 with Data %d" data
// </snippet2402>

// <snippet2403>
type MyGenericClass<'a> (x: 'a) = 
   do printfn "%A" x
// </snippet2403>

// <snippet24031>
let g1 = MyGenericClass( seq { for i in 1 .. 10 -> (i, i*i) } )
// </snippet24031>

// <snippet2404>
open System.IO

type Folder(pathIn: string) =
  let path = pathIn
  let filenameArray : string array = Directory.GetFiles(path)
  member this.FileArray = Array.map (fun elem -> new File(elem, this)) filenameArray

and File(filename: string, containingFolder: Folder) = 
   member this.Name = filename
   member this.ContainingFolder = containingFolder

let folder1 = new Folder(".")
for file in folder1.FileArray do
   printfn "%s" file.Name
// </snippet2404>
