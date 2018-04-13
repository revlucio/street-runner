test:
	dotnet test ./tests//UnitTests/UnitTests.csproj

web:
	cd ./src/Web
	dotnet watch run
