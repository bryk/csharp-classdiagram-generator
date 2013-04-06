LIBDIR=lib
SRCDIR=src
BUILDDIR=build
GRAMMARSDIR=$(SRCDIR)/grammars
PYTHONDIR=$(SRCDIR)/python

all: executable

executable: grammars python
	find build/ -name '*.py' | xargs -d '\n' chmod u+x	

grammars:
	java -jar $(LIBDIR)/antlr-3.5-complete.jar -make -fo $(BUILDDIR) $(GRAMMARSDIR)/*.g

python:
	cp $(PYTHONDIR)/*.py $(BUILDDIR)

clean:
	rm -rf `find $(BUILDDIR) | tail -n +2`

.PHONY: clean all

