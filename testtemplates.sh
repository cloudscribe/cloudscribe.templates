#!/bin/bash

# the purpose of this script is to try and test the various template options
# to make sure that they produce valid cloudscribe projects that build and run

TESTPROJECTDIR="cloudscribe.templates.test"

# this will build a new cloudscribe.templates nuget package and install it locally

rm -Rf ./nupkgs 2>/dev/null
# create a new nuget package for the template
nuget pack cloudscribe.templates.nuspec -OutputDirectory "nupkgs"
[ $? -ne 0 ] && echo "nuget pack failed" && exit 1

#Uninstall any existing local version of the template
dotnet new --uninstall cloudscribe.templates
#Install the new version of the template we've just compiled
dotnet new --install ./nupkgs/*.nupkg

# now we need to create a new project from the template and try and build it
# we'll use various options to test the different template options

# this is a list of possibly useful combinations of options that we should test
# But how do I test all combinations because there is so many!
# As a start we'll just test the most common combinations
# some of which are not supported in NoDb or SQLite mode!

# options for dotnet new cloudscribe -Da
DBOPTIONS = "NoDb MSSQL MySql SQLite pgsql AllStorage"

# cloudcribe module inclusions
MODULEOPTIONS = "-C -K -I -Q -L -F -P -Ne -Co -Fo -D"

# but some modules are not supported in NoDb or SQLite mode!




#remove and recreate the test project directory
rm -Rf ./$TESTPROJECTDIR 2>/dev/null
mkdir ./$TESTPROJECTDIR 2>/dev/null

#if the test project directory exists, try and create a new project from the template
if [ -d "./$TESTPROJECTDIR" ]; then
    cd ./$TESTPROJECTDIR
    dotnet new cloudscribe -Da AllStorage -Q
    [ $? -ne 0 ] && echo "dotnet new failed" && exit 1
    dotnet restore --force --no-cache --forec-evaluate
    [ $? -ne 0 ] && echo "dotnet restore failed" && exit 1
    dotnet build
    [ $? -ne 0 ] && echo "dotnet build failed" && exit 1
fi


