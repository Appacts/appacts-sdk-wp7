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
    internal class EventItem
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
        /// Gets or sets the type of the event.
        /// </summary>
        /// <value>
        /// The type of the event.
        /// </value>
        [Column(CanBeNull = true)]
        public EventType EventType { get; set; }

        /// <summary>
        /// Gets or sets the name of the event.
        /// </summary>
        /// <value>
        /// The name of the event.
        /// </value>
        [Column(CanBeNull = true)]
        public String EventName { get; set; }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        /// <value>
        /// The length.
        /// </value>
        [Column(CanBeNull = true)]
        public long Length { get; set; }

        /// <summary>
        /// Gets or sets the name of the screen.
        /// </summary>
        /// <value>
        /// The name of the screen.
        /// </value>
        [Column(CanBeNull = true)]
        public String ScreenName { get; set; }

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
        /// Initializes a new instance of the <see cref="EventItem"/> class.
        /// </summary>
        public EventItem()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventItem"/> class.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="screenName">Name of the screen.</param>
        /// <param name="data">The data.</param>
        /// <param name="eventType">Type of the event.</param>
        /// <param name="eventName">Name of the event.</param>
        /// <param name="length">The length.</param>
        /// <param name="sessionId">The session id.</param>
        /// <param name="version">The version.</param>
        public EventItem(Guid applicationId, String screenName, String data, 
            EventType eventType, String eventName, long length, Guid sessionId, String version)
        {
            this.ApplicationId = applicationId;
            this.ScreenName = screenName;
            this.Version = version;
            this.Data = data;
            this.EventType = eventType;
            this.EventName = eventName;
            this.Length = length;
            this.DateCreated = DateTime.Now;
            this.SessionId = sessionId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventItem"/> class.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="screenName">Name of the screen.</param>
        /// <param name="data">The data.</param>
        /// <param name="eventType">Type of the event.</param>
        /// <param name="length">The length.</param>
        /// <param name="sessionId">The session id.</param>
        /// <param name="version">The version.</param>
        public EventItem(Guid applicationId, String screenName, String data,
            EventType eventType, long length, Guid sessionId, String version)
        {
            this.ApplicationId = applicationId;
            this.ScreenName = screenName;
            this.Version = version;
            this.Data = data;
            this.EventType = eventType;
            this.Length = length;
            this.DateCreated = DateTime.Now;
            this.SessionId = sessionId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventItem"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="applicationId">The application id.</param>
        /// <param name="screenName">Name of the screen.</param>
        /// <param name="data">The data.</param>
        /// <param name="eventType">Type of the event.</param>
        /// <param name="eventName">Name of the event.</param>
        /// <param name="length">The length.</param>
        /// <param name="dateCreated">The date created.</param>
        /// <param name="sessionId">The session id.</param>
        /// <param name="version">The version.</param>
        public EventItem(int id, Guid applicationId, String screenName,
            String data, EventType eventType, String eventName, long length, DateTime dateCreated, Guid sessionId, String version)
        {
            this.Id = id;
            this.ScreenName = screenName;
            this.DateCreated = dateCreated;
            this.Version = version;
            this.Data = data;
            this.EventType = eventType;
            this.EventName = eventName;
            this.Length = length;
            this.ApplicationId = applicationId;
            this.SessionId = sessionId;
        } 
        #endregion
    }
}
