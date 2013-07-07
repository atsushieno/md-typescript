
function evaluateFromRequest (cmd : string) {
  return eval (cmd);
}

var http = require('http');
var url = require('url');
var port = IO.arguments[0];
var server = null;
server = http.createServer(function (req, res) {
    var u = url.parse("http://localhost" + req.url, true);
    req.setEncoding ('utf-8');
    var data = "";
    req.on ('data', function (chunk) {
        data += chunk.toString ();
    });
    req.on ('end', function () {
        IO.printLine('request: ' + (data.length > 80 ? data.substring (0, 80) + "..." : data));
        var ret : any;
        try {
	        ret = {'result': evaluateFromRequest (data)};
	        var result : any;
    	    try {
	    	    result = JSON.stringify (ret);
	    	} catch (err) {
	            result = JSON.stringify ({'result': null});
	    	}
    	    IO.printLine('response:' + (result.length > 80 ? result.substring (0, 80) + "..." : result));
    	    res.writeHead(200, {
                'Content-Type': 'text/plain'
    	    });
            res.end(result);
    	} catch (err) {
    	    IO.printLine('ERROR response:' + err);
    	    res.writeHead(200, {
    	        'Content-Type': 'text/plain'
    	    });
            ret = {'error': err};
            res.end(JSON.stringify (ret));
    	}
    });
    switch(u.query['command']) {
        case 'quit': {
            res.writeHead(200, {
                'Content-Type': 'text/plain'
            });
            res.end('shutting down');
            server.close();
            process.exit();
            break;

        }
        case 'eval': {
            break;
        }
        default: {
            res.writeHead(400, {
                'Content-Type': 'text/plain'
            });
            res.end('unexpected operation: ' + req.url);
            break;
        }
    }
}).listen(port, 'localhost');
console.log('Server running at http://localhost:' + port + '/');
