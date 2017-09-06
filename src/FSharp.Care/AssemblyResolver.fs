namespace FSharp.Care

module AssemblyResolver =

    let getFromCurrentStackTrace () =
        let trace = new System.Diagnostics.StackTrace()
        let frames = trace.GetFrames()
        seq [
            for frame in frames do
                let _method = frame.GetMethod()
                let declaringType = _method.DeclaringType
            
                if declaringType <> null then 
                    let assembly = declaringType.Assembly
                    yield assembly
            ]


    let getFromCurrentDomain () =
        //System.Reflection.Assembly.GetEntryAssembly().GetReferencedAssemblies()
        System.AppDomain.CurrentDomain.GetAssemblies()


    let tryGetBySimpleName (simpleName:string) =
        System.AppDomain.CurrentDomain.GetAssemblies()
        |> Array.tryFind (fun a -> a.FullName.Contains(simpleName))

