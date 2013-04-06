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

  # this is the root of the AST
  root = r.tree

  print(r)
  print(root)

  nodes = antlr3.tree.CommonTreeNodeStream(root)
  #nodes.setTokenStream(tokens)
  #eval = Eval(nodes)
  #eval.prog()
  return

if __name__ == '__main__':
  main()

