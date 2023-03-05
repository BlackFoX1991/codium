using cdm;
using System.Threading;
using codium.Compiler;
using codium.Runtime;
using System.Security.Cryptography;
class program
{

    public static void Main(string[] args)
    {

        cnaThread cna;

        List<string> AsmFiles = new List<string>();

        Console.WriteLine("Codium - Programming Language (c) 2023 Artur Loewen\n\n");
        if (args.Length == 0)
            Console.WriteLine("No Arguments, use -h for more Informations");
        else
        {
            switch (args[0])
            {
                case "-h":
                    Console.Write("[Menu -> Help]\n\n-r [-f <path of script> ] (-d) (-l <path of Library>)\n\n-Optional Parameters :\n-d\tenables Debug-mode\n-l\tIncludes a specific and supported Codium Library ( See Docs. )\n\nImportant Parameter:\n-f\tThe Script to execute\n\n ");
                    break;
                case "-r":

                    /*
                     *  0 = NULL
                     *  1 = SCRIPTFILE
                     *  2 = LIBRARY
                     */
                    int config_state = 0;
                    AsmFiles.Clear();
                    bool dbg = false;
                    string filename = "";
                    for (int i = 1; i < args.Length; i++)
                    {
                        if (config_state == 0)
                        {
                            switch (args[i])
                            {
                                case "-f":
                                    config_state = 1;
                                    break;
                                case "-l":
                                    config_state = 2;
                                    break;
                                case "-d":
                                    if (dbg)
                                        Console.WriteLine("Warning : Debug-Mode already set.");
                                    dbg = true;
                                    break;
                                default:
                                    Console.WriteLine($"Warning : Unknown Flag '{args[i]}'.");
                                    break;
                            }
                        }
                        else
                        {
                            switch (config_state)
                            {
                                case 1:
                                    filename = args[i];
                                    config_state = 0;
                                    break;
                                case 2:
                                    if (!File.Exists(args[i]) || !args[i].EndsWith(".dll"))
                                    {
                                        Console.WriteLine("Error : Library not added, does not exist or not a DLL File.");
                                    }
                                    else
                                    {
                                        AsmFiles.Add(args[i]);
                                    }
                                    config_state = 0;
                                    break;
                                default:
                                    Console.WriteLine("Warning : Unknown Parameter.");
                                    break;
                            }
                        }

                    }
                    if (filename.Trim() == string.Empty)
                        Console.WriteLine("Error : No Scriptfile, use -f <filename> in -r Command");
                    else
                    {

                        cna = new cnaThread(filename);
                        foreach (string a in AsmFiles)
                            cna.LoadAssembly(a.Trim());
                        cna.RunScript(dbg);
                    }
                    break;

                case "-v":
                    cna = new cnaThread();
                    Console.WriteLine(cna.codiumVersion());
                    break;

            }
        }


    }

}
