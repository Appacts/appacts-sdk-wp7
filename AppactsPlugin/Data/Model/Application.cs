using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Data.Linq.Mapping;

using AppactsPlugin.Data.Model.Enum;
using System;

namespace AppactsPlugin.Data.Model
{
    [Table(Name = "Application")]
    internal class ApplicationMeta : INotifyPropertyChanged
    {
        #region //Private Properties
        private Guid id;
        private ApplicationStateType state;
        private OptStatusType optStatus;
        private DateTime dateCreated;
        private bool upgraded;
        private string version;
        private Guid sessionId;
        #endregion

        #region //Public Properties
        /// <summary>
        /// Gets or sets the application id.
        /// </summary>
        /// <value>
        /// The application id.
        /// </value>
        [Column(IsPrimaryKey = true)]
        public Guid Id 
        {
            get
            {
                return this.id;
            }
            set
            {
                if (this.id != value)
                {
                    this.id = value;
                    this.NotifyPropertyChanged("Id");
                }
            }
        }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        [Column]
        public ApplicationStateType State 
        {
            get
            {
                return this.state;
            }
            set
            {
                if (this.state != value)
                {
                    this.state = value;
                    this.NotifyPropertyChanged("State");
                }
            }
        }

        /// <summary>
        /// Gets or sets the date created.
        /// </summary>
        /// <value>
        /// The date created.
        /// </value>
        [Column]
        public DateTime DateCreated 
        {
            get
            {
                return this.dateCreated;
            }
            set
            {
                if (this.dateCreated != value)
                {
                    this.dateCreated = value;
                    this.NotifyPropertyChanged("DateCreated");
                }
            }
        }

        /// <summary>
        /// Gets or sets the opt status.
        /// </summary>
        /// <value>
        /// The opt status.
        /// </value>
        [Column]
        public OptStatusType OptStatus
        {
            get
            {
                return this.optStatus;
            }
            set
            {
                if (this.optStatus != value)
                {
                    this.optStatus = value;
                    this.NotifyPropertyChanged("OptStatus");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ApplicationMeta"/> is upgraded.
        /// </summary>
        /// <value>
        ///   <c>true</c> if upgraded; otherwise, <c>false</c>.
        /// </value>
        [Column]
        public bool Upgraded
        {
            get
            {
                return this.upgraded;
            }
            set
            {
                if (this.upgraded != value)
                {
                    this.upgraded = value;
                    this.NotifyPropertyChanged("Upgraded");
                }
            }
        }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        [Column]
        public string Version 
        {
            get
            {
                return this.version;
            }
            set
            {
                if (this.version != value)
                {
                    this.version = value;
                    this.NotifyPropertyChanged("Version");
                }
            }
        }


        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        [Column]
        public Guid SessionId
        {
            get
            {
                return this.sessionId;
            }
            set
            {
                if (this.sessionId != value)
                {
                    this.sessionId = value;
                    this.NotifyPropertyChanged("SessionId");
                }
            }
        }
        #endregion

        #region //Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationMeta"/> class.
        /// </summary>
        public ApplicationMeta()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationMeta"/> class.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="applicationStateType">Type of the application state.</param>
        /// <param name="dateCreated">The date created.</param>
        public ApplicationMeta(Guid applicationId, ApplicationStateType applicationStateType, DateTime dateCreated, 
            Guid sessionId, String version, bool upgraded, OptStatusType optStatus)
        {
            this.Id = applicationId;
            this.State = applicationStateType;
            this.DateCreated = dateCreated;
            this.OptStatus = optStatus;
            this.Version = version;
            this.Upgraded = upgraded;
            this.SessionId = sessionId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationMeta"/> class.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="applicationStateType">Type of the application state.</param>
        /// <param name="dateCreated">The date created.</param>
        /// <param name="optStatusType">Type of the opt status.</param>
        public ApplicationMeta(Guid applicationId, ApplicationStateType applicationStateType, 
            DateTime dateCreated, OptStatusType optStatusType)
        {
            this.Id = applicationId;
            this.State = applicationStateType;
            this.DateCreated = dateCreated;
            this.OptStatus = optStatusType;
        } 
        #endregion


        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify the page that a data context property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
