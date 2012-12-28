/// <reference src='ls-bridge.ts' />

function evaluateFromRequest (cmd : string) : string {
  return JSON.stringify (eval (cmd));
}

var http = require('http');
var url = require('url');
var port = IO.arguments[0];
var server = null;
server = http.createServer(function (req, res) {
    var u = url.parse("http://localhost" + req.url, true);
    IO.printLine('URL: ' + u);
    IO.printLine('operation: ' + u.query['command']);
    req.setEncoding ('utf-8');
    req.on ('data', function (chunk) {
        IO.printLine('expr: ' + chunk.toString ());
        var ret = evaluateFromRequest (chunk.toString ());
        IO.printLine('ret:' + ret);
        res.writeHead(200, {
            'Content-Type': 'text/plain'
        });
        res.end(ret);
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
