call build_swagger.bat

set PORT=5940

pushd ..\

start "" http://localhost:%PORT%

docfx build docfx.json --serve --port %PORT%

PAUSE