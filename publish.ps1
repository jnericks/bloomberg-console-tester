param([string]$build_configuration="Debug")

# Build
$base_directory = resolve-path .
$solution_name = "BloombergConsoleTester"
$solution_file = "$base_directory\$solution_name.sln"
$msbuild = (Join-Path -Path (Get-ItemProperty "HKLM:\software\Microsoft\MSBuild\ToolsVersions\4.0").MSBuildToolsPath -ChildPath "msbuild.exe")
iex "$msbuild /p:Configuration=""$build_configuration"" $solution_file"

# Publish
$publish_dir = "\\sbserver\CustomApps\DEV\$solution_name"

""
"Cleaning directory: $publish_dir"
Remove-Item "$publish_dir\*" -recurse
"Publishing $build_configuration to directory: $publish_dir"
Copy-Item "$base_directory\$solution_name\bin\$build_configuration\*" $publish_dir
""