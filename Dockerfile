FROM microsoft/aspnetcore-build:2.0

COPY . app/
WORKDIR app/

RUN ["dotnet", "restore"]

WORKDIR PlayGen.SUGAR.Server.WebAPI/
RUN ["dotnet", "publish", "--configuration", "Release", "--output", "out", "--framework", "netcoreapp2.0"]

ENTRYPOINT ["dotnet", "out/PlayGen.SUGAR.Server.WebAPI.dll"]