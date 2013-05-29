csharp-classdiagram-generator
=============================

This is a simple class diagram generator for C# 4.0 language. It is a command line tool and requires Python3 to run.

# Running cscdgenerator

If you want to install scruffy:
```
$ cd lib
$ ./initscruffy
```

If you don't have antlr3 runtime installed (which is most propable):
```bash
$ cd <repository_directory>
$ source virtualenv/bin/activate
$ ./build/cscdgenerator.py
```

Alternatively, if you have all the development libraries, just run `cscdgenerator.py`

Running?

After you installed scruffy and csdgenerator, just type ./run

```bash
$ ./run
MANUAL
 OPTIONS
  -h  help
  -d  debug
  -o=outputDir/ (required)
  -s=sourceDir (oequired)
 SAMPLE USAGE
  ./run -h
  ./run -d -o=image -s=ins
  ./run -o=image -s=ins/
 PARAMETERS SHOULD BE PASSED IN ORDER!
```
