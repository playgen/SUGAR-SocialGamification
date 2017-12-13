pushd ..\

docfx metadata docfx.client.metadata.json
docfx metadata docfx.server.metadata.json
docfx build docfx.build.json