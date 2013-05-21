using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace AppactsPlugin.Data.Model
{
    public class ExceptionDescriptive : Exception
    {
        #region //Public Properties
        /// <summary>
        /// Gets a string representation of the frames on the call stack at the time the current exception was thrown.
        /// </summary>
        /// <returns>A string that describes the contents of the call stack, with the most recent method call appearing first.</returns>
        public new String StackTrace { get; set; }

        /// <summary>
        /// Gets a collection of key/value pairs that provide additional user-defined information about the exception.
        /// </summary>
        /// <returns>An object that implements the <see cref="T:System.Collections.IDictionary"/> interface and contains a collection of user-defined key/value pairs. The default is an empty collection.</returns>
        public new String Data { get; set; }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        public String Source { get; set; } 
        #endregion

        #region //Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionDescriptive"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public ExceptionDescriptive(String message)
            : base(message)
        {

            this.StackTrace = null;
            this.Data = null;
            this.Source = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionDescriptive"/> class.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public ExceptionDescriptive(Exception ex)
            : base(ex.Message)
        {
            this.Source = null;
            this.StackTrace = ex.StackTrace;
            this.Data = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionDescriptive"/> class.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="data">The data.</param>
        public ExceptionDescriptive(Exception ex, String data)
            : base(ex.Message)
        {

            this.Source = null;
            this.Data = data;
            this.StackTrace = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionDescriptive"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="stackTrace">The stack trace.</param>
        /// <param name="source">The source.</param>
        /// <param name="data">The data.</param>
        public ExceptionDescriptive(String message, String stackTrace,
            String source, String data)
            : base(message)
        {
            this.StackTrace = stackTrace;
            this.Source = source;
            this.Data = data;
        } 
        #endregion
    }
}
