#!/usr/bin/env python3
# -*- coding: utf-8 -*-
import os
import sys
import antlr3
import antlr3.tree
import ast

from CSharpParser import CSharpParser
from CSharpLexer import CSharpLexer

def getAst():
  char_stream = antlr3.ANTLRInputStream(sys.stdin)
  lexer = CSharpLexer(char_stream)
  tokens = antlr3.CommonTokenStream(lexer)
  parser = CSharpParser(tokens)
  r = parser.compilation_unit()
  print(r.ast.toStringTree())
  return r.ast

def main():
  ast = getAst()
  # maciej, its your job to generate PNG here

def sample():
  rep = ast.RepresentationV1()
  ast.createSampleV1(rep)
  ast.createPngV1(rep)

if __name__ == '__main__':
  main()

