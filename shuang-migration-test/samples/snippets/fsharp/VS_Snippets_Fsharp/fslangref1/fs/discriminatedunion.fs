// DiscriminatedUnion.fs
// created by GHogen 3/18/09
module DiscriminatedUnion

//<snippet2001>
let myOption1 = Some(10.0)
let myOption2 = Some("string")
let myOption3 = None
// </snippet2001>

//<snippet2002>
let printValue opt =
    match opt with
    | Some x -> printfn "%A" x
    | None -> printfn "No value."
//</snippet2002>

printValue(myOption1)
printValue(myOption2)
printValue(myOption3)

//<snippet2003>
type Shape =
  // The value here is the radius.
| Circle of float
  // The value here is the side length.
| EquilateralTriangle of double
  // The value here is the side length.
| Square of double
  // The values here are the height and width.
| Rectangle of double * double
//</snippet2003>

//<snippet2004>
let pi = 3.141592654

let area myShape =
    match myShape with
    | Circle radius -> pi * radius * radius
    | EquilateralTriangle s -> (sqrt 3.0) / 4.0 * s * s
    | Square s -> s * s
    | Rectangle (h, w) -> h * w

let radius = 15.0
let myCircle = Circle(radius)
printfn "Area of circle that has radius %f: %f" radius (area myCircle)

let squareSide = 10.0
let mySquare = Square(squareSide)
printfn "Area of square that has side %f: %f" squareSide (area mySquare)

let height, width = 5.0, 10.0
let myRectangle = Rectangle(height, width)
printfn "Area of rectangle that has height %f and width %f is %f" height width (area myRectangle)
// </snippet2004>

// <snippet2005>
type Tree =
    | Tip
    | Node of int * Tree * Tree

let rec sumTree tree =
    match tree with
    | Tip -> 0
    | Node(value, left, right) ->
        value + sumTree(left) + sumTree(right)
let myTree = Node(0, Node(1, Node(2, Tip, Tip), Node(3, Tip, Tip)), Node(4, Tip, Tip))
let resultSumTree = sumTree myTree
// </snippet2005>

// <snippet2006>
type Expression = 
    | Number of int
    | Add of Expression * Expression
    | Multiply of Expression * Expression
    | Variable of string
  
let rec Evaluate (env:Map<string,int>) exp = 
    match exp with
    | Number n -> n
    | Add (x, y) -> Evaluate env x + Evaluate env y
    | Multiply (x, y) -> Evaluate env x * Evaluate env y
    | Variable id    -> env.[id]
  
let environment = Map.ofList [ "a", 1 ;
                               "b", 2 ;
                               "c", 3 ]
             
// Create an expression tree that represents
// the expression: a + 2 * b.
let expressionTree1 = Add(Variable "a", Multiply(Number 2, Variable "b"))

// Evaluate the expression a + 2 * b, given the
// table of values for the variables.
let result = Evaluate environment expressionTree1
// </snippet2006>