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
internal = "internal"

class AccessModifier:
  def __init__(self):
    self.access=internal
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
  def addExtend(self,iface):
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
  #rep = Representation()
  intrn = AccessModifier()
  publ = AccessModifier()
  publ.setPublic()
  priv = AccessModifier()
  priv.setPrivate()
  prot = AccessModifier()
  prot.setProtected()
  void = Type("Void")
  
  breath = Iface("Breath")
  lungs = Type("Lungs")
  lungsA = Attr("lungs",lungs,priv)
  breath.addAttribute(lungsA)
  respire=Method("respire",void,priv)
  breath.addMethod(respire)
  rep.addInterface(breath)
  eat = Iface("Eat")
  mouth = Type("Mouth")
  mouthA = Attr("mouth",mouth,prot)
  energy = Type("Energy")
  consume = Method("consume",energy,priv)
  meal = Type("Meal")
  food=Parameter(meal,"food")
  consume.addParameter(food)
  eat.addMethod(consume)
  eat.addAttribute(mouthA)
  rep.addInterface(eat)
  
  live = Iface("Live")
  live.addExtend(breath)
  live.addExtend(eat)
  body = Type("Body")
  bodyA = Attr("body",body,prot)
  eat.addAttribute(bodyA)
  exist=Method("exist",void,priv)
  live.addMethod(exist)
  rep.addInterface(live)
  
  bite = Iface("Bite")
  teeth = Type("Teeth")
  teethA = Attr("teeth",teeth,priv)
  bite.addAttribute(teethA)
  hurt = Type("Hurt")
  attack=Method("attack",hurt,prot)
  attack.isAbstract(True)
  bite.addMethod(attack)
  rep.addInterface(bite)
  familiar = Iface("Familiar")
  cares = Method("cares",void,intrn)
  cares.isAbstract(True)
  familiar.addMethod(cares)
  rep.addInterface(familiar)
  
  animal = Cl("Animal")
  bone = Type("Bone")
  boneA = Attr("bone",bone,publ)
  animal.addAttribute(boneA)
  animal.addImplement(live)
  move = Method("move",void,prot)
  animal.addMethod(move)
  animal.isAbstract(True)
  rep.addClass(animal)
  
  
  pet = Cl("Pet")
  pet.setExtend(animal)
  pet.addImplement(familiar)
  pet.isAbstract(True)
  friendliness = Type("Friendliness")
  friendlinessA = Attr("friendliness",friendliness,publ)
  pet.addMethod(cares)
  pet.addAttribute(friendlinessA)
  rep.addClass(pet)
  
  wild = Cl("Wild")
  wild.setExtend(animal)
  wild.addImplement(bite)
  wild.isAbstract(True)
  wildness = Type("Wildness")
  wildnessA = Attr("wildness",wildness,publ)
  wild.addAttribute(wildnessA)
  wild.addMethod(attack)
  rep.addClass(wild)
  
  dog = Cl("Dog")
  dog.setExtend(pet)
  caresImpl = Method("cares",void,intrn)
  dog.addMethod(caresImpl)
  huntCat = Method("huntCat", void, prot)
  catT = Type("Cat")
  catP=Parameter(catT,"cat")
  huntCat.addParameter(catP)
  energyT = Type("Energy")
  energyP=Parameter(energyT,"energy")
  huntCat.addParameter(energyP)
  dog.addMethod(huntCat)
  rep.addClass(dog)
  
  cat = Cl("Cat")
  cat.setExtend(pet)
  cat.addMethod(caresImpl)
  mouw = Method("mouw",void,prot)
  cat.addMethod(mouw)
  rep.addClass(cat)
  
  cow = Cl("Cow")
  cow.setExtend(pet)
  milk = Type("Milk")
  milkA = Attr("milk",milk,intrn)
  cow.addAttribute(milkA)
  cow.addMethod(caresImpl)
  giveMilk = Method("giveMilk",milk,publ)
  cow.addMethod(giveMilk)
  rep.addClass(cow)
  
  tiger = Cl("Tiger")
  tiger.setExtend(wild)
  stripe = Type("Stripe")
  stripeA = Attr("stripe",stripe,prot)
  tiger.addAttribute(stripeA)
  attackImpl = Method("attack",hurt,prot)
  tiger.addMethod(attackImpl)
  rep.addClass(tiger)
  
  lion = Cl("Lion")
  lion.setExtend(wild)
  mane = Type("Mane")
  maneA = Attr("mane",mane,prot)
  lion.addAttribute(maneA)
  lion.addMethod(attackImpl)
  rep.addClass(lion)

  

  
  

def createPng(rep):
  #rep = Representation()
  defs =""# "[User|+Forename;+Surname;+HashedPassword;-Salt|+Login();+Logout()]"
  for iface in rep.interfaces:
    iface.strRep = "<<Interface>>;"+iface.name
    attrs=";"
    for attr in iface.attributes:
      if attr.access.access is public:
        attrs+="+"
      elif attr.access.access is private:
        attrs+="-"
      elif attr.access.access is protected:
        attrs+="#"
      attrs+=attr.name + " : " + attr.typeOfAttr.name+";"
    attrs = attrs[1:-1] 
    if not (attrs is ""):
      iface.strRep += "|"+attrs
    
    methods=";"
    for method  in iface.methods:
      meth=""
      if method.abstract :
        meth+="~"
      if method.access.access is public:
        meth+="+"
      elif method.access.access is private:
        meth+="-"
      elif method.access.access is protected:
        meth+="#"
      meth+=method.name + "("
      
      parms="."
      
      for param in method.params:
        parms += param.name+":"+param.typeOfParam.name+"." 
      
      parms=parms[1:-1]
      meth+=parms+"):"+method.returnType.name+";"
      
      methods+=meth+";"
      
    methods=methods[1:-1]
    if not (methods is ""):
      iface.strRep += "|"+methods
    
  for cl in rep.classes:
    if cl.abstract :
      cl.strRep= "<<Abstract>>;"+cl.name
    else :
      cl.strRep = cl.name
    attrs=";"
    for attr in cl.attributes:
      if attr.access.access is public:
        attrs+="+"
      elif attr.access.access is private:
        attrs+="-"
      elif attr.access.access is protected:
        attrs+="#"
      
      attrs+=attr.name + " : " + attr.typeOfAttr.name+";"
    attrs = attrs[1:-1]
    if not (attrs is ""):
      cl.strRep += "|"+attrs
    methods=";"
    for method  in cl.methods:
      meth=""
      if method.abstract :
        meth+="~"
      if method.access.access is public:
        meth+="+"
      elif method.access.access is private:
        meth+="-"
      elif method.access.access is protected:
        meth+="#"
      meth+=method.name + "("
      
      parms="."
      
      for param in method.params:
        parms += param.name+":"+param.typeOfParam.name+"." 
      
      parms=parms[1:-1]
      meth+=parms+"):"+method.returnType.name+";"
      
      methods+=meth+";"
      
    methods=methods[1:-1]
    if not (methods is ""):
      cl.strRep += "|"+methods
    
    
  for iface in rep.interfaces:
    defs = defs + "["+iface.strRep+"], "
    
  for iface in rep.interfaces:
    for father in iface.extends:
      defs = defs + "["+father.strRep+"]^-.-["+iface.strRep+"], "
  for cl in rep.classes:
    for father in cl.implement:
      defs = defs + "["+father.strRep+"]^-.-["+cl.strRep+"], "
  
  for cl in rep.classes:
    if cl.extends :
      defs = defs+ "["+cl.extends.strRep+"]^-["+cl.strRep+"], "
  print(defs)
  defs=defs[:-2]
  print(defs)
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
