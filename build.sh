#!/bin/bash

if [ "$1" == "test" ]
then
    dotnet test ./tests//UnitTests/UnitTests.csproj
fi

if [ "$1" == "web" ]
then
    dotnet run --project ./src/Web/Web.csproj
fi
