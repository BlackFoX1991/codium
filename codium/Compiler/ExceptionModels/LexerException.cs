using System;
using System.Collections.Generic;
using System.Text;
using codium.Runtime.ExceptionModels;

namespace codium.Compiler.ExceptionModels
{
    /// <summary>
    /// Exception for script lexing errors.
    /// </summary>
    public class LexerException
        : codiumException
    {
        #region Public Methods

        /// <summary>
        /// Constructs an exception.
        /// </summary>
        public LexerException()
            : base()
        {
        }

        /// <summary>
        /// Constructs an exception with the given message.
        /// </summary>
        /// <param name="strMessage">Exception message.</param>
        public LexerException(string strMessage)
            : base(strMessage)
        {
        }

        /// <summary>
        /// Constructs an exception with the given message
        /// and inner exception reference.
        /// </summary>
        /// <param name="strMessage">Exception message.</param>
        /// <param name="exceptionInner">Inner exception reference.</param>
        public LexerException(string strMessage, Exception exceptionInner)
            : base(strMessage, exceptionInner)
        {
        }

        /// <summary>
        /// Constructs an exception with the given message, source line,
        /// character position and text line.
        /// </summary>
        /// <param name="strMessage">Exception message.</param>
        /// <param name="iSourceLine">Source line number.</param>
        /// <param name="iSourceCharacter">Source character position.</param>
        /// <param name="strSourceText">Source text line.</param>
        public LexerException(string strMessage,
            int iSourceLine, int iSourceCharacter, string strSourceText)
            : base(strMessage + " Line " + iSourceLine
                + ", character " + iSourceCharacter
                + ": " + strSourceText)
        {
        }

        #endregion
    }
}
