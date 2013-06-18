csharp-classdiagram-generator
=============================

This is a simple class diagram generator for C# 4.0 language. It is a command line tool and requires Python3 & Python2 to run.

# Installing cscdgenerator

```bash
$ mkdir csharp-classdiagram-generator
$ cd csharp-classdiagram-generator
$ git init
$ git pull https://github.com/bryk/csharp-classdiagram-generator.git
$ make
$ sudo ./build/install-libs
```

# Running cscdgenerator

```bash
$ ./build/cscdgenerator -o=outputFilename -s=sourceDir
```

```
1. -o=outputFile  
   NOT OPTIONAL 
   Destination path to output png file.
   Output path should EXIST.
   If output directory doesn't exist then file will NOT be created.
   
   outputFile.png - created png file
   
   If path contains white spaces ' ' then outputFile should be enclosed in '"'
   -o="my output dir/my file"
   As a result: 
   "my file.png" in "my output dir" directory will be created
   If "my output dir" directory doesn't exist then file will NOT be created.
   
2. -s=sourceDir
   NOT OPTIONAL 
   Source path to directory containing .cs files.
   Recursively scans nested directories.
   Destination path should EXIST.
   If path doesn't exist then find will return error messages.
   
   If path contains spaces ' ' then source dir should be enclosed in '"'
   -s="my main source dir/source dir"
  
   Forward slash '/' is optional at the end of source dir.
   ./build/cscdgenerator -o=image -s=ins
   ./build/cscdgenerator -o=image -s=ins/
   Both of them are valid examples of usage.
  
  
3. -h
   Displays MANUAL
   Default response - If no arguments given or arguments are invalid
   
4. -d 
   Displays debugging messages during proccessing.
   Contains:
     Internal human readable class representation.
     String representation of diagram passed to scruffy.
```
   

```bash
$ ./build/cscdgenerator
```
```
MANUAL
 OPTIONS
  -h  help
  -d  debug
  -o=outputFilename (required)
  -s=sourceDir (oequired)
 SAMPLE USAGE
  ./cscdgenerator -h
  ./cscdgenerator -d -o=image -s=ins
  ./cscdgenerator -o=image -s=ins/
 PARAMETERS SHOULD BE PASSED IN ORDER!
```

