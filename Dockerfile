FROM microsoft/dotnet:2.0-sdk
COPY /out .
ENV ASPNETCORE_URLS=http://+:5000
ENTRYPOINT ["dotnet", "StreetRunner.Web.dll"]

