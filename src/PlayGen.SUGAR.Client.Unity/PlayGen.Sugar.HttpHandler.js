(function() {
		if (window.PlayGen == undefined) {
			window.PlayGen = {};
		}
		if (window.PlayGen.Sugar == undefined) {
			window.PlayGen.Sugar = {};
		}
		window.PlayGen.Sugar.HttpHandler = function(requestString) {
			try {
				var request = JSON.parse(requestString);
				var xhr = new XMLHttpRequest();
				xhr.open(request.method, request.url, false);
				if (Object.getOwnPropertyNames(request.headers).indexOf("Authorisation") > -1) {
					xhr.withCredentials = true;
				}
				Object.getOwnPropertyNames(request.headers)
					.forEach(function(headerName) {
						xhr.setRequestHeader(headerName, request.headers[headerName]);
					});
				xhr.send(JSON.stringify(request.payload));

				var headersObj = {};
				xhr.getAllResponseHeaders()
					.split("\n")
					.forEach(function(pair) {
						var pairsplit = pair.split(": ");
						if (pairsplit != "") {
							headersObj[pairsplit[0]] = pairsplit[1];
						}
					});
				var responseObj = {
					content: JSON.parse(xhr.responseText),
					headers: headersObj,
					statusCode: xhr.status
				};
				return JSON.stringify(responseObj);
			} catch (exception) {
				return JSON.stringify({ statusCode: 599 });
			}
		};
	}
)();