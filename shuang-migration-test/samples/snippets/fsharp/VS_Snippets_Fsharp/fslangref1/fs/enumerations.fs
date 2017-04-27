// Enumerations.fs
// created by GHogen 3/18/09
module Enumerations

// <snippet2101>
// Declaration of an enumeration.
type Color =
   | Red = 0
   | Green = 1
   | Blue = 2
// Use of an enumeration.
let col1 : Color = Color.Red

// </snippet2101>

// <snippet2102>
// Conversion to an integral type.
let n = int col1
// </snippet2102>

// <snippet2103>
let col2 = enum<Color>(3)
// </snippet2103>

// <snippet2104>
type uColor =
   | Red = 0u
   | Green = 1u
   | Blue = 2u
let col3 = Microsoft.FSharp.Core.LanguagePrimitives.EnumOfValue<uint32, uColor>(2u)
// </snippet2104>

