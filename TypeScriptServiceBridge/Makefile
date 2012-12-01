
TS_TOPDIR = ../external/typescript

SOURCES_TS = \
	$(TS_TOPDIR)/src/services/*.ts \
	$(TS_TOPDIR)/src/compiler/io.ts

all: ls-bridge.js

ls-bridge.js: ls.lst ls-bridge.ts
	tsc @ls.lst ls-bridge.ts --out ls-bridge.js

ls.lst: $(SOURCES_TS)
	ls $(SOURCES_TS) > ls.lst

clean:
	rm -f ls-bridge.js ls.lst
