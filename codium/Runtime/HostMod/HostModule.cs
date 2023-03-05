using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using codium.Compiler.HostModel;

namespace codium.Runtime.HostMod
{
    /// <summary>
    /// Represents a complete host funciton implementation module intended
    /// for bulk registration of host functions, possibly provided by a
    /// third party.
    /// </summary>
    public interface HostModule
        : HostFunctionHandler
    {
        #region Public Properties

        /// <summary>
        /// Host function prototypes defined and implemented by the module.
        /// </summary>
        ReadOnlyCollection<HostFunctionPrototype> HostFunctionPrototypes { get; }

        #endregion
    }
}
