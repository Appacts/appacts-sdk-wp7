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

using AppactsPlugin.Data.Model.Enum;

namespace AppactsPlugin.Data.Model
{
    internal class AnalyticsSystem
    {
        #region //Public Properties
        /// <summary>
        /// Gets the type of the device.
        /// </summary>
        /// <value>
        /// The type of the device.
        /// </value>
        public DeviceType DeviceType { get; set; }

        /// <summary>
        /// Gets the version.
        /// </summary>
        public String Version { get; private set; }
        #endregion

        #region //Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="AnalyticsSystem"/> class.
        /// </summary>
        /// <param name="deviceType">Type of the device.</param>
        /// <param name="version">The version.</param>
        public AnalyticsSystem(DeviceType deviceType, String version)
        {
            this.DeviceType = deviceType;
            this.Version = version;
        } 
        #endregion
    }
}
