namespace FSharpAux.IO

/// Performs operations on String instances that contain file or directory path information. These operations are performed in a cross-platform manner.
module PathFileName =

    open System
    open System.IO
    
    /// Active pattern which discriminates between files and directories.
    let (|File|Directory|) (fileSysInfo : FileSystemInfo) = 
        match fileSysInfo with
        | :? FileInfo as file -> File(file)
        | :? DirectoryInfo as dir -> Directory(dir, dir.EnumerateFileSystemInfos())
        | _ -> failwith "No file or directory given."

    /// Active Pattern for determining file extension.
    let (|EndsWith|_|) (extension : string) (file : string) = 
        if file.EndsWith extension then Some()
        else None

    /// Active Pattern for determining file name.
    let (|FileInfoFullName|) (f : FileInfo) = f.FullName

    /// Active Pattern for determining FileInfoNameSections.
    let (|FileInfoNameSections|) (f : FileInfo) = (f.Name, f.Extension, f.FullName)

    exception IllegalFileNameChar of string * char

    let illegalPathChars = Path.GetInvalidPathChars()

    let checkPathForIllegalChars (path:string) = 
        for c in path do
            if illegalPathChars |> Array.exists(fun c1->c1=c) then 
                raise(IllegalFileNameChar(path,c))

    // Case sensitive (original behaviour preserved).
    let checkSuffix (x:string) (y:string) = x.EndsWith(y,System.StringComparison.Ordinal) 

    // Determines whether a path includes a file name extension.
    let hasExtension (s:string) = 
        checkPathForIllegalChars s
        (s.Length >= 1 && s.[s.Length - 1] = '.' && s <> ".." && s <> ".") 
        || Path.HasExtension(s)

    let chopExtension (s:string) =
        checkPathForIllegalChars s
        if s = "." then "" else // for OCaml compatibility
        if not (hasExtension s) then 
            raise (System.ArgumentException("chopExtension")) // message has to be precisely this, for OCaml compatibility, and no argument name can be set
        Path.Combine (Path.GetDirectoryName s,Path.GetFileNameWithoutExtension(s))

    let directoryName (s:string) = 
        checkPathForIllegalChars s
        if s = "" then "."
        else 
          match Path.GetDirectoryName(s) with 
          | null -> //if FileSystem.IsPathRootedShim(s) then s else "."
                    if Path.IsPathRooted(s) then s else "."                    
          | res -> if res = "" then "." else res

    let fileNameOfPath s = 
        checkPathForIllegalChars s
        Path.GetFileName(s)        

    let fileNameWithoutExtension s = 
        checkPathForIllegalChars s
        Path.GetFileNameWithoutExtension(s) 

    /// Normalizes a filename towards plattform specific directory separator char. Removes the directory separator char at the end.
    let rec normalizeFileName (fileName : string) = 
        fileName.Replace("\\", Path.DirectorySeparatorChar.ToString()).Replace("/", Path.DirectorySeparatorChar.ToString())
                .TrimEnd(Path.DirectorySeparatorChar).ToLower()

    // Changes the extension of a path string.
    let changeExtension fullname extension =
        Path.ChangeExtension(fullname,extension) 

    // Combines two strings into a path.
    let combine path1 path2 =
        Path.Combine(path1,path2)
        |> normalizeFileName

    // Combines an array of strings into a path.
    let combineAll pathes =
        Path.Combine(pathes)
        |> normalizeFileName

