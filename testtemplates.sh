#!/bin/bash

# the purpose of this script is to try and test the various template options
# to make sure that they produce valid cloudscribe projects that build and run

SCRIPT_DIR=$( cd -- "$( dirname -- "${BASH_SOURCE[0]}" )" &> /dev/null && pwd )

TESTPROJECTDIR="cloudscribe.templates.test"

# this will build a new cloudscribe.templates nuget package and install it locally

rm -Rf ./nupkgs 2>/dev/null
# create a new nuget package for the template
nuget pack cloudscribe.templates.nuspec -OutputDirectory "nupkgs"
[ $? -ne 0 ] && echo "nuget pack failed" && exit 1

#Uninstall any existing local version of the template
dotnet new uninstall cloudscribe.templates
#Install the new version of the template we've just compiled
dotnet new install ./nupkgs/*.nupkg

# now we need to create a new project from the template and try and build it
# we'll use various options to test the different template options

# this is a list of possibly useful combinations of options that we should test
# But how do I test all combinations because there is so many!
# As a start we'll just test the most common combinations
# some of which are not supported in NoDb or SQLite mode!

# options for dotnet new cloudscribe -Da
DBOPTIONS="NoDb MSSQL MySql SQLite pgsql AllStorage"

S=0

for DB in $DBOPTIONS; do

    echo "Building project with $DB database in $SCRIPT_DIR/$TESTPROJECTDIR ..."
    echo "---------------------------------------------------------------------------------------------"

    # cloudcribe module inclusions
    MODULEOPTIONS="-C -K -I -Q -L -F -P -Ne -Co -Fo -D"

    # but some modules are not supported in NoDb or SQLite mode!
    if [ $DB == "SQLite" ]; then
        MODULEOPTIONS="-C -K -I -Q -L -F -Co -Fo -D"
    fi
    if [ $DB == "NoDb" ]; then
        MODULEOPTIONS="-C -K -I -L -F -Co -Fo -D"
    fi

    #remove and recreate the test project directory
    rm -Rf $SCRIPT_DIR/$TESTPROJECTDIR 2>/dev/null
    mkdir $SCRIPT_DIR/$TESTPROJECTDIR 2>/dev/null

    #if the test project directory exists, try and create a new project from the template
    if [ -d "$SCRIPT_DIR/$TESTPROJECTDIR" ]; then
        cd $SCRIPT_DIR/$TESTPROJECTDIR
        dotnet new cloudscribe -Da $DB $MODULEOPTIONS
        [ $? -ne 0 ] && echo "dotnet new failed" && break
        dotnet restore --force --no-cache --force-evaluate
        [ $? -ne 0 ] && echo "dotnet restore failed" && break
        dotnet build
        [ $? -ne 0 ] && echo "dotnet build failed" && break
        cd $SCRIPT_DIR
        S=$((S+1))
    fi

    echo "---------------------------------------------------------------------------------------------"

done
rm -Rf $SCRIPT_DIR/$TESTPROJECTDIR 2>/dev/null

echo "------------------------------------------------------"
echo "Successfully built $S/6 projects with various options!"
echo "------------------------------------------------------"
