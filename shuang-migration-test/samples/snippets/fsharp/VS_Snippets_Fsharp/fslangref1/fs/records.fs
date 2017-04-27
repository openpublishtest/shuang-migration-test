// Records.fs
// created by GHogen 3/18/09
module Records

module M0 =
    type Point = { x : float; y: float; z: float; }
    // <snippet1907>
    let mypoint = { x = 1.0; y = 1.0; z = -1.0; }
    // </snippet1907>
    // <snippet1908>
    let myPoint1 = { Point.x = 1.0; y = 1.0; z = 0.0; }
    // </snippet1908>
    ()

module M1 =

 // <snippet1901>
 type Point = { x : float; y: float; z: float; }
 type Customer = { First : string; Last: string; SSN: uint32; AccountNumber : uint32; }
 // </snippet1901>

 // <snippet1902>
 let mypoint1 = { new Point with x = 1.0 and y = 1.0 and z = -1.0 }
 let mypoint2 = { x = 1.0; y = 1.0; z = -1.0; }
 // </snippet1902>

module M2 =

 // <snippet1903>
 type Point = { x : float; y: float; z: float; }
 type Point3D = { x: float; y: float; z: float }
 // Ambiguity: Point or Point3D?
 let mypoint3D = { x = 1.0; y = 1.0; z = 0.0; }
 // </snippet1903>

 // <snippet1904>
 type MyRecord = {
     X: int;
     Y: int;
     Z: int 
     }

 let myRecord1 = { X = 1; Y = 2; Z = 3; }
 // </snippet1904>

 // <snippet1905>
 let myRecord2 = { MyRecord.X = 1; MyRecord.Y = 2; MyRecord.Z = 3 }
 // </snippet1905>

 // <snippet1906>
 let myRecord3 = { myRecord2 with Y = 100; Z = 2 }
 // </snippet1906>

// <snippet1909>
type Car = {
    Make : string
    Model : string
    mutable Odometer : int
    }
let myCar = { Make = "Fabrikam"; Model = "Coupe"; Odometer = 108112 }
myCar.Odometer <- myCar.Odometer + 21
// </snippet1909>


// <snippet1910>
type Point3D = { x: float; y: float; z: float }
let evaluatePoint (point: Point3D) =
    match point with
    | { x = 0.0; y = 0.0; z = 0.0 } -> printfn "Point is at the origin."
    | { x = xVal; y = 0.0; z = 0.0 } -> printfn "Point is on the x-axis. Value is %f." xVal
    | { x = 0.0; y = yVal; z = 0.0 } -> printfn "Point is on the y-axis. Value is %f." yVal
    | { x = 0.0; y = 0.0; z = zVal } -> printfn "Point is on the z-axis. Value is %f." zVal
    | { x = xVal; y = yVal; z = zVal } -> printfn "Point is at (%f, %f, %f)." xVal yVal zVal

evaluatePoint { x = 0.0; y = 0.0; z = 0.0 }
evaluatePoint { x = 100.0; y = 0.0; z = 0.0 }
evaluatePoint { x = 10.0; y = 0.0; z = -1.0 }
// </snippet1910>

// <snippet1911>
type RecordTest = { X: int; Y: int }
let record1 = { X = 1; Y = 2 }
let record2 = { X = 1; Y = 2 }
if (record1 = record2) then
    printfn "The records are equal."
else
    printfn "The records are unequal."
// </snippet1911>