# -*- coding: utf-8 -*-
import sampleast
import os
pngName = "name"




###
# AccessModifier.name moze miec tylko jedna z czterech wartosci
###
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


class AccessModifier(Node):
  def __init__(self):
    self.access = internal
  
  def set(self, value):
    self.access = value

  def setPublic(self):
    self.access = public
  
  def setProtected(self):
    self.access = protected
  
  def setPrivate(self):
    self.access = private
  
  def setProtectedIndernal(self):
    self.access = protinternal
  
  def toStringTree(self, tabs=""):
    return tabs+self.access


# to pole ma byc uzytwane jako Typ atrybutu, typ zwracany przez funkcje, albo typ parametru
class Type(Node):
  def __init__(self,name):
    self.name=name
  
  def toStringTree(self,tabs=""):
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
  def __init__(self,name,returnType,access):
    self.name=name
    self.returnType=returnType
    self.access=access
    self.params=[]
    self.abstract=False
    self.static=False

  def addToClass(self, cl):
    cl.methods += [self]

  def addParameter(self,param):
    self.params.append(param)  
  
  def isAbstract(self,val):
    self.abstract = val
  
  def isStatic(self,val):
    self.static=val

  def toStringTree(self,tabs=""):
    access = self.access.toStringTree()
    returnType = self.returnType.toStringTree()
    static = "static" if self.static else ""
    abstract = "abstract" if self.abstract else ""
    params = ""
    for prm in self.params :
      params+=prm.toStringTree()+"; "
    params = params[:-2]
    return '{6}Method: [{0} {1} {2} {3} {4}({5})]\n'.format(access,abstract,static,returnType,self.name,params,tabs)


class Index(Method):  # struktura taka jak metody, z ta uwaga ze trzeba dac inne nawiasowanie
  def __init__(self,name,returnType,access):
    super(Index,self).__init__(name,returnType,access)
  def toStringTree(self,tabs=""):
    return '{1}index: [\n{0}{1}]'.format(super(Index,self).toStringTree(tabs+TAB),tabs)


class ClOrIface(Node, NamespaceMember):
  def __init__(self,name):
    self.name=name
    self.methods = []
    self.attributes = []
    self.properties = []
    self.indexes = []
  
  def addToNamespace(self, ns):
    ns.classes += [self]

  def addMethod(self,meth):     # destruktory name postaci : "~Object" parametry zwyczajne, typ zwracany o nazwie ""
    self.methods.append(meth)
  
  def addAttribute(self,attr):
    self.attributes.append(attr)
  
  def addProperty(self,attr):
    self.attributes.append(attr)
  
  def addIndex(self,attr):
    self.attributes.append(attr)
  
  def toStringTree(self,tabs=""):
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
  def addExtend(self,iface):
    self.extends.append(iface)
  def toStringTree(self,tabs=""):
    extends=""
    if len(self.extends)>0 :
      extends = "extends "
    for iface in self.extends:
      extends += iface.name+", "
    extends=extends[:-2]
    base = super(Iface,self).toStringTree(tabs+TAB)
    return '{3}Iface[{0} {1}\n{2}{3}]\n'.format(self.name,extends,base,tabs)
    

class Cl(ClOrIface):
  def __init__(self,name):
    super(Cl,self).__init__(name)
    self.implement = []     # list of Type
    self.extends = None     # Type
    self.abstract = False
  def isAbstract(self,val):
    self.abstract = val
  def setExtend(self,cl):
    self.extends = cl
  def addImplement(self,iface):
    self.implement.append(iface)
  def toStringTree(self, tabs =""):
    abstract = "abstract" if self.abstract else ""
    extends=""
    if self.extends :
      extends = "extends "+ self.extends.name
    implement=""
    if len(self.implement)>0 :
      implement = "implements "
    for iface in self.implement:
      implement+= iface.name +", " 
    implement=implement[:-2]  
    base = super(Cl,self).toStringTree(tabs+TAB)
    return '{5}class({0} {1} {2} {3} \n{4}{5})\n'.format(abstract, self.name,extends,implement,base,tabs)



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

