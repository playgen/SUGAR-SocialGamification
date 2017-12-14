set PORT=59401

pushd ..\

start "" http://localhost:%PORT%

docfx metadata docfx.client.metadata.json
docfx metadata docfx.server.metadata.json
docfx build docfx.build.json --serve --port %PORT%