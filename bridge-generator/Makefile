

JURASSIC_DLL = ../external/jurassic/Jurassic/bin/Debug/Jurassic.dll

all: TypeScriptBridge.dll

TypeScriptBridge.dll: TypeScriptBridge.cs TS2CSImplementor.exe ts2cs_stub.dll
	mcs -debug -t:library TypeScriptBridge.cs Runtime/*.cs Hosting/*.cs -r:$(JURASSIC_DLL) -o:TypeScriptBridge.dll

TypeScriptBridge.cs: TS2CSImplementor.exe ts2cs_stub.dll
	mono --debug -O=-all TS2CSImplementor.exe ts2cs_stub.dll > TypeScriptBridge.cs

TS2CSImplementor.exe: TS2CSImplementor.cs
	mcs -debug TS2CSImplementor.cs

clean:
	rm -rf ts2cs_stub.dll* ts2cs_stub.cs ts2cs-generator.js \
		TS2CSImplementor.exe* TypeScriptBridge.dll* TypeScriptBridge.cs

ts2cs_stub.dll: ts2cs_stub.cs
	mcs -debug -t:library ts2cs_stub.cs

ts2cs_stub.cs: ts2cs-generator.js ts2cs-sources.lst
	node ts2cs-generator.js ts2cs-sources.lst > ts2cs_stub.cs

ts2cs-generator.js: ts2cs-generator-sources.lst harness-modified.ts ts2cs-generator.ts
	tsc @ts2cs-generator-sources.lst -out ts2cs-generator.js

# ts2cs-generator.ts needs to come after harness-modified.ts, but it is "ls" implementation dependent which comes first. So, do it manually.
#ts2cs-generator-sources.lst:
#	ls ../external/typescript/src/compiler/io.ts ../external/typescript/src/services/*.ts harness-modified.ts ts2cs-generator.ts | grep -v diagnosticsParser > ts2cs-generator-sources.lst

# ts2cs-generator-sources.lst:
# find ../external/typescript/src/ | grep -v /harness | grep \\.ts | grep -v \\.d\\.ts > ts2cs-generator-sources.lst

