using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Benday.TfsUtility
{
    [Serializable]
    public class MissingArgumentException : Exception
    {
        // constructors...
        #region MissingArgumentException()
        /// <summary>
        /// Constructs a new MissingArgumentException.
        /// </summary>
        public MissingArgumentException() { }
        #endregion
        #region MissingArgumentException(string message)
        /// <summary>
        /// Constructs a new MissingArgumentException.
        /// </summary>
        /// <param name="message">The exception message</param>
        public MissingArgumentException(string message) : base(message) { }
        #endregion
        #region MissingArgumentException(string message, Exception innerException)
        /// <summary>
        /// Constructs a new MissingArgumentException.
        /// </summary>
        /// <param name="message">The exception message</param>
        /// <param name="innerException">The inner exception</param>
        public MissingArgumentException(string message, Exception innerException) : base(message, innerException) { }
        #endregion
        #region MissingArgumentException(SerializationInfo info, StreamingContext context)
        /// <summary>
        /// Serialization constructor.
        /// </summary>
        protected MissingArgumentException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        #endregion
    }
}
