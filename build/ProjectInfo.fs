module ProjectInfo

open Fake.Core

let project = "FSharpAux"

let testProjects = 
    [
        "tests/FSharpAux.Core.Tests/FSharpAux.Core.Tests.fsproj"
        "tests/FSharpAux.Tests/FSharpAux.Tests.fsproj"
        "tests/FSharpAux.IO.Tests/FSharpAux.IO.Tests.fsproj"
    ]

let fableProject = "src/FSharpAux.Core/FSharpAux.Core.fsproj"
let fableProjectName = "FSharpAux.Fable"

let solutionFile  = $"{project}.sln"

let configuration = "Release"

let gitOwner = "CSBiology"

let gitHome = $"https://github.com/{gitOwner}"

let projectRepo = $"https://github.com/{gitOwner}/{project}"

let pkgDir = "pkg"

let release = ReleaseNotes.load "RELEASE_NOTES.md"

let stableVersion = SemVer.parse release.NugetVersion

let stableVersionTag = (sprintf "%i.%i.%i" stableVersion.Major stableVersion.Minor stableVersion.Patch )

let mutable prereleaseSuffix = ""

let mutable prereleaseTag = ""

let mutable isPrerelease = false