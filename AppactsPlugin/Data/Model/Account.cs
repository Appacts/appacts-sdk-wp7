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
using System.Data.Linq.Mapping;

namespace AppactsPlugin.Data.Model
{
    internal class Account
    {
        #region //Public Properties
        /// <summary>
        /// Gets the account id.
        /// </summary>
        [Column]
        public Guid AccountId  { get; private set;}

        /// <summary>
        /// Gets the application id.
        /// </summary>
        [Column]
        public Guid ApplicationId { get; private set; }
        #endregion

        #region //Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="Account"/> class.
        /// </summary>
        /// <param name="accountId">The account id.</param>
        /// <param name="applicationId">The application id.</param>
        public Account(Guid accountId, Guid applicationId)
        {
            this.AccountId = accountId;
            this.ApplicationId = applicationId;
        } 
        #endregion
    }
}
