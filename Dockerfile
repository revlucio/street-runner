FROM microsoft/dotnet:2.1-runtime
COPY /out .
ENV ASPNETCORE_URLS=http://+:5000
ENTRYPOINT ["dotnet", "StreetRunner.Web.dll"]

