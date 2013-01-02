/// <reference src='ls-bridge.ts' />

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
    req.on ('data', function (chunk) {
        IO.printLine('request: ' + chunk.toString ());
        try {
	        var ret = evaluateFromRequest (chunk.toString ());
    	    ret = {'result': ret == null ? ret : ret instanceof Object ? "__dummy__" : ret};
    	    try {
	    	    IO.printLine('response:' + JSON.stringify (ret['result']));
	    	} catch (err) {
	    	    IO.printLine('response:' + ret ['result']);
	    	}
    	    res.writeHead(200, {
    	        'Content-Type': 'text/plain'
    	    });
    	} catch (err) {
    	    IO.printLine('ERROR response:' + err);
    	    res.writeHead(200, {
    	        'Content-Type': 'text/plain'
    	    });
    	    ret = {'error': err};
    	}
        res.end(JSON.stringify (ret));
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
