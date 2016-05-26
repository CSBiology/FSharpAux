namespace FSharp.Care


/// Numerics literals wrapper for LanguagePrimitives 
module NumericLiteralG =
    
    ///Generic Zero
    let inline FromZero () = LanguagePrimitives.GenericZero
    ///Generic One
    let inline FromOne () = LanguagePrimitives.GenericOne
    ///Generic from int
    let inline FromInt32 (n:int) =
        let one = FromOne()
        let zero = FromZero()
        let n_incr = if n > 0 then 1 else -1
        let g_incr = if n > 0 then one else (zero - one)
        let rec loop i g = 
            if i = n then g
            else loop (i + n_incr) (g + g_incr)
        loop 0 zero 



    module ConstrainedOps =
        let inline (~-) (x:^a) : ^a = -x
        let inline (+) (x:^a) (y:^a) : ^a = x + y
        let inline (*) (x:^a) (y:^a) : ^a = x * y
        let inline (/) (x:^a) (y:^a) : ^a = x / y

        let inline (%) (x:^a) (y:^a) : ^a = x % y



