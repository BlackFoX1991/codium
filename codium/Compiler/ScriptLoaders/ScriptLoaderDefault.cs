using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using codium.Runtime.ExceptionModels;

namespace codium.Compiler.ScriptLoaders
{
    internal class ScriptLoaderDefault
        : ScriptLoader
    {
        #region Public Methods

        public List<string> LoadScript(string strResourceName)
        {
            try
            {
                List<string> listSourceLines = new List<string>();

                StreamReader streamReader = new StreamReader(strResourceName);
                while (!streamReader.EndOfStream)
                    listSourceLines.Add(streamReader.ReadLine());
                streamReader.Close();

                return listSourceLines;

            }
            catch (Exception exception)
            {
                throw new codiumException(
                    "Error while loading script '" + strResourceName
                    + "'.", exception);
            }
        }

        #endregion
    }
}
