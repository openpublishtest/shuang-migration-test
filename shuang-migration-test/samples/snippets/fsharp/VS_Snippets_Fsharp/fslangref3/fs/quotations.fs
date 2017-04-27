module Quotations

module Snippet501 =
    // <snippet501>
    open Microsoft.FSharp.Quotations
    // A typed code quotation.
    let expr : Expr<int> = <@ 1 + 1 @>
    // An untyped code quotation.
    let expr2 : Expr = <@@ 1 + 1 @@>
    // </snippet501>

    // <snippet502>
    // Valid:
    <@ let f x = x + 10 in f 20 @>
    // Valid:
    <@ 
        let f x = x + 10
        f 20
    @>
    // </snippet502>
    |> ignore

