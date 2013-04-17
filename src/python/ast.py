# -*- coding: utf-8 -*-

import os
pngName = "name"
allIds = dict()
###
# AccessModifier.name moze miec tylko jedna z czterech wartosci
###
public = "public"
protected = "protected"
private = "private"
internal = "internal"
protinternal = "protected internal"

TAB = "  "
class Node:
  def toStringTree(self,tabs=""):
    '''This should be implemented in child nodes'''
    raise NotImplementedError
# dodalem 'using' ... bo plik moze miec tez using
class File(Node):
  def __init__(self,name):
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
     
  def toStringTree(self,tabs=""):
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
    return '{5}File: [\n{0}{1}{2}{3}{4}{5}]\n'.format(usings,nses,classes,structs,interfaces,tabs)


class Using(Node):
  def __init__(self, name):
    self.name = name   # Oczekuje ze to bedzie mialo format : "NamespaceZewnetrzny.NamespaceSrodkowy.NamespaceWewnetrzny" itp.
  def toStringTree(self,tabs=""):
    return '{1}Using: [{0}]\n'.format(self.name,tabs)  
    
class Namespace(File):
  def __init__(self, name):
    super(Namespace,self).__init__(name)
  def toStringTree(self,tabs=""):
    return '{2}namespace: {0}, {1}'.format(self.name, super(Namespace,self).toStringTree(tabs),tabs)


class AccessModifier(Node):
  def __init__(self):
    self.access=internal
  def setPublic(self):
    self.access=public
  def setProtected(self):
    self.access=protected
  def setPrivate(self):
    self.access=private
  def setProtectedIndernal(self):
    self.access=protinternal
  def toStringTree(self,tabs=""):
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
    return '{3}attribute: [{0} {1} {2}]\n'.format(access,typeOfAttr,self.name,tabs)

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
  def addParameter(self,param):
    self.params.append(param)  
  def isAbstract(self,val):
    self.abstract = val
  def isStatic(self,val):
    self.static=val
  def toStringTree(self,tabs=""):
    access = self.access.access
    returnType = self.returnType.toStringTree()
    static = "static" if self.static else ""
    abstract = "abstract" if self.abstract else ""
    params = ""
    for prm in self.params :
      params+=prm.toStringTree()+"; "
    params = params[:-2]
    return '{6}function: [{0} {1} {2} {3} {4}({5})]\n'.format(access,abstract,static,returnType,self.name,params,tabs)

class Index(Method):  # struktura taka jak metody, z ta uwaga ze trzeba dac inne nawiasowanie
  def __init__(self,name,returnType,access):
    super(Index,self).__init__(name,returnType,access)
  def toStringTree(self,tabs=""):
    return '{1}index: [\n{0}{1}]'.format(super(Index,self).toStringTree(tabs+TAB),tabs)
    
class ClOrIface(Node):
  def __init__(self,name):
    self.name=name
    self.methods = []
    self.attributes = []
    self.properties = []
    self.indexes = []
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
  
  basicFunctions = File("BasicFunctions")
  rep.addFile(basicFunctions)
  
  breathing = Namespace("Breathing")
  basicFunctions.addNamespace(breathing)
  
  breath = Iface("Breath")
  breathing.addInterface(breath)
  lungs = Type("Lungs")
  lungsA = Attr("lungs",lungs,priv)
  breath.addAttribute(lungsA)
  respire=Method("respire",void,priv)
  breath.addMethod(respire)

  eating = Namespace("Eating")
  basicFunctions.addNamespace(eating)
  eat = Iface("Eat")
  eating.addInterface(eat)
  mouth = Type("Mouth")
  mouthA = Attr("mouth",mouth,prot)
  energy = Type("Energy")
  consume = Method("consume",energy,priv)
  meal = Type("Meal")
  food=Parameter(meal,"food")
  consume.addParameter(food)
  eat.addMethod(consume)
  eat.addAttribute(mouthA)
  body = Type("Body")
  bodyA = Attr("body",body,prot)
  eat.addAttribute(bodyA)

  
  liveFunctions = File("LiveFunctions")
  rep.addFile(liveFunctions)
  usingBreathing = Using("Breathing")
  liveFunctions.addUsing(usingBreathing)
  usingEating = Using("Eating")
  liveFunctions.addUsing(usingEating)
  living = Namespace("Living")
  liveFunctions.addNamespace(living)
  live = Iface("Live")
  living.addInterface(live)
  breathT = Type("Breath")
  eatT=Type("Eat")
  live.addExtend(breathT)
  live.addExtend(eatT)
  exist=Method("exist",void,priv)
  live.addMethod(exist)

  
  biteFile = File("Bite")
  rep.addFile(biteFile)
  biteNamespace = Namespace("Bite")
  biteFile.addInterface(biteNamespace);
  bite = Iface("Bite")
  biteNamespace.addInterface(bite)
  teeth = Type("Teeth")
  teethA = Attr("teeth",teeth,priv)
  bite.addAttribute(teethA)
  hurt = Type("Hurt")
  attack=Method("attack",hurt,prot)
  attack.isAbstract(True)
  bite.addMethod(attack)

  familiarFile = File("Familiar")
  rep.addFile(familiarFile)
  familiar = Iface("Familiar")
  familarNamespace = Namespace("Familiar")
  familiarFile.addNamespace(familarNamespace)
  familarNamespace.addInterface(familiar)
  cares = Method("cares",void,intrn)
  cares.isAbstract(True)
  familiar.addMethod(cares)

  
  
  animalFile = File("Animal")
  rep.addFile(animalFile)
  usingLiving = Using("Living")
  animalFile.addUsing(usingLiving)
  animal = Cl("Animal")
  animalFile.addClass(animal)
  bone = Type("Bone")
  boneA = Attr("bone",bone,publ)
  animal.addAttribute(boneA)
  liveT = Type("Live")
  animal.addImplement(liveT)
  move = Method("move",void,prot)
  animal.addMethod(move)
  animal.isAbstract(True)

  
  petFile = File("Pet")
  petNamespace = Namespace("Pet")
  familiarNT = Using("Familiar")
  petNamespace.addUsing(familiarNT)
  petFile.addNamespace(petNamespace)
  rep.addFile(petFile)
  animalT = Type("Animal")
  
  pet = Cl("Pet")
  petNamespace.addClass(pet)
  pet.setExtend(animalT)
  pet.addImplement(familiar)
  pet.isAbstract(True)
  friendliness = Type("Friendliness")
  friendlinessA = Attr("friendliness",friendliness,publ)
  pet.addMethod(cares)
  pet.addAttribute(friendlinessA)

  wildFile = File("Wild")
  wildNamespace=Namespace("Wild")
  wildFile.addNamespace(wildNamespace)
  rep.addFile(wildFile)  
  wild = Cl("Wild")
  wildNamespace.addClass(wild)
  wild.setExtend(animalT)
  biteT=Type("Bite")
  wild.addImplement(biteT)
  wild.isAbstract(True)
  wildness = Type("Wildness")
  wildnessA = Attr("wildness",wildness,publ)
  wild.addAttribute(wildnessA)
  wild.addMethod(attack)

  dogFile = File("Dog")
  rep.addFile(dogFile)
  usingPet = Using("Pet")
  dogFile.addUsing(usingPet)
  dog = Cl("Dog")
  dogFile.addClass(dog)
  petT= Type("Pet")
  dog.setExtend(petT)
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

  catFile = File("Cat")
  rep.addFile(catFile)
  usingPetC = Using("Pet")
  catFile.addUsing(usingPetC)
  cat = Cl("Cat")
  catFile.addClass(cat)
  cat.setExtend(petT)
  cat.addMethod(caresImpl)
  mouw = Method("mouw",void,prot)
  cat.addMethod(mouw)

  cowFile = File("Cow")
  rep.addFile(cowFile)
  usingPetCow = Using("Pet")
  cowFile.addUsing(usingPetCow)  
  cow = Cl("Cow")
  cowFile.addClass(cow)
  cow.setExtend(petT)
  milk = Type("Milk")
  milkA = Attr("milk",milk,intrn)
  cow.addAttribute(milkA)
  cow.addMethod(caresImpl)
  giveMilk = Method("giveMilk",milk,publ)
  cow.addMethod(giveMilk)

  tigerFile = File("Tiger")
  rep.addFile(tigerFile)
  usingWild = Using("Wild")
  tigerFile.addUsing(usingWild)
  tiger = Cl("Tiger")
  tigerFile.addClass(tiger)
  wildT = Type("Wild")
  tiger.setExtend(wildT)
  stripe = Type("Stripe")
  stripeA = Attr("stripe",stripe,prot)
  tiger.addAttribute(stripeA)
  attackImpl = Method("attack",hurt,prot)
  tiger.addMethod(attackImpl)

  lionFile = File("Lion")
  rep.addFile(lionFile)
  lionFile.addUsing(usingWild)
  lion = Cl("Lion")
  lionFile.addClass(lion)
  lion.setExtend(wild)
  mane = Type("Mane")
  maneA = Attr("mane",mane,prot)
  lion.addAttribute(maneA)
  lion.addMethod(attackImpl)


def setPathId(f,name):
  if isinstance(f,Namespace):
    for nspace in f.namespaces :
      setPathId(nspace, name+"."+f.name)
   #klasy
    for cl in f.classes:
      cl.pathName = name+"."+f.name+"."+cl.name
      if cl.pathName[0].startswith('.'):
        cl.pathName=cl.pathName[1:]
      allIds[cl.pathName]=cl  
   #interfejsy
    for cl in f.interfaces:
      cl.pathName = name+"."+f.name+"."+cl.name
      if cl.pathName[0].startswith('.'):
        cl.pathName=cl.pathName[1:]
      allIds[cl.pathName]=cl 
  else :
    for nspace in f.namespaces :
      setPathId(nspace, name)
    for cl in f.classes:
      cl.pathName = cl.name
      allIds[cl.pathName]=cl
    for iface in f.interfaces:
      iface.pathName = iface.name
      allIds[iface.pathName]=iface    

def createPng(rep):
  #print(rep.toStringTree())
  for f in rep.files:
    setPathId(f,"")
  for ID in allIds :
    print(ID)
    
def createPngX(rep):
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


class RepresentationV1:
  def __init__(self):
    self.classes = []
    self.interfaces = []
  def addClass(self, cl):
    self.classes.append(cl)
  def addInterface(self, iface):
    self.interfaces.append(iface)

def createSampleV1(rep):
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

def createPngV1(rep):
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
