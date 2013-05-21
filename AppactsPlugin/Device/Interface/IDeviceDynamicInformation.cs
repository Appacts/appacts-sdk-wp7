using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppactsPlugin.Data.Model;

using System.Device.Location;

namespace AppactsPlugin.Device.Interface
{
    internal interface IDeviceDynamicInformation : IDisposable
    {
        DeviceGeneralInformation GetDeviceGeneralInformation();
        GeoCoordinate GetDeviceLocation();
    }
}
