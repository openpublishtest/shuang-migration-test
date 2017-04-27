// SourceFileLinePath.fs
// created by GHogen 3/20/09
module SourceFileLinePath

// <snippet7401>
let printSourceLocation() =
    printfn "Line: %s" __LINE__
    printfn "Source Directory: %s" __SOURCE_DIRECTORY__
    printfn "Source File: %s" __SOURCE_FILE__
printSourceLocation()
// </snippet7401>
