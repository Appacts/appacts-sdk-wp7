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
using System.ComponentModel;

namespace AppactsPlugin.Data.Model
{
    [Table]
    internal class User : INotifyPropertyChanged
    {
        #region //Private Properties
        private int age;
        private SexType sex;
        private StatusType status;
        #endregion

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
        /// Gets or sets the age.
        /// </summary>
        /// <value>
        /// The age.
        /// </value>
        [Column(CanBeNull = true)]
        public int Age 
        {
            get
            {
                return this.age;
            }
            set
            {
                if (this.age != value)
                {
                    this.age = value;
                    this.NotifyPropertyChanged("Age");
                }
            }
        }

        /// <summary>
        /// Gets or sets the sex.
        /// </summary>
        /// <value>
        /// The sex.
        /// </value>
        [Column(CanBeNull = true)]
        public SexType Sex 
        {
            get
            {
                return this.sex;
            }
            set
            {
                if (this.sex != value)
                {
                    this.sex = value;
                    this.NotifyPropertyChanged("Sex");
                }
            }
        }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        [Column(CanBeNull = true)]
        public StatusType Status
        {
            get
            {
                return this.status;
            }
            set
            {
                if (this.status != value)
                {
                    this.status = value;
                    this.NotifyPropertyChanged("Status");
                }
            }
        }

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
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        public User()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="age">The age.</param>
        /// <param name="sexType">Type of the sex.</param>
        /// <param name="statusType">Type of the status.</param>
        /// <param name="applicationId">The application id.</param>
        /// <param name="version">The version.</param>
        public User(int age, SexType sexType,
            StatusType statusType, Guid applicationId, Guid sessionId, String version)
        {
            this.ApplicationId = applicationId;
            this.Version = version;
            this.Age = age;
            this.Sex = sexType;
            this.Status = statusType;
            this.DateCreated = DateTime.Now;
            this.SessionId = sessionId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="age">The age.</param>
        /// <param name="sexType">Type of the sex.</param>
        /// <param name="statusType">Type of the status.</param>
        /// <param name="applicationId">The application id.</param>
        /// <param name="dateCreated">The date created.</param>
        /// <param name="version">The version.</param>
        public User(int id, int age, SexType sexType, StatusType statusType,
            Guid applicationId, DateTime dateCreated, Guid sessionId, String version)
        {
            this.Id = id;
            this.ApplicationId = applicationId;
            this.DateCreated = dateCreated;
            this.Version = version;
            this.Age = age;
            this.Sex = sexType;
            this.Status = statusType;
            this.SessionId = sessionId;
        } 
        #endregion

       #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify the page that a data context property changed
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
