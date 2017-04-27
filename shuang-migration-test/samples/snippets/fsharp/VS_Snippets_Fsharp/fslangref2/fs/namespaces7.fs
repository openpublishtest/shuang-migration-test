// Namespaces7.fs
// created by GHogen 10/29/09

// <snippet6407>
namespace global

type SomeType() =
    member this.SomeMember = 0
// </snippet6407>

module Namespaces6408 =

// <snippet6408>
    global.System.Console.WriteLine("Hello World!")
// </snippet6408>