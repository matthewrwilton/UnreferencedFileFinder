# UnreferencedFileFinder

A small utility designed to find any files in a MSBuild project's directories that aren't referenced by the project.
Print out all unreferenced files in the project's directories.



Usage: UnreferencedFileFinder.exe projectFile
  e.g. UnreferencedFileFinder.exe C:\repos\UnreferencedFileFinder\UnreferencedFileFinder\UnreferencedFileFinder.csproj



Only tested for .NET 4.5 projects on Windows 7 environments. Adjustments may be required to suit other environments and .NET releases.
