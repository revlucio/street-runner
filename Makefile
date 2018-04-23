deploy: publish
	docker build -t street-runner .

publish:
	cd ./src/Web && dotnet publish -o ../../publish -c Release

test: stop
	dotnet test ./tests/UnitTests/UnitTests.csproj

web: stop
	cd ./src/Web && dotnet run

watch: stop
	cd ./src/Web && dotnet watch run
	
stop:
	lsof -n -i:5000 | grep LISTEN | awk '{ print $$2 }' | uniq | xargs kill -9
	
start: stop
	cd ./src/Web && dotnet run &
	sleep 2s
	
e2e: start
	dotnet test ./tests/E2ETests/E2ETests.csproj
