
all: external/jurassic bridge/TypeScriptBridge.cs

clean: clean-bridge clean-here

clean-bridge:
	make -C bridge clean

clean-here:
	xbuild /t:Clean md-typescript.sln

external/jurassic:
	make checkout-jurassic

checkout-jurassic:
	cd external
	hg clone https://hg.codeplex.com/jurassic || exit 1
	cd jurassic
	hg checkout 362:7f18d6625a84
	patch -i ../../jurassic-mono.patch -p1
	cd ..
	touch jurassic.timestamp

bridge/TypeScriptBridge.cs:
	make -C bridge

pack:
	mdtool setup pack ../monodevelop/main/build/AddIns/TypeScriptBinding/MonoDevelop.TypeScriptBinding.dll
