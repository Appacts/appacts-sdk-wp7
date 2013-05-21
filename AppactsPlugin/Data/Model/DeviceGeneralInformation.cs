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
    internal class DeviceGeneralInformation
    {
        #region //Public Properties
        /// <summary>
        /// Gets or sets the size of the available flash drive.
        /// </summary>
        /// <value>
        /// The size of the available flash drive.
        /// </value>
        public long AvailableFlashDriveSize { get; set; }

        /// <summary>
        /// Gets or sets the size of the available memory.
        /// </summary>
        /// <value>
        /// The size of the available memory.
        /// </value>
        public long AvailableMemorySize { get; set; }

        /// <summary>
        /// Gets or sets the battery.
        /// </summary>
        /// <value>
        /// The battery.
        /// </value>
        public int Battery { get; set; }

        /// <summary>
        /// Gets or sets the network coverage.
        /// </summary>
        /// <value>
        /// The network coverage.
        /// </value>
        public int NetworkCoverage { get; set; } 
        #endregion

        #region //Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceGeneralInformation"/> class.
        /// </summary>
        public DeviceGeneralInformation()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceGeneralInformation"/> class.
        /// </summary>
        /// <param name="availableFlashDriveSize">Size of the available flash drive.</param>
        /// <param name="availableMemorySize">Size of the available memory.</param>
        /// <param name="battery">The battery.</param>
        /// <param name="networkCoverage">The network coverage.</param>
        public DeviceGeneralInformation(long availableFlashDriveSize,
            long availableMemorySize, int battery, int networkCoverage)
        {
            this.AvailableFlashDriveSize = availableFlashDriveSize;
            this.AvailableMemorySize = availableMemorySize;
            this.Battery = battery;
            this.NetworkCoverage = networkCoverage;
        } 
        #endregion
    }
}
