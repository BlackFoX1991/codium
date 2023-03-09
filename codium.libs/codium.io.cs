
using codium.Compiler.ScriptLoaders;
using codium.Compiler.HostModel;
using codium.Runtime.HostMod;
using codium.Runtime.ArrayHandle;
using System.Collections.ObjectModel;
using System.Reflection;

namespace codium_extension_template
{



    public class codium_io : HostModule
    {
        private static ReadOnlyCollection<HostFunctionPrototype> s_listHostFunctionPrototypes;

        private List<codium_file> codiumFile;
        public codium_io()
        {
            if (s_listHostFunctionPrototypes != null) return;
            codiumFile = new List<codium_file>();
            List<HostFunctionPrototype> listHostFunctionPrototypes
                = new List<HostFunctionPrototype>();
            HostFunctionPrototype hostFunctionPrototype
            = null;

            List<Type> writebytesFunc = new List<Type>() { typeof(int), typeof(AssociativeArray), typeof(int), typeof(int) };



            hostFunctionPrototype = new HostFunctionPrototype(typeof(int), "fopen", typeof(string), typeof(int));
            listHostFunctionPrototypes.Add(hostFunctionPrototype);

            hostFunctionPrototype = new HostFunctionPrototype(null, "fwriteLine", typeof(int), typeof(string));
            listHostFunctionPrototypes.Add(hostFunctionPrototype);

            hostFunctionPrototype = new HostFunctionPrototype(null, "fwrite", typeof(int), typeof(string));
            listHostFunctionPrototypes.Add(hostFunctionPrototype);

            hostFunctionPrototype = new HostFunctionPrototype((typeof(int)), "fread", typeof(int));
            listHostFunctionPrototypes.Add(hostFunctionPrototype);

            hostFunctionPrototype = new HostFunctionPrototype((typeof(string)), "freadLine", typeof(int));
            listHostFunctionPrototypes.Add(hostFunctionPrototype);

            hostFunctionPrototype = new HostFunctionPrototype((typeof(string)), "freadToEnd", typeof(int));
            listHostFunctionPrototypes.Add(hostFunctionPrototype);

            hostFunctionPrototype = new HostFunctionPrototype(null, "fclose", typeof(int));
            listHostFunctionPrototypes.Add(hostFunctionPrototype);


            s_listHostFunctionPrototypes = listHostFunctionPrototypes.AsReadOnly();
        }


        public object OnHostFunctionCall(string strFunctionName, List<object> listParameters)
        {
            switch (strFunctionName)
            {

                case "fopen":
                    codium_file CF = new codium_file((string)listParameters[0], ((int)listParameters[1] == 1 ? codium_file.modus_.file_read : (int)listParameters[1] == 2 ? codium_file.modus_.file_write : codium_file.modus_.NULL));
                    codiumFile.Add(CF);
                    return (codiumFile.Count - 1);
                case "fwrite":
                    if (codiumFile[(int)listParameters[0]].MODE == codium_file.modus_.file_read) throw new Exception("Not possible to write in File Read Mode.");
                    codiumFile[(int)listParameters[0]].fwriter.Write((string)listParameters[1]);
                    return true;
                case "fwriteLine":
                    if (codiumFile[(int)listParameters[0]].MODE == codium_file.modus_.file_read) throw new Exception("Not possible to write in File Read Mode.");
                    codiumFile[(int)listParameters[0]].fwriter.WriteLine((string)listParameters[1]);
                    return true;
                case "fread":
                    if (codiumFile[(int)listParameters[0]].MODE == codium_file.modus_.file_write) throw new Exception("Not possible to read in File Write Mode.");
                    return codiumFile[(int)listParameters[0]].freader.Read();
                case "freadLine":
                    if (codiumFile[(int)listParameters[0]].MODE == codium_file.modus_.file_write) throw new Exception("Not possible to read in File Write Mode.");
                    return codiumFile[(int)listParameters[0]].freader.ReadLine();
                case "freadToEnd":
                    if (codiumFile[(int)listParameters[0]].MODE == codium_file.modus_.file_write) throw new Exception("Not possible to read in File Write Mode.");
                    return codiumFile[(int)listParameters[0]].freader.ReadToEnd();
                case "fclose":
                    if (codiumFile[(int)listParameters[0]].MODE == codium_file.modus_.file_read) codiumFile[(int)listParameters[0]].freader.Close();
                    else codiumFile[(int)listParameters[0]].fwriter.Close();
                    return true;


            }
            return false;
        }

        public ReadOnlyCollection<HostFunctionPrototype> HostFunctionPrototypes
        {
            get
            {
                return s_listHostFunctionPrototypes;
            }
        }
    }
}