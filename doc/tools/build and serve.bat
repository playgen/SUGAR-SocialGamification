call build_swagger.bat

pushd ..\
docfx --serve docfx.json --port 5940

PAUSE