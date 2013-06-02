LIBDIR=lib
SRCDIR=src
BUILDDIR=build
GRAMMARSDIR=$(SRCDIR)/grammars
PYTHONDIR=$(SRCDIR)/python

all: executable

executable: grammars python oth
	find build/ -maxdepth 1 -name '*.py' | xargs -d '\n' chmod u+x	
	chmod u+x	build/cscdgenerator
	chmod u+x	build/install-libs

oth:
	(cd src/bash && cp -u * ../../build/)

grammars:
	java -jar $(LIBDIR)/antlr-3.5-complete.jar -make -fo $(BUILDDIR) $(GRAMMARSDIR)/*.g 2>/dev/null

python:
	cp $(PYTHONDIR)/*.py $(BUILDDIR)

clean:
	rm -rf `find $(BUILDDIR) | tail -n +2`

.PHONY: clean all

