#!/usr/bin/env python3
# -*- coding: utf-8 -*-
import sys
import antlr3
import antlr3.tree
import ast
import pngcreator
import sampleast

from CSharpParser import CSharpParser
from CSharpLexer import CSharpLexer

def getAsts(files):
  asts = []
  for name in files:
    char_stream = antlr3.ANTLRFileStream(name)
    lexer = CSharpLexer(char_stream)
    tokens = antlr3.CommonTokenStream(lexer)
    parser = CSharpParser(tokens)
    r = parser.compilation_unit()
    r.ast.name = name
    asts += [r.ast]
  return asts

def usage():
  print('{0} [file1] [file2] ...\n'.format(sys.argv[0]))

def main():
  if not sys.argv[1:]:
    usage()
  else:
    asts = getAsts(sys.argv[1:])
    rep = ast.Representation()
    for a in asts:
      rep.addFile(a)
      print(a.toStringTree())
    #sampleast.createSampleV2(rep)
    pngcreator.createPng(rep)

if __name__ == '__main__':
  main()

