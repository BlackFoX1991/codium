using System;
using System.Collections.Generic;
using System.Text;
using codium.Compiler.LexAnalystic;
using codium.Runtime.ExceptionModels;

namespace codium.Compiler.ExceptionModels
{
    /// <summary>
    /// Exception for script parsing errors.
    /// </summary>
    public class ParserException
        : codiumException
    {
        #region Private Variables

        private Token m_token;

        #endregion

        #region Public Methods

        /// <summary>
        /// Constructs an exception.
        /// </summary>
        public ParserException()
            : base()
        {
            m_token = null;
        }

        /// <summary>
        /// Constructs an exception with the given message.
        /// </summary>
        /// <param name="strMessage">Exception message.</param>
        public ParserException(string strMessage)
            : base(strMessage)
        {
            m_token = null;
        }

        /// <summary>
        /// Constructs an exception with the given message
        /// and inner exception reference.
        /// </summary>
        /// <param name="strMessage">Exception message.</param>
        /// <param name="exceptionInner">Inner exception reference.</param>
        public ParserException(string strMessage, Exception exceptionInner)
            : base(strMessage, exceptionInner)
        {
            m_token = null;
        }

        /// <summary>
        /// Constructs an exception with the given message and
        /// parsing token.
        /// </summary>
        /// <param name="strMessage">Exception message.</param>
        /// <param name="token">Parsing token related to the
        /// exception.</param>
        public ParserException(string strMessage, Token token)
            : base(strMessage + " Line " + token.SourceLine
                + ", character " + token.SourceCharacter + ": "
                + token.SourceText)
        {
            m_token = token;
        }

        #endregion
    }
}
