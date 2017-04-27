
#light

// Snippet numbers start at 20

//<Snippet47>

// ** GIVE THE VALUE A NAME **

//<Snippet20>
// Integer and string.
let num = 10
let str = "F#"
//</Snippet20>

//<Snippet21>
let squareIt = fun n -> n * n
//</Snippet21>

//<Snippet22>
let squareIt2 n = n * n
//</Snippet22>


// ** STORE THE VALUE IN A DATA STRUCTURE **

//<Snippet23>
// Lists.

// Storing integers and strings.
let integerList = [ 1; 2; 3; 4; 5; 6; 7 ]
let stringList = [ "one"; "two"; "three" ]

// You cannot mix types in a list. The following declaration causes a 
// type-mismatch compiler error.
//let failedList = [ 5; "six" ]

// In F#, functions can be stored in a list, as long as the functions 
// have the same signature.

// Function doubleIt has the same signature as squareIt, declared previously.
//let squareIt = fun n -> n * n
let doubleIt = fun n -> 2 * n

// Functions squareIt and doubleIt can be stored together in a list.
let funList = [ squareIt; doubleIt ]

// Function squareIt cannot be stored in a list together with a function
// that has a different signature, such as the following body mass 
// index (BMI) calculator.
let BMICalculator = fun ht wt -> 
                    (float wt / float (squareIt ht)) * 703.0

// The following expression causes a type-mismatch compiler error.
//let failedFunList = [ squareIt; BMICalculator ]


// Tuples.

// Integers and strings.
let integerTuple = ( 1, -7 )
let stringTuple = ( "one", "two", "three" )

// A tuple does not require its elements to be of the same type.
let mixedTuple = ( 1, "two", 3.3 )

// Similarly, function elements in tuples can have different signatures.
let funTuple = ( squareIt, BMICalculator )

// Functions can be mixed with integers, strings, and other types in
// a tuple. Identifier num was declared previously.
//let num = 10
let moreMixedTuple = ( num, "two", 3.3, squareIt )
//</Snippet23>

//<Snippet24>
// You can pull a function out of a tuple and apply it. Both squareIt and num
// were defined previously.
let funAndArgTuple = (squareIt, num)

// The following expression applies squareIt to num, returns 100, and 
// then displays 100.
System.Console.WriteLine((fst funAndArgTuple)(snd funAndArgTuple))
//</Snippet24>

//<Snippet25>
// Make a list of values instead of identifiers.
let funAndArgTuple2 = ((fun n -> n * n), 10)

// The following expression applies a squaring function to 10, returns
// 100, and then displays 100.
System.Console.WriteLine((fst funAndArgTuple2)(snd funAndArgTuple2))
//</Snippet25>


// ** PASS THE VALUE AS AN ARGUMENT **

//<Snippet26>
// An integer is passed to squareIt. Both squareIt and num are defined in 
// previous examples.
//let num = 10
//let squareIt = fun n -> n * n
System.Console.WriteLine(squareIt num)

// String.
// Function repeatString concatenates a string with itself.
let repeatString = fun s -> s + s

// A string is passed to repeatString. HelloHello is returned and displayed.
let greeting = "Hello"
System.Console.WriteLine(repeatString greeting)
//</Snippet26>

//<Snippet27>
// Define the function, again using lambda expression syntax.
let applyIt = fun op arg -> op arg

// Send squareIt for the function, op, and num for the argument you want to 
// apply squareIt to, arg. Both squareIt and num are defined in previous 
// examples. The result returned and displayed is 100.
System.Console.WriteLine(applyIt squareIt num)

// The following expression shows the concise syntax for the previous function
// definition.
let applyIt2 op arg = op arg
// The following line also displays 100.
System.Console.WriteLine(applyIt2 squareIt num)
//</Snippet27>

//<Snippet28>
// List integerList was defined previously:
//let integerList = [ 1; 2; 3; 4; 5; 6; 7 ]

// You can send the function argument by name, if an appropriate function
// is available. The following expression uses squareIt.
let squareAll = List.map squareIt integerList

// The following line displays [1; 4; 9; 16; 25; 36; 49]
printfn "%A" squareAll

// Or you can define the action to apply to each list element inline.
// For example, no function that tests for even integers has been defined,
// so the following expression defines the appropriate function inline.
// The function returns true if n is even; otherwise it returns false.
let evenOrNot = List.map (fun n -> n % 2 = 0) integerList

// The following line displays [false; true; false; true; false; true; false]
printfn "%A" evenOrNot
//</Snippet28>


// ** RETURN THE VALUE FROM A FUNCTION CALL **

//<Snippet29>
// Function doubleIt is defined in a previous example.
//let doubleIt = fun n -> 2 * n
System.Console.WriteLine(doubleIt 3)
System.Console.WriteLine(squareIt 4)
//</Snippet29>

// The following function call returns a string:
//<Snippet30>
// str is defined in a previous section.
//let str = "F#"
let lowercase = str.ToLower()
//</Snippet30>

//<Snippet31>
System.Console.WriteLine((fun n -> n % 2 = 1) 15)
//</Snippet31>
 
//<Snippet32>
let checkFor item = 
    let functionToReturn = fun lst ->
                           List.exists (fun a -> a = item) lst
    functionToReturn
//</Snippet32>

//<Snippet33>
// integerList and stringList were defined earlier.
//let integerList = [ 1; 2; 3; 4; 5; 6; 7 ]
//let stringList = [ "one"; "two"; "three" ]

// The returned function is given the name checkFor7. 
let checkFor7 = checkFor 7

// The result displayed when checkFor7 is applied to integerList is True.
System.Console.WriteLine(checkFor7 integerList)

// The following code repeats the process for "seven" in stringList.
let checkForSeven = checkFor "seven"

// The result displayed is False.
System.Console.WriteLine(checkForSeven stringList)
//</Snippet33>

//<Snippet34>
// Function compose takes two arguments. Each argument is a function 
// that takes one argument of the same type. The following declaration
// uses lambda expresson syntax.
let compose = 
    fun op1 op2 ->
        fun n ->
            op1 (op2 n)

// To clarify what you are returning, use a nested let expression:
let compose2 = 
    fun op1 op2 ->
        // Use a let expression to build the function that will be returned.
        let funToReturn = fun n ->
                            op1 (op2 n)
        // Then just return it.
        funToReturn

// Or, integrating the more concise syntax:
let compose3 op1 op2 =
    let funToReturn = fun n ->
                        op1 (op2 n)
    funToReturn
//</Snippet34>

//<Snippet35>
// Functions squareIt and doubleIt were defined in a previous example.
let doubleAndSquare = compose squareIt doubleIt
// The following expression doubles 3, squares 6, and returns and
// displays 36.
System.Console.WriteLine(doubleAndSquare 3)

let squareAndDouble = compose doubleIt squareIt
// The following expression squares 3, doubles 9, returns 18, and
// then displays 18.
System.Console.WriteLine(squareAndDouble 3)
//</Snippet35>

//<Snippet36>
let makeGame target = 
    // Build a lambda expression that is the function that plays the game.
    let game = fun guess -> 
                   if guess = target then
                      System.Console.WriteLine("You win!")
                   else 
                      System.Console.WriteLine("Wrong. Try again.")
    // Now just return it.
    game
//</Snippet36>

//<Snippet37>
let playGame = makeGame 7
// Send in some guesses.
playGame 2
playGame 9
playGame 7

// Output:
// Wrong. Try again.
// Wrong. Try again.
// You win!

// The following game specifies a character instead of an integer for target. 
let alphaGame = makeGame 'q'
alphaGame 'c'
alphaGame 'r'
alphaGame 'j'
alphaGame 'q'

// Output:
// Wrong. Try again.
// Wrong. Try again.
// Wrong. Try again.
// You win!
//</Snippet37>


// ** CURRIED FUNCTIONS **

//<Snippet38>
let compose4 op1 op2 n = op1 (op2 n)
//</Snippet38>

//<Snippet39>
let compose4curried =
    fun op1 ->
        fun op2 ->
            fun n -> op1 (op2 n)
//</Snippet39>
  
//<Snippet40>
// Access one layer at a time.
System.Console.WriteLine(((compose4 doubleIt) squareIt) 3)

// Access as in the original compose examples, sending arguments for 
// op1 and op2, then applying the resulting function to a value.
System.Console.WriteLine((compose4 doubleIt squareIt) 3)

// Access by sending all three arguments at the same time.
System.Console.WriteLine(compose4 doubleIt squareIt 3)
//</Snippet40>

//<Snippet41>
let doubleAndSquare4 = compose4 squareIt doubleIt
// The following expression returns and displays 36.
System.Console.WriteLine(doubleAndSquare4 3)

let squareAndDouble4 = compose4 doubleIt squareIt
// The following expression returns and displays 18.
System.Console.WriteLine(squareAndDouble4 3)
//</Snippet41>

//<Snippet42>
let makeGame2 target guess =
    if guess = target then
       System.Console.WriteLine("You win!")
    else 
       System.Console.WriteLine("Wrong. Try again.")
        
let playGame2 = makeGame2 7
playGame2 2
playGame2 9
playGame2 7

let alphaGame2 = makeGame2 'q'
alphaGame2 'c'
alphaGame2 'r'
alphaGame2 'j'
alphaGame2 'q'
//</Snippet42>


// ** IDENTIFIER AND FUNCTION DEFINITION ARE INTERCHANGEABLE **

//<Snippet43>
let isNegative = fun n -> n < 0

// This example uses the names of the function argument and the integer
// argument. Identifier num is defined in a previous example.
//let num = 10
System.Console.WriteLine(applyIt isNegative num)

// This example substitutes the value that num is bound to for num, and the
// value that isNegative is bound to for isNegative.
System.Console.WriteLine(applyIt (fun n -> n < 0) 10) 
//</Snippet43>

//<Snippet44>
System.Console.WriteLine((fun op arg -> op arg) (fun n -> n < 0)  10)
//</Snippet44>


// ** FUNCTIONS ARE FIRST-CLASS VALUES IN F# **

//let squareIt = fun n -> n * n

//<Snippet45>
let funTuple2 = ( BMICalculator, fun n -> n * n )
//</Snippet45>

//<Snippet46>
let increments = List.map (fun n -> n + 1) [ 1; 2; 3; 4; 5; 6; 7 ]
//</Snippet46>

//let checkFor item = 
//    let functionToReturn = fun lst ->
//                           List.exists (fun a -> a = item) lst
//    functionToReturn
//</Snippet47>


// *************************************************************
System.Console.WriteLine (squareIt num)

let repeatS = fun s -> s + s
System.Console.WriteLine(repeatS str)



// multiplyBy returns a function as its value.
let multiplyBy = 
  fun m -> 
    fun n -> 
      m * n

// An embedded let expression may make this easier to see.
let multiplyBy2 =
  // multiplyBy2 is a function that takes one argument, m.
  fun m ->
    // In multiplyBy2, use a let to define the function you want to return.
    let returnValue =
      fun n -> m * n
    // Return the value.
    returnValue

let multiplyBy3 m = fun n -> m * n

// Now use multiplyBy to create a new function that takes one argument
// and multiplies it by 5.
let times5 = multiplyBy 5
// The following expression returns 30.
System.Console.WriteLine(times5 6)

// Or
let doubleIt2 = multiplyBy2 2
System.Console.WriteLine(doubleIt2 6)


//let integerList = [3; 6; 2; 8; 7; 1; 5]
//let stringList = [ "one"; "two"; "three" ]
let findIt item =
  let returnValue = fun lst ->
                      List.findIndex (fun a -> a = item) lst
  returnValue


let find7 = findIt 7
System.Console.WriteLine(find7 integerList)

let findOne = findIt "one"
System.Console.WriteLine(findOne stringList)


//System.Console.WriteLine((findIt 9) integerList)
System.Console.WriteLine((findIt "one") stringList)


// Or more concisely:
let compose3_2 op1 op2 =
    fun n -> op1 (op2 n)


//let composeSquareItWith = compose4 squareIt
//let composeSquareItWIthDoubleIt = compose4 squareIt doubleIt
//let result = compose4 squareIt doubleIt 3

//System.Console.WriteLine((compose5 doubleIt squareIt) 2)
//System.Console.WriteLine(((compose5 doubleIt) squareIt) 2)
//System.Console.WriteLine(compose5 doubleIt squareIt 2)

// Send two functions to compose, both of which take a single argument 
// of the same type. The return value is a composition of the two 
// functions.



//let capFirst = compose (fun (a:string) -> a.ToUpper()) (fun (a: string) -> a.Substring(0, 1)) 
//System.Console.WriteLine "%A\n" (capFirst "abcdefg")

System.Console.WriteLine((fun n -> n % 2 = 1) 15)

// Another example. Make a guessing game â€¦..
//let makeGame m = 
//  let theGame = fun n -> 
//                if n = m then
//                 System.Console.WriteLine("You win!")
//                else 
//                 System.Console.WriteLine("Wrong.")
//  theGame
//    
//let makeGame = 
//  fun m ->
//    fun n -> 
//      if n = m then
//        System.Console.WriteLine("You win!")
//      else 
//        System.Console.WriteLine("Wrong. Try again.")






// List integerList was defined previously:
//let integerList = [3; 6; 2; 8; 7; 1; 5]
//
//let target = (new System.Random()).Next()% 11
//System.Console.Write("Random: ")
//System.Console.WriteLine(target)
    
let makeGame3 = 
    // Generate a number to test, between 0 and 10, inclusive.
    let target = (new System.Random()).Next()% 11
    // You can peek at the target if you want to.
    //System.Console.WriteLine("Target: " + target.ToString())
    
    // Build a lambda expression that is the function that plays the game.
    // It takes one argument, guess, compares the guess to the target, and
    // displays an appropriate message.
    let game = fun guess -> 
                 //System.Console.WriteLine(target)
                 if guess = target then
                   System.Console.WriteLine("You win!")
                 else 
                   System.Console.WriteLine("Wrong. Try again.")
    // Now just return it.
    game
    
let playGame3 = makeGame3
playGame3 2
playGame3 9
playGame3 7

//let alphaGame3 = makeGame3
//alphaGame3 'c'
//alphaGame3 'r'
//alphaGame3 'j'
//alphaGame3 'q'
// You can define the action to apply to each list element inline.
let incrementAll = List.map (fun n -> n + 1) integerList
//val incrAll : int list = [4; 7; 3; 9; 8; 2; 6]



// You can use any method that is appropriate for the element type.
//let uppercaseAll = List.map (fun (str:string) -> str.ToLower) stringList


let evens = List.map (fun n -> n % 2 = 0) integerList
//val evens : bool list = [false; true; true; true; false; false; false]
// To print the list, use a similar List method, iter, which applies a
// function argument to each element of a list, but does not return a list
// of the results. Iter is appropriate when the desired action is a side
// effect, such as display.

List.iter (fun n -> printf "%d " n) incrementAll
printfn ""

List.iter (fun n -> printf "%d " n) squareAll
printfn ""
//List.iter (fun n -> printf "%b " n) testForEven
printfn ""

//List.iter (fun n -> System.Console.Write ((string n) + " ")) incrAll

printfn "%A" squareAll
//printfn "%A" testForEven

//// To print the list, use a similar collection method, iter, which applies
//// a function argument to each element of a list. Method iter does not
//// return a list of the results. It is appropriate when the desired action
//// is a side effect, such as display.
//// The following line displays 9 36 4 64 49 1 25
//List.iter (fun n -> printf "%d " n) squareAll
//printfn ""
//// The following line displays false true true true false false false
//List.iter (fun n -> printf "%b " n) isItEven
//printfn ""
