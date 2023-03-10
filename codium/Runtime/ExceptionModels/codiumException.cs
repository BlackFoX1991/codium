using System;
using System.Collections.Generic;
using System.Text;

namespace codium.Runtime.ExceptionModels
{
    /// <summary>
    /// Generic script engine exception.
    /// </summary>
    public class codiumException
        : Exception
    {
        #region Private Variables

        private string m_strMessage;
        private Exception m_exceptionInner;

        #endregion

        #region Public Methods

        /// <summary>
        /// Constructs an exception
        /// </summary>
        public codiumException()
        {
            m_strMessage = "No details specified.";
            m_exceptionInner = null;
        }

        /// <summary>
        /// Constructs an exception with the given message.
        /// </summary>
        /// <param name="strMessage">Exception message.</param>
        public codiumException(string strMessage)
        {
            m_strMessage = strMessage;
            m_exceptionInner = null;
        }

        /// <summary>
        /// Constructs an exception with the given message
        /// and inner exception reference.
        /// </summary>
        /// <param name="strMessage">Exception message.</param>
        /// <param name="exceptionInner">Inner exception reference.</param>
        public codiumException(string strMessage, Exception exceptionInner)
        {
            m_strMessage = strMessage;
            m_exceptionInner = exceptionInner;
        }

        /// <summary>
        /// Returns a string representation of the exception.
        /// </summary>
        /// <returns>A string representation of the exception.</returns>
        public override string ToString()
        {
            return MessageTrace;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Exception message.
        /// </summary>
        public new string Message
        {
            get { return m_strMessage; }
        }

        /// <summary>
        /// Complete message trace recursively including any
        /// inner exceptions.
        /// </summary>
        public string MessageTrace
        {
            get
            {
                if (m_exceptionInner != null)
                {
                    string strMessageTrace = m_strMessage + " Reason: ";
                    if (typeof(codiumException).IsAssignableFrom(
                        m_exceptionInner.GetType()))
                        strMessageTrace += ((codiumException)m_exceptionInner).MessageTrace;
                    else
                        strMessageTrace += m_exceptionInner.Message;
                    return strMessageTrace;
                }
                else
                    return m_strMessage;
            }
        }

        #endregion
    }
}
