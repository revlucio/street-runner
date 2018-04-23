FROM microsoft/dotnet:2.0-sdk
COPY /publish .
ENTRYPOINT ["dotnet", "StreetRunner.Web.dll"]

