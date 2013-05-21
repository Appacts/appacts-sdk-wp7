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

using AppactsPlugin.Device.Interface;
using AppactsPlugin.Data.Model;
using System.IO.IsolatedStorage;
using System.Device.Location;
using System.Windows.Threading;

namespace AppactsPlugin.Device
{
    internal class DeviceDynamicInformation : IDeviceDynamicInformation
    {
        #region //Private Properties
        private GeoCoordinate geoCoordinate;
        private GeoCoordinateWatcher geoCoordinateWatcher;
        #endregion

        #region //Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceDynamicInformation"/> class.
        /// </summary>
        public DeviceDynamicInformation()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                this.geoCoordinateWatcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
                this.geoCoordinateWatcher.PositionChanged += geoCoordinateWatcher_PositionChanged;
                this.geoCoordinateWatcher.Start();
            });
        }
	    #endregion        

        #region //Public Properties
        /// <summary>
        /// Gets the device general information.
        /// </summary>
        /// <returns></returns>
        public DeviceGeneralInformation GetDeviceGeneralInformation()
        {
            return new DeviceGeneralInformation
            (
                -1,
                Microsoft.Phone.Info.DeviceStatus.ApplicationMemoryUsageLimit,
                -1,
                -1
            );

        }

        /// <summary>
        /// Gets the device location.
        /// </summary>
        /// <returns></returns>
        public GeoCoordinate GetDeviceLocation()
        {
            return this.geoCoordinate;
        } 

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                this.geoCoordinateWatcher.PositionChanged -= this.geoCoordinateWatcher_PositionChanged;
            });
        }
        #endregion

        #region //Private Methods
        /// <summary>
        /// Handles the PositionChanged event of the geoCoordinateWatcher control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Device.Location.GeoPositionChangedEventArgs&lt;System.Device.Location.GeoCoordinate&gt;"/> instance containing the event data.</param>
        private void geoCoordinateWatcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            this.geoCoordinate = e.Position.Location;
        } 
        #endregion
    }
}
