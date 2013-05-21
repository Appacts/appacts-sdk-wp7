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

namespace AppactsPlugin.Device
{
    internal class Platform : IPlatform
    {
        /// <summary>
        /// Gets the carrier.
        /// </summary>
        /// <returns></returns>
        public string GetCarrier()
        {
            return Microsoft.Phone.Net.NetworkInformation.DeviceNetworkInformation.CellularMobileOperator;
        }

        /// <summary>
        /// Gets the OS.
        /// </summary>
        /// <returns></returns>
        public string GetOS()
        {
            string osVersion = null;

            try
            {
                osVersion = System.Environment.OSVersion.Version.ToString();
            }
            catch (Exception) { }

            return osVersion;
        }
    }
}
