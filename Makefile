deploy: publish
	docker build -t revlucio/street-runner .
	docker push revlucio/street-runner
	
publish:
	cd ./src/Web && dotnet publish -o ../../out -c Release

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
	
setupAzure:
	az group create --name ukSouth --location "UK South"
	az appservice plan create --name streetRunnerPlan --resource-group ukSouth --sku S1 --is-linux
	az webapp create --resource-group ukSouth --plan streetRunnerPlan --name street-runner --deployment-container-image-name revlucio/street-runner
	az webapp config appsettings set --resource-group ukSouth --name street-runner --settings WEBSITES_PORT=5000
	az webapp config appsettings set --resource-group ukSouth --name street-runner --settings ASPNETCORE_URLS=http://+:5000
	# setup CD
	
deleteAzure:
	az group delete --name ukSouth -y