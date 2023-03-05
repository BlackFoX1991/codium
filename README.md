# Codium Scripting Language

Wellcome to the Codium Project, Codium is a lightweigt Scripting Language to create small Scripts as a Extension of your Application. Since Codium is written
in C# you can implement the Codium Library in your .NET Projects and use it Classes to provide kind of API. In this Project you will find 3 Components, cdm, codium itself
and codium.libs. Cdm is the Console App which i created to run Scripts on the fly and mainly test them, codium itself is the main Lib, codium.libs is just an example of how
you can extend codium easily and pass functions to the HostProvider.

# Current Stage of Codium

Please notice Codium is still in it early stages, not finished yet. Currently i plan to add more Features like custom Object in form of Class-Definitions.
There a few other Bugs, like some struggle with Arrays for now. Like i said its in the early stages.

# How does Codium work ?

Codium is a jit-compiler which means it translate the code into Opcode but as you can see it is more like a custom VM. From there it just executes the given Instructions.
Sounds simple, right ? Still the way to get there was hard enough, hours of hours to solve problems. As a Dev you will know what i talking about.


# How to use Codium ?

Before we get to the Code-samples Section you have to know how to implement this Engine and how to use it. We will use C# here to show some examples.

```
// Important to include the Scriptloader, more infos later
using codium.Compiler.ScriptLoaders;
// HostModel and Hostmod, which includes the HostFunctionProvider ( if you want provide Host-Functions for your Scripts otherwise it is obsolete )
using codium.Compiler.HostModel;
using codium.Runtime.HostMod;
// The ScriptEngine itself which includes the whole ScriptContext and so on
using codium.Runtime.ScriptEngine;
// Important since it manages the whole Script including Script-Instructions and so on
using codium.Runtime.Management;
```

Before we start to create everything, there are 2 Classes you can implement in your Application. ScriptLoader, HostFunctionHandler.
To make sure your Script will loaded properly you have to inherit ScriptLoader which brings "LoadScript" to your Application. This Function will be called after you
try to execute your script. Please notice "LoadScript" provides a Parameter with "strRessourceName" which is the Scriptname you can set by creating a Script Object references
to the given ScriptManager. 

Some more Informations related to the ScriptManager, Script Object and the later used ScriptContext. The ScriptManager manages the Loader which in this case should be your
used Class by inheriting it. The Script Object creates a Instruction set which will later run by the ScriptContext. This whole Management will let you loader several
Sourcefiles of your Scripts by giving them different Ressource-Names.


So lets get started to use it in your Application. As i wrote before, we have 4 Object you have to know about.
ScriptManager, which manages your ScriptLoader
ScriptLoader, which holds the Loader of your Sourcefiles which should be inherited in your Class to manage your Scripts also.
Script, which holds your Sourcefile in form of Opcode with a given Name ( can be null also )
ScriptContext which runs Script Instruction by creating Opcode.

So lets create :

```
ScriptLoader Loader;
ScriptManager Manager;
ScriptContext Context;
Script theScript;
```

For now we place it in the root of the class which used to run Scripts and so on.
To make everything work initialize it :

```
Manager = new();
Loader = this;
Manager.Loader = Loader;
```

Create a new ScriptManager Instance, set the Loader to your App Class since we inherited the ScriptLoader and finally set the Loader of the ScriptManager
to the Loader which is set to your App Class. Make sure "LoadScript" is in your App Class also.

```
public List<string> LoadScript(string strResourceName)
{
  String strSource = <SourceCode>;
  strSource = strSource.Replace("\r\n", "\r");
  List<String> listSourceLines = new List<string>();
  listSourceLines.AddRange(strSource.Split('\r'));
  return listSourceLines;
}
```

To give you an example of how it should look like. The Sourcecode should be loaded separately, by file or other source, it doesnt matter. All what matters is
to split the Lines into a List and return it since Codium will process it this way currently. ( It will be changed in further Releases of Codium ).

Lets go back to our other Function and make sure the Script will be loaded.

```
theScript = new Script(Manager,null);
```

Now we have kind of a Library which contains all the Functions and other Declarations of the given Sourcefile.
Make sure your Script does contain any Functions and if this is the Script you want to run it should contain a "main"
Function which is the Entry-Point to even execute it.

To execute your Script just ref your Script to the ScriptContext:
```
Context = new ScriptContext(theScript);
```

This will create a Executable with a List of Opcodes/ Script-Instructions. Context contains a Indicator called "Terminated" now, it will report the state of the
current execution process.

To run the whole Script just loop until it it finished.

```
while(!Context.Terminated)
{
  Context.Execute(1);
}
```

or just do it without a loop like this

```
Context.Execute();
```

The Difference between this 2 kinds of Executions is you can control the whole process by executing step by step or just run the script straight away.
You can decide at least.
This whole Example is just a way to do it. You could load multiple Script at once and run them. It all depends on your needs.


Before we get to the Code-Samples section there are 2 more things, one is the HostFunctionPrototype to include and provide your Application Function in your
Script and the other Thing is to create Libraries in .NET which also including the HostFunctionPrototype to provide them as a Library to your Scripts.

To invoke your own Functions to Codium and provide them to your Script your have to define a HostFunctionPrototype after you created the ScriptManager Object and referenced
it to the Loader. 

```
HostFunctionPrototype hostprint = new HostFunctionPrototype(null, "print", (Type)null);
```

This Code creates a HostFunctionPrototype Object named hostprint and creates a new HostFunctionPrototyp. The later used Function in your script will named "print",
returns nothing ( null ) and also have a Parameter of Type null which means print can take any codium datatype, doesnt matter its a Array, a int , a float and so on.
To provide the datatype you need just use typeof(int) for example which leads to expected datatype int.

to register the Prototype to your Manager just use ( in our case Manager is the used ScriptManager Object ) :

```
Manager.RegisterHostFunction(hostprint,this);
```

Now your Prototype is registered to the ScriptManager Object which uses the callback of your App Class since we inherited HostFunctionHandler before.
Your Class should inherit following function by this :

```
public object OnHostFunctionCall(string strFunctionName, List<object> listParameters)
{
  if (strFunctionName == "print")
  {
    Console.WriteLine(listParameters[0]);
  }
  return null;
}
```

This Function will check which of your Script-Host-Function called and your can decide what the Function will do. The Variable listParameter will contain the given Parameters
given by the Script-Call, if your function returns something please make sure to return the right value and type with return in your if or switch statement ( however you do it :D ).
Note : if you have Arrays as your Parameter please make sure to use the given class of AssociativeArray in Codium which can be found in codium.Runtime.ArrayHandle.
       just use typeof(AssociativeArray) in your Prototype Declaration. Thats all, codium will take care of the Rest since listParameters is a Object Variable in OnHostFunctionCall
       it will be handled already.
       
       
Since we can load this kind of Function from another Library you should take a look at "LoadAssembly" which i provided in "cdm" already. There you have some simple Processes.
Loading the Assembly with Reflection Feature, check if its a HostModule Type of Codium, if not it will not loaded and all this stuff.
The Trick is to inherit from the HostModule which can be found in codium.Runtime.Hostmod. Create a static Object of HostFunctionPrototypes, add your Prototypes and inherit
the OnHostFunctionCall Function to make sure your Functions do what you want to do. I could explain it even further but just take a look at codium.libs Project which is included already.


Finally. Lets take a look at Codium and the Syntax.

```

// Single line Comment

/*

  Multi Line Comment 
  
  */
function SortArray(array)
{
    var length = array.size;
    var temp   = array[0];
    for (var i = 0; i < length; i++) // Schleife 1
    {
        for (var j = i+1; j < length; j++) // Schleife 2
        {
            if (array[i] > array[j])
            {
                temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }
        }
    }
    return array;        
}

function main()
{
	var liste = {3,1,8,2,6,9,10,7,4,5};
	print(SortArray(liste));
	var arrayObject = {};
	arrayObject["Test"] = 5;
	print(arrayObject["Test"]);
	print(arrayObject.Test);
	arrayObject.Test = 5;
	arrayObject.varp = 6;
	print(arrayObject["varp"]);
	
	var ob = "";
	foreach(ob in arrayObject)
	{
		print(ob);
	}
	
	for(var i = 0;i<200;i++)
		print("For Count is " + i);
		
	
	if(i == 200)
		print("End Reached ? No, 200!");
}
```


This is just a small Example of the Syntax. A Documentation will follow soon.


If there further Question, just ask. If you running into Issues just ask, if you need a Cookie ... ok Cookies are all mine :D
