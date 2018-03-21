FROM microsoft/dotnet:1.0.0-preview2-sdk

RUN apt-get -qq update && apt-get -y install netcat

COPY . app/
WORKDIR app/

RUN ["dotnet", "restore"]

WORKDIR src/PlayGen.SUGAR.WebAPI/
RUN ["dotnet", "publish", "-c", "Release", "-o", "out"]

ENTRYPOINT ["./delay-startup.sh", "dotnet", "out/PlayGen.SUGAR.WebAPI.dll"]
