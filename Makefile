test:
	dotnet test ./tests/UnitTests/UnitTests.csproj

web:
	cd ./src/Web && dotnet run

watch:
	cd ./src/Web && dotnet watch run

e2e:
	lsof -n -i:5000 | grep LISTEN | awk '{ print $$2 }' | uniq | xargs kill -9
	cd ./src/Web && dotnet run &
	sleep 2s
	dotnet test ./tests/E2ETests/E2ETests.csproj
