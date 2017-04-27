// UnitsOfMeasure.fs
// created by GHogen 3/20/09
module UnitsOfMeasure

module M6901 =

// <snippet6901>
 // Mass, grams.
 [<Measure>] type g
 // Mass, kilograms.
 [<Measure>] type kg
 // Weight, pounds.
 [<Measure>] type lb 

 // Distance, meters. 
 [<Measure>] type m
 // Distance, cm
 [<Measure>] type cm

 // Distance, inches.
 [<Measure>] type inch
 // Distance, feet
 [<Measure>] type ft

 // Time, seconds.
 [<Measure>] type s

 // Force, Newtons.
 [<Measure>] type N = kg m / s 

 // Pressure, bar.
 [<Measure>] type bar 
 // Pressure, Pascals
 [<Measure>] type Pa = N / m^2 

 // Volume, milliliters.
 [<Measure>] type ml 
 // Volume, liters.
 [<Measure>] type L

 // Define conversion constants.
 let gramsPerKilogram : float<g kg^-1> = 1000.0<g/kg>
 let cmPerMeter : float<cm/m> = 100.0<cm/m>
 let cmPerInch : float<cm/inch> = 2.54<cm/inch>

 let mlPerCubicCentimeter : float<ml/cm^3> = 1.0<ml/cm^3>
 let mlPerLiter : float<ml/L> = 1000.0<ml/L>

 // Define conversion functions.
 let convertGramsToKilograms (x : float<g>) = x / gramsPerKilogram
 let convertCentimetersToInches (x : float<cm>) = x / cmPerInch
// </snippet6901>

module M6902 =

 // <snippet6902>
 [<Measure>] type degC // temperature, Celsius/Centigrade
 [<Measure>] type degF // temperature, Fahrenheit

 let convertCtoF ( temp : float<degC> ) = 9.0<degF> / 5.0<degC> * temp + 32.0<degF>
 let convertFtoC ( temp: float<degF> ) = 5.0<degC> / 9.0<degF> * ( temp - 32.0<degF>)

 // Define conversion functions from dimensionless floating point values.
 let degreesFahrenheit temp = temp * 1.0<degF>
 let degreesCelsius temp = temp * 1.0<degC>

 printfn "Enter a temperature in degrees Fahrenheit."
 let input = System.Console.ReadLine()
 let mutable floatValue = 0.
 if System.Double.TryParse(input, &floatValue)
    then 
       printfn "That temperature in Celsius is %8.2f degrees C." ((convertFtoC (degreesFahrenheit floatValue))/(1.0<degC>))
    else
       printfn "Error parsing input."
 // </snippet6902>

module M6903 =

 // <snippet6903>
 // Distance, meters. 
 [<Measure>] type m 
 // Time, seconds. 
 [<Measure>] type s

 let genericSumUnits ( x : float<'u>) (y: float<'u>) = x + y

 let v1 = 3.1<m/s>
 let v2 = 2.7<m/s>
 let x1 = 1.2<m>
 let t1 = 1.0<s>

 // OK: a function that has unit consistency checking.
 let result1 = genericSumUnits v1 v2
 // Error reported: mismatched units.
 // Uncomment to see error.
 // let result2 = genericSumUnits v1 x1
 // </snippet6903>

module M6904 =

 // <snippet6904>
  // Distance, meters.
 [<Measure>] type m 
 // Time, seconds. 
 [<Measure>] type s 

 // Define a vector together with a measure type parameter.
 // Note the attribute applied to the type parameter.
 type vector3D<[<Measure>] 'u> = { x : float<'u>; y : float<'u>; z : float<'u>}

 // Create instances that have two different measures.
 // Create a position vector.
 let xvec : vector3D<m> = { x = 0.0<m>; y = 0.0<m>; z = 0.0<m> }
 // Create a velocity vector.
 let v1vec : vector3D<m/s> = { x = 1.0<m/s>; y = -1.0<m/s>; z = 0.0<m/s> }
 // </snippet6904>

module Snippet6905 =
// <snippet6905>
 [<Measure>]
 type cm
 let length = 12.0<cm>
 let x = float length
 // </snippet6905>

 // <snippet6906>
 open Microsoft.FSharp.Core
 let height:float<cm> = LanguagePrimitives.FloatWithMeasure x
 // </snippet6906>
