var UnityWebGlHttpHandlerPlugin = { 
	HttpRequest: function (requestPointer) {
		var debug = false;
		debug && console.debug("HttpRequest");
		try {
			var requestString = Pointer_stringify(requestPointer);
			debug && console.debug("HttpRequest::requestString::" + requestString);
			
			var request = JSON.parse(requestString);
			debug && console.debug("HttpRequest::XMLHttpRequest::request" + request);
			
			debug && console.debug("HttpRequest::XMLHttpRequest::Open");
			var xhr = new XMLHttpRequest();	
			xhr.open(request.method, request.url, false);

			debug && console.debug("HttpRequest::XMLHttpRequest::withCredentials");
			var headers = Object.getOwnPropertyNames(request.headers);
			
			if (headers.indexOf("Authorization") > -1 && headers["Authorization"]) {
				debug && console.debug("HttpRequest::XMLHttpRequest::authHeaderFound");
				xhr.withCredentials = true;
			}

			xhr.setRequestHeader('Content-Type', 'application/json');
			
			debug && console.debug("HttpRequest::XMLHttpRequest::setRequestHeader");
			Object.getOwnPropertyNames(request.headers)
				.forEach(function(headerName) {
					xhr.setRequestHeader(headerName, request.headers[headerName]);
				});
			
			debug && console.debug("HttpRequest::XMLHttpRequest::send");
			xhr.send(request.content);
			
			debug && console.debug("HttpRequest::XMLHttpRequest::getAllResponseHeaders");
			var headersObj = {};
			xhr.getAllResponseHeaders()
				.split(/\r|\n/)
				.forEach(function(pair) {
					var pairsplit = pair.split(': ');
					if (pairsplit != "") {
						headersObj[pairsplit[0]] = pairsplit[1];
					}
				});
			
			debug && console.debug("HttpRequest::responseObj");
			var responseObj = {
				content: xhr.responseText,
				headers: headersObj,
				statusCode: xhr.status
			};
			var responseString = JSON.stringify(responseObj);
			
			debug && console.debug("HttpRequest::responseString::" + responseString);
			var buffer = _malloc(lengthBytesUTF8(responseString) + 1);
			
			debug && console.debug("HttpRequest::writeStringToMemory");
			writeStringToMemory(responseString, buffer);
			return buffer;
		} catch (exception) {
			debug && console.debug("HttpRequest::Exception::" + exception);
			return JSON.stringify({ statusCode: 599 });
		}
	}
};

mergeInto(LibraryManager.library, UnityWebGlHttpHandlerPlugin);
