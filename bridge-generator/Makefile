

JURASSIC_DLL = ../external/jurassic/Jurassic/bin/Debug/Jurassic.dll

all: TypeScriptBridge.dll node-ts-server.js V8DebuggerClientBridge.cs

# ts-cs conversion tools and runtime
ts2cs-generator.js: service-binding-sources.lst harness-modified.ts ts2cs-generator.ts
	tsc @service-binding-sources.lst ts2cs-generator.ts -out ts2cs-generator.js

node-ts-server.js: node-ts-server.ts harness-modified.ts
	tsc @service-binding-sources.lst node-ts-server.ts -out node-ts-server.js

TS2CSImplementor.exe: TS2CSImplementor.cs
	mcs -debug TS2CSImplementor.cs

# typescript language service client
TypeScriptBridge.dll: TypeScriptBridge.cs TS2CSImplementor.exe
	mcs -debug -t:library TypeScriptBridge.cs Runtime/*.cs Hosting/*.cs -r:$(JURASSIC_DLL) -o:TypeScriptBridge.dll

TypeScriptBridge.cs: TS2CSImplementor.exe ts2cs_stub.dll
	mono --debug -O=-all TS2CSImplementor.exe ts2cs_stub.dll > TypeScriptBridge.cs

ts2cs_stub.dll: ts2cs_stub.cs
	mcs -debug -t:library ts2cs_stub.cs

ts2cs_stub.cs: ts2cs-generator.js ts2cs-sources.lst
	node ts2cs-generator.js ts2cs-sources.lst > ts2cs_stub.cs

# V8 debugger client
V8DebuggerClientBridge.dll: V8DebuggerClientBridge.cs TS2CSImplementor.exe
	mcs -debug -t:library TypeScriptBridge.cs Runtime/*.cs Hosting/*.cs -r:$(JURASSIC_DLL) -o:TypeScriptBridge.dll

V8DebuggerClientBridge.cs: TS2CSImplementor.exe v8dbg_stub.dll
	mono --debug -O=-all TS2CSImplementor.exe v8dbg_stub.dll > V8DebuggerClientBridge.cs

v8dbg_stub.dll: v8dbg_stub.cs
	mcs -debug -t:library v8dbg_stub.cs

v8dbg_stub.cs: ts2cs-generator.js v8dbg-sources.lst
	node ts2cs-generator.js v8dbg-sources.lst > v8dbg_stub.cs

# ts2cs-generator.ts needs to come after harness-modified.ts, but it is "ls" implementation dependent which comes first. So, do it manually.
#service-binding-sources.lst:
#	ls ../external/typescript/src/compiler/io.ts ../external/typescript/src/services/*.ts harness-modified.ts ts2cs-generator.ts | grep -v diagnosticsParser > service-binding-sources.lst

# service-binding-sources.lst:
# find ../external/typescript/src/ | grep -v /harness | grep \\.ts | grep -v \\.d\\.ts > service-binding-sources.lst


clean:
	rm -rf
		ts2cs-generator.js node-ts-server.js TS2CSImplementor.exe* \
		ts2cs_stub.dll* ts2cs_stub.cs \
		TypeScriptBridge.dll* TypeScriptBridge.cs \
		v8dbg_stub.dll* v8dbg_stub.cs \
		V8DebuggerClientBridge.cs 
