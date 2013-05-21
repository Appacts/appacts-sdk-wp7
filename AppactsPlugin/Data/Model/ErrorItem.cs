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
    internal class ErrorItem
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
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        [Column(CanBeNull = true)]
        public String Data { get; set; }

        /// <summary>
        /// Gets or sets the size of the available flash drive.
        /// </summary>
        /// <value>
        /// The size of the available flash drive.
        /// </value>
        [Column(CanBeNull = true)]
        public long AvailableFlashDriveSize { get; set; }

        /// <summary>
        /// Gets or sets the size of the available memory.
        /// </summary>
        /// <value>
        /// The size of the available memory.
        /// </value>
        [Column(CanBeNull = true)]
        public long AvailableMemorySize { get; set; }

        /// <summary>
        /// Gets or sets the battery.
        /// </summary>
        /// <value>
        /// The battery.
        /// </value>
        [Column(CanBeNull = true)]
        public int Battery { get; set; }

        /// <summary>
        /// Gets or sets the network coverage.
        /// </summary>
        /// <value>
        /// The network coverage.
        /// </value>
        [Column(CanBeNull = true)]
        public int NetworkCoverage { get; set; } 

        /// <summary>
        /// Gets or sets the name of the event.
        /// </summary>
        /// <value>
        /// The name of the event.
        /// </value>
        [Column(CanBeNull = true)]
        public String EventName { get; set; }

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
        /// Gets or sets the name of the screen.
        /// </summary>
        /// <value>
        /// The name of the screen.
        /// </value>
        [Column(CanBeNull = true)]
        public String ScreenName { get; set; }

        /// <summary>
        /// Gets the session id.
        /// </summary>
        [Column]
        public Guid SessionId { get; private set; }
        #endregion

        #region //Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorItem"/> class.
        /// </summary>
        public ErrorItem()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorItem"/> class.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="screenName">Name of the screen.</param>
        /// <param name="data">The data.</param>
        /// <param name="deviceGeneralInformation">The device general information.</param>
        /// <param name="eventName">Name of the event.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="sessionId">The session id.</param>
        /// <param name="version">The version.</param>
        public ErrorItem(Guid applicationId, String screenName, String data, DeviceGeneralInformation deviceGeneralInformation,
    String eventName, ExceptionDescriptive ex, Guid sessionId, String version)
        {
            this.ApplicationId = applicationId;
            this.Version = version;
            this.ScreenName = screenName;
            this.Data = data;
            this.AvailableFlashDriveSize = deviceGeneralInformation.AvailableFlashDriveSize;
            this.AvailableMemorySize = deviceGeneralInformation.AvailableMemorySize;
            this.Battery = deviceGeneralInformation.Battery;
            this.NetworkCoverage = deviceGeneralInformation.NetworkCoverage;
            this.EventName = eventName;
            this.DateCreated = DateTime.Now;
            this.SessionId = sessionId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorItem"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="applicationId">The application id.</param>
        /// <param name="screenName">Name of the screen.</param>
        /// <param name="data">The data.</param>
        /// <param name="deviceGeneralInformation">The device general information.</param>
        /// <param name="eventName">Name of the event.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="dateCreated">The date created.</param>
        /// <param name="sessionId">The session id.</param>
        /// <param name="version">The version.</param>
        public ErrorItem(int id, Guid applicationId, String screenName,
            String data, DeviceGeneralInformation deviceGeneralInformation, String eventName,
            ExceptionDescriptive ex, DateTime dateCreated, Guid sessionId, String version)
        {
            this.Id = id;
            this.ApplicationId = applicationId;
            this.DateCreated = dateCreated;
            this.Version = version;
            this.ScreenName = screenName;
            this.Data = data;
            this.AvailableFlashDriveSize = deviceGeneralInformation.AvailableFlashDriveSize;
            this.AvailableMemorySize = deviceGeneralInformation.AvailableMemorySize;
            this.Battery = deviceGeneralInformation.Battery;
            this.NetworkCoverage = deviceGeneralInformation.NetworkCoverage;
            this.EventName = eventName;
            this.DateCreated = dateCreated;
            this.SessionId = sessionId;
        } 
        #endregion
    }
}
