module XMLDoc
module Snippet7101 = begin
  // <snippet7101>
  /// <summary>Builds a new string whose characters are the results of applying the function <c>mapping</c>
  /// to each of the characters of the input string and concatenating the resulting
  /// strings.</summary>
  /// <param name="mapping">The function to produce a string from each character of the input string.</param>
  ///<param name="str">The input string.</param>
  ///<returns>The concatenated string.</returns>
  ///<exception cref="System.ArgumentNullException">Thrown when the input string is null.</exception>
  val collect : (char -> string) -> string -> string
  // </snippet7101>
end
module Snippet7102 = begin
  // <snippet7102>
  /// Creates a new string whose characters are the result of applying 
  /// the function mapping to each of the characters of the input string
  /// and concatenating the resulting strings.
  val collect : (char -> string) -> string -> string
  // </snippet7102>
end

