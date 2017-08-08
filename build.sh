#!/bin/bash

if [ "$1" == "test" ]
then
    dotnet test ./tests/StreetRunner.Tests/tests.csproj
fi

if [ "$1" == "web" ]
then
    dotnet run --project ./src/Web/Web.csproj
fi