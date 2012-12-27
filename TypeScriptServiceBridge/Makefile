
TS_TOPDIR = ../external/typescript

SOURCES_TS = \
	$(TS_TOPDIR)/src/services/*.ts \
	$(TS_TOPDIR)/src/compiler/io.ts

all: ls-bridge.js node-ts-server.js

ls-bridge.js: ls.lst ls-bridge.ts
	tsc @ls.lst ls-bridge.ts --out ls-bridge.js

node-ts-server.js: ls.lst ls-bridge.ts node-ts-server.ts
	tsc @ls.lst ls-bridge.ts node-ts-server.ts --out node-ts-server.js

ls.lst: $(SOURCES_TS)
	ls $(SOURCES_TS) > ls.lst

clean:
	rm -f ls-bridge.js node-ts-server.ts ls.lst
