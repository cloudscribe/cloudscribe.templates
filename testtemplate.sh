#!/bin/bash

rm -Rf ./nupkgs 2>/dev/null
dotnet new --uninstall cloudscribe.templates
nuget pack cloudscribe.templates.nuspec -OutputDirectory "nupkgs"
dotnet new --install ./nupkgs/*.nupkg

rm -Rf ./cloudscribe.templates.test 2>/dev/null
mkdir ./cloudscribe.templates.test 2>/dev/null
cd ./cloudscribe.template.test
dotnet new cloudscribe -Da AllStorage -Q
dotnet restore
dotnet build

