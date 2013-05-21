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
using AppactsPlugin.Data.Model.Enum;

namespace AppactsPlugin.Data.Model
{
    [Table]
    internal class Crash
    {
        #region //Public Properties
        /// <summary>s
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        [Column(IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert)]
        public int Id { get; set; }

        /// <summary>
        /// Gets the application id.
        /// </summary>
        [Column]
        public Guid ApplicationId { get; private set; }

        /// <summary>
        /// Gets or sets the date created.
        /// </summary>
        /// <value>
        /// The date created.
        /// </value>
        [Column]
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Gets the date created formated.
        /// </summary>
        public string DateCreatedFormatted
        {
            get
            {
                return this.DateCreated.ToString("dd/MM/yyyy HH:mm:ss");
            }
        }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        [Column]
        public String Version { get; set; }

        /// <summary>
        /// Gets the session id.
        /// </summary>
        [Column]
        public Guid SessionId { get; private set; }
        #endregion

        #region //Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="Crash"/> class.
        /// </summary>
        public Crash()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Crash"/> class.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="version">The version.</param>
        public Crash(Guid applicationId, Guid sessionId, String version)
        {
            this.ApplicationId = applicationId;
            this.SessionId = sessionId;
            this.Version = version;
            this.DateCreated = DateTime.Now;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Crash"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="applicationId">The application id.</param>
        /// <param name="dateCreated">The date created.</param>
        /// <param name="version">The version.</param>
        public Crash(int id, Guid applicationId, DateTime dateCreated, Guid sessionId, String version)
        {
            this.Id = id;
            this.ApplicationId = applicationId;
            this.SessionId = sessionId;
            this.DateCreated = dateCreated;
            this.Version = version;
        } 
        #endregion
    
    }
}
