
TS_TOPDIR = ../external/typescript

HARNESS = ../bridge-generator/harness-modified.ts

SOURCES_TS = \
	$(HARNESS) \
	$(TS_TOPDIR)/src/services/*.ts \
	$(TS_TOPDIR)/src/compiler/io.ts

all: node-ts-server.js

ls-bridge.js: ls.lst $(SOURCES_TS)
	tsc @ls.lst --out ls-bridge.js --sourcemap

node-ts-server.js: ls.lst $(SOURCES_TS) node-ts-server.ts $(HARNESS)
	tsc @ls.lst $(HARNESS) node-ts-server.ts --out node-ts-server.js --sourcemap

ls.lst: $(SOURCES_TS)
	ls $(SOURCES_TS) > ls.lst

clean:
	rm -f ls-bridge.js node-ts-server.js ls.lst

run:
	node node-ts-server.js 36140
