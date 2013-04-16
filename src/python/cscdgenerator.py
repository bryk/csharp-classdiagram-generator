#!/usr/bin/env python3
# -*- coding: utf-8 -*-
import os

import sys
import antlr3
import antlr3.tree
from CSharpParser import CSharpParser
from CSharpLexer import CSharpLexer

pngName = "name"
public = "public"
protected = "protected"
private = "private"
package = "package"

class AccessModifier:
  def __init__(self):
    self.access=package
  def setPublic(self):
    self.access=public
  def setProtected(self):
    self.access=protected
  def setPrivate(self):
    self.access=private
    
class Type:
  def __init__(self,name):
    self.name=name

class Attr:
  def __init__(self,name,typeOfAttr,access):
    self.name=name
    self.typeOfAttr=typeOfAttr
    self.access=access

class Parameter:
  def __init__(self,typeOfParam,name):
    self.name=name
    self.typeOfParam=typeOfParam


class Method:
  def __init__(self,name,returnType,access):
    self.name=name
    self.returnType=returnType
    self.access=access
    self.params=[]
    self.abstract=False
  def addParameter(self,param):
    self.params.append(param)  
  def isAbstract(self,val):
    self.abstract = val

class Iface:
  def __init__(self,name):
    self.name=name
    self.extends = []
    self.methods = []
    self.attributes = []
  def setExtend(self,iface):
    self.extends.append(iface)
  def addMethod(self,meth):
    self.methods.append(meth)
  def addAttribute(self,attr):
    self.attributes.append(attr)
    
class Cl:
  def __init__(self,name):
    self.name=name
    self.implement = []
    self.extends = None
    self.abstract = False
    self.methods = []
    self.attributes = []
  def isAbstract(self,val):
    self.abstract = val
  def setExtend(self,cl):
    self.extends = cl
  def addImplement(self,iface):
    self.implement.append(iface)
  def addMethod(self,meth):
    self.methods.append(meth)
  def addAttribute(self,attr):
    self.attributes.append(attr)

class Representation:
  def __init__(self):
    self.classes = []
    self.interfaces = []
  def addClass(self, cl):
    self.classes.append(cl)
  def addInterface(self, iface):
    self.interfaces.append(iface)

def createSample(rep):
  pass

def createPng(rep):
  defs = "[User|+Forename;+Surname;+HashedPassword;-Salt|+Login();+Logout()]"
  os.system("suml --png \""+defs+"\" > pngs/"+pngName+".png")



  
  
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
  
  #rep = Representation()
  #createSample(rep)
  #createPng(rep)
  
  return

def sample():
  rep = Representation()
  createSample(rep)
  createPng(rep)

if __name__ == '__main__':
  #main()
  sample()
