
all: bridge/TypeScriptBridge.cs bridge/node-ts-server.js

clean: clean-bridge clean-here

clean-bridge:
	make -C bridge clean

clean-here:
	xbuild /t:Clean md-typescript.sln

bridge/TypeScriptBridge.cs bridge/node-ts-server.js:
	cd bridge && make all

run:
	cd ../monodevelop && make run

pack:
	mdtool setup pack ../monodevelop/main/build/AddIns/TypeScriptBinding/MonoDevelop.TypeScriptBinding.dll
