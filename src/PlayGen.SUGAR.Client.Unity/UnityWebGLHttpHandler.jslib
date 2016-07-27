var UnityWebGlHttpHandlerPlugin = { 
	HttpRequest: function (requestString) {
		try {
			var request = JSON.parse(Pointer_stringify(requestString));
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
				.split('\n')
				.forEach(function(pair) {
					var pairsplit = pair.split(': ');
					if (pairsplit != "") {
						headersObj[pairsplit[0]] = pairsplit[1];
					}
				});
			var responseObj = {
				content: JSON.parse(xhr.responseText),
				headers: headersObj,
				statusCode: xhr.status
			};
			var responseString = JSON.stringify(responseObj);
			var buffer = _malloc(lengthBytesUTF8(responseString) + 1);
			writeStringToMemory(responseString, buffer);
			return buffer;
		} catch (exception) {
			return JSON.stringify({ statusCode: 599 });
		}
	}
};

mergeInto(LibraryManager.library, UnityWebGlHttpHandlerPlugin);
