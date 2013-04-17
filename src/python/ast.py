# -*- coding: utf-8 -*-

class Node:
  def toStringTree(self):
    '''This should be implemented in child nodes'''
    raise NotImplementedError

class CompilationUnit(Node):
  def __init__(self):
    self.nses = []

  def toStringTree(self):
    nses = ''
    for ns in self.nses:
      nses += ns.toStringTree()
    return '(compilation_unit: [{0}])'.format(nses)

class Namespace(Node):
  def __init__(self, name):
    self.classes = []
    self.namespaces = []
    self.name = name

  def toStringTree(self):
    classes = ''
    for cls in self.classes:
      classes += cls.toStringTree()
    return '(namespace: {0}, [{1}])'.format(self.name, classes)

class Class:
  pass

class Method:
  pass

