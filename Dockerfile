FROM microsoft/dotnet

COPY . app/
WORKDIR app/

RUN ["dotnet", "restore"]

WORKDIR src/PlayGen.SUGAR.WebAPI/
RUN ["dotnet", "publish", "-c", "Release", "-o", "out"]

