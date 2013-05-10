from ast import *

# to jest moja wewnetrzna reprezentacja nie bedziesz musial tego uzywac
class RepresentationV1:
  def __init__(self):
    self.classes = []
    self.interfaces = []
    self.structs = []
  def addClass(self, cl):
    self.classes.append(cl)
  def addInterface(self, iface):
    self.interfaces.append(iface)
  def addStruct(self, struct):
    self.structs.append(struct)
    
repV1 = RepresentationV1()

debugMode = False

allIds = dict()
allUsings = dict()
usingDefault = Using("")
defaultUsing = dict()
defaultUsing[""] = usingDefault
allUsings[""] = defaultUsing

public = "public"
protected = "protected"
private = "private"
internal = "internal"
protinternal = "protected internal"

TAB = "  "

def addToNamespaceUsings(name, superName, usings):
  sp = allUsings[superName]
  usi = dict()
  for u in sp:
    usi[u]=sp[u]
  for u in usings:
    usi[u.name]=u
  allUsings[name]=usi

def printIds():
  print("IDS:")
  for ID in allIds :
    print(ID)

def printUsings():
  print("USINGS:")
  for ID in allUsings.keys():
    print(ID)
    for using in allUsings[ID]:
      print("   "+using)


def attrsRep(attributes):
  attrs=";"
  for attr in attributes:
    if attr.access.access is public:
      attrs+="+"
    elif attr.access.access is private:
      attrs+="-"
    elif attr.access.access is protected:
      attrs+="#"
    attrs+=attr.name + " : " + attr.typeOfAttr.name+";"
  attrs = attrs[1:-1]   
  return attrs

def methRep(meths):
  methods=";"
  for method  in meths:
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
  return methods

def ifaceRep(iface):
  iface.strRep = "<<Interface>>;"+iface.pathName
  attrs=attrsRep(iface.attributes)
  methods=methRep(iface.methods)
  if not (attrs is ""):
    iface.strRep += "|"+attrs
  if not (methods is ""):
    iface.strRep += "|"+methods

def classRep(cl):
  if cl.abstract :
    cl.strRep= "<<Abstract>>;"+cl.pathName
  else :
    cl.strRep = cl.name
  attrs=attrsRep(cl.attributes)
  methods=methRep(cl.methods)
  if not (attrs is ""):
    cl.strRep += "|"+attrs
  if not (methods is ""):
    cl.strRep += "|"+methods

def setPathId(f,prefName,prev):
  isFile = False
  if debugMode:
    print(prefName)
  if isinstance(f,Namespace):
    f.pathName=prefName
    addToNamespaceUsings(f.pathName ,prev,f.usings)
    for nspace in f.namespaces:
      setPathId(nspace, prefName+nspace.name+".", f.pathName) 
  else:
    isFile = True
    f.pathName = "$File$"+f.name+"."
    addToNamespaceUsings(f.pathName,"",f.usings)
    for nspace in f.namespaces :
      setPathId(nspace,nspace.name+".",f.pathName)

   #klasy  
  for cl in f.classes:
    
    cl.pathName = prefName+cl.name
    allIds[cl.pathName]=cl
    classRep(cl) 
    repV1.addClass(cl)
    allUsings[cl.pathName] = allUsings[f.pathName]
   #interfejsy
  for iface in f.interfaces:
    iface.pathName = prefName+iface.name 
    allIds[iface.pathName]=iface
    ifaceRep(iface)
    repV1.addInterface(iface)
    allUsings[iface.pathName] = allUsings[f.pathName]
  #structs 
  for struct in f.structs:
    struct.pathName = prefName+struct.name
    allIds[iface.pathName]=iface
    repV1.addStruct(struct)
    allUsings[struct.pathName] = allUsings[f.pathName]

def getFather(obj,father):
  dic = allUsings[obj.pathName]
  if debugMode: 
    print("Obj: "+obj.pathName+"\nFather: "+father)
  for us in dic :
    name = ""
    if not (us is "") :
      name = us + "."
    if debugMode:
      print("Using: "+name)
      print("Using+Father:"+name+father)
    if (allIds.get(name+father)):
      if debugMode:
        print(True)
      return allIds[name+father].strRep
  return "sth"


def createOutString():
  defs=""
  for iface in repV1.interfaces:
    defs = defs + "["+iface.strRep+"], "
  for cl in repV1.classes:
    defs = defs + "["+cl.strRep+"], "
  
  
  for iface in repV1.interfaces:
    for father in iface.extends:
      defs = defs + "["+getFather(iface,father.name)+"]^-.-["+iface.strRep+"], "
      
  for cl in repV1.classes:
    for father in cl.implement:
      defs = defs + "["+getFather(cl,father.name)+"]^-.-["+cl.strRep+"], "
  
  for cl in repV1.classes:
    if cl.extends :
      defs = defs+ "["+getFather(cl,cl.extends.name)+"]^-["+cl.strRep+"], "
  if debugMode:
    print(defs)
  defs=defs[:-2]
  return defs
  
  
def createPng(rep):
  #sampleast.createSample(rep)
  if debugMode:
    print(rep.toStringTree())
  for f in rep.files:
    setPathId(f,"","")
  if debugMode:
    printIds()
    printUsings()


  defs =createOutString()
  if debugMode:
    print(defs)
  os.system("suml --png \""+defs+"\" > pngs/"+pngName+".png")


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


