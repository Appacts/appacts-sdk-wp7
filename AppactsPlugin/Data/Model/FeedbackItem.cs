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
    internal class FeedbackItem
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
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        [Column(CanBeNull = true)]
        public String Message { get; set; }

        /// <summary>
        /// Gets or sets the rating.
        /// </summary>
        /// <value>
        /// The rating.
        /// </value>
        [Column(CanBeNull = true)]
        public RatingType Rating { get; set; }

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
        /// Initializes a new instance of the <see cref="FeedbackItem"/> class.
        /// </summary>
        public FeedbackItem()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedbackItem"/> class.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="screenName">Name of the screen.</param>
        /// <param name="message">The message.</param>
        /// <param name="ratingType">Type of the rating.</param>
        /// <param name="sessionId">The session id.</param>
        /// <param name="version">The version.</param>
        public FeedbackItem(Guid applicationId, 
            String screenName, String message, RatingType ratingType, Guid sessionId, String version)
        {
            this.ApplicationId = applicationId;
            this.Version = version;
            this.ScreenName = screenName;
            this.Message = message;
            this.Rating = ratingType;
            this.DateCreated = DateTime.Now;
            this.SessionId = sessionId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedbackItem"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="applicationId">The application id.</param>
        /// <param name="screenName">Name of the screen.</param>
        /// <param name="message">The message.</param>
        /// <param name="ratingType">Type of the rating.</param>
        /// <param name="dateCreated">The date created.</param>
        /// <param name="sessionId">The session id.</param>
        /// <param name="version">The version.</param>
        public FeedbackItem(int id, Guid applicationId, String screenName, String message,
            RatingType ratingType, DateTime dateCreated, Guid sessionId, String version)
        {
            this.Id = id;
            this.ApplicationId = applicationId;
            this.ScreenName = screenName;
            this.DateCreated = dateCreated;
            this.Version = version;
            this.Message = message;
            this.Rating = ratingType;
            this.SessionId = sessionId;
        } 
        #endregion
    }
}
