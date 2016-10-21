namespace FSharp.Care.Monads


// the two-track type
type Either<'TSuccess,'TFailure> = 
    | Success of 'TSuccess
    | Failure of 'TFailure


module Either =

    // convert a single value into a two-track result
    let succeed x = 
        Success x

    // convert a single value into a two-track result
    let fail x = 
        Failure x

    // apply either a success function or failure function
    let either successFunc failureFunc twoTrackInput =
        match twoTrackInput with
        | Success s -> successFunc s
        | Failure f -> failureFunc f

    // convert a switch function into a two-track function
    let bind f = 
        either f fail

    // pipe a two-track value into a switch function 
    let (>>=) x f = 
        bind f x

    // compose two switches into another switch
    let (>=>) s1 s2 = 
        s1 >> bind s2

    // convert a one-track function into a switch
    let switch f = 
        f >> succeed

    // convert a one-track function into a two-track function
    let map f = 
        either (f >> succeed) fail

    // convert a dead-end function into a one-track function
    let tee f x = 
        f x; x 

    // convert a one-track function into a switch with exception handling
    let tryCatch f exnHandler x =
        try
            f x |> succeed
        with
        | ex -> exnHandler ex |> fail

    // convert two one-track functions into a two-track function
    let doubleMap successFunc failureFunc =
        either (successFunc >> succeed) (failureFunc >> fail)

    // add two switches in parallel
    let plus addSuccess addFailure switch1 switch2 x = 
        match (switch1 x),(switch2 x) with
        | Success s1,Success s2 -> Success (addSuccess s1 s2)
        | Failure f1,Success _  -> Failure f1
        | Success _ ,Failure f2 -> Failure f2
        | Failure f1,Failure f2 -> Failure (addFailure f1 f2)

    /// Returns success result or fails with exception.
    let getOrFailwith (either: Either<'a,string>) =
        match either with
        | Success suc  -> suc
        | Failure fail -> failwith fail //raise (new System.Exception(fail))

    /// Returns success result or default value
    let getOrDefault defValue (either: Either<'a,string>) =
        match either with
        | Success suc  -> suc
        | Failure _ -> defValue

    /// Returns success value as optional. In case of failure None.
    let toOption (either:Either<'a,'b>) = 
        match either with
        | Success s -> Some s
        | Failure f -> None

    [<AutoOpen>]
    module List =
        

        let firstSuccessOrDefault (defaultValue:'a) (data:list<Either<'a,'b>>) =
            let rec loop l =
                match l with
                | h::t -> 
                    match h with
                    | Success s -> s
                    | Failure f -> loop t 
                | [] -> defaultValue
            loop data

