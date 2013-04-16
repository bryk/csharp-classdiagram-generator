#!/usr/bin/env python3
# -*- coding: utf-8 -*-
import sys
import antlr3
import antlr3.tree

from CSharpParser import CSharpParser
from CSharpLexer import CSharpLexer

#TODO(bryk): this is just some piece of dummy code
def main():
  char_stream = antlr3.ANTLRInputStream(sys.stdin)
  lexer = CSharpLexer(char_stream)
  tokens = antlr3.CommonTokenStream(lexer)
  parser = CSharpParser(tokens)
  r = parser.compilation_unit()
  print(r.ast)
  print(r.ast.toStringTree())
  return

if __name__ == '__main__':
  main()

