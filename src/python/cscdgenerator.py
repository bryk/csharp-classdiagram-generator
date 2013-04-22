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
  astRep = getAst()
  rep = ast.Representation()
  rep.addFile(astRep)
  ast.createPng(rep)

def sample():
  rep = ast.Representation()
  ast.createSample(rep)
  ast.createPng(rep)

if __name__ == '__main__':
  #main()
  sample()
