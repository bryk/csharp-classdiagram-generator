# -*- coding: utf-8 -*-
import sampleast
import os
pngName = "name"




public = "public"
protected = "protected"
private = "private"
internal = "internal"
protinternal = "protected internal"

TAB = "  "
class Node(object):
  def toStringTree(self,tabs=""):
    '''This should be implemented in child nodes'''
    raise NotImplementedError

class NamespaceMember:
  def addToNamespace(self, ns):
    raise NotImplementedError

# dodalem 'using' ... bo plik moze miec tez using
class File(Node):
  def __init__(self, name='UnknownFile'):
    self.namespaces = []
    self.classes = []
    self.interfaces= []
    self.structs = []
    self.usings = []
    self.name = name

  def addNamespace(self,namespace):
    self.namespaces.append(namespace)
  
  def addClass(self,cl):
    self.classes.append(cl)
  
  def addInterface(self,iface):
    self.interfaces.append(iface)
  
  def addStructs(self,struct):
    self.structs.append(struct)
  
  def addUsing(self,using):
    self.usings.append(using)
     
  def toStringTree(self,tabs="", name='File'):
    nses = ''
    for ns in self.namespaces:
      nses += ns.toStringTree(tabs+TAB)
    usings = ''
    for us in self.usings:
      usings += us.toStringTree(tabs+TAB)
    classes = ''
    for cl in self.classes:
      classes += cl.toStringTree(tabs+TAB)
    structs = ''
    for st in self.structs:
      structs += st.toStringTree(tabs+TAB)  
    interfaces = ''
    for iface in self.interfaces:
      interfaces += iface.toStringTree(tabs+TAB)
    return '{5}{6} "{7}": [\n{0}{1}{2}{3}{4}{5}]\n'.format(usings,nses,classes,structs,interfaces,tabs, name, self.name)


class Using(Node):
  def __init__(self, name):
    self.name = name   # Oczekuje ze to bedzie mialo format : "NamespaceZewnetrzny.NamespaceSrodkowy.NamespaceWewnetrzny" itp.

  def toStringTree(self,tabs=""):
    return '{1}Using: [{0}]\n'.format(self.name,tabs)  



    
class Namespace(File, NamespaceMember):
  def __init__(self, name):
    super(Namespace,self).__init__(name)
  
  def addToNamespace(self, oth):
    oth.namespaces += [self]

  def toStringTree(self,tabs=""):
    return super(Namespace, self).toStringTree(tabs, 'Namespace')


class Modifier(Node):
  def __init__(self):
    self.access = internal
    self.static = False
    self.abstract = False
  
  def parseFromString(self, s):
    if s == 'static':
      self.static = True
    if s == 'abstract':
      self.abstract = True
    if s in (public, protected, private, protinternal):
      self.access = s
    
  def isStatic(self):
    return self.static

  def isAbstract(self):
    return self.abstract
  
  def toStringTree(self, tabs=""):
    return tabs + self.access + (" static" if self.static else "") + (" abstract" if self.abstract else "")


# to pole ma byc uzytwane jako Typ atrybutu, typ zwracany przez funkcje, albo typ parametru
class Type(Node):
  def __init__(self, name):
    self.name = name.replace(r'[', r'［').replace(r']', '］')
  
  def toStringTree(self, tabs=""):
    return tabs+self.name

# atrybut klasy/interfejsu
# jako typOfAttr nie chce obiektow "Cl" ani "Iface" tylko "Type"
class Attr(Node):
  def __init__(self,name,typeOfAttr,access):
    self.name=name
    self.typeOfAttr=typeOfAttr
    self.access=access
  
  def toStringTree(self,tabs=""):
    access = self.access.toStringTree()
    typeOfAttr = self.typeOfAttr.toStringTree()
    return '{3}Attribute: [{0} {1} {2}]\n'.format(access,typeOfAttr,self.name,tabs)

class Property(Node):
  def __init__(self, name):
    self.name = name
    self.typ = None
    self.modifiers = None

  def setModifiers(self, mods):
    self.modifiers = mods

  def setType(self, ret):
    self.typ = ret

  def addToClass(self, cls):
    cls.addProperty(self)

  def toStringTree(self, tabs=""):
    modifiers = self.modifiers.toStringTree()
    typ = self.typ.toStringTree()
    return '{3}Property: [{0} {1} {2}]\n'.format(modifiers, typ, self.name, tabs)




# parametr wystepujacy w metodzie 
class Parameter(Node):
  def __init__(self,typeOfParam,name):
    self.name=name
    self.typeOfParam=typeOfParam
  def toStringTree(self,tabs=""):
    typeOfParam = self.typeOfParam.name
    return 'param: {0} {1} '.format(typeOfParam,self.name)
    
    
# return type ma miec typ "Type"
# parametry maja typu Parameter
class Method(Node):
  def __init__(self):
    self.name = None
    self.returnType = None
    self.modifiers = None
    self.params = []

  def addToClass(self, cl):
    cl.methods += [self]

  def addParameter(self, param):
    self.params.append(param)  
  
  def setModifiers(self, mods):
    self.modifiers = mods

  def setReturnType(self, ret):
    self.returnType = ret

  def isStatic(self):
    return self.modifiers.isStatic()

  def isAbstract(self):
    return self.modifiers.isAbstract()

  def toStringTree(self, tabs=""):
    returnType = self.returnType.toStringTree()
    params = ""
    for prm in self.params :
      params += prm.toStringTree() + "; "
    params = params[:-2]
    return '{4}Method: [{0} {1} {2} ({3})]\n'.format(self.modifiers.toStringTree(),
        returnType, self.name, params, tabs)


class Index(Method):  # struktura taka jak metody, z ta uwaga ze trzeba dac inne nawiasowanie
  def __init__(self,name,returnType,access):
    super(Index,self).__init__(name,returnType,access)
  def toStringTree(self,tabs=""):
    return '{1}index: [\n{0}{1}]'.format(super(Index,self).toStringTree(tabs+TAB),tabs)


class ClOrIface(Node, NamespaceMember):
  def __init__(self, name):
    self.name = name
    self.methods = []
    self.attributes = []
    self.properties = []
    self.indexes = []
  
  def addMethod(self, meth):     # destruktory name postaci : "~Object" parametry zwyczajne, typ zwracany o nazwie ""
    self.methods.append(meth)
  
  def addAttribute(self, attr):
    self.attributes.append(attr)
  
  def addProperty(self, attr):
    self.properties.append(attr)
  
  def addIndex(self, attr):
    self.attributes.append(attr)
  
  def toStringTree(self, tabs=""):
    attributes = ""
    for atr in self.attributes :
      attributes += atr.toStringTree(tabs+TAB) 
    
    methods = ""
    for meth in self.methods:
      methods += meth.toStringTree(tabs+TAB)
    
    properties =""
    for prop in self.properties :
      properties += prop.toStringTree(tabs+TAB)
    
    indexes = ""
    for ind in self.indexes :
      indexes += ind.toStringTree(tabs+TAB) 
    
    return '{6}ClorIface[{0}{1}\n{2}{3}{4}{5}{6}]\n'.format(tabs,self.name,attributes,methods,properties,indexes,tabs)


class Iface(ClOrIface):
  def __init__(self,name):
    super(Iface,self).__init__(name)
    self.extends = []             # list of Type
  
  def addExtend(self, iface):
    self.extends.append(iface)
  
  def toStringTree(self, tabs=""):
    extends = ""
    if len(self.extends)>0 :
      extends = "extends "
    for iface in self.extends:
      extends += iface.name+", "
    extends = extends[:-2]
    base = super(Iface, self).toStringTree(tabs+TAB)
    return '{3}Iface[{0} {1}\n{2}{3}]\n'.format(self.name, extends, base, tabs)
 
  def addToNamespace(self, ns):
    ns.interfaces += [self]
 

class Cl(ClOrIface):
  def __init__(self, name):
    super(Cl, self).__init__(name)
    self.implement = []     # list of Type
    self.extends = None     # Type
    self.abstract = False

  def setExtend(self, cl):
    self.extends = cl
  
  def addImplement(self, iface):
    self.implement.append(iface)
  
  def toStringTree(self, tabs =""):
    abstract = "abstract" if self.abstract else ""
    extends = ""
    if self.extends :
      extends = "extends "+ self.extends.name
    implement = ""
    if len(self.implement)>0 :
      implement = "implements "
    for iface in self.implement:
      implement += iface.name +", " 
    implement = implement[:-2]  
    base = super(Cl, self).toStringTree(tabs+TAB)
    return '{5}class({0} {1} {2} {3} \n{4}{5})\n'.format(abstract, self.name, extends, implement, base, tabs)

  def addToNamespace(self, ns):
    ns.classes += [self]




class Struct(Cl):  # w zasadzie to samo co klasa
  def __init__(self,name):
    super(Struct,self).__init__(name)
  def toStringTree(self,tabs=""):
    return tabs+'struct(\n{0} {1})'.format(super(Struct,self).toStringTree(tabs+TAB),tabs)


class Representation:
  def __init__(self):
    self.files = []
  def addFile(self, f):
    self.files.append(f)
  def toStringTree(self):
    rep=""
    for f in self.files:
       rep+= f.toStringTree()+"\n"
    return rep

