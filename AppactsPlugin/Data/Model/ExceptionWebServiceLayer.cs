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
    public class ExceptionWebServiceLayer : ExceptionDescriptive
    {
        #region //Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionWebServiceLayer"/> class.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public ExceptionWebServiceLayer(Exception ex)
            : base(ex)
        {

        } 
        #endregion

        #region //Constructor
        /// <summary>
        /// Toes the string.
        /// </summary>
        /// <returns></returns>
        public String toString()
        {
            return String.Concat("ExceptionWebServiceLayer: ", base.ToString());
        } 
        #endregion
    }
}
