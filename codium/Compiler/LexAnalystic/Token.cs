using System;
using System.Collections.Generic;
using System.Text;

namespace codium.Compiler.LexAnalystic
{
    /// <summary>
    /// Represents a lexing token generated by the script lexer and used
    /// by the parser.
    /// </summary>
    public class Token
    {
        #region Private Variables

        private TokenType m_tokenType;
        private object m_objectLexeme;
        private int m_iSourceLine;
        private int m_iSourceChar;
        private string m_strSourceLine;

        #endregion

        #region Public Methods

        /// <summary>
        /// Creats a token with the given <see cref="TokenType"/>, lexeme,
        /// source line and character position and source text for the given
        /// line.
        /// </summary>
        /// <param name="tokenType">The token's <see cref="TokenType"/>.
        /// </param>
        /// <param name="objectLexeme">The lexeme associated with the token.
        /// </param>
        /// <param name="iSourceLine">The source line number where the token
        /// occurs.</param>
        /// <param name="iSourceChar">The source character number where the
        /// token occurs.</param>
        /// <param name="strSourceLine">The source text line where the token
        /// occurs.</param>
        public Token(TokenType tokenType, object objectLexeme,
            int iSourceLine, int iSourceChar, string strSourceLine)
        {
            m_tokenType = tokenType;
            m_objectLexeme = objectLexeme;
            m_iSourceLine = iSourceLine;
            m_iSourceChar = Math.Max(0, iSourceChar - objectLexeme.ToString().Length - 1);
            m_strSourceLine = strSourceLine;
        }

        /// <summary>
        /// Returns a string representation of the token.
        /// </summary>
        /// <returns>String representation of the token.</returns>
        public override string ToString()
        {
            return m_tokenType + " (\"" + m_objectLexeme.ToString() + "\")";
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The token's <see cref="TokenType"/>.
        /// </summary>
        public TokenType Type
        {
            get { return m_tokenType; }
        }

        /// <summary>
        /// The lexeme associated with the token.
        /// </summary>
        public object Lexeme
        {
            get { return m_objectLexeme; }
        }

        /// <summary>
        /// The source line number where the token occurs.
        /// </summary>
        public int SourceLine
        {
            get { return m_iSourceLine; }
        }

        /// <summary>
        /// The source character position where the token occurs.
        /// </summary>
        public int SourceCharacter
        {
            get { return m_iSourceChar; }
        }

        /// <summary>
        /// The source text line where the token occurs.
        /// </summary>
        public string SourceText
        {
            get { return m_strSourceLine; }
        }

        #endregion
    }
}
