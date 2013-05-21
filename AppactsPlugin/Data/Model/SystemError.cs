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
    internal class SystemError
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
        /// Gets the type of the device.
        /// </summary>
        /// <value>
        /// The type of the device.
        /// </value>
        [Column(CanBeNull = true)]
        public DeviceType DeviceType { get; set; }

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
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        [Column]
        public String ExceptionMessage { get; set; }

        /// <summary>
        /// Gets a string representation of the frames on the call stack at the time the current exception was thrown.
        /// </summary>
        /// <returns>A string that describes the contents of the call stack, with the most recent method call appearing first.</returns>
        [Column(CanBeNull = true)]
        public String ExceptionStackTrace { get; set; }

        /// <summary>
        /// Gets a collection of key/value pairs that provide additional user-defined information about the exception.
        /// </summary>
        /// <returns>An object that implements the <see cref="T:System.Collections.IDictionary"/> interface and contains a collection of user-defined key/value pairs. The default is an empty collection.</returns>
        [Column(CanBeNull = true)]
        public String ExceptionData { get; set; }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        [Column(CanBeNull = true)]
        public String ExceptionSource { get; set; }

        /// <summary>
        /// Gets the session id.
        /// </summary>
        [Column]
        public Guid SessionId { get; private set; }
        #endregion

        #region //Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemError"/> class.
        /// </summary>
        public SystemError()
        {
   
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemError"/> class.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="system">The system.</param>
        /// <param name="version">The version.</param>
        public SystemError(Guid applicationId, ExceptionDescriptive ex, AnalyticsSystem system, Guid sessionId, String version)
        {
            this.ApplicationId = applicationId;
            this.Version = version;
            this.DeviceType = system.DeviceType;
            this.ExceptionMessage = ex.Message;
            this.ExceptionData = ex.Data;
            this.ExceptionSource = ex.Source;
            this.ExceptionStackTrace = ex.StackTrace;
            this.DateCreated = DateTime.Now;
            this.SessionId = sessionId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemError"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="applicationId">The application id.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="system">The system.</param>
        /// <param name="dateCreated">The date created.</param>
        /// <param name="sessionId">The session id.</param>
        /// <param name="version">The version.</param>
        public SystemError(int id, Guid applicationId, ExceptionDescriptive ex, 
            AnalyticsSystem system, DateTime dateCreated, Guid sessionId, String version)
        {
            this.Id = id;
            this.ApplicationId = applicationId;
            this.DateCreated = dateCreated;
            this.Version = version;
            this.DeviceType = system.DeviceType;
            this.ExceptionMessage = ex.Message;
            this.ExceptionData = ex.Data;
            this.ExceptionSource = ex.Source;
            this.ExceptionStackTrace = ex.StackTrace;
            this.SessionId = sessionId;
        } 
        #endregion
    }
}
