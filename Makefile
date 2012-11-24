
all: external/jurassic

external/jurassic:
	make -C checkout-jurassic

checkout-jurassic:
	cd external
	hg clone https://hg.codeplex.com/jurassic || exit 1
	cd jurassic
	hg checkout 362:7f18d6625a84
	patch -i ../../jurassic-mono.patch -p1
	cd ..
