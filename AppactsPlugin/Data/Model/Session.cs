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
    internal class Session
    {
        #region //Private Properties
        /// <summary>
        /// Gets or sets the date start.
        /// </summary>
        /// <value>
        /// The date start.
        /// </value>
        private DateTime dateStart { get; set; } 

        #endregion
        #region //Public Properties
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public String Name { get; set; } 
        #endregion

        #region //Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="Session"/> class.
        /// </summary>
        public Session()
        {
            this.Name = null;
            this.dateStart = DateTime.Now;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Session"/> class.
        /// </summary>
        /// <param name="screenName">Name of the screen.</param>
        public Session(String screenName)
        {
            this.Name = screenName;
            this.dateStart = DateTime.Now;
        } 
        #endregion

        #region //Public Properties
        /// <summary>
        /// Ends this instance.
        /// </summary>
        /// <returns></returns>
        public long End()
        {
            return (long)(DateTime.Now - this.dateStart).TotalMilliseconds;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            Session objToCompare = obj as Session;

            if (objToCompare != null)
            {
                return this.Name == objToCompare.Name;
            }

            return false;
        } 
        #endregion
    }
}
