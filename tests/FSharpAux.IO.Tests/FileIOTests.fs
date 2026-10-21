module FileIOTests

open System
open System.IO
open System.IO.Compression
open System.Text
open FSharpAux.IO
open Expecto

let private testLines1                          = ["@read1"; "ACGT"; "+"; "IIII"]
let private testLines2                          = ["first"; ""; "third"; ""]
let private testLines3                          = ["first\r"; "second\r"]
let private testLines4                          = [for i in 1 .. 1000 -> sprintf "line %i %s" i (String('N', i % 100))]
let private testLine5                           = String('A', 99)

let private testLines3_readFileGZip_Equal       = ["first"; "second"]

// helper functions
let private writeGZip (path: string) (members: seq<string> list) =
    use file = File.Create path
    for lines in members do
        use gzipStream = new GZipStream(file, CompressionLevel.Fastest, true)
        use writer = new StreamWriter(gzipStream, UTF8Encoding(false))
        for line in lines do
            writer.Write(line + "\n")

let private withTempFile (test: string -> unit) =
    let path = Path.GetTempFileName()
    try test path
    finally File.Delete path

[<Tests>]
let fileIOTests =
    testList "FileIOTests" [
        testList "FileIO.readFileGZip" [
            testCase "returns correct lines" (fun _ ->
                withTempFile (fun path ->
                    writeGZip path [testLines1]
                    Expect.equal (FileIO.readFileGZip path |> List.ofSeq) testLines1 "FileIO.readFileGZip did return correct lines"
                )
            )
            testCase "returns correct lines, case: empty lines" (fun _ ->
                withTempFile (fun path ->
                    writeGZip path [testLines2]
                    Expect.equal (FileIO.readFileGZip path |> List.ofSeq) testLines2 "FileIO.readFileGZip did not keep empty lines"
                )
            )
            testCase "returns correct lines, case: CRLF line endings" (fun _ ->
                withTempFile (fun path ->
                    writeGZip path [testLines3]
                    Expect.equal (FileIO.readFileGZip path |> List.ofSeq) testLines3_readFileGZip_Equal "FileIO.readFileGZip did not remove '\\r'"
                )
            )
            testCase "returns correct lines, case: empty file" (fun _ ->
                withTempFile (fun path ->
                    writeGZip path [[]]
                    Expect.isEmpty (FileIO.readFileGZip path) "FileIO.readFileGZip did not return empty sequence"
                )
            )
            testCase "returns correct lines, case: multi-member gzip file" (fun _ ->
                withTempFile (fun path ->
                    writeGZip path [testLines1; testLines2]
                    Expect.equal (FileIO.readFileGZip path |> List.ofSeq) (testLines1 @ testLines2) "FileIO.readFileGZip did not return lines of all gzip members"
                )
            )
            testCase "returns same lines as FileIO.readFile" (fun _ ->
                withTempFile (fun plainPath ->
                    withTempFile (fun gzipPath ->
                        File.WriteAllText(plainPath, String.Join("\n", testLines4) + "\n")
                        writeGZip gzipPath [testLines4]
                        Expect.equal (FileIO.readFileGZip gzipPath |> List.ofSeq) (FileIO.readFile plainPath |> List.ofSeq) "FileIO.readFileGZip did not return same lines as FileIO.readFile"
                    )
                )
            )
            testCase "throws only when enumerated, case: missing file" (fun _ ->
                let lines = FileIO.readFileGZip (Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".gz"))
                Expect.throwsT<FileNotFoundException> (fun () -> lines |> Seq.iter ignore) "FileIO.readFileGZip did not throw FileNotFoundException when enumerated"
            )
            testCase "returns first line without decompressing the whole file" (fun _ ->
                withTempFile (fun path ->
                    writeGZip path [Seq.replicate 1_000_000 testLine5]
                    let before = GC.GetAllocatedBytesForCurrentThread()
                    let first = FileIO.readFileGZip path |> Seq.head
                    let allocated = GC.GetAllocatedBytesForCurrentThread() - before
                    Expect.equal first testLine5 "FileIO.readFileGZip did return correct first line"
                    Expect.isLessThan allocated (10L * 1024L * 1024L) $"FileIO.readFileGZip allocated {allocated / 1024L / 1024L} MB to read the first line of a 100 MB file"
                )
            )
        ]
    ]
    