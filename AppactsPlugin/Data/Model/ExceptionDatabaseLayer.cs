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
    public class ExceptionDatabaseLayer : ExceptionDescriptive
    {
        #region //Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionDatabaseLayer"/> class.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public ExceptionDatabaseLayer(Exception ex)
            : base(ex)
        {

        } 
        #endregion

        #region //Public Methods
        /// <summary>
        /// Toes the string.
        /// </summary>
        /// <returns></returns>
        public String toString()
        {
            return String.Concat("ExceptionDatabaseLayer: ", base.ToString());
        } 
        #endregion
    }
}
