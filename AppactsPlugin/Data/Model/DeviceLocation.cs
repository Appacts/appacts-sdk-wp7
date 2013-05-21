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
    [Table(Name = "Device")]
    internal class DeviceLocation : INotifyPropertyChanged
    {
        #region //Private Properties
        private Guid id;
        private double latitude;
        private double longitude;
        private string countryName;
        private string countryCode;
        private string countryAdminName;
        private string countryAdminCode;
        private StatusType status;
        private DateTime dateCreated;
        #endregion

        #region //Public Properties
        /// <summary>
        /// Gets the id.
        /// </summary>
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
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>
        /// The latitude.
        /// </value>
        [Column(CanBeNull = true)]
        public double Latitude 
        {
            get
            {
                return this.latitude;
            }
            set
            {
                if(this.latitude != value)
                {
                    this.latitude = value;
                    this.NotifyPropertyChanged("Latitude");
                }
            }
        }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>
        /// The longitude.
        /// </value>
        [Column(CanBeNull = true)]
        public double Longitude 
        {
            get
            {
                return this.longitude;
            }
            set
            {
                if (this.longitude != value)
                {
                    this.longitude = value;
                    this.NotifyPropertyChanged("Longitude");
                }
            }
        }

        /// <summary>
        /// Gets or sets the name of the country.
        /// </summary>
        /// <value>
        /// The name of the country.
        /// </value>
        [Column(CanBeNull = true)]
        public String CountryName 
        {
            get
            {
                return this.countryName;
            }
            set
            {
                if(this.countryName != value)
                {
                    this.countryName = value;
                    this.NotifyPropertyChanged("CountryName");
                }
            }
        }

        /// <summary>
        /// Gets or sets the country code.
        /// </summary>
        /// <value>
        /// The country code.
        /// </value>
        [Column(CanBeNull = true)]
        public String CountryCode 
        {
            get
            {
                return this.countryCode;
            }
            set
            {
                if (this.countryCode != value)
                {
                    this.countryCode = value;
                    this.NotifyPropertyChanged("CountryCode");
                }
            }
        }

        /// <summary>
        /// Gets or sets the name of the country admin.
        /// </summary>
        /// <value>
        /// The name of the country admin.
        /// </value>
        [Column(CanBeNull = true)]
        public String CountryAdminName 
        {
            get
            {
                return this.countryAdminCode;
            }
            set
            {
                if (this.countryAdminCode != value)
                {
                    this.countryAdminCode = value;
                    this.NotifyPropertyChanged("CountryAdminName");
                }
            }
        }

        /// <summary>
        /// Gets or sets the country admin code.
        /// </summary>
        /// <value>
        /// The country admin code.
        /// </value>
        [Column(CanBeNull = true)]
        public String CountryAdminCode 
        {
            get
            {
                return this.countryAdminCode;
            }
            set
            {
                if (this.countryAdminCode != value)
                {
                    this.countryAdminCode = value;
                    this.NotifyPropertyChanged("CountryAdminCode");
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
        #endregion

        #region //Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceLocation"/> class.
        /// </summary>
        public DeviceLocation()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceLocation"/> class.
        /// </summary>
        /// <param name="deviceId">The device id.</param>
        public DeviceLocation(Guid deviceId)
        {
            this.Id = deviceId;
            this.DateCreated = DateTime.Now;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceLocation"/> class.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        public DeviceLocation(double latitude, double longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.CountryName = null;
            this.CountryCode = null;
            this.CountryAdminName = null;
            this.CountryAdminCode = null;
            this.DateCreated = DateTime.Now;
            this.Status = StatusType.Pending;

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceLocation"/> class.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <param name="dateCreated">The date created.</param>
        public DeviceLocation(double latitude, double longitude, DateTime dateCreated)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.CountryName = null;
            this.CountryCode = null;
            this.CountryAdminName = null;
            this.CountryAdminCode = null;
            this.DateCreated = dateCreated;
            this.Status = StatusType.Pending;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceLocation"/> class.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <param name="countryName">Name of the country.</param>
        /// <param name="countryCode">The country code.</param>
        /// <param name="countryAdminName">Name of the country admin.</param>
        /// <param name="countryAdminCode">The country admin code.</param>
        /// <param name="dateCreated">The date created.</param>
        public DeviceLocation(double latitude, double longitude, String countryName, String countryCode,
            String countryAdminName, String countryAdminCode, DateTime dateCreated)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.CountryName = countryName;
            this.CountryCode = countryCode;
            this.CountryAdminName = countryAdminName;
            this.CountryAdminCode = countryAdminCode;
            this.DateCreated = dateCreated;
            this.Status = StatusType.Pending;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceLocation"/> class.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <param name="countryName">Name of the country.</param>
        /// <param name="countryCode">The country code.</param>
        /// <param name="countryAdminName">Name of the country admin.</param>
        /// <param name="countryAdminCode">The country admin code.</param>
        public DeviceLocation(double latitude, double longitude, String countryName, String countryCode,
        String countryAdminName, String countryAdminCode)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.CountryName = countryName;
            this.CountryCode = countryCode;
            this.CountryAdminName = countryAdminName;
            this.CountryAdminCode = countryAdminCode;
            this.DateCreated = DateTime.Now;
            this.Status = StatusType.Pending;
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
