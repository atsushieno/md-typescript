
all: external/jurassic external/ts2cs/src/TypeScriptBridge.cs

external/jurassic:
	make -C checkout-jurassic

checkout-jurassic:
	cd external
	hg clone https://hg.codeplex.com/jurassic || exit 1
	cd jurassic
	hg checkout 362:7f18d6625a84
	patch -i ../../jurassic-mono.patch -p1
	cd ..

external/ts2cs/src/TypeScriptBridge.cs:
	make -C external/ts2cs
	make -C external/ts2cs/src

pack:
	mdtool setup pack ../monodevelop-master/main/build/AddIns/TypeScriptBinding/MonoDevelop.TypeScriptBinding.dll
