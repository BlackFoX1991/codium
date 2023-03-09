using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using codium.Compiler.ScriptLoaders;
using codium.Compiler.HostModel;
using codium.Runtime.HostMod;
using codium.Runtime.ScriptEngine;
using codium.Runtime.Management;
using System.Drawing;
using System.DirectoryServices.ActiveDirectory;
using System.Diagnostics;

#pragma warning disable CA1416
namespace cdm
{
    internal class cnaThread : ScriptLoader, HostFunctionHandler
    {
        ScriptLoader DefLoader;
        ScriptLoader Loader;
        ScriptManager Manager;
        ScriptContext Context;
        Script CurScript;

        string MainScript = string.Empty;
        string SourceCode = string.Empty;
        int curInstruction = -1;

        List<String> Bytecode;


        public cnaThread(string sourcefile)
        {
            try
            {
                MainScript = sourcefile;

                Manager = new();
                Bytecode = new List<String>();

                DefLoader = Manager.Loader;
                Loader = this;


                Manager.Loader = Loader;


                StreamReader streamScript = new StreamReader(sourcefile);
                SourceCode = (streamScript.ReadToEnd());
                streamScript.Close();

                HostFunctionPrototype hostFunctionPrototypePrint
                   = new HostFunctionPrototype(null, "print", (Type)null);
                HostFunctionPrototype gettheline = new HostFunctionPrototype(typeof(string), "getline");
                HostFunctionPrototype hstForegcolor = new HostFunctionPrototype(null, "forecolor", typeof(int));
                HostFunctionPrototype hstClear = new HostFunctionPrototype(null, "clear");
                Manager.RegisterHostFunction(hostFunctionPrototypePrint, this);
                Manager.RegisterHostFunction(gettheline, this);
                Manager.RegisterHostFunction(hstClear, this);
                Manager.RegisterHostFunction(hstForegcolor, this);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }


        }
        public cnaThread() { }

        public string codiumVersion()
        {
            return ScriptManager.COMPILER_VERSION;
        }
        public void LoadAssembly(string Path)
        {
            Assembly assembly = null;
            try
            {
                assembly = System.Reflection.Assembly.LoadFile(
                    Path);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error while loading assembly. Reason: " + exception);
                return;
            }
            Type[] arrayTypes = assembly.GetExportedTypes();
            foreach (Type type in arrayTypes)
            {
                if (!typeof(HostModule).IsAssignableFrom(type))
                    continue;

                ConstructorInfo constructorInfo = null;
                try
                {
                    constructorInfo
                        = type.GetConstructor(new Type[0]);
                }
                catch (Exception)
                {
                    continue;
                }
                try
                {
                    object objectHostModule = constructorInfo.Invoke(new object[0]);
                    HostModule hostModule = (HostModule)objectHostModule;
                    Manager.RegisterHostModule(hostModule);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

            }

        }
        public void RunScript(bool DebugMode = false)
        {

            try
            {

                CurScript = new Script(Manager, null);


                if (CurScript.Functions.Count == 0)
                {
                    Console.WriteLine($"[{MainScript}] Script have not Functions defined.");
                    return;
                }
                if (!CurScript.HasMainFunction())
                {
                    Console.WriteLine($"[{MainScript}] Script does not have Entrypoint 'main'.");
                    return;
                }
                Context = new ScriptContext(CurScript);



                while (!Context.Terminated)
                {
                    if (DebugMode)
                    {
                        for (int i = 0; i < CurScript.Executable.Instructions.Count; i++)
                        {
                            curInstruction++;
                            Console.WriteLine(CurScript.Executable.Instructions[i].ToString());
                            if (Console.ReadLine() == "exit") return;
                            Context.Execute(1);
                        }
                    }
                    else
                    {
                        Context.Execute();
                    }

                }

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }


        }
        public List<string> LoadScript(string strResourceName)
        {
            if (strResourceName != null)
                return DefLoader.LoadScript(strResourceName);
            String strSource = SourceCode;
            strSource = strSource.Replace("\r\n", "\r");
            List<String> listSourceLines = new List<string>();
            listSourceLines.AddRange(strSource.Split('\r'));
            return listSourceLines;
        }
        public object OnHostFunctionCall(string strFunctionName, List<object> listParameters)
        {
            if (strFunctionName == "print")
            {
                //Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(listParameters[0]);
                //Console.ForegroundColor = ConsoleColor.White;
            }
            else if (strFunctionName == "getline")
            {
                return Console.ReadLine();
            }
            else if (strFunctionName == "clear")
            {
                Console.Clear();
            }
            else if (strFunctionName == "forecolor")
            {
                if ((int)listParameters[0] == -1) return (int)Console.ForegroundColor;
                Console.ForegroundColor = (ConsoleColor)listParameters[0];

            }
            return null;
        }
    }
}
