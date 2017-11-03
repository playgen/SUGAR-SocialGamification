FROM microsoft/dotnet

RUN apt-get -qq update && apt-get -y install netcat

COPY . app/
WORKDIR app/

RUN ["dotnet", "restore"]

WORKDIR PlayGen.SUGAR.Server.WebAPI/
RUN ["dotnet", "publish", "--configuration", "Release", "--output", "out", "--framework", "netcoreapp1.1"]

ENTRYPOINT ["./delay-startup.sh", "dotnet", "out/PlayGen.SUGAR.Server.WebAPI.dll"]
